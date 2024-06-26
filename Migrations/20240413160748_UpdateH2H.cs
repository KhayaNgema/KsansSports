using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class UpdateH2H : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Draw",
                table: "HeadToHead");

            migrationBuilder.DropColumn(
                name: "Lose",
                table: "HeadToHead");

            migrationBuilder.RenameColumn(
                name: "Win",
                table: "HeadToHead",
                newName: "MatchResults");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MatchResults",
                table: "HeadToHead",
                newName: "Win");

            migrationBuilder.AddColumn<string>(
                name: "Draw",
                table: "HeadToHead",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Lose",
                table: "HeadToHead",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
