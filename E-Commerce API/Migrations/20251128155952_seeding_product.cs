using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace E_Commerce_API.Migrations
{
    /// <inheritdoc />
    public partial class seeding_product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "IsDeleted", "NameAr", "NameEn", "Price" },
                values: new object[,]
                {
                    { 1, false, "لابتوب ديل", "Dell Laptop", 899.99m },
                    { 2, false, "ماوس لاسلكي", "Wireless Mouse", 30m },
                    { 3, false, "لوحة مفاتيح", "Keyboard", 50m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
