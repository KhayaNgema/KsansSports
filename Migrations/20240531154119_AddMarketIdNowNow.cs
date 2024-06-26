﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class AddMarketIdNowNow : Migration
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
                name: "PlayerMarketMarketId",
                table: "Transfer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transfer_PlayerMarketMarketId",
                table: "Transfer",
                column: "PlayerMarketMarketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfer_PlayerTransferMarket_PlayerMarketMarketId",
                table: "Transfer",
                column: "PlayerMarketMarketId",
                principalTable: "PlayerTransferMarket",
                principalColumn: "MarketId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfer_PlayerTransferMarket_PlayerMarketMarketId",
                table: "Transfer");

            migrationBuilder.DropIndex(
                name: "IX_Transfer_PlayerMarketMarketId",
                table: "Transfer");

            migrationBuilder.DropColumn(
                name: "MarketId",
                table: "Transfer");

            migrationBuilder.DropColumn(
                name: "PlayerMarketMarketId",
                table: "Transfer");
        }
    }
}
