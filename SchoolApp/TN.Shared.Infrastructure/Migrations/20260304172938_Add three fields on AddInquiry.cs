using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TN.Shared.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddthreefieldsonAddInquiry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeadCountries",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CountryId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LeadId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadCountries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeadCountries_Leads_LeadId",
                        column: x => x.LeadId,
                        principalTable: "Leads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LeadUniversities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UniversityId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LeadCountryId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadUniversities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeadUniversities_LeadCountries_LeadCountryId",
                        column: x => x.LeadCountryId,
                        principalTable: "LeadCountries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeadCourses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LeadUniversityId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeadCourses_LeadUniversities_LeadUniversityId",
                        column: x => x.LeadUniversityId,
                        principalTable: "LeadUniversities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeadCountries_LeadId",
                table: "LeadCountries",
                column: "LeadId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadCourses_LeadUniversityId",
                table: "LeadCourses",
                column: "LeadUniversityId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadUniversities_LeadCountryId",
                table: "LeadUniversities",
                column: "LeadCountryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeadCourses");

            migrationBuilder.DropTable(
                name: "LeadUniversities");

            migrationBuilder.DropTable(
                name: "LeadCountries");
        }
    }
}
