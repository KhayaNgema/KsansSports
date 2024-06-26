using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMarketValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarketValue",
                table: "PlayerTransferMarket");

            migrationBuilder.DropColumn(
               name: "MarketId",
               table: "PlayerTransferMarket");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MarketValue",
                table: "PlayerTransferMarket",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
