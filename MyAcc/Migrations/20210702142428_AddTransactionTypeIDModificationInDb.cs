using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAcc.Migrations
{
    public partial class AddTransactionTypeIDModificationInDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransactionType",
                table: "Transaction",
                newName: "TransactionTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransactionTypeId",
                table: "Transaction",
                newName: "TransactionType");
        }
    }
}
