using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class ClubId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClubId",
                table: "HeadToHead",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_HeadToHead_ClubId",
                table: "HeadToHead",
                column: "ClubId");

            migrationBuilder.AddForeignKey(
                name: "FK_HeadToHead_Club_ClubId",
                table: "HeadToHead",
                column: "ClubId",
                principalTable: "Club",
                principalColumn: "ClubId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HeadToHead_Club_ClubId",
                table: "HeadToHead");

            migrationBuilder.DropIndex(
                name: "IX_HeadToHead_ClubId",
                table: "HeadToHead");

            migrationBuilder.DropColumn(
                name: "ClubId",
                table: "HeadToHead");
        }
    }
}
