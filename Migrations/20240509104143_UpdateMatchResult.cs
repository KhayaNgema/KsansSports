using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMatchResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwayTeam",
                table: "MatchResult");

            migrationBuilder.DropColumn(
                name: "HomeTeam",
                table: "MatchResult");

            migrationBuilder.CreateIndex(
                name: "IX_MatchResult_AwayTeamId",
                table: "MatchResult",
                column: "AwayTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchResult_HomeTeamId",
                table: "MatchResult",
                column: "HomeTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchResult_Club_AwayTeamId",
                table: "MatchResult",
                column: "AwayTeamId",
                principalTable: "Club",
                principalColumn: "ClubId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchResult_Club_HomeTeamId",
                table: "MatchResult",
                column: "HomeTeamId",
                principalTable: "Club",
                principalColumn: "ClubId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchResult_Club_AwayTeamId",
                table: "MatchResult");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchResult_Club_HomeTeamId",
                table: "MatchResult");

            migrationBuilder.DropIndex(
                name: "IX_MatchResult_AwayTeamId",
                table: "MatchResult");

            migrationBuilder.DropIndex(
                name: "IX_MatchResult_HomeTeamId",
                table: "MatchResult");

            migrationBuilder.AddColumn<string>(
                name: "AwayTeam",
                table: "MatchResult",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HomeTeam",
                table: "MatchResult",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
