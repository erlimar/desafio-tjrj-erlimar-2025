namespace DesafioTjRjErlimar.WebApi.Model;

/// <summary>
/// Dados para cadastro de livro
/// </summary>
public class DadosLivroViewModel
{
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

    /// <summary>
    /// Autores do livro
    /// </summary>
    public IEnumerable<CodigoViewModel>? Autores { get; set; }

    /// <summary>
    /// Assuntos do livro
    /// </summary>
    public IEnumerable<CodigoViewModel>? Assuntos { get; set; }
}