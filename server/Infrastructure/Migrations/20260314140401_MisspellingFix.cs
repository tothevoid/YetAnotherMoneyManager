using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MisspellingFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CryptoAccountCryptocurrency_Cryptocurrency_CrpytocurrencyId",
                table: "CryptoAccountCryptocurrency");

            migrationBuilder.RenameColumn(
                name: "CrpytocurrencyId",
                table: "CryptoAccountCryptocurrency",
                newName: "CryptocurrencyId");

            migrationBuilder.RenameIndex(
                name: "IX_CryptoAccountCryptocurrency_CrpytocurrencyId",
                table: "CryptoAccountCryptocurrency",
                newName: "IX_CryptoAccountCryptocurrency_CryptocurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CryptoAccountCryptocurrency_Cryptocurrency_CryptocurrencyId",
                table: "CryptoAccountCryptocurrency",
                column: "CryptocurrencyId",
                principalTable: "Cryptocurrency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CryptoAccountCryptocurrency_Cryptocurrency_CryptocurrencyId",
                table: "CryptoAccountCryptocurrency");

            migrationBuilder.RenameColumn(
                name: "CryptocurrencyId",
                table: "CryptoAccountCryptocurrency",
                newName: "CrpytocurrencyId");

            migrationBuilder.RenameIndex(
                name: "IX_CryptoAccountCryptocurrency_CryptocurrencyId",
                table: "CryptoAccountCryptocurrency",
                newName: "IX_CryptoAccountCryptocurrency_CrpytocurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CryptoAccountCryptocurrency_Cryptocurrency_CrpytocurrencyId",
                table: "CryptoAccountCryptocurrency",
                column: "CrpytocurrencyId",
                principalTable: "Cryptocurrency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
