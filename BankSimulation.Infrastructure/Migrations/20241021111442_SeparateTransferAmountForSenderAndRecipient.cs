using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankSimulation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeparateTransferAmountForSenderAndRecipient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Transfers",
                newName: "SenderAmount");

            migrationBuilder.AddColumn<decimal>(
                name: "RecipientAmount",
                table: "Transfers",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipientAmount",
                table: "Transfers");

            migrationBuilder.RenameColumn(
                name: "SenderAmount",
                table: "Transfers",
                newName: "Amount");
        }
    }
}
