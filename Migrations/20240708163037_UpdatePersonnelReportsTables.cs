using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePersonnelReportsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PendingPaymentsRate",
                table: "Reports",
                newName: "PendingFinesRate");

            migrationBuilder.RenameColumn(
                name: "PendingPaymentsCount",
                table: "Reports",
                newName: "PendingPaymentFinesCount");

            migrationBuilder.RenameColumn(
                name: "PaidAccountsRate",
                table: "Reports",
                newName: "PaidFinesRate");

            migrationBuilder.RenameColumn(
                name: "PaidAccountsCount",
                table: "Reports",
                newName: "PaidPaymentFinesCount");

            migrationBuilder.RenameColumn(
                name: "OverduePaymentsRate",
                table: "Reports",
                newName: "OverdueFinesRate");

            migrationBuilder.RenameColumn(
                name: "OverduePaymentsCount",
                table: "Reports",
                newName: "OverduePaymentFineCount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PendingPaymentFinesCount",
                table: "Reports",
                newName: "PendingPaymentsCount");

            migrationBuilder.RenameColumn(
                name: "PendingFinesRate",
                table: "Reports",
                newName: "PendingPaymentsRate");

            migrationBuilder.RenameColumn(
                name: "PaidPaymentFinesCount",
                table: "Reports",
                newName: "PaidAccountsCount");

            migrationBuilder.RenameColumn(
                name: "PaidFinesRate",
                table: "Reports",
                newName: "PaidAccountsRate");

            migrationBuilder.RenameColumn(
                name: "OverduePaymentFineCount",
                table: "Reports",
                newName: "OverduePaymentsCount");

            migrationBuilder.RenameColumn(
                name: "OverdueFinesRate",
                table: "Reports",
                newName: "OverduePaymentsRate");
        }
    }
}
