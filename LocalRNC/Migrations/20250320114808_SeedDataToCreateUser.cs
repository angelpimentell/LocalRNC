using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocalRNC.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataToCreateUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string hashedPassword = new PasswordHasher<object>().HashPassword(null, "admin");

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "CreatedAt", "Email", "IsAdmin", "Password" },
                values: new object[] { 1, new DateTime(2025, 3, 20, 0, 0, 0, 0, DateTimeKind.Utc), "admin@admin.com", true, hashedPassword });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
