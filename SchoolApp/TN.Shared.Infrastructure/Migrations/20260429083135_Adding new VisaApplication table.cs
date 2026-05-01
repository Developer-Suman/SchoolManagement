using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddingnewVisaApplicationtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VisaStatuses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VisaStatusType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisaStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VisaApplications",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicantId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CountryId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UniversityId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Intakeid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AppliedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VisaStatusId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SchoolId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisaApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisaApplications_Applicants_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "Applicants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisaApplications_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisaApplications_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisaApplications_Intakes_Intakeid",
                        column: x => x.Intakeid,
                        principalTable: "Intakes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisaApplications_Universities_UniversityId",
                        column: x => x.UniversityId,
                        principalTable: "Universities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisaApplications_VisaStatuses_VisaStatusId",
                        column: x => x.VisaStatusId,
                        principalTable: "VisaStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VisaApplicationDocuments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VisaApplicationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentTypeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VisaStatusId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "VisaApplicationStatusHistories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VisaApplicationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VisaStatusId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisaApplicationStatusHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisaApplicationStatusHistories_VisaApplications_VisaApplicationId",
                        column: x => x.VisaApplicationId,
                        principalTable: "VisaApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisaApplicationStatusHistories_VisaStatuses_VisaStatusId",
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

            migrationBuilder.CreateIndex(
                name: "IX_VisaApplications_ApplicantId",
                table: "VisaApplications",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_VisaApplications_CountryId",
                table: "VisaApplications",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_VisaApplications_CourseId",
                table: "VisaApplications",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_VisaApplications_Intakeid",
                table: "VisaApplications",
                column: "Intakeid");

            migrationBuilder.CreateIndex(
                name: "IX_VisaApplications_UniversityId",
                table: "VisaApplications",
                column: "UniversityId");

            migrationBuilder.CreateIndex(
                name: "IX_VisaApplications_VisaStatusId",
                table: "VisaApplications",
                column: "VisaStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_VisaApplicationStatusHistories_VisaApplicationId",
                table: "VisaApplicationStatusHistories",
                column: "VisaApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_VisaApplicationStatusHistories_VisaStatusId",
                table: "VisaApplicationStatusHistories",
                column: "VisaStatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VisaApplicationDocuments");

            migrationBuilder.DropTable(
                name: "VisaApplicationStatusHistories");

            migrationBuilder.DropTable(
                name: "VisaApplications");

            migrationBuilder.DropTable(
                name: "VisaStatuses");
        }
    }
}
