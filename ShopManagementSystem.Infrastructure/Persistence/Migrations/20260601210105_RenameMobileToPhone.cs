using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopManagementSystem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameMobileToPhone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MobileNumber",
                table: "Customers",
                newName: "PhoneNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_MobileNumber",
                table: "Customers",
                newName: "IX_Customers_PhoneNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Customers",
                newName: "MobileNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_PhoneNumber",
                table: "Customers",
                newName: "IX_Customers_MobileNumber");
        }
    }
}
