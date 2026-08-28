using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EnhancedDividendPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DividendPayment_Dividend_BrokerAccountId",
                table: "DividendPayment");

            migrationBuilder.AddColumn<DateOnly>(
                name: "ReceivedAt",
                table: "DividendPayment",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.CreateIndex(
                name: "IX_DividendPayment_DividendId",
                table: "DividendPayment",
                column: "DividendId");

            migrationBuilder.AddForeignKey(
                name: "FK_DividendPayment_Dividend_DividendId",
                table: "DividendPayment",
                column: "DividendId",
                principalTable: "Dividend",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DividendPayment_Dividend_DividendId",
                table: "DividendPayment");

            migrationBuilder.DropIndex(
                name: "IX_DividendPayment_DividendId",
                table: "DividendPayment");

            migrationBuilder.DropColumn(
                name: "ReceivedAt",
                table: "DividendPayment");

            migrationBuilder.AddForeignKey(
                name: "FK_DividendPayment_Dividend_BrokerAccountId",
                table: "DividendPayment",
                column: "BrokerAccountId",
                principalTable: "Dividend",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
