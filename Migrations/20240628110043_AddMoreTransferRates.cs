using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreTransferRates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TranferRate",
                table: "Reports",
                newName: "UnsuccessfulTranferRate");

            migrationBuilder.AddColumn<decimal>(
                name: "NotStartedTransferRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SuccessfulTranferRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotStartedTransferRate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "SuccessfulTranferRate",
                table: "Reports");

            migrationBuilder.RenameColumn(
                name: "UnsuccessfulTranferRate",
                table: "Reports",
                newName: "TranferRate");
        }
    }
}
