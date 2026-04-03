using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddnewtablesFeeStructureDetailswithFeeStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeeStructures_FeeTypes_FeeTypeId",
                table: "FeeStructures");

            migrationBuilder.DropIndex(
                name: "IX_FeeStructures_FeeTypeId",
                table: "FeeStructures");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "FeeStructures");

            migrationBuilder.DropColumn(
                name: "FeeTypeId",
                table: "FeeStructures");

            migrationBuilder.DropColumn(
                name: "NameOfMonths",
                table: "FeeStructures");

            migrationBuilder.AddColumn<string>(
                name: "FeeCategoryId",
                table: "FeeStructures",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FeeCategories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeeStructureDetails",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Times = table.Column<int>(type: "int", nullable: false),
                    FeeTypeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FeeStructureId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    FeePaidType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeStructureDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeeStructureDetails_FeeStructures_FeeStructureId",
                        column: x => x.FeeStructureId,
                        principalTable: "FeeStructures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeeStructureDetails_FeeTypes_FeeTypeId",
                        column: x => x.FeeTypeId,
                        principalTable: "FeeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeeStructures_FeeCategoryId",
                table: "FeeStructures",
                column: "FeeCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeStructureDetails_FeeStructureId",
                table: "FeeStructureDetails",
                column: "FeeStructureId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeStructureDetails_FeeTypeId",
                table: "FeeStructureDetails",
                column: "FeeTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeeStructures_FeeCategories_FeeCategoryId",
                table: "FeeStructures",
                column: "FeeCategoryId",
                principalTable: "FeeCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeeStructures_FeeCategories_FeeCategoryId",
                table: "FeeStructures");

            migrationBuilder.DropTable(
                name: "FeeCategories");

            migrationBuilder.DropTable(
                name: "FeeStructureDetails");

            migrationBuilder.DropIndex(
                name: "IX_FeeStructures_FeeCategoryId",
                table: "FeeStructures");

            migrationBuilder.DropColumn(
                name: "FeeCategoryId",
                table: "FeeStructures");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "FeeStructures",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "FeeTypeId",
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
                name: "IX_FeeStructures_FeeTypeId",
                table: "FeeStructures",
                column: "FeeTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeeStructures_FeeTypes_FeeTypeId",
                table: "FeeStructures",
                column: "FeeTypeId",
                principalTable: "FeeTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
