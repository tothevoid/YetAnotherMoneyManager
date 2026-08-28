using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DebtPaymentTransactionRefRemoval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DebtPayment_Transaction_TransactionId",
                table: "DebtPayment");

            migrationBuilder.DropIndex(
                name: "IX_DebtPayment_TransactionId",
                table: "DebtPayment");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "DebtPayment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TransactionId",
                table: "DebtPayment",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_DebtPayment_TransactionId",
                table: "DebtPayment",
                column: "TransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_DebtPayment_Transaction_TransactionId",
                table: "DebtPayment",
                column: "TransactionId",
                principalTable: "Transaction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
