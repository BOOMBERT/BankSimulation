using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankSimulation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SetBankAccountIdAsAccountNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deposits_BankAccounts_BankAccountId",
                table: "Deposits");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_BankAccounts_RecipientBankAccountId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_BankAccounts_SenderBankAccountId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Withdraws_BankAccounts_BankAccountId",
                table: "Withdraws");

            migrationBuilder.DropIndex(
                name: "IX_Withdraws_BankAccountId",
                table: "Withdraws");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_RecipientBankAccountId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_SenderBankAccountId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Deposits_BankAccountId",
                table: "Deposits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BankAccounts",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "BankAccountId",
                table: "Withdraws");

            migrationBuilder.DropColumn(
                name: "RecipientBankAccountId",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "SenderBankAccountId",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "BankAccountId",
                table: "Deposits");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "BankAccounts");

            migrationBuilder.AddColumn<string>(
                name: "BankAccountNumber",
                table: "Withdraws",
                type: "nvarchar(6)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RecipientBankAccountNumber",
                table: "Transfers",
                type: "nvarchar(6)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SenderBankAccountNumber",
                table: "Transfers",
                type: "nvarchar(6)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BankAccountNumber",
                table: "Deposits",
                type: "nvarchar(6)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "BankAccounts",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(26)",
                oldMaxLength: 26);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BankAccounts",
                table: "BankAccounts",
                column: "Number");

            migrationBuilder.CreateIndex(
                name: "IX_Withdraws_BankAccountNumber",
                table: "Withdraws",
                column: "BankAccountNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_RecipientBankAccountNumber",
                table: "Transfers",
                column: "RecipientBankAccountNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_SenderBankAccountNumber",
                table: "Transfers",
                column: "SenderBankAccountNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Deposits_BankAccountNumber",
                table: "Deposits",
                column: "BankAccountNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Deposits_BankAccounts_BankAccountNumber",
                table: "Deposits",
                column: "BankAccountNumber",
                principalTable: "BankAccounts",
                principalColumn: "Number");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_BankAccounts_RecipientBankAccountNumber",
                table: "Transfers",
                column: "RecipientBankAccountNumber",
                principalTable: "BankAccounts",
                principalColumn: "Number");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_BankAccounts_SenderBankAccountNumber",
                table: "Transfers",
                column: "SenderBankAccountNumber",
                principalTable: "BankAccounts",
                principalColumn: "Number");

            migrationBuilder.AddForeignKey(
                name: "FK_Withdraws_BankAccounts_BankAccountNumber",
                table: "Withdraws",
                column: "BankAccountNumber",
                principalTable: "BankAccounts",
                principalColumn: "Number");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deposits_BankAccounts_BankAccountNumber",
                table: "Deposits");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_BankAccounts_RecipientBankAccountNumber",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_BankAccounts_SenderBankAccountNumber",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Withdraws_BankAccounts_BankAccountNumber",
                table: "Withdraws");

            migrationBuilder.DropIndex(
                name: "IX_Withdraws_BankAccountNumber",
                table: "Withdraws");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_RecipientBankAccountNumber",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_SenderBankAccountNumber",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Deposits_BankAccountNumber",
                table: "Deposits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BankAccounts",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "BankAccountNumber",
                table: "Withdraws");

            migrationBuilder.DropColumn(
                name: "RecipientBankAccountNumber",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "SenderBankAccountNumber",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "BankAccountNumber",
                table: "Deposits");

            migrationBuilder.AddColumn<Guid>(
                name: "BankAccountId",
                table: "Withdraws",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "RecipientBankAccountId",
                table: "Transfers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SenderBankAccountId",
                table: "Transfers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BankAccountId",
                table: "Deposits",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "BankAccounts",
                type: "nvarchar(26)",
                maxLength: 26,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(6)",
                oldMaxLength: 6);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "BankAccounts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_BankAccounts",
                table: "BankAccounts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Withdraws_BankAccountId",
                table: "Withdraws",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_RecipientBankAccountId",
                table: "Transfers",
                column: "RecipientBankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_SenderBankAccountId",
                table: "Transfers",
                column: "SenderBankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Deposits_BankAccountId",
                table: "Deposits",
                column: "BankAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deposits_BankAccounts_BankAccountId",
                table: "Deposits",
                column: "BankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_BankAccounts_RecipientBankAccountId",
                table: "Transfers",
                column: "RecipientBankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_BankAccounts_SenderBankAccountId",
                table: "Transfers",
                column: "SenderBankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Withdraws_BankAccounts_BankAccountId",
                table: "Withdraws",
                column: "BankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id");
        }
    }
}
