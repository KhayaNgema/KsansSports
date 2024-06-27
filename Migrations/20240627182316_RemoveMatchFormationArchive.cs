using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMatchFormationArchive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "MatchFormation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "MatchFormation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
