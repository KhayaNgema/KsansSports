using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNewsId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_SportNew_SportNewsNewsId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_SportNewsNewsId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "NewsId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "SportNewsNewsId",
                table: "Reports");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NewsId",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SportNewsNewsId",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_SportNewsNewsId",
                table: "Reports",
                column: "SportNewsNewsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_SportNew_SportNewsNewsId",
                table: "Reports",
                column: "SportNewsNewsId",
                principalTable: "SportNew",
                principalColumn: "NewsId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
