using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreLiveTimeOnCards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LiveTime",
                table: "Substitutes",
                newName: "SubTime");

            migrationBuilder.AddColumn<string>(
                name: "CardTime",
                table: "YellowCards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CardTime",
                table: "RedCards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PenaltyTime",
                table: "Penalties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CardTime",
                table: "LiveYellowCardHolders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CardTime",
                table: "LiveRedCardHolders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardTime",
                table: "YellowCards");

            migrationBuilder.DropColumn(
                name: "CardTime",
                table: "RedCards");

            migrationBuilder.DropColumn(
                name: "PenaltyTime",
                table: "Penalties");

            migrationBuilder.DropColumn(
                name: "CardTime",
                table: "LiveYellowCardHolders");

            migrationBuilder.DropColumn(
                name: "CardTime",
                table: "LiveRedCardHolders");

            migrationBuilder.RenameColumn(
                name: "SubTime",
                table: "Substitutes",
                newName: "LiveTime");
        }
    }
}
