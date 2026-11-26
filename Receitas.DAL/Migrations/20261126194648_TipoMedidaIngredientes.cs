using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Receitas.DAL.Migrations
{
    /// <inheritdoc />
    public partial class TipoMedidaIngredientes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoMedida",
                table: "Ingredientes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoMedida",
                table: "Ingredientes");
        }
    }
}
