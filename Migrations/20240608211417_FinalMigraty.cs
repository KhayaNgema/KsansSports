using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class FinalMigraty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BrowserVersion",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OSName",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OSVersion",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrowserVersion",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "OSName",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "OSVersion",
                table: "DeviceInfo");
        }
    }
}
