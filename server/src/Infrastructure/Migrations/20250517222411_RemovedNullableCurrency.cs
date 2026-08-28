using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovedNullableCurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Security_Currency_CurrencyId",
                table: "Security");

            migrationBuilder.AlterColumn<Guid>(
                name: "CurrencyId",
                table: "Security",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Security_Currency_CurrencyId",
                table: "Security",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Security_Currency_CurrencyId",
                table: "Security");

            migrationBuilder.AlterColumn<Guid>(
                name: "CurrencyId",
                table: "Security",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Security_Currency_CurrencyId",
                table: "Security",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id");
        }
    }
}
