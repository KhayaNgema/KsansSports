using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class ReInstertTheidTing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssistedById",
                table: "LiveEvents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LiveGoalHolder_AssistedById",
                table: "LiveEvents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LiveEvents_AssistedById",
                table: "LiveEvents",
                column: "AssistedById");

            migrationBuilder.CreateIndex(
                name: "IX_LiveEvents_LiveGoalHolder_AssistedById",
                table: "LiveEvents",
                column: "LiveGoalHolder_AssistedById");

            migrationBuilder.AddForeignKey(
                name: "FK_LiveEvents_AspNetUsers_AssistedById",
                table: "LiveEvents",
                column: "AssistedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LiveEvents_AspNetUsers_LiveGoalHolder_AssistedById",
                table: "LiveEvents",
                column: "LiveGoalHolder_AssistedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LiveEvents_AspNetUsers_AssistedById",
                table: "LiveEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_LiveEvents_AspNetUsers_LiveGoalHolder_AssistedById",
                table: "LiveEvents");

            migrationBuilder.DropIndex(
                name: "IX_LiveEvents_AssistedById",
                table: "LiveEvents");

            migrationBuilder.DropIndex(
                name: "IX_LiveEvents_LiveGoalHolder_AssistedById",
                table: "LiveEvents");

            migrationBuilder.DropColumn(
                name: "AssistedById",
                table: "LiveEvents");

            migrationBuilder.DropColumn(
                name: "LiveGoalHolder_AssistedById",
                table: "LiveEvents");
        }
    }
}
