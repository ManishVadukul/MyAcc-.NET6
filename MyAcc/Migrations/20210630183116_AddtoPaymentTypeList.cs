using Microsoft.EntityFrameworkCore.Migrations;

namespace MyAcc.Migrations
{
    public partial class AddtoPaymentTypeList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Products_ProductId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTypeList_Orders_OrderId",
                table: "PaymentTypeList");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentTypeList",
                table: "PaymentTypeList");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "OrderDetails");

            migrationBuilder.RenameTable(
                name: "PaymentTypeList",
                newName: "PaymentTypes");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentTypeList_OrderId",
                table: "PaymentTypes",
                newName: "IX_PaymentTypes_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentTypes",
                table: "PaymentTypes",
                column: "PaymentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTypes_Orders_OrderId",
                table: "PaymentTypes",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentTypes_Orders_OrderId",
                table: "PaymentTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentTypes",
                table: "PaymentTypes");

            migrationBuilder.RenameTable(
                name: "PaymentTypes",
                newName: "PaymentTypeList");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentTypes_OrderId",
                table: "PaymentTypeList",
                newName: "IX_PaymentTypeList_OrderId");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "OrderDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentTypeList",
                table: "PaymentTypeList",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Products_ProductId",
                table: "OrderDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentTypeList_Orders_OrderId",
                table: "PaymentTypeList",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
