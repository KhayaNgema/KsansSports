using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class Fixxxxthething : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GamesToPlayRate",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "GamesToPlayCOunt",
                table: "Reports",
                newName: "GamesToPlayCount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GamesToPlayCount",
                table: "Reports",
                newName: "GamesToPlayCOunt");

            migrationBuilder.AddColumn<decimal>(
                name: "GamesToPlayRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
