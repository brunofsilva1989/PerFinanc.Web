using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PerFinanc.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContaFixa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DiaVencimento = table.Column<int>(type: "int", nullable: false),
                    ValorPadrao = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    JaVemDescontado = table.Column<bool>(type: "bit", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContaFixa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LancamentoContaFixa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContaFixaId = table.Column<int>(type: "int", nullable: false),
                    Ano = table.Column<int>(type: "int", nullable: false),
                    Mes = table.Column<int>(type: "int", nullable: false),
                    ValorPrevisto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorPago = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DataPagamento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Observacao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LancamentoContaFixa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LancamentoContaFixa_ContaFixa_ContaFixaId",
                        column: x => x.ContaFixaId,
                        principalTable: "ContaFixa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LancamentoContaFixa_Ano_Mes_ContaFixaId",
                table: "LancamentoContaFixa",
                columns: new[] { "Ano", "Mes", "ContaFixaId" });

            migrationBuilder.CreateIndex(
                name: "IX_LancamentoContaFixa_ContaFixaId",
                table: "LancamentoContaFixa",
                column: "ContaFixaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LancamentoContaFixa");

            migrationBuilder.DropTable(
                name: "ContaFixa");
        }
    }
}
