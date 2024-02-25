using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingAlpha.App.Migrations
{
    /// <inheritdoc />
    public partial class Restructurefortransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddedToPortfolio",
                table: "PortfolioEntries");

            migrationBuilder.DropColumn(
                name: "CryptoEntry_Amount",
                table: "PortfolioEntries");

            migrationBuilder.DropColumn(
                name: "CryptoEntry_CurrentPrice",
                table: "PortfolioEntries");

            migrationBuilder.DropColumn(
                name: "CryptoEntry_LastUpdate",
                table: "PortfolioEntries");

            migrationBuilder.DropColumn(
                name: "CryptoEntry_Name",
                table: "PortfolioEntries");

            migrationBuilder.DropColumn(
                name: "CryptoEntry_PriceBoughtAt",
                table: "PortfolioEntries");

            migrationBuilder.DropColumn(
                name: "PriceBoughtAt",
                table: "PortfolioEntries");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Transactions",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Transactions",
                type: "TEXT",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Transactions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Symbol",
                table: "Transactions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Transactions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Transactions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdate",
                table: "PortfolioEntries",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentPrice",
                table: "PortfolioEntries",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "PortfolioEntries",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserId",
                table: "Transactions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AspNetUsers_UserId",
                table: "Transactions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AspNetUsers_UserId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_UserId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Symbol",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Transactions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdate",
                table: "PortfolioEntries",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentPrice",
                table: "PortfolioEntries",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "PortfolioEntries",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AddColumn<DateTime>(
                name: "AddedToPortfolio",
                table: "PortfolioEntries",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "CryptoEntry_Amount",
                table: "PortfolioEntries",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CryptoEntry_CurrentPrice",
                table: "PortfolioEntries",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CryptoEntry_LastUpdate",
                table: "PortfolioEntries",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CryptoEntry_Name",
                table: "PortfolioEntries",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CryptoEntry_PriceBoughtAt",
                table: "PortfolioEntries",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceBoughtAt",
                table: "PortfolioEntries",
                type: "TEXT",
                nullable: true);
        }
    }
}
