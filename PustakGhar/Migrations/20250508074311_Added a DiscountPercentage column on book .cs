using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlishPustakGhar.Migrations
{
    /// <inheritdoc />
    public partial class AddedaDiscountPercentagecolumnonbook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GenreType",
                table: "Genre",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "DiscountPercentage",
                table: "Book",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountPercentage",
                table: "Book");

            migrationBuilder.AlterColumn<int>(
                name: "GenreType",
                table: "Genre",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
