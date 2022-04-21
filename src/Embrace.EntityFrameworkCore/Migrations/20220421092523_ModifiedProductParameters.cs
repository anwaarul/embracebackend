using Microsoft.EntityFrameworkCore.Migrations;

namespace Embrace.Migrations
{
    public partial class ModifiedProductParameters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductTypId",
                table: "ProductParametersInfo",
                newName: "ProductTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductTypeId",
                table: "ProductParametersInfo",
                newName: "ProductTypId");
        }
    }
}
