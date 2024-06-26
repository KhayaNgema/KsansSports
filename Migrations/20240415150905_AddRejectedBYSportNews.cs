using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class AddRejectedBYSportNews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RejectedById",
                table: "SportNew",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "RejectedDateTime",
                table: "SportNew",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_SportNew_RejectedById",
                table: "SportNew",
                column: "RejectedById");

            migrationBuilder.AddForeignKey(
                name: "FK_SportNew_AspNetUsers_RejectedById",
                table: "SportNew",
                column: "RejectedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SportNew_AspNetUsers_RejectedById",
                table: "SportNew");

            migrationBuilder.DropIndex(
                name: "IX_SportNew_RejectedById",
                table: "SportNew");

            migrationBuilder.DropColumn(
                name: "RejectedById",
                table: "SportNew");

            migrationBuilder.DropColumn(
                name: "RejectedDateTime",
                table: "SportNew");
        }
    }
}
