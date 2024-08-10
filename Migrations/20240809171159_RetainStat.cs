using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class RetainStat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           /* migrationBuilder.DropForeignKey(
                name: "FK_LiveEvents_AspNetUsers_OwnGoalScoredById",
                table: "LiveEvents");

            migrationBuilder.DropIndex(
                name: "IX_LiveEvents_OwnGoalScoredById",
                table: "LiveEvents");

            migrationBuilder.DropColumn(
                name: "OwnGoalScoredById",
                table: "LiveEvents");

            migrationBuilder.DropColumn(
                name: "OwnGoalTime",
                table: "LiveEvents");*/
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnGoalScoredById",
                table: "LiveEvents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnGoalTime",
                table: "LiveEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LiveEvents_OwnGoalScoredById",
                table: "LiveEvents",
                column: "OwnGoalScoredById");

            migrationBuilder.AddForeignKey(
                name: "FK_LiveEvents_AspNetUsers_OwnGoalScoredById",
                table: "LiveEvents",
                column: "OwnGoalScoredById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
