using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHomeTeamIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwayTeam",
                table: "HeadToHead");

            migrationBuilder.DropColumn(
                name: "HomeTeam",
                table: "HeadToHead");

            migrationBuilder.AddColumn<int>(
                name: "AwayTeamId",
                table: "HeadToHead",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HomeTeamId",
                table: "HeadToHead",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_HeadToHead_AwayTeamId",
                table: "HeadToHead",
                column: "AwayTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_HeadToHead_HomeTeamId",
                table: "HeadToHead",
                column: "HomeTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_HeadToHead_Club_AwayTeamId",
                table: "HeadToHead",
                column: "AwayTeamId",
                principalTable: "Club",
                principalColumn: "ClubId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_HeadToHead_Club_HomeTeamId",
                table: "HeadToHead",
                column: "HomeTeamId",
                principalTable: "Club",
                principalColumn: "ClubId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HeadToHead_Club_AwayTeamId",
                table: "HeadToHead");

            migrationBuilder.DropForeignKey(
                name: "FK_HeadToHead_Club_HomeTeamId",
                table: "HeadToHead");

            migrationBuilder.DropIndex(
                name: "IX_HeadToHead_AwayTeamId",
                table: "HeadToHead");

            migrationBuilder.DropIndex(
                name: "IX_HeadToHead_HomeTeamId",
                table: "HeadToHead");

            migrationBuilder.DropColumn(
                name: "AwayTeamId",
                table: "HeadToHead");

            migrationBuilder.DropColumn(
                name: "HomeTeamId",
                table: "HeadToHead");

            migrationBuilder.AddColumn<string>(
                name: "AwayTeam",
                table: "HeadToHead",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HomeTeam",
                table: "HeadToHead",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
