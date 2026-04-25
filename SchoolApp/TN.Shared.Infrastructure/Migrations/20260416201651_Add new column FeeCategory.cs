using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddnewcolumnFeeCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FeeCategoryid",
                table: "Students",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_FeeCategoryid",
                table: "Students",
                column: "FeeCategoryid");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_FeeCategories_FeeCategoryid",
                table: "Students",
                column: "FeeCategoryid",
                principalTable: "FeeCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_FeeCategories_FeeCategoryid",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_FeeCategoryid",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "FeeCategoryid",
                table: "Students");
        }
    }
}
