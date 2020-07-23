using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Banka.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    firstName = table.Column<string>(nullable: false),
                    lastName = table.Column<string>(nullable: false),
                    birthDate = table.Column<DateTime>(nullable: false),
                    Position = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Firma",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    firmName = table.Column<string>(nullable: false),
                    dataOsnovanje = table.Column<DateTime>(nullable: false),
                    Address = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Firma", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Korisnik",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    firstName = table.Column<string>(nullable: false),
                    lastName = table.Column<string>(nullable: false),
                    birthDate = table.Column<DateTime>(nullable: false),
                    Address = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnik", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KompaniskaSmetka",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    bankarskiBroj = table.Column<string>(nullable: false),
                    paricnaSostojba = table.Column<string>(nullable: false),
                    dataIzdavanje = table.Column<DateTime>(nullable: false),
                    tip = table.Column<string>(nullable: false),
                    firmaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KompaniskaSmetka", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KompaniskaSmetka_Firma_firmaId",
                        column: x => x.firmaId,
                        principalTable: "Firma",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FirmiSopstvenici",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    sopstvenikId = table.Column<int>(nullable: false),
                    firmaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FirmiSopstvenici", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FirmiSopstvenici_Firma_firmaId",
                        column: x => x.firmaId,
                        principalTable: "Firma",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FirmiSopstvenici_Korisnik_sopstvenikId",
                        column: x => x.sopstvenikId,
                        principalTable: "Korisnik",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KorisnickaSmetka",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    bankarskiBroj = table.Column<string>(nullable: false),
                    paricnaSostojba = table.Column<string>(nullable: false),
                    dataIzdavanje = table.Column<DateTime>(nullable: false),
                    tip = table.Column<string>(nullable: false),
                    korisnikId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KorisnickaSmetka", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KorisnickaSmetka_Korisnik_korisnikId",
                        column: x => x.korisnikId,
                        principalTable: "Korisnik",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeFirms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    employeeId = table.Column<int>(nullable: false),
                    kompaniskaSmetkaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeFirms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeFirms_Employee_employeeId",
                        column: x => x.employeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeFirms_KompaniskaSmetka_kompaniskaSmetkaId",
                        column: x => x.kompaniskaSmetkaId,
                        principalTable: "KompaniskaSmetka",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Karticka",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    brojNaKarticka = table.Column<string>(nullable: false),
                    tipNaKarticka = table.Column<string>(nullable: false),
                    paricnaSostojba = table.Column<string>(nullable: false),
                    korisnickaSmetkaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Karticka", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Karticka_KorisnickaSmetka_korisnickaSmetkaId",
                        column: x => x.korisnickaSmetkaId,
                        principalTable: "KorisnickaSmetka",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeFirms_employeeId",
                table: "EmployeeFirms",
                column: "employeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeFirms_kompaniskaSmetkaId",
                table: "EmployeeFirms",
                column: "kompaniskaSmetkaId");

            migrationBuilder.CreateIndex(
                name: "IX_FirmiSopstvenici_firmaId",
                table: "FirmiSopstvenici",
                column: "firmaId");

            migrationBuilder.CreateIndex(
                name: "IX_FirmiSopstvenici_sopstvenikId",
                table: "FirmiSopstvenici",
                column: "sopstvenikId");

            migrationBuilder.CreateIndex(
                name: "IX_Karticka_korisnickaSmetkaId",
                table: "Karticka",
                column: "korisnickaSmetkaId");

            migrationBuilder.CreateIndex(
                name: "IX_KompaniskaSmetka_firmaId",
                table: "KompaniskaSmetka",
                column: "firmaId");

            migrationBuilder.CreateIndex(
                name: "IX_KorisnickaSmetka_korisnikId",
                table: "KorisnickaSmetka",
                column: "korisnikId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeFirms");

            migrationBuilder.DropTable(
                name: "FirmiSopstvenici");

            migrationBuilder.DropTable(
                name: "Karticka");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "KompaniskaSmetka");

            migrationBuilder.DropTable(
                name: "KorisnickaSmetka");

            migrationBuilder.DropTable(
                name: "Firma");

            migrationBuilder.DropTable(
                name: "Korisnik");
        }
    }
}
