using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class AddAllDeviceInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BrowserVersion",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Continent",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeviceBrand",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeviceId",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeviceModel",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeviceOrientation",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DevicePixelRatio",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeviceToken",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Latitude",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Longitude",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OsVersion",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ScreenDensity",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ScreenResolution",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ScreenSize",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TimeZoneOffset",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Timezone",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrowserVersion",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "City",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "Continent",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "DeviceBrand",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "DeviceModel",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "DeviceOrientation",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "DevicePixelRatio",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "DeviceToken",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "OsVersion",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "ScreenDensity",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "ScreenResolution",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "ScreenSize",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "TimeZoneOffset",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "Timezone",
                table: "DeviceInfo");
        }
    }
}
