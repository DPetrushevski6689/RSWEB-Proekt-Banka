using Microsoft.EntityFrameworkCore.Migrations;

namespace Banka.Migrations
{
    public partial class Initial1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tip",
                table: "KompaniskaSmetka");

            migrationBuilder.AddColumn<string>(
                name: "tip",
                table: "EmployeeFirms",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tip",
                table: "EmployeeFirms");

            migrationBuilder.AddColumn<string>(
                name: "tip",
                table: "KompaniskaSmetka",
                nullable: false,
                defaultValue: "");
        }
    }
}
