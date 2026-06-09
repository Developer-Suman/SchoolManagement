using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveVisaDocumentsTbles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VisaApplicationDocuments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VisaApplicationDocuments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentTypeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VisaApplicationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VisaStatusId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VerifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisaApplicationDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisaApplicationDocuments_DocumentTypes_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisaApplicationDocuments_VisaApplications_VisaApplicationId",
                        column: x => x.VisaApplicationId,
                        principalTable: "VisaApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisaApplicationDocuments_VisaStatuses_VisaStatusId",
                        column: x => x.VisaStatusId,
                        principalTable: "VisaStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VisaApplicationDocuments_DocumentTypeId",
                table: "VisaApplicationDocuments",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_VisaApplicationDocuments_VisaApplicationId",
                table: "VisaApplicationDocuments",
                column: "VisaApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_VisaApplicationDocuments_VisaStatusId",
                table: "VisaApplicationDocuments",
                column: "VisaStatusId");
        }
    }
}
