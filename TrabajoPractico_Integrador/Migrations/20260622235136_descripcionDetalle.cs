using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrabajoPractico_Integrador.Migrations
{
    /// <inheritdoc />
    public partial class descripcionDetalle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "DetalleVentas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "DetalleVentas");
        }
    }
}
