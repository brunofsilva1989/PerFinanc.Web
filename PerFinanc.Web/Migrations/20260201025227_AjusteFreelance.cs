using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PerFinanc.Web.Migrations
{
    /// <inheritdoc />
    public partial class AjusteFreelance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ano",
                table: "Freelance");

            migrationBuilder.DropColumn(
                name: "Mes",
                table: "Freelance");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Ano",
                table: "Freelance",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Mes",
                table: "Freelance",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
