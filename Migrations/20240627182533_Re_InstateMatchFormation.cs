using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class Re_InstateMatchFormation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "MatchFormation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "MatchFormation");
        }
    }
}
