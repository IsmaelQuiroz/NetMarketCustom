using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessLogic.Data.Migrations
{
    public partial class AddTbProducto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Venta_CategoriaProducto_CategoriaProductoId",
                table: "Venta");

            migrationBuilder.DropIndex(
                name: "IX_Venta_CategoriaProductoId",
                table: "Venta");

            migrationBuilder.DropColumn(
                name: "CategoriaProductoId",
                table: "Venta");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                table: "Venta",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "cancelada",
                table: "Venta",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Producto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Modelo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Costo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    MinStock = table.Column<int>(type: "int", nullable: false),
                    MaxStock = table.Column<int>(type: "int", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    Atributo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ubicacion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Producto_CategoriaProducto_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "CategoriaProducto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Producto_CategoriaId",
                table: "Producto",
                column: "CategoriaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Producto");

            migrationBuilder.DropColumn(
                name: "cancelada",
                table: "Venta");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Fecha",
                table: "Venta",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AddColumn<int>(
                name: "CategoriaProductoId",
                table: "Venta",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
    }
}
