using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAcc.Migrations
{
    public partial class AddTransactionTypeIDToTypeModificationInDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionType");

            migrationBuilder.DropColumn(
                name: "TransactionTypeId",
                table: "Transaction");

            migrationBuilder.AddColumn<string>(
                name: "TransactionType",
                table: "Transaction",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionType",
                table: "Transaction");

            migrationBuilder.AddColumn<int>(
                name: "TransactionTypeId",
                table: "Transaction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TransactionType",
                columns: table => new
                {
                    TransactionTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionType", x => x.TransactionTypeId);
                });
        }
    }
}
