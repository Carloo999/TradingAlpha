using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradingAlpha.App.Migrations
{
    /// <inheritdoc />
    public partial class Addedtransactionbuyandsellcategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TransactionBaseType",
                table: "Transactions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionBaseType",
                table: "Transactions");
        }
    }
}
