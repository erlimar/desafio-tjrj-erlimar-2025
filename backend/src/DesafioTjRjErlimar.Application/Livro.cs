using DesafioTjRjErlimar.Application.ManutencaoAutor;

namespace DesafioTjRjErlimar.Application;

/// <summary>
/// Representa um livro
/// </summary>
public class Livro
{
    public int LivroId { get; set; }

    public required string Titulo { get; set; }

    public required string Editora { get; set; }

    public int Edicao { get; set; }

    public int AnoPublicacao { get; set; }

    public List<Assunto> Assuntos { get; set; } = [];

    public List<Autor> Autores { get; set; } = [];
}