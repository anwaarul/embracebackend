using Microsoft.EntityFrameworkCore.Migrations;

namespace Embrace.Migrations
{
    public partial class AlertTable_AddColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UniqueKey",
                table: "AlertInfo",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniqueKey",
                table: "AlertInfo");
        }
    }
}
