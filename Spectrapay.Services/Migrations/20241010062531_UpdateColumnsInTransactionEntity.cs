using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spectrapay.Services.Migrations
{
    /// <inheritdoc />
    public partial class UpdateColumnsInTransactionEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Transactions",
                newName: "SenderAcctId");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "Transactions",
                newName: "ReceiverAcctId");

            migrationBuilder.AddColumn<int>(
                name: "ReceiverIdNum",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SenderIdNum",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiverIdNum",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SenderIdNum",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "SenderAcctId",
                table: "Transactions",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "ReceiverAcctId",
                table: "Transactions",
                newName: "ReceiverId");
        }
    }
}
