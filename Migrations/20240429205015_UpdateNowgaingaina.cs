using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNowgaingaina : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LineUpXIHolder_AspNetUsers_PlayerId",
                table: "LineUpXIHolder");

            migrationBuilder.RenameColumn(
                name: "PlayerId",
                table: "LineUpXIHolder",
                newName: "ClubPlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_LineUpXIHolder_PlayerId",
                table: "LineUpXIHolder",
                newName: "IX_LineUpXIHolder_ClubPlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_LineUpXIHolder_AspNetUsers_ClubPlayerId",
                table: "LineUpXIHolder",
                column: "ClubPlayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LineUpXIHolder_AspNetUsers_ClubPlayerId",
                table: "LineUpXIHolder");

            migrationBuilder.RenameColumn(
                name: "ClubPlayerId",
                table: "LineUpXIHolder",
                newName: "PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_LineUpXIHolder_ClubPlayerId",
                table: "LineUpXIHolder",
                newName: "IX_LineUpXIHolder_PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_LineUpXIHolder_AspNetUsers_PlayerId",
                table: "LineUpXIHolder",
                column: "PlayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
