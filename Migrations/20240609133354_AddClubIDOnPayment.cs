using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class AddClubIDOnPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AspNetUsers_PaymentMadeById",
                table: "Payments");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMadeById",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "ClubId",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceNumber",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailed",
                table: "Invoices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ClubId",
                table: "Payments",
                column: "ClubId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AspNetUsers_PaymentMadeById",
                table: "Payments",
                column: "PaymentMadeById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Club_ClubId",
                table: "Payments",
                column: "ClubId",
                principalTable: "Club",
                principalColumn: "ClubId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AspNetUsers_PaymentMadeById",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Club_ClubId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_ClubId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ClubId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "InvoiceNumber",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "IsEmailed",
                table: "Invoices");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMadeById",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AspNetUsers_PaymentMadeById",
                table: "Payments",
                column: "PaymentMadeById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
