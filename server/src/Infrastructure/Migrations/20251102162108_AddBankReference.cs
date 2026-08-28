using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBankReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BankId",
                table: "Deposit",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BankId",
                table: "BrokerAccount",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BankId",
                table: "Account",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Bank",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deposit_BankId",
                table: "Deposit",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_BrokerAccount_BankId",
                table: "BrokerAccount",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_Account_BankId",
                table: "Account",
                column: "BankId");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Bank_BankId",
                table: "Account",
                column: "BankId",
                principalTable: "Bank",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BrokerAccount_Bank_BankId",
                table: "BrokerAccount",
                column: "BankId",
                principalTable: "Bank",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Deposit_Bank_BankId",
                table: "Deposit",
                column: "BankId",
                principalTable: "Bank",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Bank_BankId",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_BrokerAccount_Bank_BankId",
                table: "BrokerAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_Deposit_Bank_BankId",
                table: "Deposit");

            migrationBuilder.DropTable(
                name: "Bank");

            migrationBuilder.DropIndex(
                name: "IX_Deposit_BankId",
                table: "Deposit");

            migrationBuilder.DropIndex(
                name: "IX_BrokerAccount_BankId",
                table: "BrokerAccount");

            migrationBuilder.DropIndex(
                name: "IX_Account_BankId",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "BankId",
                table: "Deposit");

            migrationBuilder.DropColumn(
                name: "BankId",
                table: "BrokerAccount");

            migrationBuilder.DropColumn(
                name: "BankId",
                table: "Account");
        }
    }
}
