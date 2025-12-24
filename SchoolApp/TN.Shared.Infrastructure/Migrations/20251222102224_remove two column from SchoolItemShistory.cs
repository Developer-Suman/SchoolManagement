using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removetwocolumnfromSchoolItemShistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionBy",
                table: "SchoolItemsHistories");

            migrationBuilder.DropColumn(
                name: "ActionDate",
                table: "SchoolItemsHistories");

            migrationBuilder.AddColumn<string>(
                name: "FiscalYearId",
                table: "SchoolItems",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SchoolItems_FiscalYearId",
                table: "SchoolItems",
                column: "FiscalYearId");

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolItems_FiscalYears_FiscalYearId",
                table: "SchoolItems",
                column: "FiscalYearId",
                principalTable: "FiscalYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchoolItems_FiscalYears_FiscalYearId",
                table: "SchoolItems");

            migrationBuilder.DropIndex(
                name: "IX_SchoolItems_FiscalYearId",
                table: "SchoolItems");

            migrationBuilder.DropColumn(
                name: "FiscalYearId",
                table: "SchoolItems");

            migrationBuilder.AddColumn<string>(
                name: "ActionBy",
                table: "SchoolItemsHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActionDate",
                table: "SchoolItemsHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
