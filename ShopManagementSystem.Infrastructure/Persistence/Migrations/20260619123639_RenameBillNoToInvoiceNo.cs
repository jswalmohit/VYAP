using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopManagementSystem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RenameBillNoToInvoiceNo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BillNo",
                table: "Sales",
                newName: "InvoiceNo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InvoiceNo",
                table: "Sales",
                newName: "BillNo");
        }
    }
}
