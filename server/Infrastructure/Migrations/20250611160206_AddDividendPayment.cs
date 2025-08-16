using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDividendPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DividendPayment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BrokerAccountId = table.Column<Guid>(type: "uuid", nullable: false),
                    DividendId = table.Column<Guid>(type: "uuid", nullable: false),
                    SecuritiesQuantity = table.Column<int>(type: "integer", nullable: false),
                    Tax = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DividendPayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DividendPayment_BrokerAccount_BrokerAccountId",
                        column: x => x.BrokerAccountId,
                        principalTable: "BrokerAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DividendPayment_Dividend_BrokerAccountId",
                        column: x => x.BrokerAccountId,
                        principalTable: "Dividend",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DividendPayment_BrokerAccountId",
                table: "DividendPayment",
                column: "BrokerAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DividendPayment");
        }
    }
}
