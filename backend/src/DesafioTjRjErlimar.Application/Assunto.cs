namespace DesafioTjRjErlimar.Application;

/// <summary>
/// Representa um assunto para agrupar livros
/// </summary>
public class Assunto
{
    public int AssuntoId { get; set; }

    public required string Descricao { get; set; }

    public List<Livro> Livros { get; set; } = [];
}