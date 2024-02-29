using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingAlpha.App.Migrations
{
    /// <inheritdoc />
    public partial class Changedtransactionflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Transactions");

            migrationBuilder.AddColumn<decimal>(
                name: "AtPrice",
                table: "Transactions",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AtPrice",
                table: "Transactions");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Transactions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
