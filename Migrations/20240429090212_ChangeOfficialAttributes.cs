using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class ChangeOfficialAttributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssistantOne",
                table: "MatchOfficials");

            migrationBuilder.DropColumn(
                name: "AssistantTwo",
                table: "MatchOfficials");

            migrationBuilder.DropColumn(
                name: "Refeere",
                table: "MatchOfficials");

            migrationBuilder.AddColumn<string>(
                name: "AssistantOneId",
                table: "MatchOfficials",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AssistantTwoId",
                table: "MatchOfficials",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RefeereId",
                table: "MatchOfficials",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_MatchOfficials_AssistantOneId",
                table: "MatchOfficials",
                column: "AssistantOneId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchOfficials_AssistantTwoId",
                table: "MatchOfficials",
                column: "AssistantTwoId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchOfficials_RefeereId",
                table: "MatchOfficials",
                column: "RefeereId");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchOfficials_AspNetUsers_AssistantOneId",
                table: "MatchOfficials",
                column: "AssistantOneId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchOfficials_AspNetUsers_AssistantTwoId",
                table: "MatchOfficials",
                column: "AssistantTwoId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchOfficials_AspNetUsers_RefeereId",
                table: "MatchOfficials",
                column: "RefeereId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchOfficials_AspNetUsers_AssistantOneId",
                table: "MatchOfficials");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchOfficials_AspNetUsers_AssistantTwoId",
                table: "MatchOfficials");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchOfficials_AspNetUsers_RefeereId",
                table: "MatchOfficials");

            migrationBuilder.DropIndex(
                name: "IX_MatchOfficials_AssistantOneId",
                table: "MatchOfficials");

            migrationBuilder.DropIndex(
                name: "IX_MatchOfficials_AssistantTwoId",
                table: "MatchOfficials");

            migrationBuilder.DropIndex(
                name: "IX_MatchOfficials_RefeereId",
                table: "MatchOfficials");

            migrationBuilder.DropColumn(
                name: "AssistantOneId",
                table: "MatchOfficials");

            migrationBuilder.DropColumn(
                name: "AssistantTwoId",
                table: "MatchOfficials");

            migrationBuilder.DropColumn(
                name: "RefeereId",
                table: "MatchOfficials");

            migrationBuilder.AddColumn<string>(
                name: "AssistantOne",
                table: "MatchOfficials",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AssistantTwo",
                table: "MatchOfficials",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Refeere",
                table: "MatchOfficials",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
