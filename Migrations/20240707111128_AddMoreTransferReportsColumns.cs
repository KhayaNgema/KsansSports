using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreTransferReportsColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "RejectedIncomingTransferRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RejectedIncomingTransfersCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RejectedOutgoingTransferRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RejectedOutgoingTransfersCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuccessfulIncomingTransfersCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuccessfulOutgoingTransfersCount",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SuccessfullIncomingTransferRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SuccessfullOutgoingTransferRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RejectedIncomingTransferRate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "RejectedIncomingTransfersCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "RejectedOutgoingTransferRate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "RejectedOutgoingTransfersCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "SuccessfulIncomingTransfersCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "SuccessfulOutgoingTransfersCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "SuccessfullIncomingTransferRate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "SuccessfullOutgoingTransferRate",
                table: "Reports");
        }
    }
}
