using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class AddLeeeeee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LeagueId",
                table: "PlayerTransferMarket",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTransferMarket_LeagueId",
                table: "PlayerTransferMarket",
                column: "LeagueId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerTransferMarket_League_LeagueId",
                table: "PlayerTransferMarket",
                column: "LeagueId",
                principalTable: "League",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerTransferMarket_League_LeagueId",
                table: "PlayerTransferMarket");

            migrationBuilder.DropIndex(
                name: "IX_PlayerTransferMarket_LeagueId",
                table: "PlayerTransferMarket");

            migrationBuilder.DropColumn(
                name: "LeagueId",
                table: "PlayerTransferMarket");
        }
    }
}
