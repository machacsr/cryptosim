using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoSim.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TransactionType",
                table: "CryptoTransactions",
                type: "nvarchar(4)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "CryptoListings",
                type: "nvarchar(8)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TransactionType",
                table: "CryptoTransactions",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(4)");

            migrationBuilder.AlterColumn<int>(
                name: "State",
                table: "CryptoListings",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(8)");
        }
    }
}
