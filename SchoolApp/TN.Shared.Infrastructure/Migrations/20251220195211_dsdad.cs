using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class dsdad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PassingMarks",
                table: "Exams");

            migrationBuilder.AddColumn<string>(
                name: "ExamId",
                table: "Subjects",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "FullMarks",
                table: "Subjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PassMarks",
                table: "Subjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_ExamId",
                table: "Subjects",
                column: "ExamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Exams_ExamId",
                table: "Subjects",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Exams_ExamId",
                table: "Subjects");

            migrationBuilder.DropIndex(
                name: "IX_Subjects_ExamId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "ExamId",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "FullMarks",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "PassMarks",
                table: "Subjects");

            migrationBuilder.AddColumn<decimal>(
                name: "PassingMarks",
                table: "Exams",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
