using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class Chhange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HeadToHead",
                table: "HeadToHead",
                newName: "HomeTeam");

            migrationBuilder.RenameColumn(
                name: "GoalsScored",
                table: "HeadToHead",
                newName: "HomeTeamGoals");

            migrationBuilder.RenameColumn(
                name: "GoalsConceded",
                table: "HeadToHead",
                newName: "AwayTeamGoals");

            migrationBuilder.AddColumn<string>(
                name: "AwayTeam",
                table: "HeadToHead",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwayTeam",
                table: "HeadToHead");

            migrationBuilder.RenameColumn(
                name: "HomeTeamGoals",
                table: "HeadToHead",
                newName: "GoalsScored");

            migrationBuilder.RenameColumn(
                name: "HomeTeam",
                table: "HeadToHead",
                newName: "HeadToHead");

            migrationBuilder.RenameColumn(
                name: "AwayTeamGoals",
                table: "HeadToHead",
                newName: "GoalsConceded");
        }
    }
}
