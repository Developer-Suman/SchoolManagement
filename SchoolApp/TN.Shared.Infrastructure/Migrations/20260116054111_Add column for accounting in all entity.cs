using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Addcolumnforaccountinginallentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "NameOfMonths",
                table: "FeeTypes");

            migrationBuilder.AddColumn<string>(
                name: "LedgerId",
                table: "Students",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LedgerId",
                table: "Parents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LedgerId",
                table: "FeeStructures",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NameOfMonths",
                table: "FeeStructures",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_LedgerId",
                table: "Students",
                column: "LedgerId",
                unique: true,
                filter: "[LedgerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Parents_LedgerId",
                table: "Parents",
                column: "LedgerId",
                unique: true,
                filter: "[LedgerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FeeStructures_LedgerId",
                table: "FeeStructures",
                column: "LedgerId");


            migrationBuilder.AddForeignKey(
                name: "FK_Parents_Ledgers_LedgerId",
                table: "Parents",
                column: "LedgerId",
                principalTable: "Ledgers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Ledgers_LedgerId",
                table: "Students",
                column: "LedgerId",
                principalTable: "Ledgers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropForeignKey(
                name: "FK_Parents_Ledgers_LedgerId",
                table: "Parents");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Ledgers_LedgerId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_LedgerId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Parents_LedgerId",
                table: "Parents");

            migrationBuilder.DropIndex(
                name: "IX_FeeStructures_LedgerId",
                table: "FeeStructures");

            migrationBuilder.DropColumn(
                name: "LedgerId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "LedgerId",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "LedgerId",
                table: "FeeStructures");

            migrationBuilder.DropColumn(
                name: "NameOfMonths",
                table: "FeeStructures");

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

        }
    }
}
