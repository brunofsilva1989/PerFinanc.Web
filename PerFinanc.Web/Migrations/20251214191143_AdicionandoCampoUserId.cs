using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PerFinanc.Web.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoCampoUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "LancamentoContaFixa",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "GastoGeral",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Freelance",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "LancamentoContaFixa");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "GastoGeral");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Freelance");
        }
    }
}
