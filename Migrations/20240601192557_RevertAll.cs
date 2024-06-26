using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class RevertAll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfer_PlayerTransferMarket_PlayerTransferMarketTransferMarketId",
                table: "Transfer");

            migrationBuilder.DropColumn(
                name: "PlayerMarketId",
                table: "Transfer");

            migrationBuilder.RenameColumn(
                name: "PlayerTransferMarketTransferMarketId",
                table: "Transfer",
                newName: "PlayerTransferMarketId");

            migrationBuilder.RenameIndex(
                name: "IX_Transfer_PlayerTransferMarketTransferMarketId",
                table: "Transfer",
                newName: "IX_Transfer_PlayerTransferMarketId");

            migrationBuilder.RenameColumn(
                name: "TransferMarketId",
                table: "PlayerTransferMarket",
                newName: "PlayerTransferMarketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfer_PlayerTransferMarket_PlayerTransferMarketId",
                table: "Transfer",
                column: "PlayerTransferMarketId",
                principalTable: "PlayerTransferMarket",
                principalColumn: "PlayerTransferMarketId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfer_PlayerTransferMarket_PlayerTransferMarketId",
                table: "Transfer");

            migrationBuilder.RenameColumn(
                name: "PlayerTransferMarketId",
                table: "Transfer",
                newName: "PlayerTransferMarketTransferMarketId");

            migrationBuilder.RenameIndex(
                name: "IX_Transfer_PlayerTransferMarketId",
                table: "Transfer",
                newName: "IX_Transfer_PlayerTransferMarketTransferMarketId");

            migrationBuilder.RenameColumn(
                name: "PlayerTransferMarketId",
                table: "PlayerTransferMarket",
                newName: "TransferMarketId");

            migrationBuilder.AddColumn<int>(
                name: "PlayerMarketId",
                table: "Transfer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfer_PlayerTransferMarket_PlayerTransferMarketTransferMarketId",
                table: "Transfer",
                column: "PlayerTransferMarketTransferMarketId",
                principalTable: "PlayerTransferMarket",
                principalColumn: "TransferMarketId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
