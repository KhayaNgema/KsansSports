using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class AddSubsandLiveCardsHolders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LiveRedCardHolders",
                columns: table => new
                {
                    RedCardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LiveId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                    LiveId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                        name: "FK_LiveYellowCardHolders_Live_LiveId",
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
                    LiveId = table.Column<int>(type: "int", nullable: false),
                    OutPlayerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InPlayerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LiveTime = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                        name: "FK_Substitutes_Live_LiveId",
                        column: x => x.LiveId,
                        principalTable: "Live",
                        principalColumn: "LiveId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LiveRedCardHolders_LiveId",
                table: "LiveRedCardHolders",
                column: "LiveId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveRedCardHolders_PlayerId",
                table: "LiveRedCardHolders",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveYellowCardHolders_LiveId",
                table: "LiveYellowCardHolders",
                column: "LiveId");

            migrationBuilder.CreateIndex(
                name: "IX_LiveYellowCardHolders_PlayerId",
                table: "LiveYellowCardHolders",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Substitutes_InPlayerId",
                table: "Substitutes",
                column: "InPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Substitutes_LiveId",
                table: "Substitutes",
                column: "LiveId");

            migrationBuilder.CreateIndex(
                name: "IX_Substitutes_OutPlayerId",
                table: "Substitutes",
                column: "OutPlayerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LiveRedCardHolders");

            migrationBuilder.DropTable(
                name: "LiveYellowCardHolders");

            migrationBuilder.DropTable(
                name: "Substitutes");
        }
    }
}
