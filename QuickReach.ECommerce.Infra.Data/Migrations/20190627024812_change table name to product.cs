using Microsoft.EntityFrameworkCore.Migrations;

namespace QuickReach.ECommerce.Infra.Data.Migrations
{
    public partial class changetablenametoproduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prdct_Category_CategoryID",
                table: "Prdct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Prdct",
                table: "Prdct");

            migrationBuilder.RenameTable(
                name: "Prdct",
                newName: "Product");

            migrationBuilder.RenameIndex(
                name: "IX_Prdct_CategoryID",
                table: "Product",
                newName: "IX_Product_CategoryID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product",
                table: "Product",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Category_CategoryID",
                table: "Product",
                column: "CategoryID",
                principalTable: "Category",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Category_CategoryID",
                table: "Product");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Product",
                table: "Product");

            migrationBuilder.RenameTable(
                name: "Product",
                newName: "Prdct");

            migrationBuilder.RenameIndex(
                name: "IX_Product_CategoryID",
                table: "Prdct",
                newName: "IX_Prdct_CategoryID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Prdct",
                table: "Prdct",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Prdct_Category_CategoryID",
                table: "Prdct",
                column: "CategoryID",
                principalTable: "Category",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
