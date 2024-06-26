using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFines_ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OffenderId",
                table: "Fines",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClubId",
                table: "Fines",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fines_OffenderId",
                table: "Fines",
                column: "OffenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fines_AspNetUsers_OffenderId",
                table: "Fines",
                column: "OffenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fines_AspNetUsers_OffenderId",
                table: "Fines");

            migrationBuilder.DropIndex(
                name: "IX_Fines_OffenderId",
                table: "Fines");

            migrationBuilder.DropColumn(
                name: "OffenderId",
                table: "Fines");
        }
    }
}
