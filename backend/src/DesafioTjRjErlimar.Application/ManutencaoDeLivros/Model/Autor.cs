namespace DesafioTjRjErlimar.Application.ManutencaoDeLivros.Model;

/// <summary>
/// Representa um autor de livro
/// </summary>
public class Autor
{
    public int AutorId { get; set; }

    public required string Nome { get; set; }

    public List<Livro> Livros { get; set; } = [];
}