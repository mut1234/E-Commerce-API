using E_Commerce_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace E_Commerce_API
{
    public static class DbContextExtensions
    {
        public static void ApplyGlobalFilters<TInterface>(this ModelBuilder modelBuilder,
            Expression<Func<TInterface, bool>> expression)
        {
            var entities = modelBuilder.Model
                .GetEntityTypes()
                .Where(e => e.ClrType.GetInterface(typeof(TInterface).Name) != null)
                .Select(e => e.ClrType);

            foreach (var entity in entities)
            {
                var newParam = Expression.Parameter(entity);
                var newbody = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), newParam, expression.Body);
                modelBuilder.Entity(entity).HasQueryFilter(Expression.Lambda(newbody, newParam));
            }
        }
    }
    public class ECommerceDbContext :DbContext
    {
 
        
        public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyGlobalFilters<ISoftDelete>(e => !e.IsDeleted);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FullName = "Admin User",
                    Email = "Admin@gmail.com",
                    Username = "Admin",
                    PasswordHash = "$2a$11$CPfCyL7rsl0Id5b3dZ/0D.086QjoZouqJFWl2.kSKNnF5l4XGE3ce", //BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    Role = "Admin"
                },
               new User
               {
                   Id = 2,
                   FullName = "Visitor User",
                   Email = "Visitor@gmail.com",
                   Username = "Visitor",
                   PasswordHash = "$2a$11$PEGr4ZzWWfI8U0JkX.kYJuTAaWfIUOBgSLqdNhN7S58NZUpMLZ0bG", //BCrypt.Net.BCrypt.HashPassword("Visitor@123"),
                   Role = "Visitor"
               }
                );
            modelBuilder.Entity<Product>().HasData(
                 
                new Product
                {
                    Id=1,
                    NameAr = "لابتوب ديل",
                    NameEn = "Dell Laptop",
                    Price = 899.99m,
                    IsDeleted = false
                },
                new Product
                {   Id=2,
                    NameAr = "ماوس لاسلكي",
                    NameEn = "Wireless Mouse",
                    Price = 30,
                    IsDeleted = false
                },
                new Product
                {   Id=3,
                    NameAr = "لوحة مفاتيح",
                    NameEn = "Keyboard",
                    Price = 50,
                    IsDeleted = false
                }
            
                );
        }

        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

    }

}
