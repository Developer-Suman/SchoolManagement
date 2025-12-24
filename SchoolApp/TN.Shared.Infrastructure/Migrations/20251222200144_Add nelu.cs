using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Addnelu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SchoolPayments");

            migrationBuilder.DropColumn(
                name: "DueAmount",
                table: "StudentFees");

            migrationBuilder.RenameColumn(
                name: "LastPaymentDate",
                table: "StudentFees",
                newName: "DueDate");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "StudentFees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SchoolId",
                table: "StudentFees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "FeeTypes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "FeeTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SchoolId",
                table: "FeeTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "FeeStructures",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SchoolId",
                table: "FeeStructures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "PaymentsRecords",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Schoolid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentfeeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentsRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentsRecords_StudentFees_StudentfeeId",
                        column: x => x.StudentfeeId,
                        principalTable: "StudentFees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsRecords_StudentfeeId",
                table: "PaymentsRecords",
                column: "StudentfeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentsRecords");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "StudentFees");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "StudentFees");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "FeeTypes");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "FeeTypes");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "FeeStructures");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "FeeStructures");

            migrationBuilder.RenameColumn(
                name: "DueDate",
                table: "StudentFees",
                newName: "LastPaymentDate");

            migrationBuilder.AddColumn<decimal>(
                name: "DueAmount",
                table: "StudentFees",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "FeeTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "SchoolPayments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StudentfeeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolPayments_StudentFees_StudentfeeId",
                        column: x => x.StudentfeeId,
                        principalTable: "StudentFees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SchoolPayments_StudentfeeId",
                table: "SchoolPayments",
                column: "StudentfeeId");
        }
    }
}
