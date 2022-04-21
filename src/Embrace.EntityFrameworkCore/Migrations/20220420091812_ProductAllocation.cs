using Microsoft.EntityFrameworkCore.Migrations;

namespace Embrace.Migrations
{
    public partial class ProductAllocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "ProductParametersInfo");

            migrationBuilder.AddColumn<long>(
                name: "ProductId",
                table: "ProductParametersInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "MyProperty",
                table: "AbpUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductParametersInfo");

            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "AbpUsers");

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "ProductParametersInfo",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
