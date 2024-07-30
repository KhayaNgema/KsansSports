using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class AddLeagueIdsssssss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LeagueId",
                table: "YellowCards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LeagueId",
                table: "Substitutes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MatchResultsReports_LeagueId",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LeagueId",
                table: "RedCards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LeagueId",
                table: "Penalties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LeagueId",
                table: "LiveYellowCardHolders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LeagueId",
                table: "LiveRedCardHolders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LeagueId",
                table: "LiveGoals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LeagueId",
                table: "LiveGoalHolders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LeagueId",
                table: "LiveAssists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LeagueId",
                table: "LiveAssistHolders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LeagueId",
                table: "Live",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_YellowCards_LeagueId",
                table: "YellowCards",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_Substitutes_LeagueId",
                table: "Substitutes",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_MatchResultsReports_LeagueId",
                table: "Reports",
                column: "MatchResultsReports_LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_RedCards_LeagueId",
                table: "RedCards",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_Penalties_LeagueId",
                table: "Penalties",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveYellowCardHolders_LeagueId",
                table: "LiveYellowCardHolders",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveRedCardHolders_LeagueId",
                table: "LiveRedCardHolders",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveGoals_LeagueId",
                table: "LiveGoals",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveGoalHolders_LeagueId",
                table: "LiveGoalHolders",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveAssists_LeagueId",
                table: "LiveAssists",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveAssistHolders_LeagueId",
                table: "LiveAssistHolders",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_Live_LeagueId",
                table: "Live",
                column: "LeagueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Live_League_LeagueId",
                table: "Live",
                column: "LeagueId",
                principalTable: "League",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LiveAssistHolders_League_LeagueId",
                table: "LiveAssistHolders",
                column: "LeagueId",
                principalTable: "League",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LiveAssists_League_LeagueId",
                table: "LiveAssists",
                column: "LeagueId",
                principalTable: "League",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LiveGoalHolders_League_LeagueId",
                table: "LiveGoalHolders",
                column: "LeagueId",
                principalTable: "League",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LiveGoals_League_LeagueId",
                table: "LiveGoals",
                column: "LeagueId",
                principalTable: "League",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LiveRedCardHolders_League_LeagueId",
                table: "LiveRedCardHolders",
                column: "LeagueId",
                principalTable: "League",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LiveYellowCardHolders_League_LeagueId",
                table: "LiveYellowCardHolders",
                column: "LeagueId",
                principalTable: "League",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Penalties_League_LeagueId",
                table: "Penalties",
                column: "LeagueId",
                principalTable: "League",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_RedCards_League_LeagueId",
                table: "RedCards",
                column: "LeagueId",
                principalTable: "League",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_League_MatchResultsReports_LeagueId",
                table: "Reports",
                column: "MatchResultsReports_LeagueId",
                principalTable: "League",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Substitutes_League_LeagueId",
                table: "Substitutes",
                column: "LeagueId",
                principalTable: "League",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_YellowCards_League_LeagueId",
                table: "YellowCards",
                column: "LeagueId",
                principalTable: "League",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Live_League_LeagueId",
                table: "Live");

            migrationBuilder.DropForeignKey(
                name: "FK_LiveAssistHolders_League_LeagueId",
                table: "LiveAssistHolders");

            migrationBuilder.DropForeignKey(
                name: "FK_LiveAssists_League_LeagueId",
                table: "LiveAssists");

            migrationBuilder.DropForeignKey(
                name: "FK_LiveGoalHolders_League_LeagueId",
                table: "LiveGoalHolders");

            migrationBuilder.DropForeignKey(
                name: "FK_LiveGoals_League_LeagueId",
                table: "LiveGoals");

            migrationBuilder.DropForeignKey(
                name: "FK_LiveRedCardHolders_League_LeagueId",
                table: "LiveRedCardHolders");

            migrationBuilder.DropForeignKey(
                name: "FK_LiveYellowCardHolders_League_LeagueId",
                table: "LiveYellowCardHolders");

            migrationBuilder.DropForeignKey(
                name: "FK_Penalties_League_LeagueId",
                table: "Penalties");

            migrationBuilder.DropForeignKey(
                name: "FK_RedCards_League_LeagueId",
                table: "RedCards");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_League_MatchResultsReports_LeagueId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Substitutes_League_LeagueId",
                table: "Substitutes");

            migrationBuilder.DropForeignKey(
                name: "FK_YellowCards_League_LeagueId",
                table: "YellowCards");

            migrationBuilder.DropIndex(
                name: "IX_YellowCards_LeagueId",
                table: "YellowCards");

            migrationBuilder.DropIndex(
                name: "IX_Substitutes_LeagueId",
                table: "Substitutes");

            migrationBuilder.DropIndex(
                name: "IX_Reports_MatchResultsReports_LeagueId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_RedCards_LeagueId",
                table: "RedCards");

            migrationBuilder.DropIndex(
                name: "IX_Penalties_LeagueId",
                table: "Penalties");

            migrationBuilder.DropIndex(
                name: "IX_LiveYellowCardHolders_LeagueId",
                table: "LiveYellowCardHolders");

            migrationBuilder.DropIndex(
                name: "IX_LiveRedCardHolders_LeagueId",
                table: "LiveRedCardHolders");

            migrationBuilder.DropIndex(
                name: "IX_LiveGoals_LeagueId",
                table: "LiveGoals");

            migrationBuilder.DropIndex(
                name: "IX_LiveGoalHolders_LeagueId",
                table: "LiveGoalHolders");

            migrationBuilder.DropIndex(
                name: "IX_LiveAssists_LeagueId",
                table: "LiveAssists");

            migrationBuilder.DropIndex(
                name: "IX_LiveAssistHolders_LeagueId",
                table: "LiveAssistHolders");

            migrationBuilder.DropIndex(
                name: "IX_Live_LeagueId",
                table: "Live");

            migrationBuilder.DropColumn(
                name: "LeagueId",
                table: "YellowCards");

            migrationBuilder.DropColumn(
                name: "LeagueId",
                table: "Substitutes");

            migrationBuilder.DropColumn(
                name: "MatchResultsReports_LeagueId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "LeagueId",
                table: "RedCards");

            migrationBuilder.DropColumn(
                name: "LeagueId",
                table: "Penalties");

            migrationBuilder.DropColumn(
                name: "LeagueId",
                table: "LiveYellowCardHolders");

            migrationBuilder.DropColumn(
                name: "LeagueId",
                table: "LiveRedCardHolders");

            migrationBuilder.DropColumn(
                name: "LeagueId",
                table: "LiveGoals");

            migrationBuilder.DropColumn(
                name: "LeagueId",
                table: "LiveGoalHolders");

            migrationBuilder.DropColumn(
                name: "LeagueId",
                table: "LiveAssists");

            migrationBuilder.DropColumn(
                name: "LeagueId",
                table: "LiveAssistHolders");

            migrationBuilder.DropColumn(
                name: "LeagueId",
                table: "Live");
        }
    }
}
