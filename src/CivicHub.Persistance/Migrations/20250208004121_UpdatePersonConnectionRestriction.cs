using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivicHub.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePersonConnectionRestriction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonConnections_Persons_PersonId",
                table: "PersonConnections");

            migrationBuilder.DropIndex(
                name: "IX_PersonConnections_PersonId",
                table: "PersonConnections");

            migrationBuilder.CreateIndex(
                name: "IX_PersonConnections_PersonId_ConnectedPersonId",
                table: "PersonConnections",
                columns: new[] { "PersonId", "ConnectedPersonId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonConnections_Persons_PersonId",
                table: "PersonConnections",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonConnections_Persons_PersonId",
                table: "PersonConnections");

            migrationBuilder.DropIndex(
                name: "IX_PersonConnections_PersonId_ConnectedPersonId",
                table: "PersonConnections");

            migrationBuilder.CreateIndex(
                name: "IX_PersonConnections_PersonId",
                table: "PersonConnections",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonConnections_Persons_PersonId",
                table: "PersonConnections",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
