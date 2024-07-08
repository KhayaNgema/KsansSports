using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class AddPersonnelReportsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActiveAccountsCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ActiveAccountsRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExpectedRepayableAmount",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InactiveAccountsCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "InactiveAccountsRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OverallAccountsCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OverduePaymentsCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OverduePaymentsRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaidAccountsCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PaidAccountsRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PendingPaymentsCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PendingPaymentsRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepayableFinesCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuspendedAccountsCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SuspendedAccountsRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPaidAmount",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalUnpaidAmount",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveAccountsCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ActiveAccountsRate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ExpectedRepayableAmount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "InactiveAccountsCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "InactiveAccountsRate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "OverallAccountsCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "OverduePaymentsCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "OverduePaymentsRate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "PaidAccountsCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "PaidAccountsRate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "PendingPaymentsCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "PendingPaymentsRate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "RepayableFinesCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "SuspendedAccountsCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "SuspendedAccountsRate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "TotalPaidAmount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "TotalUnpaidAmount",
                table: "Reports");
        }
    }
}
