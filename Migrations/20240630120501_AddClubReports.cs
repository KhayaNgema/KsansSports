using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class AddClubReports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClubId",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClubPerformanceReport_ClubId",
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
                name: "GamesToPlayCOunt",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GamesToPlayRate",
                table: "Reports",
                type: "decimal(18,2)",
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

            migrationBuilder.AddColumn<decimal>(
                name: "IncomingTransferRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IncomingTransfersCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OutgoingTransferRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OutgoingTransfersCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OverallTransfersCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StandingId",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ClubId",
                table: "Reports",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ClubPerformanceReport_ClubId",
                table: "Reports",
                column: "ClubPerformanceReport_ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_StandingId",
                table: "Reports",
                column: "StandingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Club_ClubId",
                table: "Reports",
                column: "ClubId",
                principalTable: "Club",
                principalColumn: "ClubId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Club_ClubPerformanceReport_ClubId",
                table: "Reports",
                column: "ClubPerformanceReport_ClubId",
                principalTable: "Club",
                principalColumn: "ClubId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Standing_StandingId",
                table: "Reports",
                column: "StandingId",
                principalTable: "Standing",
                principalColumn: "StandingId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Club_ClubId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Club_ClubPerformanceReport_ClubId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Standing_StandingId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ClubId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ClubPerformanceReport_ClubId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_StandingId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ClubId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ClubPerformanceReport_ClubId",
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
                name: "GamesToPlayCOunt",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "GamesToPlayRate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "GamesWinCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "GamesWinRate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "IncomingTransferRate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "IncomingTransfersCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "OutgoingTransferRate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "OutgoingTransfersCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "OverallTransfersCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "StandingId",
                table: "Reports");
        }
    }
}
