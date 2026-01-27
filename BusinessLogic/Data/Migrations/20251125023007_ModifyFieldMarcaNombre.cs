using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessLogic.Data.Migrations
{
    public partial class ModifyFieldMarcaNombre : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "nombre",
                table: "Marca",
                newName: "Nombre");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "Marca",
                newName: "nombre");
        }
    }
}
