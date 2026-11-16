using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Receitas.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaUtilizadores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AutorId",
                table: "Receitas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Utilizadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Perfil = table.Column<int>(type: "int", nullable: false),
                    Bloqueado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizadores", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Receitas_AutorId",
                table: "Receitas",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Utilizadores_Email",
                table: "Utilizadores",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Receitas_Utilizadores_AutorId",
                table: "Receitas",
                column: "AutorId",
                principalTable: "Utilizadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receitas_Utilizadores_AutorId",
                table: "Receitas");

            migrationBuilder.DropTable(
                name: "Utilizadores");

            migrationBuilder.DropIndex(
                name: "IX_Receitas_AutorId",
                table: "Receitas");

            migrationBuilder.DropColumn(
                name: "AutorId",
                table: "Receitas");
        }
    }
}
