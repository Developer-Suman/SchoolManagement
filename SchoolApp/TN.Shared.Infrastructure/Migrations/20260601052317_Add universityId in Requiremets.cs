using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdduniversityIdinRequiremets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UniversityId",
                table: "Requirements",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requirements_UniversityId",
                table: "Requirements",
                column: "UniversityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requirements_Universities_UniversityId",
                table: "Requirements",
                column: "UniversityId",
                principalTable: "Universities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requirements_Universities_UniversityId",
                table: "Requirements");

            migrationBuilder.DropIndex(
                name: "IX_Requirements_UniversityId",
                table: "Requirements");

            migrationBuilder.DropColumn(
                name: "UniversityId",
                table: "Requirements");
        }
    }
}
