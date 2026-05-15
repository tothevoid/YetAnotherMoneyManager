using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPreciousMetalSecurityType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SecurityType",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("2deb13a0-c49a-4589-83e5-52df6ac46174"), "Precious metal" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SecurityType",
                keyColumn: "Id",
                keyValue: new Guid("2deb13a0-c49a-4589-83e5-52df6ac46174"));
        }
    }
}
