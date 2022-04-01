using Microsoft.EntityFrameworkCore.Migrations;

namespace Embrace.Migrations
{
    public partial class UniqueNameAndDateInfo2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UniqueKey",
                table: "UniqueNameAndDateInfo",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniqueKey",
                table: "UniqueNameAndDateInfo");
        }
    }
}
