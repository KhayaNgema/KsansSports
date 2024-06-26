using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRemoveRemove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrowserVersion",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "DeviceBrand",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "OperatingSystem",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "OsVersion",
                table: "DeviceInfo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "OperatingSystem",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OsVersion",
                table: "DeviceInfo",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
