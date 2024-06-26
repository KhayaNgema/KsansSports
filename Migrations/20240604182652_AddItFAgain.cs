using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class AddItFAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Club_ClubManager_ClubId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ClubManager_ClubId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "ClubManagerId",
                table: "Club",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ClubManager_ClubId",
                table: "AspNetUsers",
                column: "ClubManager_ClubId",
                unique: true,
                filter: "[ClubId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Club_ClubManager_ClubId",
                table: "AspNetUsers",
                column: "ClubManager_ClubId",
                principalTable: "Club",
                principalColumn: "ClubId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Club_ClubManager_ClubId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ClubManager_ClubId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ClubManagerId",
                table: "Club");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ClubManager_ClubId",
                table: "AspNetUsers",
                column: "ClubManager_ClubId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Club_ClubManager_ClubId",
                table: "AspNetUsers",
                column: "ClubManager_ClubId",
                principalTable: "Club",
                principalColumn: "ClubId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
