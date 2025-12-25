using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddcoluminTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FeeTypeid",
                table: "Ledgers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentId",
                table: "Ledgers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FyId",
                table: "FeeTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NameOfMonths",
                table: "FeeTypes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ledgers_FeeTypeid",
                table: "Ledgers",
                column: "FeeTypeid",
                unique: true,
                filter: "[FeeTypeid] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Ledgers_StudentId",
                table: "Ledgers",
                column: "StudentId",
                unique: true,
                filter: "[StudentId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Ledgers_FeeTypes_FeeTypeid",
                table: "Ledgers",
                column: "FeeTypeid",
                principalTable: "FeeTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ledgers_Students_StudentId",
                table: "Ledgers",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ledgers_FeeTypes_FeeTypeid",
                table: "Ledgers");

            migrationBuilder.DropForeignKey(
                name: "FK_Ledgers_Students_StudentId",
                table: "Ledgers");

            migrationBuilder.DropIndex(
                name: "IX_Ledgers_FeeTypeid",
                table: "Ledgers");

            migrationBuilder.DropIndex(
                name: "IX_Ledgers_StudentId",
                table: "Ledgers");

            migrationBuilder.DropColumn(
                name: "FeeTypeid",
                table: "Ledgers");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Ledgers");

            migrationBuilder.DropColumn(
                name: "FyId",
                table: "FeeTypes");

            migrationBuilder.DropColumn(
                name: "NameOfMonths",
                table: "FeeTypes");
        }
    }
}
