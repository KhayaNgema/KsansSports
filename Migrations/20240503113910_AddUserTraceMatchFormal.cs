using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class AddUserTraceMatchFormal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "MatchFormation",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "MatchFormation",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_MatchFormation_CreatedById",
                table: "MatchFormation",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_MatchFormation_ModifiedById",
                table: "MatchFormation",
                column: "ModifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchFormation_AspNetUsers_CreatedById",
                table: "MatchFormation",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchFormation_AspNetUsers_ModifiedById",
                table: "MatchFormation",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchFormation_AspNetUsers_CreatedById",
                table: "MatchFormation");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchFormation_AspNetUsers_ModifiedById",
                table: "MatchFormation");

            migrationBuilder.DropIndex(
                name: "IX_MatchFormation_CreatedById",
                table: "MatchFormation");

            migrationBuilder.DropIndex(
                name: "IX_MatchFormation_ModifiedById",
                table: "MatchFormation");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "MatchFormation");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "MatchFormation");
        }
    }
}
