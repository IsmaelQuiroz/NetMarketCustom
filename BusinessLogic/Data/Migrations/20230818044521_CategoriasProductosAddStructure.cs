using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessLogic.Data.Migrations
{
    public partial class CategoriasProductosAddStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoriaProductoId",
                table: "Venta",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CategoriaProducto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Porcentaje = table.Column<decimal>(type: "decimal(18,4)", nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaProducto", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Venta_CategoriaProductoId",
                table: "Venta",
                column: "CategoriaProductoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Venta_CategoriaProducto_CategoriaProductoId",
                table: "Venta",
                column: "CategoriaProductoId",
                principalTable: "CategoriaProducto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Venta_CategoriaProducto_CategoriaProductoId",
                table: "Venta");

            migrationBuilder.DropTable(
                name: "CategoriaProducto");

            migrationBuilder.DropIndex(
                name: "IX_Venta_CategoriaProductoId",
                table: "Venta");

            migrationBuilder.DropColumn(
                name: "CategoriaProductoId",
                table: "Venta");
        }
    }
}
