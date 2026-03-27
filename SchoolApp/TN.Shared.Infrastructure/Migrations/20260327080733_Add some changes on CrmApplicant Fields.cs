using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddsomechangesonCrmApplicantFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetCountry",
                table: "Applicants");

            migrationBuilder.AlterColumn<string>(
                name: "PassportNumber",
                table: "Applicants",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "CountryId",
                table: "Applicants",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CourseId",
                table: "Applicants",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UniversityId",
                table: "Applicants",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_CountryId",
                table: "Applicants",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_CourseId",
                table: "Applicants",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_UniversityId",
                table: "Applicants",
                column: "UniversityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applicants_Countries_CountryId",
                table: "Applicants",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Applicants_Courses_CourseId",
                table: "Applicants",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Applicants_Universities_UniversityId",
                table: "Applicants",
                column: "UniversityId",
                principalTable: "Universities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applicants_Countries_CountryId",
                table: "Applicants");

            migrationBuilder.DropForeignKey(
                name: "FK_Applicants_Courses_CourseId",
                table: "Applicants");

            migrationBuilder.DropForeignKey(
                name: "FK_Applicants_Universities_UniversityId",
                table: "Applicants");

            migrationBuilder.DropIndex(
                name: "IX_Applicants_CountryId",
                table: "Applicants");

            migrationBuilder.DropIndex(
                name: "IX_Applicants_CourseId",
                table: "Applicants");

            migrationBuilder.DropIndex(
                name: "IX_Applicants_UniversityId",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Applicants");

            migrationBuilder.DropColumn(
                name: "UniversityId",
                table: "Applicants");

            migrationBuilder.AlterColumn<string>(
                name: "PassportNumber",
                table: "Applicants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetCountry",
                table: "Applicants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
