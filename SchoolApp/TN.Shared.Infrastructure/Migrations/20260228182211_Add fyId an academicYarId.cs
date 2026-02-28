using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddfyIdanacademicYarId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AcademicYearId",
                table: "Subjects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FyId",
                table: "Subjects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicYearId",
                table: "ExamSessions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FyId",
                table: "ExamSessions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicYearId",
                table: "Exams",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicYearId",
                table: "ExamResults",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicYearId",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FyId",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicYearId",
                table: "AssignmentStudents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FyId",
                table: "AssignmentStudents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcademicYearId",
                table: "Assignments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FyId",
                table: "Assignments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcademicYearId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "FyId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "AcademicYearId",
                table: "ExamSessions");

            migrationBuilder.DropColumn(
                name: "FyId",
                table: "ExamSessions");

            migrationBuilder.DropColumn(
                name: "AcademicYearId",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "AcademicYearId",
                table: "ExamResults");

            migrationBuilder.DropColumn(
                name: "AcademicYearId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "FyId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "AcademicYearId",
                table: "AssignmentStudents");

            migrationBuilder.DropColumn(
                name: "FyId",
                table: "AssignmentStudents");

            migrationBuilder.DropColumn(
                name: "AcademicYearId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "FyId",
                table: "Assignments");
        }
    }
}
