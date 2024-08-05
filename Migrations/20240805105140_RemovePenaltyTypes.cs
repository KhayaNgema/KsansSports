using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class RemovePenaltyTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LiveEvents_AspNetUsers_AssistedById",
                table: "LiveEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_LiveEvents_AspNetUsers_LiveGoalHolder_AssistedById",
                table: "LiveEvents");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "LiveEvents");

            migrationBuilder.AddForeignKey(
                name: "FK_LiveEvents_AspNetUsers_AssistedById",
                table: "LiveEvents",
                column: "AssistedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LiveEvents_AspNetUsers_LiveGoalHolder_AssistedById",
                table: "LiveEvents",
                column: "LiveGoalHolder_AssistedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
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

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "LiveEvents",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LiveEvents_AspNetUsers_AssistedById",
                table: "LiveEvents",
                column: "AssistedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LiveEvents_AspNetUsers_LiveGoalHolder_AssistedById",
                table: "LiveEvents",
                column: "LiveGoalHolder_AssistedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
