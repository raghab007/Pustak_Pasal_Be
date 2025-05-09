using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlishPustakGhar.Migrations
{
    /// <inheritdoc />
    public partial class AddFavouritesentityv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartBook_Book_BookId",
                table: "CartBook");

            migrationBuilder.DropForeignKey(
                name: "FK_CartBook_Cart_CartId",
                table: "CartBook");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartBook",
                table: "CartBook");

            migrationBuilder.RenameTable(
                name: "CartBook",
                newName: "CartBooks");

            migrationBuilder.RenameIndex(
                name: "IX_CartBook_CartId",
                table: "CartBooks",
                newName: "IX_CartBooks_CartId");

            migrationBuilder.RenameIndex(
                name: "IX_CartBook_BookId",
                table: "CartBooks",
                newName: "IX_CartBooks_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartBooks",
                table: "CartBooks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartBooks_Book_BookId",
                table: "CartBooks",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartBooks_Cart_CartId",
                table: "CartBooks",
                column: "CartId",
                principalTable: "Cart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartBooks_Book_BookId",
                table: "CartBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_CartBooks_Cart_CartId",
                table: "CartBooks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartBooks",
                table: "CartBooks");

            migrationBuilder.RenameTable(
                name: "CartBooks",
                newName: "CartBook");

            migrationBuilder.RenameIndex(
                name: "IX_CartBooks_CartId",
                table: "CartBook",
                newName: "IX_CartBook_CartId");

            migrationBuilder.RenameIndex(
                name: "IX_CartBooks_BookId",
                table: "CartBook",
                newName: "IX_CartBook_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartBook",
                table: "CartBook",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartBook_Book_BookId",
                table: "CartBook",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartBook_Cart_CartId",
                table: "CartBook",
                column: "CartId",
                principalTable: "Cart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
