using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnoShop.Domain.Migrations
{
    /// <inheritdoc />
    public partial class ProdTypeSetNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Products__ProdTy__084B3915",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "ProdTypeId",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK__Products__ProdTy__084B3915",
                table: "Products",
                column: "ProdTypeId",
                principalTable: "ProdTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Products__ProdTy__084B3915",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "ProdTypeId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK__Products__ProdTy__084B3915",
                table: "Products",
                column: "ProdTypeId",
                principalTable: "ProdTypes",
                principalColumn: "Id");
        }
    }
}
