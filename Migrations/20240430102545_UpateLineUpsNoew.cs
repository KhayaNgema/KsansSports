using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class UpateLineUpsNoew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LineUpSubstitutes_LineUp_LineUpId",
                table: "LineUpSubstitutes");

            migrationBuilder.DropForeignKey(
                name: "FK_LineUpXI_LineUp_LineUpId",
                table: "LineUpXI");

            migrationBuilder.AlterColumn<int>(
                name: "LineUpId",
                table: "LineUpXI",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "FixtureId",
                table: "LineUpXI",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "LineUpId",
                table: "LineUpSubstitutes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "FixtureId",
                table: "LineUpSubstitutes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LineUpXI_FixtureId",
                table: "LineUpXI",
                column: "FixtureId");

            migrationBuilder.CreateIndex(
                name: "IX_LineUpSubstitutes_FixtureId",
                table: "LineUpSubstitutes",
                column: "FixtureId");

            migrationBuilder.AddForeignKey(
                name: "FK_LineUpSubstitutes_Fixture_FixtureId",
                table: "LineUpSubstitutes",
                column: "FixtureId",
                principalTable: "Fixture",
                principalColumn: "FixtureId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LineUpSubstitutes_LineUp_LineUpId",
                table: "LineUpSubstitutes",
                column: "LineUpId",
                principalTable: "LineUp",
                principalColumn: "LineUpId");

            migrationBuilder.AddForeignKey(
                name: "FK_LineUpXI_Fixture_FixtureId",
                table: "LineUpXI",
                column: "FixtureId",
                principalTable: "Fixture",
                principalColumn: "FixtureId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LineUpXI_LineUp_LineUpId",
                table: "LineUpXI",
                column: "LineUpId",
                principalTable: "LineUp",
                principalColumn: "LineUpId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LineUpSubstitutes_Fixture_FixtureId",
                table: "LineUpSubstitutes");

            migrationBuilder.DropForeignKey(
                name: "FK_LineUpSubstitutes_LineUp_LineUpId",
                table: "LineUpSubstitutes");

            migrationBuilder.DropForeignKey(
                name: "FK_LineUpXI_Fixture_FixtureId",
                table: "LineUpXI");

            migrationBuilder.DropForeignKey(
                name: "FK_LineUpXI_LineUp_LineUpId",
                table: "LineUpXI");

            migrationBuilder.DropIndex(
                name: "IX_LineUpXI_FixtureId",
                table: "LineUpXI");

            migrationBuilder.DropIndex(
                name: "IX_LineUpSubstitutes_FixtureId",
                table: "LineUpSubstitutes");

            migrationBuilder.DropColumn(
                name: "FixtureId",
                table: "LineUpXI");

            migrationBuilder.DropColumn(
                name: "FixtureId",
                table: "LineUpSubstitutes");

            migrationBuilder.AlterColumn<int>(
                name: "LineUpId",
                table: "LineUpXI",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LineUpId",
                table: "LineUpSubstitutes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LineUpSubstitutes_LineUp_LineUpId",
                table: "LineUpSubstitutes",
                column: "LineUpId",
                principalTable: "LineUp",
                principalColumn: "LineUpId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LineUpXI_LineUp_LineUpId",
                table: "LineUpXI",
                column: "LineUpId",
                principalTable: "LineUp",
                principalColumn: "LineUpId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
