using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedTransactionTypeIcon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IconKey",
                table: "TransactionType",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "TransactionType",
                keyColumn: "Id",
                keyValue: new Guid("fe21a187-2963-43d0-b27e-84f372504028"),
                column: "IconKey",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconKey",
                table: "TransactionType");
        }
    }
}
