using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SecurityTransactionDataEnhancement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Commission",
                table: "SecurityTransaction",
                newName: "StockExchangeCommission");

            migrationBuilder.AddColumn<decimal>(
                name: "BrokerCommission",
                table: "SecurityTransaction",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "SecurityType",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("2ccd0a40-4339-4be7-9f4d-fbf5c31b20c2"), "Currency" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SecurityType",
                keyColumn: "Id",
                keyValue: new Guid("2ccd0a40-4339-4be7-9f4d-fbf5c31b20c2"));

            migrationBuilder.DropColumn(
                name: "BrokerCommission",
                table: "SecurityTransaction");

            migrationBuilder.RenameColumn(
                name: "StockExchangeCommission",
                table: "SecurityTransaction",
                newName: "Commission");
        }
    }
}
