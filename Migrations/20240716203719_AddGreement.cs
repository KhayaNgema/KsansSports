using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class AddGreement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TermsAggreements",
                columns: table => new
                {
                    TermsAggreementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserBaseModelId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsAggreed = table.Column<bool>(type: "bit", nullable: false),
                    AggreementTimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermsAggreements", x => x.TermsAggreementId);
                    table.ForeignKey(
                        name: "FK_TermsAggreements_AspNetUsers_UserBaseModelId",
                        column: x => x.UserBaseModelId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TermsAggreements_UserBaseModelId",
                table: "TermsAggreements",
                column: "UserBaseModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TermsAggreements");
        }
    }
}
