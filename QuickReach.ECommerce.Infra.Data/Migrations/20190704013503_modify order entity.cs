using Microsoft.EntityFrameworkCore.Migrations;

namespace QuickReach.ECommerce.Infra.Data.Migrations
{
    public partial class modifyorderentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_Order_OrderID",
                table: "OrderItem");

            migrationBuilder.RenameColumn(
                name: "OrderID",
                table: "OrderItem",
                newName: "CartId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItem_OrderID",
                table: "OrderItem",
                newName: "IX_OrderItem_CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_Order_CartId",
                table: "OrderItem",
                column: "CartId",
                principalTable: "Order",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_Order_CartId",
                table: "OrderItem");

            migrationBuilder.RenameColumn(
                name: "CartId",
                table: "OrderItem",
                newName: "OrderID");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItem_CartId",
                table: "OrderItem",
                newName: "IX_OrderItem_OrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_Order_OrderID",
                table: "OrderItem",
                column: "OrderID",
                principalTable: "Order",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
