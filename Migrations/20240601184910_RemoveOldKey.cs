using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOldKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfer_PlayerTransferMarket_PlayerTransferMarketMarketId",
                table: "Transfer");

            migrationBuilder.RenameColumn(
                name: "PlayerTransferMarketMarketId",
                table: "Transfer",
                newName: "PlayerTransferMarketTransferMarketId");

            migrationBuilder.RenameIndex(
                name: "IX_Transfer_PlayerTransferMarketMarketId",
                table: "Transfer",
                newName: "IX_Transfer_PlayerTransferMarketTransferMarketId");

            migrationBuilder.RenameColumn(
                name: "MarketId",
                table: "PlayerTransferMarket",
                newName: "TransferMarketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfer_PlayerTransferMarket_PlayerTransferMarketTransferMarketId",
                table: "Transfer",
                column: "PlayerTransferMarketTransferMarketId",
                principalTable: "PlayerTransferMarket",
                principalColumn: "TransferMarketId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfer_PlayerTransferMarket_PlayerTransferMarketTransferMarketId",
                table: "Transfer");

            migrationBuilder.RenameColumn(
                name: "PlayerTransferMarketTransferMarketId",
                table: "Transfer",
                newName: "PlayerTransferMarketMarketId");

            migrationBuilder.RenameIndex(
                name: "IX_Transfer_PlayerTransferMarketTransferMarketId",
                table: "Transfer",
                newName: "IX_Transfer_PlayerTransferMarketMarketId");

            migrationBuilder.RenameColumn(
                name: "TransferMarketId",
                table: "PlayerTransferMarket",
                newName: "MarketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfer_PlayerTransferMarket_PlayerTransferMarketMarketId",
                table: "Transfer",
                column: "PlayerTransferMarketMarketId",
                principalTable: "PlayerTransferMarket",
                principalColumn: "MarketId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
