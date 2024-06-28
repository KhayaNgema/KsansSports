using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreRRRRR : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResultsRate",
                table: "Reports",
                newName: "WinningRate");

            migrationBuilder.RenameColumn(
                name: "MatchesRate",
                table: "Reports",
                newName: "UnreleasedMatchesRate");

            migrationBuilder.AddColumn<decimal>(
                name: "DrawingRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FixturedMatchesRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "InterruptedMatchesRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "LosingRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PlayedMatchesRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PostponedMatchesRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ReleasedResultsRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UnfixturedMatchesRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DrawingRate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "FixturedMatchesRate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "InterruptedMatchesRate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "LosingRate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "PlayedMatchesRate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "PostponedMatchesRate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ReleasedResultsRate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "UnfixturedMatchesRate",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "WinningRate",
                table: "Reports",
                newName: "ResultsRate");

            migrationBuilder.RenameColumn(
                name: "UnreleasedMatchesRate",
                table: "Reports",
                newName: "MatchesRate");
        }
    }
}
