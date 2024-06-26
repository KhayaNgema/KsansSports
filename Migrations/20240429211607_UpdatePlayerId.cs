using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePlayerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LineUpXIHolder_AspNetUsers_ClubPlayerId",
                table: "LineUpXIHolder");

            migrationBuilder.DropIndex(
                name: "IX_LineUpXIHolder_ClubPlayerId",
                table: "LineUpXIHolder");

            migrationBuilder.DropColumn(
                name: "ClubPlayerId",
                table: "LineUpXIHolder");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "LineUpXIHolder");

            migrationBuilder.AddColumn<string>(
                name: "PlayerId",
                table: "LineUpXIHolder",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_LineUpXIHolder_PlayerId",
                table: "LineUpXIHolder",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_LineUpXIHolder_AspNetUsers_PlayerId",
                table: "LineUpXIHolder",
                column: "PlayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LineUpXIHolder_AspNetUsers_PlayerId",
                table: "LineUpXIHolder");

            migrationBuilder.DropIndex(
                name: "IX_LineUpXIHolder_PlayerId",
                table: "LineUpXIHolder");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "LineUpXIHolder");

            migrationBuilder.AddColumn<string>(
                name: "ClubPlayerId",
                table: "LineUpXIHolder",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "LineUpXIHolder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_LineUpXIHolder_ClubPlayerId",
                table: "LineUpXIHolder",
                column: "ClubPlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_LineUpXIHolder_AspNetUsers_ClubPlayerId",
                table: "LineUpXIHolder",
                column: "ClubPlayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
