using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddnewfieldsonExamDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PassMarks",
                table: "ExamSubjects",
                newName: "PassMarksTh");

            migrationBuilder.RenameColumn(
                name: "FullMarks",
                table: "ExamSubjects",
                newName: "PassMarksPr");

            migrationBuilder.AddColumn<int>(
                name: "FullMarksPr",
                table: "ExamSubjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FullMarksTh",
                table: "ExamSubjects",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullMarksPr",
                table: "ExamSubjects");

            migrationBuilder.DropColumn(
                name: "FullMarksTh",
                table: "ExamSubjects");

            migrationBuilder.RenameColumn(
                name: "PassMarksTh",
                table: "ExamSubjects",
                newName: "PassMarks");

            migrationBuilder.RenameColumn(
                name: "PassMarksPr",
                table: "ExamSubjects",
                newName: "FullMarks");
        }
    }
}
