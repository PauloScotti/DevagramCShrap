using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevagramCShrap.Migrations
{
    public partial class FavotirasAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Favoritar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdPublicacao = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favoritar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favoritar_Publicacoes_IdPublicacao",
                        column: x => x.IdPublicacao,
                        principalTable: "Publicacoes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Favoritar_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Favoritar_IdPublicacao",
                table: "Favoritar",
                column: "IdPublicacao");

            migrationBuilder.CreateIndex(
                name: "IX_Favoritar_IdUsuario",
                table: "Favoritar",
                column: "IdUsuario");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Favoritar");
        }
    }
}
