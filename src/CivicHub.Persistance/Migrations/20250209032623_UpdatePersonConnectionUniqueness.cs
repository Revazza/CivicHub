using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivicHub.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePersonConnectionUniqueness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PersonConnections_PersonId_ConnectedPersonId",
                table: "PersonConnections");

            migrationBuilder.AlterColumn<string>(
                name: "ConnectionType",
                table: "PersonConnections",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonConnections_PersonId_ConnectedPersonId_ConnectionType",
                table: "PersonConnections",
                columns: new[] { "PersonId", "ConnectedPersonId", "ConnectionType" },
                unique: true,
                filter: "[ConnectionType] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PersonConnections_PersonId_ConnectedPersonId_ConnectionType",
                table: "PersonConnections");

            migrationBuilder.AlterColumn<string>(
                name: "ConnectionType",
                table: "PersonConnections",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonConnections_PersonId_ConnectedPersonId",
                table: "PersonConnections",
                columns: new[] { "PersonId", "ConnectedPersonId" },
                unique: true);
        }
    }
}
