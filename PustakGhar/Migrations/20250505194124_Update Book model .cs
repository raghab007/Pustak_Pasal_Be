using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlishPustakGhar.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageURL",
                table: "Book",
                newName: "FrontImage");

            migrationBuilder.AddColumn<string>(
                name: "BackImage",
                table: "Book",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackImage",
                table: "Book");

            migrationBuilder.RenameColumn(
                name: "FrontImage",
                table: "Book",
                newName: "ImageURL");
        }
    }
}
