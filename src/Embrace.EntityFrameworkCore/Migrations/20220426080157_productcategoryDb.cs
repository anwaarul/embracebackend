using Microsoft.EntityFrameworkCore.Migrations;

namespace Embrace.Migrations
{
    public partial class productcategoryDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "ProductParametersInfo",
                newName: "ProductCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductCategoryId",
                table: "ProductParametersInfo",
                newName: "CategoryId");
        }
    }
}
