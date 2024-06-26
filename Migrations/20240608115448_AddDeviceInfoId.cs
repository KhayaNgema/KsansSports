using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class AddDeviceInfoId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Device",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Payments");

            migrationBuilder.AddColumn<int>(
                name: "DeviceInfoId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DeviceInfoId",
                table: "DeviceInfo",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeviceInfo",
                table: "DeviceInfo",
                column: "DeviceInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_DeviceInfoId",
                table: "Payments",
                column: "DeviceInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_DeviceInfo_DeviceInfoId",
                table: "Payments",
                column: "DeviceInfoId",
                principalTable: "DeviceInfo",
                principalColumn: "DeviceInfoId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_DeviceInfo_DeviceInfoId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_DeviceInfoId",
                table: "Payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeviceInfo",
                table: "DeviceInfo");

            migrationBuilder.DropColumn(
                name: "DeviceInfoId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DeviceInfoId",
                table: "DeviceInfo");

            migrationBuilder.AddColumn<string>(
                name: "Device",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
