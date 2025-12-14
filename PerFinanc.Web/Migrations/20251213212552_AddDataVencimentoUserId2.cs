using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PerFinanc.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddDataVencimentoUserId2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataVencimento",
                table: "LancamentoContaFixa",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ContaFixa",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataVencimento",
                table: "LancamentoContaFixa");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ContaFixa");
        }
    }
}
