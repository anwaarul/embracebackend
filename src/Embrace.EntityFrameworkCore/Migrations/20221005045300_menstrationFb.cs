using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Embrace.Migrations
{
    public partial class menstrationFb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDatePeriod",
                table: "UniqueNameAndDateInfo");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "MenstruationDetailsInfo",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "MenstruationDetailsInfo");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDatePeriod",
                table: "UniqueNameAndDateInfo",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
