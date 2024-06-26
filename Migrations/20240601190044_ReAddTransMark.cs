using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class ReAddTransMark : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlayerMarketId",
                table: "Transfer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlayerTransferMarketTransferMarketId",
                table: "Transfer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PlayerTransferMarket",
                columns: table => new
                {
                    TransferMarketId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClubId = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SellingPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SaleStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerTransferMarket", x => x.TransferMarketId);
                    table.ForeignKey(
                        name: "FK_PlayerTransferMarket_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PlayerTransferMarket_AspNetUsers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PlayerTransferMarket_Club_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Club",
                        principalColumn: "ClubId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transfer_PlayerTransferMarketTransferMarketId",
                table: "Transfer",
                column: "PlayerTransferMarketTransferMarketId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTransferMarket_ClubId",
                table: "PlayerTransferMarket",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTransferMarket_CreatedById",
                table: "PlayerTransferMarket",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTransferMarket_PlayerId",
                table: "PlayerTransferMarket",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfer_PlayerTransferMarket_PlayerTransferMarketTransferMarketId",
                table: "Transfer",
                column: "PlayerTransferMarketTransferMarketId",
                principalTable: "PlayerTransferMarket",
                principalColumn: "TransferMarketId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfer_PlayerTransferMarket_PlayerTransferMarketTransferMarketId",
                table: "Transfer");

            migrationBuilder.DropTable(
                name: "PlayerTransferMarket");

            migrationBuilder.DropIndex(
                name: "IX_Transfer_PlayerTransferMarketTransferMarketId",
                table: "Transfer");

            migrationBuilder.DropColumn(
                name: "PlayerMarketId",
                table: "Transfer");

            migrationBuilder.DropColumn(
                name: "PlayerTransferMarketTransferMarketId",
                table: "Transfer");
        }
    }
}
