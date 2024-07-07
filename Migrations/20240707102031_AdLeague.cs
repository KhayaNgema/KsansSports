using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class AdLeague : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LeagueId",
                table: "Transfer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transfer_LeagueId",
                table: "Transfer",
                column: "LeagueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfer_League_LeagueId",
                table: "Transfer",
                column: "LeagueId",
                principalTable: "League",
                principalColumn: "LeagueId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfer_League_LeagueId",
                table: "Transfer");

            migrationBuilder.DropIndex(
                name: "IX_Transfer_LeagueId",
                table: "Transfer");

            migrationBuilder.DropColumn(
                name: "LeagueId",
                table: "Transfer");
        }
    }
}
