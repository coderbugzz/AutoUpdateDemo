using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoUpdateDemo.Migrations
{
    public partial class innitial_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "installers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    System = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    cur_version = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    location = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_installers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "updates",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    installer_ID = table.Column<int>(type: "int", nullable: false),
                    version = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    update_location = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_updates", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "installers");

            migrationBuilder.DropTable(
                name: "updates");
        }
    }
}
