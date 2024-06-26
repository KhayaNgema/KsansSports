using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class AddArchiveModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LeagueId",
                table: "Standing",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LeagueId",
                table: "MatchResult",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LeagueId",
                table: "Fixture",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LeagueId",
                table: "Club",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Standing_LeagueId",
                table: "Standing",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchResult_LeagueId",
                table: "MatchResult",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_Fixture_LeagueId",
                table: "Fixture",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_Club_LeagueId",
                table: "Club",
                column: "LeagueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Club_League_LeagueId",
                table: "Club",
                column: "LeagueId",
                principalTable: "League",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Fixture_League_LeagueId",
                table: "Fixture",
                column: "LeagueId",
                principalTable: "League",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchResult_League_LeagueId",
                table: "MatchResult",
                column: "LeagueId",
                principalTable: "League",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Standing_League_LeagueId",
                table: "Standing",
                column: "LeagueId",
                principalTable: "League",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Club_League_LeagueId",
                table: "Club");

            migrationBuilder.DropForeignKey(
                name: "FK_Fixture_League_LeagueId",
                table: "Fixture");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchResult_League_LeagueId",
                table: "MatchResult");

            migrationBuilder.DropForeignKey(
                name: "FK_Standing_League_LeagueId",
                table: "Standing");

            migrationBuilder.DropIndex(
                name: "IX_Standing_LeagueId",
                table: "Standing");

            migrationBuilder.DropIndex(
                name: "IX_MatchResult_LeagueId",
                table: "MatchResult");

            migrationBuilder.DropIndex(
                name: "IX_Fixture_LeagueId",
                table: "Fixture");

            migrationBuilder.DropIndex(
                name: "IX_Club_LeagueId",
                table: "Club");

            migrationBuilder.DropColumn(
                name: "LeagueId",
                table: "Standing");

            migrationBuilder.DropColumn(
                name: "LeagueId",
                table: "MatchResult");

            migrationBuilder.DropColumn(
                name: "LeagueId",
                table: "Fixture");

            migrationBuilder.DropColumn(
                name: "LeagueId",
                table: "Club");
        }
    }
}
