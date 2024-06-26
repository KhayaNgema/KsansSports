using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMoreDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Browser",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "BrowserVersion",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "DeviceBrand",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "DeviceModel",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "DeviceType",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "OperatingSystem",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "Platform",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "ScreenResolution",
                table: "DeviceInfo");

            migrationBuilder.AddColumn<string>(
                name: "Device",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Device",
                table: "DeviceInfo");

            migrationBuilder.AddColumn<string>(
                name: "Browser",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BrowserVersion",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceBrand",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceModel",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceType",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OperatingSystem",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Platform",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScreenResolution",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
