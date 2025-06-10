using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DepositAccountTypeRemoval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deposit_Account_AccountId",
                table: "Deposit");

            migrationBuilder.DeleteData(
                table: "AccountType",
                keyColumn: "Id",
                keyValue: new Guid("b9dcea63-49f4-47bf-ae7b-eb596479ba57"));

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Deposit",
                newName: "CurrencyId");

            migrationBuilder.RenameIndex(
                name: "IX_Deposit_AccountId",
                table: "Deposit",
                newName: "IX_Deposit_CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deposit_Currency_CurrencyId",
                table: "Deposit",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deposit_Currency_CurrencyId",
                table: "Deposit");

            migrationBuilder.RenameColumn(
                name: "CurrencyId",
                table: "Deposit",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Deposit_CurrencyId",
                table: "Deposit",
                newName: "IX_Deposit_AccountId");

            migrationBuilder.InsertData(
                table: "AccountType",
                columns: new[] { "Id", "Active", "Name" },
                values: new object[] { new Guid("b9dcea63-49f4-47bf-ae7b-eb596479ba57"), true, "Deposit account" });

            migrationBuilder.AddForeignKey(
                name: "FK_Deposit_Account_AccountId",
                table: "Deposit",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
