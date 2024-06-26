using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class AddInvoiceModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayerTransferInvoices",
                columns: table => new
                {
                    InvoiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentId = table.Column<int>(type: "int", nullable: true),
                    FineId = table.Column<int>(type: "int", nullable: true),
                    TransferId = table.Column<int>(type: "int", nullable: true),
                    InvoiceTimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerTransferInvoices", x => x.InvoiceId);
                    table.ForeignKey(
                        name: "FK_PlayerTransferInvoices_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_PlayerTransferInvoices_Fines_FineId",
                        column: x => x.FineId,
                        principalTable: "Fines",
                        principalColumn: "FineId");
                    table.ForeignKey(
                        name: "FK_PlayerTransferInvoices_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "PaymentId");
                    table.ForeignKey(
                        name: "FK_PlayerTransferInvoices_Transfer_TransferId",
                        column: x => x.TransferId,
                        principalTable: "Transfer",
                        principalColumn: "TransferId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTransferInvoices_CreatedById",
                table: "PlayerTransferInvoices",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTransferInvoices_FineId",
                table: "PlayerTransferInvoices",
                column: "FineId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTransferInvoices_PaymentId",
                table: "PlayerTransferInvoices",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTransferInvoices_TransferId",
                table: "PlayerTransferInvoices",
                column: "TransferId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerTransferInvoices");
        }
    }
}
