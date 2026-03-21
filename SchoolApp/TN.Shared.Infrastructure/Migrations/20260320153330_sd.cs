using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class sd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassRegistrations_Applicants_ApplicantId",
                table: "ClassRegistrations");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassRegistrations_ConsultancyClasses_ConsultancyClassId",
                table: "ClassRegistrations");

            migrationBuilder.CreateTable(
                name: "FollowUps",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    AppointmentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FollowUpDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FollowUpStatus = table.Column<int>(type: "int", nullable: false),
                    SchoolId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowUps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FollowUps_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FollowUps_AppointmentId",
                table: "FollowUps",
                column: "AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassRegistrations_Applicants_ApplicantId",
                table: "ClassRegistrations",
                column: "ApplicantId",
                principalTable: "Applicants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassRegistrations_ConsultancyClasses_ConsultancyClassId",
                table: "ClassRegistrations",
                column: "ConsultancyClassId",
                principalTable: "ConsultancyClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassRegistrations_Applicants_ApplicantId",
                table: "ClassRegistrations");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassRegistrations_ConsultancyClasses_ConsultancyClassId",
                table: "ClassRegistrations");

            migrationBuilder.DropTable(
                name: "FollowUps");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassRegistrations_Applicants_ApplicantId",
                table: "ClassRegistrations",
                column: "ApplicantId",
                principalTable: "Applicants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassRegistrations_ConsultancyClasses_ConsultancyClassId",
                table: "ClassRegistrations",
                column: "ConsultancyClassId",
                principalTable: "ConsultancyClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
