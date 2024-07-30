using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class ChangeToOneBaseModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "LiveRedCardHolders");

            migrationBuilder.DropTable(
                name: "LiveYellowCardHolders");

            migrationBuilder.DropTable(
                name: "Penalties");

            migrationBuilder.DropTable(
                name: "RedCards");

            migrationBuilder.DropTable(
                name: "Substitutes");

            migrationBuilder.DropTable(
                name: "YellowCards");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "Live",
                type: "nvarchar(34)",
                maxLength: 34,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13);

            migrationBuilder.AddColumn<string>(
                name: "AssistedById",
                table: "Live",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InPlayerId",
                table: "Live",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LiveAssistHolder_AssistedById",
                table: "Live",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LiveGoalHolder_ScoreById",
                table: "Live",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LiveGoalHolder_ScoredTime",
                table: "Live",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OutPlayerId",
                table: "Live",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PenaltyTime",
                table: "Live",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlayerId",
                table: "Live",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RedCardTime",
                table: "Live",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RedCardTime1",
                table: "Live",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RedCard_RedCommitedById",
                table: "Live",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RedCommitedById",
                table: "Live",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScoreById",
                table: "Live",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScoredById",
                table: "Live",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScoredTime",
                table: "Live",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubTime",
                table: "Live",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Live",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YellowCardTime",
                table: "Live",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YellowCardTime1",
                table: "Live",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YellowCard_YellowCommitedById",
                table: "Live",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YellowCommitedById",
                table: "Live",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Live_AssistedById",
                table: "Live",
                column: "AssistedById");

            migrationBuilder.CreateIndex(
                name: "IX_Live_InPlayerId",
                table: "Live",
                column: "InPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Live_LiveAssistHolder_AssistedById",
                table: "Live",
                column: "LiveAssistHolder_AssistedById");

            migrationBuilder.CreateIndex(
                name: "IX_Live_LiveGoalHolder_ScoreById",
                table: "Live",
                column: "LiveGoalHolder_ScoreById");

            migrationBuilder.CreateIndex(
                name: "IX_Live_OutPlayerId",
                table: "Live",
                column: "OutPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Live_PlayerId",
                table: "Live",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Live_RedCard_RedCommitedById",
                table: "Live",
                column: "RedCard_RedCommitedById");

            migrationBuilder.CreateIndex(
                name: "IX_Live_RedCommitedById",
                table: "Live",
                column: "RedCommitedById");

            migrationBuilder.CreateIndex(
                name: "IX_Live_ScoreById",
                table: "Live",
                column: "ScoreById");

            migrationBuilder.CreateIndex(
                name: "IX_Live_YellowCard_YellowCommitedById",
                table: "Live",
                column: "YellowCard_YellowCommitedById");

            migrationBuilder.CreateIndex(
                name: "IX_Live_YellowCommitedById",
                table: "Live",
                column: "YellowCommitedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Live_AspNetUsers_AssistedById",
                table: "Live",
                column: "AssistedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Live_AspNetUsers_InPlayerId",
                table: "Live",
                column: "InPlayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Live_AspNetUsers_LiveAssistHolder_AssistedById",
                table: "Live",
                column: "LiveAssistHolder_AssistedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Live_AspNetUsers_LiveGoalHolder_ScoreById",
                table: "Live",
                column: "LiveGoalHolder_ScoreById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Live_AspNetUsers_OutPlayerId",
                table: "Live",
                column: "OutPlayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Live_AspNetUsers_PlayerId",
                table: "Live",
                column: "PlayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Live_AspNetUsers_RedCard_RedCommitedById",
                table: "Live",
                column: "RedCard_RedCommitedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Live_AspNetUsers_RedCommitedById",
                table: "Live",
                column: "RedCommitedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Live_AspNetUsers_ScoreById",
                table: "Live",
                column: "ScoreById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Live_AspNetUsers_YellowCard_YellowCommitedById",
                table: "Live",
                column: "YellowCard_YellowCommitedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Live_AspNetUsers_YellowCommitedById",
                table: "Live",
                column: "YellowCommitedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Live_AspNetUsers_AssistedById",
                table: "Live");

            migrationBuilder.DropForeignKey(
                name: "FK_Live_AspNetUsers_InPlayerId",
                table: "Live");

            migrationBuilder.DropForeignKey(
                name: "FK_Live_AspNetUsers_LiveAssistHolder_AssistedById",
                table: "Live");

            migrationBuilder.DropForeignKey(
                name: "FK_Live_AspNetUsers_LiveGoalHolder_ScoreById",
                table: "Live");

            migrationBuilder.DropForeignKey(
                name: "FK_Live_AspNetUsers_OutPlayerId",
                table: "Live");

            migrationBuilder.DropForeignKey(
                name: "FK_Live_AspNetUsers_PlayerId",
                table: "Live");

            migrationBuilder.DropForeignKey(
                name: "FK_Live_AspNetUsers_RedCard_RedCommitedById",
                table: "Live");

            migrationBuilder.DropForeignKey(
                name: "FK_Live_AspNetUsers_RedCommitedById",
                table: "Live");

            migrationBuilder.DropForeignKey(
                name: "FK_Live_AspNetUsers_ScoreById",
                table: "Live");

            migrationBuilder.DropForeignKey(
                name: "FK_Live_AspNetUsers_YellowCard_YellowCommitedById",
                table: "Live");

            migrationBuilder.DropForeignKey(
                name: "FK_Live_AspNetUsers_YellowCommitedById",
                table: "Live");

            migrationBuilder.DropIndex(
                name: "IX_Live_AssistedById",
                table: "Live");

            migrationBuilder.DropIndex(
                name: "IX_Live_InPlayerId",
                table: "Live");

            migrationBuilder.DropIndex(
                name: "IX_Live_LiveAssistHolder_AssistedById",
                table: "Live");

            migrationBuilder.DropIndex(
                name: "IX_Live_LiveGoalHolder_ScoreById",
                table: "Live");

            migrationBuilder.DropIndex(
                name: "IX_Live_OutPlayerId",
                table: "Live");

            migrationBuilder.DropIndex(
                name: "IX_Live_PlayerId",
                table: "Live");

            migrationBuilder.DropIndex(
                name: "IX_Live_RedCard_RedCommitedById",
                table: "Live");

            migrationBuilder.DropIndex(
                name: "IX_Live_RedCommitedById",
                table: "Live");

            migrationBuilder.DropIndex(
                name: "IX_Live_ScoreById",
                table: "Live");

            migrationBuilder.DropIndex(
                name: "IX_Live_YellowCard_YellowCommitedById",
                table: "Live");

            migrationBuilder.DropIndex(
                name: "IX_Live_YellowCommitedById",
                table: "Live");

            migrationBuilder.DropColumn(
                name: "AssistedById",
                table: "Live");

            migrationBuilder.DropColumn(
                name: "InPlayerId",
                table: "Live");

            migrationBuilder.DropColumn(
                name: "LiveAssistHolder_AssistedById",
                table: "Live");

            migrationBuilder.DropColumn(
                name: "LiveGoalHolder_ScoreById",
                table: "Live");

            migrationBuilder.DropColumn(
                name: "LiveGoalHolder_ScoredTime",
                table: "Live");

            migrationBuilder.DropColumn(
                name: "OutPlayerId",
                table: "Live");

            migrationBuilder.DropColumn(
                name: "PenaltyTime",
                table: "Live");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "Live");

            migrationBuilder.DropColumn(
                name: "RedCardTime",
                table: "Live");

            migrationBuilder.DropColumn(
                name: "RedCardTime1",
                table: "Live");

            migrationBuilder.DropColumn(
                name: "RedCard_RedCommitedById",
                table: "Live");

            migrationBuilder.DropColumn(
                name: "RedCommitedById",
                table: "Live");

            migrationBuilder.DropColumn(
                name: "ScoreById",
                table: "Live");

            migrationBuilder.DropColumn(
                name: "ScoredById",
                table: "Live");

            migrationBuilder.DropColumn(
                name: "ScoredTime",
                table: "Live");

            migrationBuilder.DropColumn(
                name: "SubTime",
                table: "Live");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Live");

            migrationBuilder.DropColumn(
                name: "YellowCardTime",
                table: "Live");

            migrationBuilder.DropColumn(
                name: "YellowCardTime1",
                table: "Live");

            migrationBuilder.DropColumn(
                name: "YellowCard_YellowCommitedById",
                table: "Live");

            migrationBuilder.DropColumn(
                name: "YellowCommitedById",
                table: "Live");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "Live",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(34)",
                oldMaxLength: 34);

            migrationBuilder.CreateTable(
                name: "LiveAssistHolders",
                columns: table => new
                {
                    AssistId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeagueId = table.Column<int>(type: "int", nullable: false),
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
                        name: "FK_LiveAssistHolders_League_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "League",
                        principalColumn: "LeagueId",
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
                    LeagueId = table.Column<int>(type: "int", nullable: false),
                    LiveId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false)
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
                        name: "FK_LiveAssists_League_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "League",
                        principalColumn: "LeagueId",
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
                    LeagueId = table.Column<int>(type: "int", nullable: false),
                    LiveId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ScoredTime = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                        name: "FK_LiveGoalHolders_League_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "League",
                        principalColumn: "LeagueId",
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
                    LeagueId = table.Column<int>(type: "int", nullable: false),
                    LiveId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false)
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
                        name: "FK_LiveGoals_League_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "League",
                        principalColumn: "LeagueId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LiveGoals_Live_LiveId",
                        column: x => x.LiveId,
                        principalTable: "Live",
                        principalColumn: "LiveId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "LiveRedCardHolders",
                columns: table => new
                {
                    RedCardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeagueId = table.Column<int>(type: "int", nullable: false),
                    LiveId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CardTime = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveRedCardHolders", x => x.RedCardId);
                    table.ForeignKey(
                        name: "FK_LiveRedCardHolders_AspNetUsers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LiveRedCardHolders_League_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "League",
                        principalColumn: "LeagueId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LiveRedCardHolders_Live_LiveId",
                        column: x => x.LiveId,
                        principalTable: "Live",
                        principalColumn: "LiveId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "LiveYellowCardHolders",
                columns: table => new
                {
                    YellowCardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeagueId = table.Column<int>(type: "int", nullable: false),
                    LiveId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CardTime = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveYellowCardHolders", x => x.YellowCardId);
                    table.ForeignKey(
                        name: "FK_LiveYellowCardHolders_AspNetUsers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LiveYellowCardHolders_League_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "League",
                        principalColumn: "LeagueId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LiveYellowCardHolders_Live_LiveId",
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
                    LeagueId = table.Column<int>(type: "int", nullable: false),
                    LiveId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    PenaltyTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Penalties", x => x.PenaltyId);
                    table.ForeignKey(
                        name: "FK_Penalties_AspNetUsers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Penalties_League_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "League",
                        principalColumn: "LeagueId",
                        onDelete: ReferentialAction.NoAction);
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
                    LeagueId = table.Column<int>(type: "int", nullable: false),
                    LiveId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CardTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false)
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
                        name: "FK_RedCards_League_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "League",
                        principalColumn: "LeagueId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RedCards_Live_LiveId",
                        column: x => x.LiveId,
                        principalTable: "Live",
                        principalColumn: "LiveId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Substitutes",
                columns: table => new
                {
                    SubstituteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InPlayerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LeagueId = table.Column<int>(type: "int", nullable: false),
                    LiveId = table.Column<int>(type: "int", nullable: false),
                    OutPlayerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    SubTime = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Substitutes", x => x.SubstituteId);
                    table.ForeignKey(
                        name: "FK_Substitutes_AspNetUsers_InPlayerId",
                        column: x => x.InPlayerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Substitutes_AspNetUsers_OutPlayerId",
                        column: x => x.OutPlayerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Substitutes_League_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "League",
                        principalColumn: "LeagueId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Substitutes_Live_LiveId",
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
                    LeagueId = table.Column<int>(type: "int", nullable: false),
                    LiveId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CardTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: false)
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
                        name: "FK_YellowCards_League_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "League",
                        principalColumn: "LeagueId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_YellowCards_Live_LiveId",
                        column: x => x.LiveId,
                        principalTable: "Live",
                        principalColumn: "LiveId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LiveAssistHolders_LeagueId",
                table: "LiveAssistHolders",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveAssistHolders_LiveId",
                table: "LiveAssistHolders",
                column: "LiveId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveAssistHolders_PlayerId",
                table: "LiveAssistHolders",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveAssists_LeagueId",
                table: "LiveAssists",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveAssists_LiveId",
                table: "LiveAssists",
                column: "LiveId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveAssists_PlayerId",
                table: "LiveAssists",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveGoalHolders_LeagueId",
                table: "LiveGoalHolders",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveGoalHolders_LiveId",
                table: "LiveGoalHolders",
                column: "LiveId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveGoalHolders_PlayerId",
                table: "LiveGoalHolders",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveGoals_LeagueId",
                table: "LiveGoals",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveGoals_LiveId",
                table: "LiveGoals",
                column: "LiveId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveGoals_PlayerId",
                table: "LiveGoals",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveRedCardHolders_LeagueId",
                table: "LiveRedCardHolders",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveRedCardHolders_LiveId",
                table: "LiveRedCardHolders",
                column: "LiveId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveRedCardHolders_PlayerId",
                table: "LiveRedCardHolders",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveYellowCardHolders_LeagueId",
                table: "LiveYellowCardHolders",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveYellowCardHolders_LiveId",
                table: "LiveYellowCardHolders",
                column: "LiveId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveYellowCardHolders_PlayerId",
                table: "LiveYellowCardHolders",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Penalties_LeagueId",
                table: "Penalties",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_Penalties_LiveId",
                table: "Penalties",
                column: "LiveId");

            migrationBuilder.CreateIndex(
                name: "IX_Penalties_PlayerId",
                table: "Penalties",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_RedCards_LeagueId",
                table: "RedCards",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_RedCards_LiveId",
                table: "RedCards",
                column: "LiveId");

            migrationBuilder.CreateIndex(
                name: "IX_RedCards_PlayerId",
                table: "RedCards",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Substitutes_InPlayerId",
                table: "Substitutes",
                column: "InPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Substitutes_LeagueId",
                table: "Substitutes",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_Substitutes_LiveId",
                table: "Substitutes",
                column: "LiveId");

            migrationBuilder.CreateIndex(
                name: "IX_Substitutes_OutPlayerId",
                table: "Substitutes",
                column: "OutPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_YellowCards_LeagueId",
                table: "YellowCards",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_YellowCards_LiveId",
                table: "YellowCards",
                column: "LiveId");

            migrationBuilder.CreateIndex(
                name: "IX_YellowCards_PlayerId",
                table: "YellowCards",
                column: "PlayerId");
        }
    }
}
