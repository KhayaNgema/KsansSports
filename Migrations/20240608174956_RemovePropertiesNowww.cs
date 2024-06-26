using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class RemovePropertiesNowww : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceOrientation",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "DevicePixelRatio",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "DeviceType",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "ScreenResolution",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "Timezone",
                table: "DeviceInfo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeviceOrientation",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DevicePixelRatio",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceType",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScreenResolution",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Timezone",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
