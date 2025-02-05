using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CivicHub.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedLocationByNameAndPropertiesToCity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Locations_LocationId",
                table: "Persons");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Persons_LocationId",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Persons");

            migrationBuilder.AddColumn<string>(
                name: "CityCode",
                table: "Persons",
                type: "nvarchar(3)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Code);
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Code", "Name" },
                values: new object[,]
                {
                    { "BT", "Batumi" },
                    { "TB", "Tbilisi" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Persons_CityCode",
                table: "Persons",
                column: "CityCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Cities_CityCode",
                table: "Persons",
                column: "CityCode",
                principalTable: "Cities",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Cities_CityCode",
                table: "Persons");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Persons_CityCode",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "CityCode",
                table: "Persons");

            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "Persons",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CityCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Persons_LocationId",
                table: "Persons",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Locations_LocationId",
                table: "Persons",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
