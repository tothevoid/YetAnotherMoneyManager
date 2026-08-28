using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedSecurityCurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CurrencyId",
                table: "Security",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Security_CurrencyId",
                table: "Security",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Security_Currency_CurrencyId",
                table: "Security",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Security_Currency_CurrencyId",
                table: "Security");

            migrationBuilder.DropIndex(
                name: "IX_Security_CurrencyId",
                table: "Security");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "Security");
        }
    }
}
