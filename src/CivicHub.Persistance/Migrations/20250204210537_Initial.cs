using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivicHub.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CityCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PersonalNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Persons_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonConnections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonId = table.Column<long>(type: "bigint", nullable: false),
                    ConnectedPersonId = table.Column<long>(type: "bigint", nullable: false),
                    ConnectionType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonConnections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonConnections_Persons_ConnectedPersonId",
                        column: x => x.ConnectedPersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonConnections_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhoneNumber",
                columns: table => new
                {
                    PersonId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneNumber", x => new { x.PersonId, x.Id });
                    table.ForeignKey(
                        name: "FK_PhoneNumber_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonConnections_ConnectedPersonId",
                table: "PersonConnections",
                column: "ConnectedPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonConnections_PersonId",
                table: "PersonConnections",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_FirstName_LastName_PersonalNumber",
                table: "Persons",
                columns: new[] { "FirstName", "LastName", "PersonalNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_Persons_FirstName_LastName_PersonalNumber_Gender_BirthDate",
                table: "Persons",
                columns: new[] { "FirstName", "LastName", "PersonalNumber", "Gender", "BirthDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Persons_LocationId",
                table: "Persons",
                column: "LocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonConnections");

            migrationBuilder.DropTable(
                name: "PhoneNumber");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Locations");
        }
    }
}
