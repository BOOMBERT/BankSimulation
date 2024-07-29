using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankSimulation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexToEmailAndRenameColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Deleted",
                table: "Users",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Users",
                newName: "CreationDate");

            migrationBuilder.RenameColumn(
                name: "Deleted",
                table: "BankAccounts",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "BankAccounts",
                newName: "CreationDate");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Users",
                newName: "Deleted");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Users",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "BankAccounts",
                newName: "Deleted");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "BankAccounts",
                newName: "Created");
        }
    }
}
