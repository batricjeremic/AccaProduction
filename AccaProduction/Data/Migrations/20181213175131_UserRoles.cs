using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccaProduction.Data.Migrations
{
    public partial class UserRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ispit",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Old_code = table.Column<string>(maxLength: 2, nullable: false),
                    New_code = table.Column<string>(maxLength: 3, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ispit", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Kandidat",
                columns: table => new
                {
                    IDACCA_Number = table.Column<int>(name: "ID(ACCA_Number)", nullable: false),
                    Ime = table.Column<string>(maxLength: 50, nullable: false),
                    Prezime = table.Column<string>(maxLength: 50, nullable: false),
                    email = table.Column<string>(nullable: false),
                    Drzava = table.Column<string>(maxLength: 50, nullable: false),
                    Odeljenje = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kandidat", x => x.IDACCA_Number);
                });

            migrationBuilder.CreateTable(
                name: "Status_Prijave",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Status_Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status_Prijave", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Polaganja",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Kandidat_ID = table.Column<int>(nullable: false),
                    Ispit_ID = table.Column<int>(nullable: false),
                    Status_ID = table.Column<int>(nullable: false),
                    Datum_Polaganja = table.Column<DateTime>(type: "date", nullable: false),
                    Rezultat = table.Column<bool>(nullable: false),
                    Potrebne_Knjige = table.Column<bool>(nullable: false),
                    StudyLeave_StartDate = table.Column<DateTime>(type: "date", nullable: true),
                    StudyLeave_EndDate = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Polaganja", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Polaganja_Ispit",
                        column: x => x.Ispit_ID,
                        principalTable: "Ispit",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Polaganja_Kandidat",
                        column: x => x.Kandidat_ID,
                        principalTable: "Kandidat",
                        principalColumn: "ID(ACCA_Number)",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Polaganja_Status_Prijave",
                        column: x => x.Status_ID,
                        principalTable: "Status_Prijave",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Polaganja_Ispit_ID",
                table: "Polaganja",
                column: "Ispit_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Polaganja_Kandidat_ID",
                table: "Polaganja",
                column: "Kandidat_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Polaganja_Status_ID",
                table: "Polaganja",
                column: "Status_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Polaganja");

            migrationBuilder.DropTable(
                name: "Ispit");

            migrationBuilder.DropTable(
                name: "Kandidat");

            migrationBuilder.DropTable(
                name: "Status_Prijave");
        }
    }
}
