using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddnewcolumnIspaidStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsPaidStatus",
                table: "StudentFees",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaidStatus",
                table: "StudentFees");
        }
    }
}
