using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAcc.Migrations
{
    public partial class OrderDetailUnitChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "OrderDetails");

            migrationBuilder.AddColumn<string>(
                name: "Unitname",
                table: "OrderDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Unitname",
                table: "OrderDetails");

            migrationBuilder.AddColumn<decimal>(
                name: "UnitId",
                table: "OrderDetails",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
