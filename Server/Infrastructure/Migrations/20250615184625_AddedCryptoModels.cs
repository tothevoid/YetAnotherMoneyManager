using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedCryptoModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cryptocurrency",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Symbol = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    IconKey = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cryptocurrency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CryptoProvider",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CryptoProvider", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CryptoAccount",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    CryptoProviderId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CryptoAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CryptoAccount_CryptoProvider_CryptoProviderId",
                        column: x => x.CryptoProviderId,
                        principalTable: "CryptoProvider",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CryptoAccountCryptocurrency",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CrpytocurrencyId = table.Column<Guid>(type: "uuid", nullable: false),
                    CryptoAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CryptoAccountCryptocurrency", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CryptoAccountCryptocurrency_CryptoAccount_CryptoAccountId",
                        column: x => x.CryptoAccountId,
                        principalTable: "CryptoAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CryptoAccountCryptocurrency_Cryptocurrency_CrpytocurrencyId",
                        column: x => x.CrpytocurrencyId,
                        principalTable: "Cryptocurrency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CryptoAccount_CryptoProviderId",
                table: "CryptoAccount",
                column: "CryptoProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_CryptoAccountCryptocurrency_CrpytocurrencyId",
                table: "CryptoAccountCryptocurrency",
                column: "CrpytocurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CryptoAccountCryptocurrency_CryptoAccountId",
                table: "CryptoAccountCryptocurrency",
                column: "CryptoAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CryptoAccountCryptocurrency");

            migrationBuilder.DropTable(
                name: "CryptoAccount");

            migrationBuilder.DropTable(
                name: "Cryptocurrency");

            migrationBuilder.DropTable(
                name: "CryptoProvider");
        }
    }
}
