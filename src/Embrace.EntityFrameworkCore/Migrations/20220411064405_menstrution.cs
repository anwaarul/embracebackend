using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Embrace.Migrations
{
    public partial class menstrution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MenstruationDetailsInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniqueKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MyCycle = table.Column<int>(type: "int", nullable: false),
                    Ovulation_day = table.Column<int>(type: "int", nullable: false),
                    Last_ovulation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Next_mens = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Next_ovulation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ovulation_window1 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ovulation_window2 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ovulation_window3 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ovulation_window4 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Safe_period1 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Safe_period2 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Safe_period3 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Safe_period4 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenstruationDetailsInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubCategoryAndDateInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    DateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UniqueKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategoryAndDateInfo", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenstruationDetailsInfo");

            migrationBuilder.DropTable(
                name: "SubCategoryAndDateInfo");
        }
    }
}
