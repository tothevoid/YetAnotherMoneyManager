using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBrokerAccountTaxDeduction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BrokerAccountTaxDeduction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    DateApplied = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BrokerAccountId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrokerAccountTaxDeduction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrokerAccountTaxDeduction_BrokerAccount_BrokerAccountId",
                        column: x => x.BrokerAccountId,
                        principalTable: "BrokerAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrokerAccountTaxDeduction_BrokerAccountId",
                table: "BrokerAccountTaxDeduction",
                column: "BrokerAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrokerAccountTaxDeduction");
        }
    }
}
