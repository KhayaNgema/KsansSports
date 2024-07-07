using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class AddLeagueToClubReports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClubPerformanceReport_LeagueId",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClubTransferReport_LeagueId",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ClubPerformanceReport_LeagueId",
                table: "Reports",
                column: "ClubPerformanceReport_LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ClubTransferReport_LeagueId",
                table: "Reports",
                column: "ClubTransferReport_LeagueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_League_ClubPerformanceReport_LeagueId",
                table: "Reports",
                column: "ClubPerformanceReport_LeagueId",
                principalTable: "League",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_League_ClubTransferReport_LeagueId",
                table: "Reports",
                column: "ClubTransferReport_LeagueId",
                principalTable: "League",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_League_ClubPerformanceReport_LeagueId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_League_ClubTransferReport_LeagueId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ClubPerformanceReport_LeagueId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ClubTransferReport_LeagueId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ClubPerformanceReport_LeagueId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ClubTransferReport_LeagueId",
                table: "Reports");
        }
    }
}
