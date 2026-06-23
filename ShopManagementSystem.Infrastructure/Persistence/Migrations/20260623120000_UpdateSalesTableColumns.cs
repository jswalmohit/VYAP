using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopManagementSystem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSalesTableColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Rename SellingPrice to USP
            migrationBuilder.RenameColumn(
                name: "SellingPrice",
                table: "Sales",
                newName: "USP");

            // Rename SaleDate to UpdatedDate
            migrationBuilder.RenameColumn(
                name: "SaleDate",
                table: "Sales",
                newName: "UpdatedDate");

            // Add new columns
            migrationBuilder.AddColumn<decimal>(
                name: "CGSTRate",
                table: "Sales",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SGSTRate",
                table: "Sales",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Sales",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IPAddress",
                table: "Sales",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            // Drop old index on SaleDate
            migrationBuilder.DropIndex(
                name: "IX_Sales_SaleDate",
                table: "Sales");

            // Create new index on UpdatedDate
            migrationBuilder.CreateIndex(
                name: "IX_Sales_UpdatedDate",
                table: "Sales",
                column: "UpdatedDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop new index on UpdatedDate
            migrationBuilder.DropIndex(
                name: "IX_Sales_UpdatedDate",
                table: "Sales");

            // Create old index on SaleDate
            migrationBuilder.CreateIndex(
                name: "IX_Sales_SaleDate",
                table: "Sales",
                column: "SaleDate");

            // Remove new columns
            migrationBuilder.DropColumn(
                name: "IPAddress",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "SGSTRate",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "CGSTRate",
                table: "Sales");

            // Rename UpdatedDate back to SaleDate
            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "Sales",
                newName: "SaleDate");

            // Rename USP back to SellingPrice
            migrationBuilder.RenameColumn(
                name: "USP",
                table: "Sales",
                newName: "SellingPrice");
        }
    }
}
