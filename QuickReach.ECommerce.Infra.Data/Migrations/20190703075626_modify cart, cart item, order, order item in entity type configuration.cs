using Microsoft.EntityFrameworkCore.Migrations;

namespace QuickReach.ECommerce.Infra.Data.Migrations
{
    public partial class modifycartcartitemorderorderiteminentitytypeconfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItem_Cart_CartID",
                table: "CartItem");

            migrationBuilder.RenameColumn(
                name: "CartID",
                table: "CartItem",
                newName: "CartId");

            migrationBuilder.RenameIndex(
                name: "IX_CartItem_CartID",
                table: "CartItem",
                newName: "IX_CartItem_CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItem_Cart_CartId",
                table: "CartItem",
                column: "CartId",
                principalTable: "Cart",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItem_Cart_CartId",
                table: "CartItem");

            migrationBuilder.RenameColumn(
                name: "CartId",
                table: "CartItem",
                newName: "CartID");

            migrationBuilder.RenameIndex(
                name: "IX_CartItem_CartId",
                table: "CartItem",
                newName: "IX_CartItem_CartID");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItem_Cart_CartID",
                table: "CartItem",
                column: "CartID",
                principalTable: "Cart",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
