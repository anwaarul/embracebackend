using Microsoft.EntityFrameworkCore.Migrations;

namespace Embrace.Migrations
{
    public partial class ModifiedProductParametersAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "UserProductAllocationInfo");

            migrationBuilder.AddColumn<long>(
                name: "Quantity",
                table: "ProductParametersInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ProductParametersInfo");

            migrationBuilder.AddColumn<long>(
                name: "Quantity",
                table: "UserProductAllocationInfo",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
