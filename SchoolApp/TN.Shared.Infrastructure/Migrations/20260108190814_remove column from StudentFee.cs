using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removecolumnfromStudentFee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "StudentFees");

            migrationBuilder.RenameColumn(
                name: "Discount",
                table: "StudentFees",
                newName: "DueAmount");

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "StudentFees",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountPercentage",
                table: "StudentFees",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "StudentFees");

            migrationBuilder.DropColumn(
                name: "DiscountPercentage",
                table: "StudentFees");

            migrationBuilder.RenameColumn(
                name: "DueAmount",
                table: "StudentFees",
                newName: "Discount");

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "StudentFees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
