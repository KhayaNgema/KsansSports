using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class AddMigrationUpdateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LineUpSubstitutes_AspNetUsers_PlayerId",
                table: "LineUpSubstitutes");

            migrationBuilder.DropForeignKey(
                name: "FK_LineUpSubstitutesHolder_AspNetUsers_PlayerId",
                table: "LineUpSubstitutesHolder");

            migrationBuilder.DropForeignKey(
                name: "FK_LineUpXI_AspNetUsers_PlayerId",
                table: "LineUpXI");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "LineUpXI");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "LineUpSubstitutesHolder");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "LineUpSubstitutes");

            migrationBuilder.AlterColumn<string>(
                name: "PlayerId",
                table: "LineUpXI",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlayerId",
                table: "LineUpSubstitutesHolder",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlayerId",
                table: "LineUpSubstitutes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LineUpSubstitutes_AspNetUsers_PlayerId",
                table: "LineUpSubstitutes",
                column: "PlayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LineUpSubstitutesHolder_AspNetUsers_PlayerId",
                table: "LineUpSubstitutesHolder",
                column: "PlayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LineUpXI_AspNetUsers_PlayerId",
                table: "LineUpXI",
                column: "PlayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
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
                name: "FK_LineUpXI_AspNetUsers_PlayerId",
                table: "LineUpXI");

            migrationBuilder.AlterColumn<string>(
                name: "PlayerId",
                table: "LineUpXI",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "LineUpXI",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "PlayerId",
                table: "LineUpSubstitutesHolder",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "LineUpSubstitutesHolder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "PlayerId",
                table: "LineUpSubstitutes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "LineUpSubstitutes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
                name: "FK_LineUpXI_AspNetUsers_PlayerId",
                table: "LineUpXI",
                column: "PlayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
