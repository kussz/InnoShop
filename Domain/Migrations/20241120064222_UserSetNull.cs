using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnoShop.Domain.Migrations
{
    /// <inheritdoc />
    public partial class UserSetNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Users__LocalityI__056ECC6A",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "LocalityId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK__Users__LocalityI__056ECC6A",
                table: "AspNetUsers",
                column: "LocalityId",
                principalTable: "Localities",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Users__LocalityI__056ECC6A",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "LocalityId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK__Users__LocalityI__056ECC6A",
                table: "AspNetUsers",
                column: "LocalityId",
                principalTable: "Localities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
