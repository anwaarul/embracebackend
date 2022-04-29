using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Embrace.Migrations
{
    public partial class orderplacementDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderPlacementInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserUniqueName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VarientName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SizeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipPostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_OrderPlacementInfo", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderPlacementInfo");
        }
    }
}
