using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnoShop.Domain.Migrations
{
    /// <inheritdoc />
    public partial class NamesUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "UQ__ProdType__5E55825B2B853D4K",
                table: "ProdTypes",
                column: "Name",
                unique: true 
                );

            migrationBuilder.CreateIndex(
                name: "UQ__Localiti__5E55825B2B853D4G",
                table: "Localities",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UQ__ProdType__5E55825B2B853D4K",
                table: "ProdTypes");

            migrationBuilder.DropIndex(
                name: "UQ__Localiti__5E55825B2B853D4G",
                table: "Localities");
        }
    }
}
