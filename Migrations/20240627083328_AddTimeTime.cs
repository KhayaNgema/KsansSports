using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class AddTimeTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Fixture_KickOff",
                table: "Fixture");

            migrationBuilder.RenameColumn(
                name: "KickOff",
                table: "Fixture",
                newName: "KickOffTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "MatchTime",
                table: "MatchResult",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "KickOffDate",
                table: "Fixture",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Fixture_KickOffDate",
                table: "Fixture",
                column: "KickOffDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Fixture_KickOffDate",
                table: "Fixture");

            migrationBuilder.DropColumn(
                name: "MatchTime",
                table: "MatchResult");

            migrationBuilder.DropColumn(
                name: "KickOffDate",
                table: "Fixture");

            migrationBuilder.RenameColumn(
                name: "KickOffTime",
                table: "Fixture",
                newName: "KickOff");

            migrationBuilder.CreateIndex(
                name: "IX_Fixture_KickOff",
                table: "Fixture",
                column: "KickOff");
        }
    }
}
