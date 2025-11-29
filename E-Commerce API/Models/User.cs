

using System.ComponentModel.DataAnnotations;

namespace E_Commerce_API.Models
{
    public class User : ISoftDelete
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
