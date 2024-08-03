using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSingleAssistAndShowReasons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LiveEvents_AspNetUsers_LiveAssistHolder_AssistedById",
                table: "LiveEvents");

            migrationBuilder.RenameColumn(
                name: "LiveAssistHolder_AssistedById",
                table: "LiveEvents",
                newName: "LiveGoalHolder_AssistedById");

            migrationBuilder.RenameIndex(
                name: "IX_LiveEvents_LiveAssistHolder_AssistedById",
                table: "LiveEvents",
                newName: "IX_LiveEvents_LiveGoalHolder_AssistedById");

            migrationBuilder.AddColumn<string>(
                name: "RedCardReason",
                table: "LiveEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RedCardReason1",
                table: "LiveEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YellowCardReason",
                table: "LiveEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YellowCardReason1",
                table: "LiveEvents",
                type: "nvarchar(max)",
                nullable: true);

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
                name: "FK_LiveEvents_AspNetUsers_LiveGoalHolder_AssistedById",
                table: "LiveEvents");

            migrationBuilder.DropColumn(
                name: "RedCardReason",
                table: "LiveEvents");

            migrationBuilder.DropColumn(
                name: "RedCardReason1",
                table: "LiveEvents");

            migrationBuilder.DropColumn(
                name: "YellowCardReason",
                table: "LiveEvents");

            migrationBuilder.DropColumn(
                name: "YellowCardReason1",
                table: "LiveEvents");

            migrationBuilder.RenameColumn(
                name: "LiveGoalHolder_AssistedById",
                table: "LiveEvents",
                newName: "LiveAssistHolder_AssistedById");

            migrationBuilder.RenameIndex(
                name: "IX_LiveEvents_LiveGoalHolder_AssistedById",
                table: "LiveEvents",
                newName: "IX_LiveEvents_LiveAssistHolder_AssistedById");

            migrationBuilder.AddForeignKey(
                name: "FK_LiveEvents_AspNetUsers_LiveAssistHolder_AssistedById",
                table: "LiveEvents",
                column: "LiveAssistHolder_AssistedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
