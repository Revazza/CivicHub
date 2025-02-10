using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivicHub.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultSchemaName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "CivicHub");

            migrationBuilder.RenameTable(
                name: "PhoneNumber",
                newName: "PhoneNumber",
                newSchema: "CivicHub");

            migrationBuilder.RenameTable(
                name: "Persons",
                newName: "Persons",
                newSchema: "CivicHub");

            migrationBuilder.RenameTable(
                name: "PersonConnections",
                newName: "PersonConnections",
                newSchema: "CivicHub");

            migrationBuilder.RenameTable(
                name: "Cities",
                newName: "Cities",
                newSchema: "CivicHub");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "PhoneNumber",
                schema: "CivicHub",
                newName: "PhoneNumber");

            migrationBuilder.RenameTable(
                name: "Persons",
                schema: "CivicHub",
                newName: "Persons");

            migrationBuilder.RenameTable(
                name: "PersonConnections",
                schema: "CivicHub",
                newName: "PersonConnections");

            migrationBuilder.RenameTable(
                name: "Cities",
                schema: "CivicHub",
                newName: "Cities");
        }
    }
}
