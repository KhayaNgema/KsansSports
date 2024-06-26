using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class RemoveClubIdTiat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fines_Club_ClubId",
                table: "Fines");

            migrationBuilder.DropIndex(
                name: "IX_Fines_ClubId",
                table: "Fines");

            migrationBuilder.DropColumn(
                name: "ClubId",
                table: "Fines");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClubId",
                table: "Fines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Fines_ClubId",
                table: "Fines",
                column: "ClubId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fines_Club_ClubId",
                table: "Fines",
                column: "ClubId",
                principalTable: "Club",
                principalColumn: "ClubId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
