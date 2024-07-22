using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class DropClubPerfReports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Club_ClubPerformanceReport_ClubId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_League_ClubPerformanceReport_LeagueId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ClubPerformanceReport_ClubId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ClubPerformanceReport_LeagueId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ClubPerformanceReport_ClubId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ClubPerformanceReport_LeagueId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "GamesDrawCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "GamesDrawRate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "GamesLoseCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "GamesLoseRate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "GamesNotPlayedCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "GamesNotPlayedRate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "GamesPlayedCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "GamesPlayedRate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "GamesToPlayCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "GamesWinCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "GamesWinRate",
                table: "Reports");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClubPerformanceReport_ClubId",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClubPerformanceReport_LeagueId",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GamesDrawCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GamesDrawRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GamesLoseCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GamesLoseRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GamesNotPlayedCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GamesNotPlayedRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GamesPlayedCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GamesPlayedRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GamesToPlayCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GamesWinCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GamesWinRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StandingId",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ClubPerformanceReport_ClubId",
                table: "Reports",
                column: "ClubPerformanceReport_ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ClubPerformanceReport_LeagueId",
                table: "Reports",
                column: "ClubPerformanceReport_LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_StandingId",
                table: "Reports",
                column: "StandingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Club_ClubPerformanceReport_ClubId",
                table: "Reports",
                column: "ClubPerformanceReport_ClubId",
                principalTable: "Club",
                principalColumn: "ClubId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_League_ClubPerformanceReport_LeagueId",
                table: "Reports",
                column: "ClubPerformanceReport_LeagueId",
                principalTable: "League",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Standing_StandingId",
                table: "Reports",
                column: "StandingId",
                principalTable: "Standing",
                principalColumn: "StandingId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
