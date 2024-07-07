using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class AddNotActionedCoulumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "NotActionedIncomingTransferCount",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NotActionedIncomingTransferRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NotActionedOutgoigTransferCount",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NotActionedOutgoingTransferRate",
                table: "Reports",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotActionedIncomingTransferCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "NotActionedIncomingTransferRate",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "NotActionedOutgoigTransferCount",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "NotActionedOutgoingTransferRate",
                table: "Reports");
        }
    }
}
