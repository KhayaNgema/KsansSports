using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class AddReportsModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatchReports_LeagueId = table.Column<int>(type: "int", nullable: true),
                    MatchesToBePlayedCount = table.Column<int>(type: "int", nullable: true),
                    FixturedMatchesCount = table.Column<int>(type: "int", nullable: true),
                    UnreleasedFixturesCount = table.Column<int>(type: "int", nullable: true),
                    PlayedMatchesCounts = table.Column<int>(type: "int", nullable: true),
                    PostponedMatchesCount = table.Column<int>(type: "int", nullable: true),
                    InterruptedMatchesCount = table.Column<int>(type: "int", nullable: true),
                    MatchesRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LeagueId = table.Column<int>(type: "int", nullable: true),
                    ExpectedResultsCount = table.Column<int>(type: "int", nullable: true),
                    ReleasedResultsCount = table.Column<int>(type: "int", nullable: true),
                    UnreleasedResultsCount = table.Column<int>(type: "int", nullable: true),
                    WinsCount = table.Column<int>(type: "int", nullable: true),
                    LosesCount = table.Column<int>(type: "int", nullable: true),
                    DrawsCount = table.Column<int>(type: "int", nullable: true),
                    ResultsRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TransfersReports_LeagueId = table.Column<int>(type: "int", nullable: true),
                    TransferPeriodId = table.Column<int>(type: "int", nullable: true),
                    TransferMarketCount = table.Column<int>(type: "int", nullable: true),
                    PurchasedPlayersCount = table.Column<int>(type: "int", nullable: true),
                    DeclinedTransfersCount = table.Column<int>(type: "int", nullable: true),
                    TranferAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AssociationCut = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ClubsCut = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TranferRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_Reports_League_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "League",
                        principalColumn: "LeagueId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Reports_League_MatchReports_LeagueId",
                        column: x => x.MatchReports_LeagueId,
                        principalTable: "League",
                        principalColumn: "LeagueId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Reports_League_TransfersReports_LeagueId",
                        column: x => x.TransfersReports_LeagueId,
                        principalTable: "League",
                        principalColumn: "LeagueId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Reports_TransferPeriod_TransferPeriodId",
                        column: x => x.TransferPeriodId,
                        principalTable: "TransferPeriod",
                        principalColumn: "TransferPeriodId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_LeagueId",
                table: "Reports",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_MatchReports_LeagueId",
                table: "Reports",
                column: "MatchReports_LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_TransferPeriodId",
                table: "Reports",
                column: "TransferPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_TransfersReports_LeagueId",
                table: "Reports",
                column: "TransfersReports_LeagueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reports");
        }
    }
}
