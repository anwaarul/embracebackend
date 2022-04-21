using Microsoft.EntityFrameworkCore.Migrations;

namespace Embrace.Migrations
{
    public partial class ProductParameterDataTypeChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductType",
                table: "ProductParametersInfo",
                newName: "ProductName");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ProductParametersInfo",
                newName: "ProductTypId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductTypId",
                table: "ProductParametersInfo",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "ProductParametersInfo",
                newName: "ProductType");
        }
    }
}
