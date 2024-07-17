using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class MakeClubIdNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the existing index
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ClubManager_ClubId",
                table: "AspNetUsers");

            // Alter the column to be nullable
            migrationBuilder.AlterColumn<int>(
                name: "ClubId",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            // Recreate the index
            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ClubManager_ClubId",
                table: "AspNetUsers",
                column: "ClubId",
                unique: true,
                filter: "[ClubId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the index created in Up method
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ClubManager_ClubId",
                table: "AspNetUsers");

            // Revert the column to be non-nullable
            migrationBuilder.AlterColumn<int>(
                name: "ClubId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            // Recreate the original index
            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ClubManager_ClubId",
                table: "AspNetUsers",
                column: "ClubId");
        }
    }
}
