using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DisabledCascadeRemoval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_AccountType_AccountTypeId",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_Account_Currency_CurrencyId",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_BrokerAccount_BrokerAccountType_TypeId",
                table: "BrokerAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_BrokerAccount_Broker_BrokerId",
                table: "BrokerAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_BrokerAccount_Currency_CurrencyId",
                table: "BrokerAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_BrokerAccountSecurity_BrokerAccount_BrokerAccountId",
                table: "BrokerAccountSecurity");

            migrationBuilder.DropForeignKey(
                name: "FK_BrokerAccountSecurity_Security_SecurityId",
                table: "BrokerAccountSecurity");

            migrationBuilder.DropForeignKey(
                name: "FK_Debt_Currency_CurrencyId",
                table: "Debt");

            migrationBuilder.DropForeignKey(
                name: "FK_DebtPayment_Account_TargetAccountId",
                table: "DebtPayment");

            migrationBuilder.DropForeignKey(
                name: "FK_DebtPayment_Debt_DebtId",
                table: "DebtPayment");

            migrationBuilder.DropForeignKey(
                name: "FK_DebtPayment_Transaction_TransactionId",
                table: "DebtPayment");

            migrationBuilder.DropForeignKey(
                name: "FK_Deposit_Account_AccountId",
                table: "Deposit");

            migrationBuilder.DropForeignKey(
                name: "FK_Dividend_Security_SecurityId",
                table: "Dividend");

            migrationBuilder.DropForeignKey(
                name: "FK_Security_Currency_CurrencyId",
                table: "Security");

            migrationBuilder.DropForeignKey(
                name: "FK_Security_SecurityType_TypeId",
                table: "Security");

            migrationBuilder.DropForeignKey(
                name: "FK_SecurityTransaction_BrokerAccount_BrokerAccountId",
                table: "SecurityTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_SecurityTransaction_Security_SecurityId",
                table: "SecurityTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Account_AccountId",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_TransactionType_TransactionTypeId",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfile_Currency_CurrencyId",
                table: "UserProfile");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_AccountType_AccountTypeId",
                table: "Account",
                column: "AccountTypeId",
                principalTable: "AccountType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Currency_CurrencyId",
                table: "Account",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BrokerAccount_BrokerAccountType_TypeId",
                table: "BrokerAccount",
                column: "TypeId",
                principalTable: "BrokerAccountType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BrokerAccount_Broker_BrokerId",
                table: "BrokerAccount",
                column: "BrokerId",
                principalTable: "Broker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BrokerAccount_Currency_CurrencyId",
                table: "BrokerAccount",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BrokerAccountSecurity_BrokerAccount_BrokerAccountId",
                table: "BrokerAccountSecurity",
                column: "BrokerAccountId",
                principalTable: "BrokerAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BrokerAccountSecurity_Security_SecurityId",
                table: "BrokerAccountSecurity",
                column: "SecurityId",
                principalTable: "Security",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Debt_Currency_CurrencyId",
                table: "Debt",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DebtPayment_Account_TargetAccountId",
                table: "DebtPayment",
                column: "TargetAccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DebtPayment_Debt_DebtId",
                table: "DebtPayment",
                column: "DebtId",
                principalTable: "Debt",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DebtPayment_Transaction_TransactionId",
                table: "DebtPayment",
                column: "TransactionId",
                principalTable: "Transaction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Deposit_Account_AccountId",
                table: "Deposit",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Dividend_Security_SecurityId",
                table: "Dividend",
                column: "SecurityId",
                principalTable: "Security",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Security_Currency_CurrencyId",
                table: "Security",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Security_SecurityType_TypeId",
                table: "Security",
                column: "TypeId",
                principalTable: "SecurityType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SecurityTransaction_BrokerAccount_BrokerAccountId",
                table: "SecurityTransaction",
                column: "BrokerAccountId",
                principalTable: "BrokerAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SecurityTransaction_Security_SecurityId",
                table: "SecurityTransaction",
                column: "SecurityId",
                principalTable: "Security",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Account_AccountId",
                table: "Transaction",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_TransactionType_TransactionTypeId",
                table: "Transaction",
                column: "TransactionTypeId",
                principalTable: "TransactionType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfile_Currency_CurrencyId",
                table: "UserProfile",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_AccountType_AccountTypeId",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_Account_Currency_CurrencyId",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_BrokerAccount_BrokerAccountType_TypeId",
                table: "BrokerAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_BrokerAccount_Broker_BrokerId",
                table: "BrokerAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_BrokerAccount_Currency_CurrencyId",
                table: "BrokerAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_BrokerAccountSecurity_BrokerAccount_BrokerAccountId",
                table: "BrokerAccountSecurity");

            migrationBuilder.DropForeignKey(
                name: "FK_BrokerAccountSecurity_Security_SecurityId",
                table: "BrokerAccountSecurity");

            migrationBuilder.DropForeignKey(
                name: "FK_Debt_Currency_CurrencyId",
                table: "Debt");

            migrationBuilder.DropForeignKey(
                name: "FK_DebtPayment_Account_TargetAccountId",
                table: "DebtPayment");

            migrationBuilder.DropForeignKey(
                name: "FK_DebtPayment_Debt_DebtId",
                table: "DebtPayment");

            migrationBuilder.DropForeignKey(
                name: "FK_DebtPayment_Transaction_TransactionId",
                table: "DebtPayment");

            migrationBuilder.DropForeignKey(
                name: "FK_Deposit_Account_AccountId",
                table: "Deposit");

            migrationBuilder.DropForeignKey(
                name: "FK_Dividend_Security_SecurityId",
                table: "Dividend");

            migrationBuilder.DropForeignKey(
                name: "FK_Security_Currency_CurrencyId",
                table: "Security");

            migrationBuilder.DropForeignKey(
                name: "FK_Security_SecurityType_TypeId",
                table: "Security");

            migrationBuilder.DropForeignKey(
                name: "FK_SecurityTransaction_BrokerAccount_BrokerAccountId",
                table: "SecurityTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_SecurityTransaction_Security_SecurityId",
                table: "SecurityTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Account_AccountId",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_TransactionType_TransactionTypeId",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfile_Currency_CurrencyId",
                table: "UserProfile");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_AccountType_AccountTypeId",
                table: "Account",
                column: "AccountTypeId",
                principalTable: "AccountType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Currency_CurrencyId",
                table: "Account",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BrokerAccount_BrokerAccountType_TypeId",
                table: "BrokerAccount",
                column: "TypeId",
                principalTable: "BrokerAccountType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BrokerAccount_Broker_BrokerId",
                table: "BrokerAccount",
                column: "BrokerId",
                principalTable: "Broker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BrokerAccount_Currency_CurrencyId",
                table: "BrokerAccount",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BrokerAccountSecurity_BrokerAccount_BrokerAccountId",
                table: "BrokerAccountSecurity",
                column: "BrokerAccountId",
                principalTable: "BrokerAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BrokerAccountSecurity_Security_SecurityId",
                table: "BrokerAccountSecurity",
                column: "SecurityId",
                principalTable: "Security",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Debt_Currency_CurrencyId",
                table: "Debt",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DebtPayment_Account_TargetAccountId",
                table: "DebtPayment",
                column: "TargetAccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DebtPayment_Debt_DebtId",
                table: "DebtPayment",
                column: "DebtId",
                principalTable: "Debt",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DebtPayment_Transaction_TransactionId",
                table: "DebtPayment",
                column: "TransactionId",
                principalTable: "Transaction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Deposit_Account_AccountId",
                table: "Deposit",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Dividend_Security_SecurityId",
                table: "Dividend",
                column: "SecurityId",
                principalTable: "Security",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Security_Currency_CurrencyId",
                table: "Security",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Security_SecurityType_TypeId",
                table: "Security",
                column: "TypeId",
                principalTable: "SecurityType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SecurityTransaction_BrokerAccount_BrokerAccountId",
                table: "SecurityTransaction",
                column: "BrokerAccountId",
                principalTable: "BrokerAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SecurityTransaction_Security_SecurityId",
                table: "SecurityTransaction",
                column: "SecurityId",
                principalTable: "Security",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Account_AccountId",
                table: "Transaction",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_TransactionType_TransactionTypeId",
                table: "Transaction",
                column: "TransactionTypeId",
                principalTable: "TransactionType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfile_Currency_CurrencyId",
                table: "UserProfile",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
