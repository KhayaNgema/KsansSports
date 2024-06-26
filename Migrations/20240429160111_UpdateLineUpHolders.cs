using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLineUpHolders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LineUpSubstitutes_AspNetUsers_UserId",
                table: "LineUpSubstitutes");

            migrationBuilder.DropForeignKey(
                name: "FK_LineUpSubstitutesHolder_AspNetUsers_UserId",
                table: "LineUpSubstitutesHolder");

            migrationBuilder.DropForeignKey(
                name: "FK_LineUpXI_AspNetUsers_UserId",
                table: "LineUpXI");

            migrationBuilder.DropForeignKey(
                name: "FK_LineUpXIHolder_AspNetUsers_UserId",
                table: "LineUpXIHolder");

            migrationBuilder.DropIndex(
                name: "IX_LineUpXIHolder_UserId",
                table: "LineUpXIHolder");

            migrationBuilder.DropIndex(
                name: "IX_LineUpXI_UserId",
                table: "LineUpXI");

            migrationBuilder.DropIndex(
                name: "IX_LineUpSubstitutesHolder_UserId",
                table: "LineUpSubstitutesHolder");

            migrationBuilder.DropIndex(
                name: "IX_LineUpSubstitutes_UserId",
                table: "LineUpSubstitutes");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "LineUpXIHolder",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "FixtureId",
                table: "LineUpXIHolder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PlayerId",
                table: "LineUpXIHolder",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "LineUpXI",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "PlayerId",
                table: "LineUpXI",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "LineUpSubstitutesHolder",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "FixtureId",
                table: "LineUpSubstitutesHolder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PlayerId",
                table: "LineUpSubstitutesHolder",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "LineUpSubstitutes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "PlayerId",
                table: "LineUpSubstitutes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LineUpXIHolder_FixtureId",
                table: "LineUpXIHolder",
                column: "FixtureId");

            migrationBuilder.CreateIndex(
                name: "IX_LineUpXIHolder_PlayerId",
                table: "LineUpXIHolder",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_LineUpXI_PlayerId",
                table: "LineUpXI",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_LineUpSubstitutesHolder_FixtureId",
                table: "LineUpSubstitutesHolder",
                column: "FixtureId");

            migrationBuilder.CreateIndex(
                name: "IX_LineUpSubstitutesHolder_PlayerId",
                table: "LineUpSubstitutesHolder",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_LineUpSubstitutes_PlayerId",
                table: "LineUpSubstitutes",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_LineUpSubstitutes_AspNetUsers_PlayerId",
                table: "LineUpSubstitutes",
                column: "PlayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LineUpSubstitutesHolder_AspNetUsers_PlayerId",
                table: "LineUpSubstitutesHolder",
                column: "PlayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LineUpSubstitutesHolder_Fixture_FixtureId",
                table: "LineUpSubstitutesHolder",
                column: "FixtureId",
                principalTable: "Fixture",
                principalColumn: "FixtureId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_LineUpXI_AspNetUsers_PlayerId",
                table: "LineUpXI",
                column: "PlayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LineUpXIHolder_AspNetUsers_PlayerId",
                table: "LineUpXIHolder",
                column: "PlayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LineUpXIHolder_Fixture_FixtureId",
                table: "LineUpXIHolder",
                column: "FixtureId",
                principalTable: "Fixture",
                principalColumn: "FixtureId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LineUpSubstitutes_AspNetUsers_PlayerId",
                table: "LineUpSubstitutes");

            migrationBuilder.DropForeignKey(
                name: "FK_LineUpSubstitutesHolder_AspNetUsers_PlayerId",
                table: "LineUpSubstitutesHolder");

            migrationBuilder.DropForeignKey(
                name: "FK_LineUpSubstitutesHolder_Fixture_FixtureId",
                table: "LineUpSubstitutesHolder");

            migrationBuilder.DropForeignKey(
                name: "FK_LineUpXI_AspNetUsers_PlayerId",
                table: "LineUpXI");

            migrationBuilder.DropForeignKey(
                name: "FK_LineUpXIHolder_AspNetUsers_PlayerId",
                table: "LineUpXIHolder");

            migrationBuilder.DropForeignKey(
                name: "FK_LineUpXIHolder_Fixture_FixtureId",
                table: "LineUpXIHolder");

            migrationBuilder.DropIndex(
                name: "IX_LineUpXIHolder_FixtureId",
                table: "LineUpXIHolder");

            migrationBuilder.DropIndex(
                name: "IX_LineUpXIHolder_PlayerId",
                table: "LineUpXIHolder");

            migrationBuilder.DropIndex(
                name: "IX_LineUpXI_PlayerId",
                table: "LineUpXI");

            migrationBuilder.DropIndex(
                name: "IX_LineUpSubstitutesHolder_FixtureId",
                table: "LineUpSubstitutesHolder");

            migrationBuilder.DropIndex(
                name: "IX_LineUpSubstitutesHolder_PlayerId",
                table: "LineUpSubstitutesHolder");

            migrationBuilder.DropIndex(
                name: "IX_LineUpSubstitutes_PlayerId",
                table: "LineUpSubstitutes");

            migrationBuilder.DropColumn(
                name: "FixtureId",
                table: "LineUpXIHolder");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "LineUpXIHolder");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "LineUpXI");

            migrationBuilder.DropColumn(
                name: "FixtureId",
                table: "LineUpSubstitutesHolder");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "LineUpSubstitutesHolder");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "LineUpSubstitutes");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "LineUpXIHolder",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "LineUpXI",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "LineUpSubstitutesHolder",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "LineUpSubstitutes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_LineUpXIHolder_UserId",
                table: "LineUpXIHolder",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LineUpXI_UserId",
                table: "LineUpXI",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LineUpSubstitutesHolder_UserId",
                table: "LineUpSubstitutesHolder",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LineUpSubstitutes_UserId",
                table: "LineUpSubstitutes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LineUpSubstitutes_AspNetUsers_UserId",
                table: "LineUpSubstitutes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LineUpSubstitutesHolder_AspNetUsers_UserId",
                table: "LineUpSubstitutesHolder",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LineUpXI_AspNetUsers_UserId",
                table: "LineUpXI",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LineUpXIHolder_AspNetUsers_UserId",
                table: "LineUpXIHolder",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
