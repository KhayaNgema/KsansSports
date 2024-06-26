using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class AdddNa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ClubManager_ClubId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "ClubManagerId",
                table: "Club",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Club_ClubManagerId",
                table: "Club",
                column: "ClubManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ClubManager_ClubId",
                table: "AspNetUsers",
                column: "ClubManager_ClubId");

            migrationBuilder.AddForeignKey(
                name: "FK_Club_AspNetUsers_ClubManagerId",
                table: "Club",
                column: "ClubManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Club_AspNetUsers_ClubManagerId",
                table: "Club");

            migrationBuilder.DropIndex(
                name: "IX_Club_ClubManagerId",
                table: "Club");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ClubManager_ClubId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ClubManagerId",
                table: "Club");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ClubManager_ClubId",
                table: "AspNetUsers",
                column: "ClubManager_ClubId",
                unique: true,
                filter: "[ClubId] IS NOT NULL");
        }
    }
}
