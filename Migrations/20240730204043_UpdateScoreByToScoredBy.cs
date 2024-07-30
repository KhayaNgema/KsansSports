using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class UpdateScoreByToScoredBy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LiveEvents_AspNetUsers_LiveGoalHolder_ScoreById",
                table: "LiveEvents");

            migrationBuilder.DropIndex(
                name: "IX_LiveEvents_LiveGoalHolder_ScoreById",
                table: "LiveEvents");

            migrationBuilder.DropColumn(
                name: "LiveGoalHolder_ScoreById",
                table: "LiveEvents");

            migrationBuilder.AlterColumn<string>(
                name: "ScoredById",
                table: "LiveEvents",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LiveEvents_ScoredById",
                table: "LiveEvents",
                column: "ScoredById");

            migrationBuilder.AddForeignKey(
                name: "FK_LiveEvents_AspNetUsers_ScoredById",
                table: "LiveEvents",
                column: "ScoredById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LiveEvents_AspNetUsers_ScoredById",
                table: "LiveEvents");

            migrationBuilder.DropIndex(
                name: "IX_LiveEvents_ScoredById",
                table: "LiveEvents");

            migrationBuilder.AlterColumn<string>(
                name: "ScoredById",
                table: "LiveEvents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LiveGoalHolder_ScoreById",
                table: "LiveEvents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LiveEvents_LiveGoalHolder_ScoreById",
                table: "LiveEvents",
                column: "LiveGoalHolder_ScoreById");

            migrationBuilder.AddForeignKey(
                name: "FK_LiveEvents_AspNetUsers_LiveGoalHolder_ScoreById",
                table: "LiveEvents",
                column: "LiveGoalHolder_ScoreById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
