using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopManagementSystem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProductSellerInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SellerGSTIN",
                table: "Products",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SellerName",
                table: "Products",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellerGSTIN",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SellerName",
                table: "Products");
        }
    }
}
