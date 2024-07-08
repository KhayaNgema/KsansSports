using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class Re_InstateNewsIdNow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SportNewsId",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_SportNewsId",
                table: "Reports",
                column: "SportNewsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_SportNew_SportNewsId",
                table: "Reports",
                column: "SportNewsId",
                principalTable: "SportNew",
                principalColumn: "NewsId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_SportNew_SportNewsId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_SportNewsId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "SportNewsId",
                table: "Reports");
        }
    }
}
