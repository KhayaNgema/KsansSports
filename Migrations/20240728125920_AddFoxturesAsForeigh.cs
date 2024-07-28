using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class AddFoxturesAsForeigh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Live_FixtureId",
                table: "Live",
                column: "FixtureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Live_Fixture_FixtureId",
                table: "Live",
                column: "FixtureId",
                principalTable: "Fixture",
                principalColumn: "FixtureId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Live_Fixture_FixtureId",
                table: "Live");

            migrationBuilder.DropIndex(
                name: "IX_Live_FixtureId",
                table: "Live");
        }
    }
}
