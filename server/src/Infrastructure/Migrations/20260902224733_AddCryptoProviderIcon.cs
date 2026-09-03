using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Audex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCryptoProviderIcon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IconKey",
                table: "CryptoProvider",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconKey",
                table: "CryptoProvider");
        }
    }
}
