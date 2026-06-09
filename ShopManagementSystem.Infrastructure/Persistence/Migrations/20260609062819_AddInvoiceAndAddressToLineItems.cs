using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopManagementSystem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddInvoiceAndAddressToLineItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "LineItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BillId",
                table: "LineItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LineItems_BillId",
                table: "LineItems",
                column: "BillId");

            migrationBuilder.AddForeignKey(
                name: "FK_LineItems_Bills_BillId",
                table: "LineItems",
                column: "BillId",
                principalTable: "Bills",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LineItems_Bills_BillId",
                table: "LineItems");

            migrationBuilder.DropIndex(
                name: "IX_LineItems_BillId",
                table: "LineItems");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "LineItems");

            migrationBuilder.DropColumn(
                name: "BillId",
                table: "LineItems");
        }
    }
}
