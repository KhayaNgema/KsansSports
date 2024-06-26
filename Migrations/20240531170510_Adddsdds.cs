using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class Adddsdds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MarketId",
                table: "Transfer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlayerTransferMarketMarketId",
                table: "Transfer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transfer_PlayerTransferMarketMarketId",
                table: "Transfer",
                column: "PlayerTransferMarketMarketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfer_PlayerTransferMarket_PlayerTransferMarketMarketId",
                table: "Transfer",
                column: "PlayerTransferMarketMarketId",
                principalTable: "PlayerTransferMarket",
                principalColumn: "MarketId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfer_PlayerTransferMarket_PlayerTransferMarketMarketId",
                table: "Transfer");

            migrationBuilder.DropIndex(
                name: "IX_Transfer_PlayerTransferMarketMarketId",
                table: "Transfer");

            migrationBuilder.DropColumn(
                name: "MarketId",
                table: "Transfer");

            migrationBuilder.DropColumn(
                name: "PlayerTransferMarketMarketId",
                table: "Transfer");
        }
    }
}
