using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveScheduledTaskAttachmentFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledTaskAttachment_CronTickerOccurrences_OccurrenceId",
                table: "ScheduledTaskAttachment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledTaskAttachment_CronTickerOccurrences_OccurrenceId",
                table: "ScheduledTaskAttachment",
                column: "OccurrenceId",
                principalSchema: "ticker",
                principalTable: "CronTickerOccurrences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
