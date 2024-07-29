using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePenalties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlayerId",
                table: "Penalties",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Penalties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Penalties_PlayerId",
                table: "Penalties",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Penalties_AspNetUsers_PlayerId",
                table: "Penalties",
                column: "PlayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Penalties_AspNetUsers_PlayerId",
                table: "Penalties");

            migrationBuilder.DropIndex(
                name: "IX_Penalties_PlayerId",
                table: "Penalties");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "Penalties");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Penalties");
        }
    }
}
