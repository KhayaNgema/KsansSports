using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class AddReasonForReEditNews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReasonForReEdit",
                table: "SportNew",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReasonForReEdit",
                table: "SportNew");
        }
    }
}
