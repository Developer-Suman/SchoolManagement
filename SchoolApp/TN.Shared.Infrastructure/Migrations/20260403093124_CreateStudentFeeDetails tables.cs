using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateStudentFeeDetailstables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignedFeeStatuses");

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "FeeStructureDetails",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StudentFeeDetails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FeePaidType = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Times = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    FeeTypeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StudentFeeId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentFeeDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentFeeDetails_FeeTypes_FeeTypeId",
                        column: x => x.FeeTypeId,
                        principalTable: "FeeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentFeeDetails_StudentFees_StudentFeeId",
                        column: x => x.StudentFeeId,
                        principalTable: "StudentFees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentFeeDetails_FeeTypeId",
                table: "StudentFeeDetails",
                column: "FeeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentFeeDetails_StudentFeeId",
                table: "StudentFeeDetails",
                column: "StudentFeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentFeeDetails");

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "FeeStructureDetails");

            migrationBuilder.CreateTable(
                name: "AssignedFeeStatuses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StudentFeeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NameOfMonths = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignedFeeStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssignedFeeStatuses_StudentFees_StudentFeeId",
                        column: x => x.StudentFeeId,
                        principalTable: "StudentFees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssignedFeeStatuses_StudentFeeId",
                table: "AssignedFeeStatuses",
                column: "StudentFeeId");
        }
    }
}
