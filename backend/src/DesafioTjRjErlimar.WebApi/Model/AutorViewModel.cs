namespace DesafioTjRjErlimar.WebApi.Model;

/// <summary>
/// Dados de um autor
/// </summary>
public class AutorViewModel
{
    /// <summary>
    /// Código do autor
    /// </summary>
    public int Codigo { get; set; }

    /// <summary>
    /// Nome do autor
    /// </summary>
    public required string Nome { get; set; }
}