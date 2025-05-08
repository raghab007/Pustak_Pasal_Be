using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlishPustakGhar.Migrations
{
    /// <inheritdoc />
    public partial class removeImageUrlcolumnfromAuthor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "Author");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "Author",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
