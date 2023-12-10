using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevagramCShrap.Migrations
{
    public partial class FotoTypeAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileType",
                table: "Publicacoes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileType",
                table: "Publicacoes");
        }
    }
}
