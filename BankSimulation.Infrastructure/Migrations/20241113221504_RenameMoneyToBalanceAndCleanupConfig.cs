using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankSimulation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameMoneyToBalanceAndCleanupConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_Users_UserId",
                table: "BankAccounts");

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

            migrationBuilder.RenameColumn(
                name: "Money",
                table: "BankAccounts",
                newName: "Balance");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Withdraws",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(254)",
                maxLength: 254,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Transfers",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Answer",
                table: "SecurityQuestions",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "RefreshTokens",
                type: "nvarchar(88)",
                maxLength: 88,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Deposits",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "BankAccounts",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_Users_UserId",
                table: "BankAccounts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Deposits_BankAccounts_BankAccountNumber",
                table: "Deposits",
                column: "BankAccountNumber",
                principalTable: "BankAccounts",
                principalColumn: "Number",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_BankAccounts_RecipientBankAccountNumber",
                table: "Transfers",
                column: "RecipientBankAccountNumber",
                principalTable: "BankAccounts",
                principalColumn: "Number",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_BankAccounts_SenderBankAccountNumber",
                table: "Transfers",
                column: "SenderBankAccountNumber",
                principalTable: "BankAccounts",
                principalColumn: "Number",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Withdraws_BankAccounts_BankAccountNumber",
                table: "Withdraws",
                column: "BankAccountNumber",
                principalTable: "BankAccounts",
                principalColumn: "Number",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_Users_UserId",
                table: "BankAccounts");

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

            migrationBuilder.RenameColumn(
                name: "Balance",
                table: "BankAccounts",
                newName: "Money");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Withdraws",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(254)",
                oldMaxLength: 254);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "Users",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Transfers",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "Answer",
                table: "SecurityQuestions",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "RefreshTokens",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(88)",
                oldMaxLength: 88);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Deposits",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "BankAccounts",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_Users_UserId",
                table: "BankAccounts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

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
    }
}
