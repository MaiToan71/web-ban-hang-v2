using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class price_to_attribute_quantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Prince",
                table: "ProductAttributes",
                newName: "Price");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ProductAttributes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ProductAttributes");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "ProductAttributes",
                newName: "Prince");
        }
    }
}
