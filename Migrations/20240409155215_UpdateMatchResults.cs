using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMatchResults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FixtureId",
                table: "MatchResult",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MatchResult_FixtureId",
                table: "MatchResult",
                column: "FixtureId");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchResult_Fixture_FixtureId",
                table: "MatchResult",
                column: "FixtureId",
                principalTable: "Fixture",
                principalColumn: "FixtureId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchResult_Fixture_FixtureId",
                table: "MatchResult");

            migrationBuilder.DropIndex(
                name: "IX_MatchResult_FixtureId",
                table: "MatchResult");

            migrationBuilder.DropColumn(
                name: "FixtureId",
                table: "MatchResult");
        }
    }
}
