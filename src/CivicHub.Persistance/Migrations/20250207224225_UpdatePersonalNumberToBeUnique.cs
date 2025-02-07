using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivicHub.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePersonalNumberToBeUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Cities_CityCode",
                table: "Persons");

            migrationBuilder.AlterColumn<string>(
                name: "CityCode",
                table: "Persons",
                type: "nvarchar(3)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(3)");

            migrationBuilder.AlterColumn<string>(
                name: "ConnectionType",
                table: "PersonConnections",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_PersonalNumber",
                table: "Persons",
                column: "PersonalNumber",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Cities_CityCode",
                table: "Persons",
                column: "CityCode",
                principalTable: "Cities",
                principalColumn: "Code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Cities_CityCode",
                table: "Persons");

            migrationBuilder.DropIndex(
                name: "IX_Persons_PersonalNumber",
                table: "Persons");

            migrationBuilder.AlterColumn<string>(
                name: "CityCode",
                table: "Persons",
                type: "nvarchar(3)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConnectionType",
                table: "PersonConnections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Cities_CityCode",
                table: "Persons",
                column: "CityCode",
                principalTable: "Cities",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
