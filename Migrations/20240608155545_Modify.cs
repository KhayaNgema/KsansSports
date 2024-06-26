using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class Modify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Continent",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "DeviceName",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "DeviceOrientation",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "DevicePixelRatio",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "OsVersion",
                table: "DeviceInfo");

            migrationBuilder.RenameColumn(
                name: "Timezone",
                table: "DeviceInfo",
                newName: "Platform");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Platform",
                table: "DeviceInfo",
                newName: "Timezone");

            migrationBuilder.AddColumn<string>(
                name: "Continent",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeviceName",
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
                name: "IpAddress",
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
                name: "OsVersion",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
