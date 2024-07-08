using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class AddNewsReports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApprovedNewsCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ApprovedNewsRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuthoredNewsCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NewsId",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NewsReadersCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PublishedNewsCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PublishedNewsRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReadersCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RejectedNewsCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RejectedNewsRate",
                table: "Reports",
                type: "decimal(18,2)",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_SportNew_SportNewsNewsId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_SportNewsNewsId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ApprovedNewsCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ApprovedNewsRate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "AuthoredNewsCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "NewsId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "NewsReadersCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "PublishedNewsCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "PublishedNewsRate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ReadersCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "RejectedNewsCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "RejectedNewsRate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "SportNewsNewsId",
                table: "Reports");
        }
    }
}
