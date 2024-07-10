using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class AddFansAccountsReports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActiveFansAccountsCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ActiveFansAccountsRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InactiveFansAccountsCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "InactiveFansAccountsRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OverallFansAccountsCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuspendedFansAccountsCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SuspendedFansAccountsRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveFansAccountsCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ActiveFansAccountsRate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "InactiveFansAccountsCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "InactiveFansAccountsRate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "OverallFansAccountsCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "SuspendedFansAccountsCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "SuspendedFansAccountsRate",
                table: "Reports");
        }
    }
}
