using Microsoft.EntityFrameworkCore.Migrations;

namespace Banka.Migrations
{
    public partial class Initial3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "tip",
                table: "EmployeeFirms",
                nullable: true,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "tip",
                table: "EmployeeFirms",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
