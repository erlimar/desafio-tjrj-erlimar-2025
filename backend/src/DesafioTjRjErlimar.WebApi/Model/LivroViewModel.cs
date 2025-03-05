namespace DesafioTjRjErlimar.WebApi.Model;

/// <summary>
/// Dados de um livro
/// </summary>
public class LivroViewModel
{
    /// <summary>
    /// Código do livro
    /// </summary>
    public int Codigo { get; set; }

    /// <summary>
    /// Título do livro
    /// </summary>
    public required string Titulo { get; set; }

    /// <summary>
    /// Nome da editora do livro
    /// </summary>
    public required string Editora { get; set; }

    /// <summary>
    /// Número de edição do livro
    /// </summary>
    public int Edicao { get; set; }

    /// <summary>
    /// Ano de publicação do livro
    /// </summary>
    public int AnoPublicacao { get; set; }
}