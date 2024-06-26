using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class RemoveClubBagdes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwayTeamBadge",
                table: "MatchResult");

            migrationBuilder.DropColumn(
                name: "HomeTeamBadge",
                table: "MatchResult");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AwayTeamBadge",
                table: "MatchResult",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HomeTeamBadge",
                table: "MatchResult",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
