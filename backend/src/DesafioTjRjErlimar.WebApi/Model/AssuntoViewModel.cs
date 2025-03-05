namespace DesafioTjRjErlimar.WebApi.Model;

/// <summary>
/// Dados de um assunto
/// </summary>
public class AssuntoViewModel
{
    /// <summary>
    /// Código do assunto
    /// </summary>
    public int Codigo { get; set; }

    /// <summary>
    /// Descrição do assunto
    /// </summary>
    public required string Descricao { get; set; }
}