using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce_API.Migrations
{
    /// <inheritdoc />
    public partial class seeding_vistor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DeletedAt", "Email", "FullName", "IsDeleted", "PasswordHash", "Role", "Username" },
                values: new object[] { 2, null, "Visitor@gmail.com", "Visitor User", false, "$2a$11$PEGr4ZzWWfI8U0JkX.kYJuTAaWfIUOBgSLqdNhN7S58NZUpMLZ0bG", "Visitor", "Visitor" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
