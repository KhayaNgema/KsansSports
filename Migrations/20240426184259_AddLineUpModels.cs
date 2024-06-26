using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyField.Migrations
{
    /// <inheritdoc />
    public partial class AddLineUpModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LineUp",
                columns: table => new
                {
                    LineUpId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FixtureId = table.Column<int>(type: "int", nullable: false),
                    ClubId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModifiedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineUp", x => x.LineUpId);
                    table.ForeignKey(
                        name: "FK_LineUp_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LineUp_AspNetUsers_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LineUp_Club_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Club",
                        principalColumn: "ClubId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LineUp_Fixture_FixtureId",
                        column: x => x.FixtureId,
                        principalTable: "Fixture",
                        principalColumn: "FixtureId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "LineUpSubstitutesHolder",
                columns: table => new
                {
                    LineUpSubstituteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClubId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModifiedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineUpSubstitutesHolder", x => x.LineUpSubstituteId);
                    table.ForeignKey(
                        name: "FK_LineUpSubstitutesHolder_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LineUpSubstitutesHolder_AspNetUsers_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LineUpSubstitutesHolder_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LineUpSubstitutesHolder_Club_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Club",
                        principalColumn: "ClubId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "LineUpXIHolder",
                columns: table => new
                {
                    LineUpXIId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClubId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModifiedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineUpXIHolder", x => x.LineUpXIId);
                    table.ForeignKey(
                        name: "FK_LineUpXIHolder_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LineUpXIHolder_AspNetUsers_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LineUpXIHolder_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LineUpXIHolder_Club_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Club",
                        principalColumn: "ClubId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "LineUpSubstitutes",
                columns: table => new
                {
                    LineUpSubstituteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LineUpId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClubId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModifiedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineUpSubstitutes", x => x.LineUpSubstituteId);
                    table.ForeignKey(
                        name: "FK_LineUpSubstitutes_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LineUpSubstitutes_AspNetUsers_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LineUpSubstitutes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LineUpSubstitutes_Club_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Club",
                        principalColumn: "ClubId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LineUpSubstitutes_LineUp_LineUpId",
                        column: x => x.LineUpId,
                        principalTable: "LineUp",
                        principalColumn: "LineUpId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "LineUpXI",
                columns: table => new
                {
                    LineUpXIId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LineUpId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClubId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModifiedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineUpXI", x => x.LineUpXIId);
                    table.ForeignKey(
                        name: "FK_LineUpXI_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LineUpXI_AspNetUsers_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LineUpXI_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LineUpXI_Club_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Club",
                        principalColumn: "ClubId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LineUpXI_LineUp_LineUpId",
                        column: x => x.LineUpId,
                        principalTable: "LineUp",
                        principalColumn: "LineUpId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LineUp_ClubId",
                table: "LineUp",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_LineUp_CreatedById",
                table: "LineUp",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LineUp_FixtureId",
                table: "LineUp",
                column: "FixtureId");

            migrationBuilder.CreateIndex(
                name: "IX_LineUp_ModifiedById",
                table: "LineUp",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_LineUpSubstitutes_ClubId",
                table: "LineUpSubstitutes",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_LineUpSubstitutes_CreatedById",
                table: "LineUpSubstitutes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LineUpSubstitutes_LineUpId",
                table: "LineUpSubstitutes",
                column: "LineUpId");

            migrationBuilder.CreateIndex(
                name: "IX_LineUpSubstitutes_ModifiedById",
                table: "LineUpSubstitutes",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_LineUpSubstitutes_UserId",
                table: "LineUpSubstitutes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LineUpSubstitutesHolder_ClubId",
                table: "LineUpSubstitutesHolder",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_LineUpSubstitutesHolder_CreatedById",
                table: "LineUpSubstitutesHolder",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LineUpSubstitutesHolder_ModifiedById",
                table: "LineUpSubstitutesHolder",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_LineUpSubstitutesHolder_UserId",
                table: "LineUpSubstitutesHolder",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LineUpXI_ClubId",
                table: "LineUpXI",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_LineUpXI_CreatedById",
                table: "LineUpXI",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LineUpXI_LineUpId",
                table: "LineUpXI",
                column: "LineUpId");

            migrationBuilder.CreateIndex(
                name: "IX_LineUpXI_ModifiedById",
                table: "LineUpXI",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_LineUpXI_UserId",
                table: "LineUpXI",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LineUpXIHolder_ClubId",
                table: "LineUpXIHolder",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_LineUpXIHolder_CreatedById",
                table: "LineUpXIHolder",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LineUpXIHolder_ModifiedById",
                table: "LineUpXIHolder",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_LineUpXIHolder_UserId",
                table: "LineUpXIHolder",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LineUpSubstitutes");

            migrationBuilder.DropTable(
                name: "LineUpSubstitutesHolder");

            migrationBuilder.DropTable(
                name: "LineUpXI");

            migrationBuilder.DropTable(
                name: "LineUpXIHolder");

            migrationBuilder.DropTable(
                name: "LineUp");
        }
    }
}
