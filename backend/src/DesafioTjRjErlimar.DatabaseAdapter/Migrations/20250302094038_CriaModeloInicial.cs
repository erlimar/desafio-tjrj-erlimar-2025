using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesafioTjRjErlimar.DatabaseAdapter.Migrations;

/// <inheritdoc />
public partial class CriaModeloInicial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Assunto",
            columns: table => new
            {
                CodAssunto = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Descricao = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Assunto", x => x.CodAssunto);
            });

        migrationBuilder.CreateTable(
            name: "Autor",
            columns: table => new
            {
                CodAutor = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Nome = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Autor", x => x.CodAutor);
            });

        migrationBuilder.CreateTable(
            name: "Livro",
            columns: table => new
            {
                CodLivro = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Titulo = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                Editora = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                Edicao = table.Column<int>(type: "int", nullable: false),
                AnoPublicacao = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Livro", x => x.CodLivro);
            });

        migrationBuilder.CreateTable(
            name: "Livro_Assunto",
            columns: table => new
            {
                Assunto_CodAssunto = table.Column<int>(type: "int", nullable: false),
                Livro_CodLivro = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Livro_Assunto", x => new { x.Assunto_CodAssunto, x.Livro_CodLivro });
                table.ForeignKey(
                    name: "FK_Livro_Assunto_Assunto_Assunto_CodAssunto",
                    column: x => x.Assunto_CodAssunto,
                    principalTable: "Assunto",
                    principalColumn: "CodAssunto",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Livro_Assunto_Livro_Livro_CodLivro",
                    column: x => x.Livro_CodLivro,
                    principalTable: "Livro",
                    principalColumn: "CodLivro",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Livro_Autor",
            columns: table => new
            {
                Autor_CodAutor = table.Column<int>(type: "int", nullable: false),
                Livro_CodLivro = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Livro_Autor", x => new { x.Autor_CodAutor, x.Livro_CodLivro });
                table.ForeignKey(
                    name: "FK_Livro_Autor_Autor_Autor_CodAutor",
                    column: x => x.Autor_CodAutor,
                    principalTable: "Autor",
                    principalColumn: "CodAutor",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Livro_Autor_Livro_Livro_CodLivro",
                    column: x => x.Livro_CodLivro,
                    principalTable: "Livro",
                    principalColumn: "CodLivro",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Livro_Assunto_Livro_CodLivro",
            table: "Livro_Assunto",
            column: "Livro_CodLivro");

        migrationBuilder.CreateIndex(
            name: "IX_Livro_Autor_Livro_CodLivro",
            table: "Livro_Autor",
            column: "Livro_CodLivro");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Livro_Assunto");

        migrationBuilder.DropTable(
            name: "Livro_Autor");

        migrationBuilder.DropTable(
            name: "Assunto");

        migrationBuilder.DropTable(
            name: "Autor");

        migrationBuilder.DropTable(
            name: "Livro");
    }
}