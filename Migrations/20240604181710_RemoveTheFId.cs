using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTheFId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropForeignKey(
                name: "FK_Club_AspNetUsers_ClubManagerId",
                table: "Club");

            migrationBuilder.DropIndex(
                name: "IX_Club_ClubManagerId",
                table: "Club");

            migrationBuilder.DropColumn(
                name: "ClubManagerId",
                table: "Club");
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

            migrationBuilder.AddColumn<string>(
                name: "ClubManagerId",
                table: "Club",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ClubId1",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Club_ClubManagerId",
                table: "Club",
                column: "ClubManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ClubId1",
                table: "AspNetUsers",
                column: "ClubId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Club_ClubId1",
                table: "AspNetUsers",
                column: "ClubId1",
                principalTable: "Club",
                principalColumn: "ClubId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Club_AspNetUsers_ClubManagerId",
                table: "Club",
                column: "ClubManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
