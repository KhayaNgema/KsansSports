using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class AddliveEventss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_YellowCards_AspNetUsers_YellowCommitedById",
                table: "YellowCards");

            migrationBuilder.DropForeignKey(
                name: "FK_YellowCards_League_LeagueId",
                table: "YellowCards");

            migrationBuilder.DropForeignKey(
                name: "FK_YellowCards_Live_LiveId",
                table: "YellowCards");

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

            migrationBuilder.DropPrimaryKey(
                name: "PK_YellowCards",
                table: "YellowCards");

            migrationBuilder.RenameTable(
                name: "YellowCards",
                newName: "LiveEvents");

            migrationBuilder.RenameIndex(
                name: "IX_YellowCards_YellowCommitedById",
                table: "LiveEvents",
                newName: "IX_LiveEvents_YellowCommitedById");

            migrationBuilder.RenameIndex(
                name: "IX_YellowCards_LiveId",
                table: "LiveEvents",
                newName: "IX_LiveEvents_LiveId");

            migrationBuilder.RenameIndex(
                name: "IX_YellowCards_LeagueId",
                table: "LiveEvents",
                newName: "IX_LiveEvents_LeagueId");

            migrationBuilder.AlterColumn<string>(
                name: "YellowCommitedById",
                table: "LiveEvents",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "YellowCardTime",
                table: "LiveEvents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "AssistedById",
                table: "LiveEvents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InPlayerId",
                table: "LiveEvents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LiveAssistHolder_AssistedById",
                table: "LiveEvents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LiveGoalHolder_ScoreById",
                table: "LiveEvents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LiveGoalHolder_ScoredTime",
                table: "LiveEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OutPlayerId",
                table: "LiveEvents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PenaltyTime",
                table: "LiveEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlayerId",
                table: "LiveEvents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RedCardTime",
                table: "LiveEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RedCardTime1",
                table: "LiveEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RedCard_RedCommitedById",
                table: "LiveEvents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RedCommitedById",
                table: "LiveEvents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScoreById",
                table: "LiveEvents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScoredById",
                table: "LiveEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScoredTime",
                table: "LiveEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubTime",
                table: "LiveEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "LiveEvents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YellowCardTime1",
                table: "LiveEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YellowCard_YellowCommitedById",
                table: "LiveEvents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LiveEvents",
                table: "LiveEvents",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveEvents_AssistedById",
                table: "LiveEvents",
                column: "AssistedById");

            migrationBuilder.CreateIndex(
                name: "IX_LiveEvents_InPlayerId",
                table: "LiveEvents",
                column: "InPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveEvents_LiveAssistHolder_AssistedById",
                table: "LiveEvents",
                column: "LiveAssistHolder_AssistedById");

            migrationBuilder.CreateIndex(
                name: "IX_LiveEvents_LiveGoalHolder_ScoreById",
                table: "LiveEvents",
                column: "LiveGoalHolder_ScoreById");

            migrationBuilder.CreateIndex(
                name: "IX_LiveEvents_OutPlayerId",
                table: "LiveEvents",
                column: "OutPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveEvents_PlayerId",
                table: "LiveEvents",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveEvents_RedCard_RedCommitedById",
                table: "LiveEvents",
                column: "RedCard_RedCommitedById");

            migrationBuilder.CreateIndex(
                name: "IX_LiveEvents_RedCommitedById",
                table: "LiveEvents",
                column: "RedCommitedById");

            migrationBuilder.CreateIndex(
                name: "IX_LiveEvents_ScoreById",
                table: "LiveEvents",
                column: "ScoreById");

            migrationBuilder.CreateIndex(
                name: "IX_LiveEvents_YellowCard_YellowCommitedById",
                table: "LiveEvents",
                column: "YellowCard_YellowCommitedById");

            migrationBuilder.AddForeignKey(
                name: "FK_LiveEvents_AspNetUsers_AssistedById",
                table: "LiveEvents",
                column: "AssistedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LiveEvents_AspNetUsers_InPlayerId",
                table: "LiveEvents",
                column: "InPlayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LiveEvents_AspNetUsers_LiveAssistHolder_AssistedById",
                table: "LiveEvents",
                column: "LiveAssistHolder_AssistedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LiveEvents_AspNetUsers_LiveGoalHolder_ScoreById",
                table: "LiveEvents",
                column: "LiveGoalHolder_ScoreById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LiveEvents_AspNetUsers_OutPlayerId",
                table: "LiveEvents",
                column: "OutPlayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LiveEvents_AspNetUsers_PlayerId",
                table: "LiveEvents",
                column: "PlayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LiveEvents_AspNetUsers_RedCard_RedCommitedById",
                table: "LiveEvents",
                column: "RedCard_RedCommitedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LiveEvents_AspNetUsers_RedCommitedById",
                table: "LiveEvents",
                column: "RedCommitedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LiveEvents_AspNetUsers_ScoreById",
                table: "LiveEvents",
                column: "ScoreById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LiveEvents_AspNetUsers_YellowCard_YellowCommitedById",
                table: "LiveEvents",
                column: "YellowCard_YellowCommitedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LiveEvents_AspNetUsers_YellowCommitedById",
                table: "LiveEvents",
                column: "YellowCommitedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LiveEvents_League_LeagueId",
                table: "LiveEvents",
                column: "LeagueId",
                principalTable: "League",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LiveEvents_Live_LiveId",
                table: "LiveEvents",
                column: "LiveId",
                principalTable: "Live",
                principalColumn: "LiveId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LiveEvents_AspNetUsers_AssistedById",
                table: "LiveEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_LiveEvents_AspNetUsers_InPlayerId",
                table: "LiveEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_LiveEvents_AspNetUsers_LiveAssistHolder_AssistedById",
                table: "LiveEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_LiveEvents_AspNetUsers_LiveGoalHolder_ScoreById",
                table: "LiveEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_LiveEvents_AspNetUsers_OutPlayerId",
                table: "LiveEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_LiveEvents_AspNetUsers_PlayerId",
                table: "LiveEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_LiveEvents_AspNetUsers_RedCard_RedCommitedById",
                table: "LiveEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_LiveEvents_AspNetUsers_RedCommitedById",
                table: "LiveEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_LiveEvents_AspNetUsers_ScoreById",
                table: "LiveEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_LiveEvents_AspNetUsers_YellowCard_YellowCommitedById",
                table: "LiveEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_LiveEvents_AspNetUsers_YellowCommitedById",
                table: "LiveEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_LiveEvents_League_LeagueId",
                table: "LiveEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_LiveEvents_Live_LiveId",
                table: "LiveEvents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LiveEvents",
                table: "LiveEvents");

            migrationBuilder.DropIndex(
                name: "IX_LiveEvents_AssistedById",
                table: "LiveEvents");

            migrationBuilder.DropIndex(
                name: "IX_LiveEvents_InPlayerId",
                table: "LiveEvents");

            migrationBuilder.DropIndex(
                name: "IX_LiveEvents_LiveAssistHolder_AssistedById",
                table: "LiveEvents");

            migrationBuilder.DropIndex(
                name: "IX_LiveEvents_LiveGoalHolder_ScoreById",
                table: "LiveEvents");

            migrationBuilder.DropIndex(
                name: "IX_LiveEvents_OutPlayerId",
                table: "LiveEvents");

            migrationBuilder.DropIndex(
                name: "IX_LiveEvents_PlayerId",
                table: "LiveEvents");

            migrationBuilder.DropIndex(
                name: "IX_LiveEvents_RedCard_RedCommitedById",
                table: "LiveEvents");

            migrationBuilder.DropIndex(
                name: "IX_LiveEvents_RedCommitedById",
                table: "LiveEvents");

            migrationBuilder.DropIndex(
                name: "IX_LiveEvents_ScoreById",
                table: "LiveEvents");

            migrationBuilder.DropIndex(
                name: "IX_LiveEvents_YellowCard_YellowCommitedById",
                table: "LiveEvents");

            migrationBuilder.DropColumn(
                name: "AssistedById",
                table: "LiveEvents");

            migrationBuilder.DropColumn(
                name: "InPlayerId",
                table: "LiveEvents");

            migrationBuilder.DropColumn(
                name: "LiveAssistHolder_AssistedById",
                table: "LiveEvents");

            migrationBuilder.DropColumn(
                name: "LiveGoalHolder_ScoreById",
                table: "LiveEvents");

            migrationBuilder.DropColumn(
                name: "LiveGoalHolder_ScoredTime",
                table: "LiveEvents");

            migrationBuilder.DropColumn(
                name: "OutPlayerId",
                table: "LiveEvents");

            migrationBuilder.DropColumn(
                name: "PenaltyTime",
                table: "LiveEvents");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "LiveEvents");

            migrationBuilder.DropColumn(
                name: "RedCardTime",
                table: "LiveEvents");

            migrationBuilder.DropColumn(
                name: "RedCardTime1",
                table: "LiveEvents");

            migrationBuilder.DropColumn(
                name: "RedCard_RedCommitedById",
                table: "LiveEvents");

            migrationBuilder.DropColumn(
                name: "RedCommitedById",
                table: "LiveEvents");

            migrationBuilder.DropColumn(
                name: "ScoreById",
                table: "LiveEvents");

            migrationBuilder.DropColumn(
                name: "ScoredById",
                table: "LiveEvents");

            migrationBuilder.DropColumn(
                name: "ScoredTime",
                table: "LiveEvents");

            migrationBuilder.DropColumn(
                name: "SubTime",
                table: "LiveEvents");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "LiveEvents");

            migrationBuilder.DropColumn(
                name: "YellowCardTime1",
                table: "LiveEvents");

            migrationBuilder.DropColumn(
                name: "YellowCard_YellowCommitedById",
                table: "LiveEvents");

            migrationBuilder.RenameTable(
                name: "LiveEvents",
                newName: "YellowCards");

            migrationBuilder.RenameIndex(
                name: "IX_LiveEvents_YellowCommitedById",
                table: "YellowCards",
                newName: "IX_YellowCards_YellowCommitedById");

            migrationBuilder.RenameIndex(
                name: "IX_LiveEvents_LiveId",
                table: "YellowCards",
                newName: "IX_YellowCards_LiveId");

            migrationBuilder.RenameIndex(
                name: "IX_LiveEvents_LeagueId",
                table: "YellowCards",
                newName: "IX_YellowCards_LeagueId");

            migrationBuilder.AlterColumn<string>(
                name: "YellowCommitedById",
                table: "YellowCards",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YellowCardTime",
                table: "YellowCards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_YellowCards",
                table: "YellowCards",
                column: "EventId");

            migrationBuilder.CreateTable(
                name: "LiveAssistHolders",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssistedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LeagueId = table.Column<int>(type: "int", nullable: false),
                    LiveId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveAssistHolders", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_LiveAssistHolders_AspNetUsers_AssistedById",
                        column: x => x.AssistedById,
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
                    EventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssistedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LeagueId = table.Column<int>(type: "int", nullable: false),
                    LiveId = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveAssists", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_LiveAssists_AspNetUsers_AssistedById",
                        column: x => x.AssistedById,
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
                    EventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeagueId = table.Column<int>(type: "int", nullable: false),
                    LiveId = table.Column<int>(type: "int", nullable: false),
                    ScoreById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ScoredById = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScoredTime = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveGoalHolders", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_LiveGoalHolders_AspNetUsers_ScoreById",
                        column: x => x.ScoreById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
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
                    EventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeagueId = table.Column<int>(type: "int", nullable: false),
                    LiveId = table.Column<int>(type: "int", nullable: false),
                    ScoreById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    ScoredTime = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveGoals", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_LiveGoals_AspNetUsers_ScoreById",
                        column: x => x.ScoreById,
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
                    EventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeagueId = table.Column<int>(type: "int", nullable: false),
                    LiveId = table.Column<int>(type: "int", nullable: false),
                    RedCommitedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RedCardTime = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveRedCardHolders", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_LiveRedCardHolders_AspNetUsers_RedCommitedById",
                        column: x => x.RedCommitedById,
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
                    EventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeagueId = table.Column<int>(type: "int", nullable: false),
                    LiveId = table.Column<int>(type: "int", nullable: false),
                    YellowCommitedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    YellowCardTime = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveYellowCardHolders", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_LiveYellowCardHolders_AspNetUsers_YellowCommitedById",
                        column: x => x.YellowCommitedById,
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
                    EventId = table.Column<int>(type: "int", nullable: false)
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
                    table.PrimaryKey("PK_Penalties", x => x.EventId);
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
                    EventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeagueId = table.Column<int>(type: "int", nullable: false),
                    LiveId = table.Column<int>(type: "int", nullable: false),
                    RedCommitedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    RedCardTime = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedCards", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_RedCards_AspNetUsers_RedCommitedById",
                        column: x => x.RedCommitedById,
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
                    EventId = table.Column<int>(type: "int", nullable: false)
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
                    table.PrimaryKey("PK_Substitutes", x => x.EventId);
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

            migrationBuilder.CreateIndex(
                name: "IX_LiveAssistHolders_AssistedById",
                table: "LiveAssistHolders",
                column: "AssistedById");

            migrationBuilder.CreateIndex(
                name: "IX_LiveAssistHolders_LeagueId",
                table: "LiveAssistHolders",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveAssistHolders_LiveId",
                table: "LiveAssistHolders",
                column: "LiveId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveAssists_AssistedById",
                table: "LiveAssists",
                column: "AssistedById");

            migrationBuilder.CreateIndex(
                name: "IX_LiveAssists_LeagueId",
                table: "LiveAssists",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveAssists_LiveId",
                table: "LiveAssists",
                column: "LiveId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveGoalHolders_LeagueId",
                table: "LiveGoalHolders",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveGoalHolders_LiveId",
                table: "LiveGoalHolders",
                column: "LiveId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveGoalHolders_ScoreById",
                table: "LiveGoalHolders",
                column: "ScoreById");

            migrationBuilder.CreateIndex(
                name: "IX_LiveGoals_LeagueId",
                table: "LiveGoals",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveGoals_LiveId",
                table: "LiveGoals",
                column: "LiveId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveGoals_ScoreById",
                table: "LiveGoals",
                column: "ScoreById");

            migrationBuilder.CreateIndex(
                name: "IX_LiveRedCardHolders_LeagueId",
                table: "LiveRedCardHolders",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveRedCardHolders_LiveId",
                table: "LiveRedCardHolders",
                column: "LiveId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveRedCardHolders_RedCommitedById",
                table: "LiveRedCardHolders",
                column: "RedCommitedById");

            migrationBuilder.CreateIndex(
                name: "IX_LiveYellowCardHolders_LeagueId",
                table: "LiveYellowCardHolders",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveYellowCardHolders_LiveId",
                table: "LiveYellowCardHolders",
                column: "LiveId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveYellowCardHolders_YellowCommitedById",
                table: "LiveYellowCardHolders",
                column: "YellowCommitedById");

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
                name: "IX_RedCards_RedCommitedById",
                table: "RedCards",
                column: "RedCommitedById");

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

            migrationBuilder.AddForeignKey(
                name: "FK_YellowCards_AspNetUsers_YellowCommitedById",
                table: "YellowCards",
                column: "YellowCommitedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_YellowCards_League_LeagueId",
                table: "YellowCards",
                column: "LeagueId",
                principalTable: "League",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_YellowCards_Live_LiveId",
                table: "YellowCards",
                column: "LiveId",
                principalTable: "Live",
                principalColumn: "LiveId",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
