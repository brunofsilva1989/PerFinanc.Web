using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PerFinanc.Web.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAnoMesFromReceitaEntrada : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ano",
                table: "ReceitaEntrada");

            migrationBuilder.DropColumn(
                name: "Mes",
                table: "ReceitaEntrada");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Ano",
                table: "ReceitaEntrada",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Mes",
                table: "ReceitaEntrada",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
