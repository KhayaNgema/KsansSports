using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class AddAcrhivesssssss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "YellowCards",
                type: "nvarchar(34)",
                maxLength: 34,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Substitutes",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "AppearancesCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssistsCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GoalsScoredCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlayerId",
                table: "Reports",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RedCardCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "YellowCardCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "RedCards",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Penalties",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "LiveGoals",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "LiveAssists",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Live",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_PlayerId",
                table: "Reports",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_AspNetUsers_PlayerId",
                table: "Reports",
                column: "PlayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_AspNetUsers_PlayerId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_PlayerId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "YellowCards");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Substitutes");

            migrationBuilder.DropColumn(
                name: "AppearancesCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "AssistsCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "GoalsScoredCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "RedCardCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "YellowCardCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "RedCards");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Penalties");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "LiveGoals");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "LiveAssists");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Live");
        }
    }
}
