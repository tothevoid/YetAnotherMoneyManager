using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedSecurityColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentPrice",
                table: "BrokerAccountSecurity");

            migrationBuilder.AddColumn<decimal>(
                name: "ActualPrice",
                table: "Security",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "PriceFetchedAt",
                table: "Security",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualPrice",
                table: "Security");

            migrationBuilder.DropColumn(
                name: "PriceFetchedAt",
                table: "Security");

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentPrice",
                table: "BrokerAccountSecurity",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
