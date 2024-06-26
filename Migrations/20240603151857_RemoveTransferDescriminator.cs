using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTransferDescriminator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptedTransfer_TransferAmount",
                table: "Transfer");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Transfer");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "Transfer");

            migrationBuilder.DropColumn(
                name: "TransferAmount",
                table: "Transfer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AcceptedTransfer_TransferAmount",
                table: "Transfer",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Transfer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PaymentStatus",
                table: "Transfer",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TransferAmount",
                table: "Transfer",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
