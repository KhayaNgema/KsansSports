using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInvoiceName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerTransferInvoices_AspNetUsers_CreatedById",
                table: "PlayerTransferInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerTransferInvoices_Fines_FineId",
                table: "PlayerTransferInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerTransferInvoices_Payments_PaymentId",
                table: "PlayerTransferInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerTransferInvoices_Transfer_TransferId",
                table: "PlayerTransferInvoices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerTransferInvoices",
                table: "PlayerTransferInvoices");

            migrationBuilder.RenameTable(
                name: "PlayerTransferInvoices",
                newName: "Invoices");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerTransferInvoices_TransferId",
                table: "Invoices",
                newName: "IX_Invoices_TransferId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerTransferInvoices_PaymentId",
                table: "Invoices",
                newName: "IX_Invoices_PaymentId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerTransferInvoices_FineId",
                table: "Invoices",
                newName: "IX_Invoices_FineId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerTransferInvoices_CreatedById",
                table: "Invoices",
                newName: "IX_Invoices_CreatedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invoices",
                table: "Invoices",
                column: "InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_AspNetUsers_CreatedById",
                table: "Invoices",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Fines_FineId",
                table: "Invoices",
                column: "FineId",
                principalTable: "Fines",
                principalColumn: "FineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Payments_PaymentId",
                table: "Invoices",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Transfer_TransferId",
                table: "Invoices",
                column: "TransferId",
                principalTable: "Transfer",
                principalColumn: "TransferId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_AspNetUsers_CreatedById",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Fines_FineId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Payments_PaymentId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Transfer_TransferId",
                table: "Invoices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invoices",
                table: "Invoices");

            migrationBuilder.RenameTable(
                name: "Invoices",
                newName: "PlayerTransferInvoices");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_TransferId",
                table: "PlayerTransferInvoices",
                newName: "IX_PlayerTransferInvoices_TransferId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_PaymentId",
                table: "PlayerTransferInvoices",
                newName: "IX_PlayerTransferInvoices_PaymentId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_FineId",
                table: "PlayerTransferInvoices",
                newName: "IX_PlayerTransferInvoices_FineId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_CreatedById",
                table: "PlayerTransferInvoices",
                newName: "IX_PlayerTransferInvoices_CreatedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerTransferInvoices",
                table: "PlayerTransferInvoices",
                column: "InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerTransferInvoices_AspNetUsers_CreatedById",
                table: "PlayerTransferInvoices",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerTransferInvoices_Fines_FineId",
                table: "PlayerTransferInvoices",
                column: "FineId",
                principalTable: "Fines",
                principalColumn: "FineId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerTransferInvoices_Payments_PaymentId",
                table: "PlayerTransferInvoices",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerTransferInvoices_Transfer_TransferId",
                table: "PlayerTransferInvoices",
                column: "TransferId",
                principalTable: "Transfer",
                principalColumn: "TransferId");
        }
    }
}
