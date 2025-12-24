using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddnewcolumnonSchoolItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Quantity",
                table: "SchoolItems",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitType",
                table: "SchoolItems",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "SchoolItems");

            migrationBuilder.DropColumn(
                name: "UnitType",
                table: "SchoolItems");
        }
    }
}
