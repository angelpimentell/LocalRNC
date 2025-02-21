using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocalRNC.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultValueForCreatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Created_at",
                table: "YourTableName", // Replace with your actual table name
                type: "timestamp with time zone", // Keep the same type
                nullable: false,
                defaultValue: DateTime.UtcNow, // Sets the default to the current timestamp
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: false); // Adjust this if needed based on your existing column settings
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Created_at",
                table: "YourTableName", // Replace with your actual table name
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: DateTime.UtcNow); // Remove the default value on rollback
        }
    }
}
