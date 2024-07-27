using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class AddLiveMatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Live",
                columns: table => new
                {
                    LiveId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FixtureId = table.Column<int>(type: "int", nullable: false),
                    LiveTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomeTeamScore = table.Column<int>(type: "int", nullable: false),
                    AwayTeamScore = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsLive = table.Column<bool>(type: "bit", nullable: false),
                    IsHalfTime = table.Column<bool>(type: "bit", nullable: false),
                    ISEnded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Live", x => x.LiveId);
                });

            migrationBuilder.CreateTable(
                name: "LiveAssistHolders",
                columns: table => new
                {
                    AssistId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LiveId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveAssistHolders", x => x.AssistId);
                    table.ForeignKey(
                        name: "FK_LiveAssistHolders_AspNetUsers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LiveAssistHolders_Live_LiveId",
                        column: x => x.LiveId,
                        principalTable: "Live",
                        principalColumn: "LiveId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "LiveAssists",
                columns: table => new
                {
                    AssistId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LiveId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveAssists", x => x.AssistId);
                    table.ForeignKey(
                        name: "FK_LiveAssists_AspNetUsers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LiveAssists_Live_LiveId",
                        column: x => x.LiveId,
                        principalTable: "Live",
                        principalColumn: "LiveId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "LiveGoalHolders",
                columns: table => new
                {
                    GoalsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LiveId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveGoalHolders", x => x.GoalsId);
                    table.ForeignKey(
                        name: "FK_LiveGoalHolders_AspNetUsers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LiveGoalHolders_Live_LiveId",
                        column: x => x.LiveId,
                        principalTable: "Live",
                        principalColumn: "LiveId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "LiveGoals",
                columns: table => new
                {
                    GoalsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LiveId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveGoals", x => x.GoalsId);
                    table.ForeignKey(
                        name: "FK_LiveGoals_AspNetUsers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LiveGoals_Live_LiveId",
                        column: x => x.LiveId,
                        principalTable: "Live",
                        principalColumn: "LiveId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Penalties",
                columns: table => new
                {
                    PenaltyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LiveId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Penalties", x => x.PenaltyId);
                    table.ForeignKey(
                        name: "FK_Penalties_Live_LiveId",
                        column: x => x.LiveId,
                        principalTable: "Live",
                        principalColumn: "LiveId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "RedCards",
                columns: table => new
                {
                    RedCardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LiveId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedCards", x => x.RedCardId);
                    table.ForeignKey(
                        name: "FK_RedCards_AspNetUsers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RedCards_Live_LiveId",
                        column: x => x.LiveId,
                        principalTable: "Live",
                        principalColumn: "LiveId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "YellowCards",
                columns: table => new
                {
                    YellowCardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LiveId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YellowCards", x => x.YellowCardId);
                    table.ForeignKey(
                        name: "FK_YellowCards_AspNetUsers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_YellowCards_Live_LiveId",
                        column: x => x.LiveId,
                        principalTable: "Live",
                        principalColumn: "LiveId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LiveAssistHolders_LiveId",
                table: "LiveAssistHolders",
                column: "LiveId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveAssistHolders_PlayerId",
                table: "LiveAssistHolders",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveAssists_LiveId",
                table: "LiveAssists",
                column: "LiveId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveAssists_PlayerId",
                table: "LiveAssists",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveGoalHolders_LiveId",
                table: "LiveGoalHolders",
                column: "LiveId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveGoalHolders_PlayerId",
                table: "LiveGoalHolders",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveGoals_LiveId",
                table: "LiveGoals",
                column: "LiveId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveGoals_PlayerId",
                table: "LiveGoals",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Penalties_LiveId",
                table: "Penalties",
                column: "LiveId");

            migrationBuilder.CreateIndex(
                name: "IX_RedCards_LiveId",
                table: "RedCards",
                column: "LiveId");

            migrationBuilder.CreateIndex(
                name: "IX_RedCards_PlayerId",
                table: "RedCards",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_YellowCards_LiveId",
                table: "YellowCards",
                column: "LiveId");

            migrationBuilder.CreateIndex(
                name: "IX_YellowCards_PlayerId",
                table: "YellowCards",
                column: "PlayerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LiveAssistHolders");

            migrationBuilder.DropTable(
                name: "LiveAssists");

            migrationBuilder.DropTable(
                name: "LiveGoalHolders");

            migrationBuilder.DropTable(
                name: "LiveGoals");

            migrationBuilder.DropTable(
                name: "Penalties");

            migrationBuilder.DropTable(
                name: "RedCards");

            migrationBuilder.DropTable(
                name: "YellowCards");

            migrationBuilder.DropTable(
                name: "Live");
        }
    }
}
