using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MoneyManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AccountType",
                columns: new[] { "Id", "Active", "Name" },
                values: new object[,]
                {
                    { new Guid("6ea1867f-c067-412c-b443-8b9bc2467202"), true, "Credit card" },
                    { new Guid("a08f5553-379e-4294-a2e5-75e88219433c"), true, "Cash" },
                    { new Guid("b9dcea63-49f4-47bf-ae7b-eb596479ba57"), true, "Deposit account" },
                    { new Guid("cda2ce07-551e-48cf-988d-270c0d022866"), true, "Debit card" }
                });

            migrationBuilder.InsertData(
                table: "Currency",
                columns: new[] { "Id", "Active", "Name" },
                values: new object[,]
                {
                    { new Guid("08762100-b4e0-4a70-b48c-dd9e9411c4a2"), true, "RUB" },
                    { new Guid("4c073dc4-20be-44d4-80c9-3f09d4ac12ef"), true, "EUR" },
                    { new Guid("c7f31af3-5091-439c-8854-c90872420ecf"), true, "USD" }
                });

            migrationBuilder.InsertData(
                table: "SecurityType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("16184209-1716-4854-a293-75776e1b4ec0"), "Bond" },
                    { new Guid("209dcb50-989f-44f7-b886-0d7f5c763593"), "Investment fund unit" },
                    { new Guid("23b0a73a-9ac1-4fb5-a763-3c10424ed798"), "Stock" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AccountType",
                keyColumn: "Id",
                keyValue: new Guid("6ea1867f-c067-412c-b443-8b9bc2467202"));

            migrationBuilder.DeleteData(
                table: "AccountType",
                keyColumn: "Id",
                keyValue: new Guid("a08f5553-379e-4294-a2e5-75e88219433c"));

            migrationBuilder.DeleteData(
                table: "AccountType",
                keyColumn: "Id",
                keyValue: new Guid("b9dcea63-49f4-47bf-ae7b-eb596479ba57"));

            migrationBuilder.DeleteData(
                table: "AccountType",
                keyColumn: "Id",
                keyValue: new Guid("cda2ce07-551e-48cf-988d-270c0d022866"));

            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("08762100-b4e0-4a70-b48c-dd9e9411c4a2"));

            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("4c073dc4-20be-44d4-80c9-3f09d4ac12ef"));

            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyValue: new Guid("c7f31af3-5091-439c-8854-c90872420ecf"));

            migrationBuilder.DeleteData(
                table: "SecurityType",
                keyColumn: "Id",
                keyValue: new Guid("16184209-1716-4854-a293-75776e1b4ec0"));

            migrationBuilder.DeleteData(
                table: "SecurityType",
                keyColumn: "Id",
                keyValue: new Guid("209dcb50-989f-44f7-b886-0d7f5c763593"));

            migrationBuilder.DeleteData(
                table: "SecurityType",
                keyColumn: "Id",
                keyValue: new Guid("23b0a73a-9ac1-4fb5-a763-3c10424ed798"));
        }
    }
}
