namespace DesafioTjRjErlimar.Application.ManutencaoLivro;

/// <summary>
/// Modelo de dados para view de banco "dbo.RelatorioConsolidadoPorAutor"
/// </summary>
public class RelatorioConsolidadoPorAutorModel
{
    public int AutorPrincipalCodAutor { get; set; }
    public string? AutorPrincipalNome { get; set; }
    public int LivroCodLivro { get; set; }
    public string? LivroTitulo { get; set; }
    public string? LivroEditora { get; set; }
    public int LivroEdicao { get; set; }
    public int LivroAnoPublicacao { get; set; }
    public int AutorSecundarioCodAutor { get; set; }
    public string? AutorSecundarioNome { get; set; }
    public int AssuntoCodAssunto { get; set; }
    public string? AssuntoDescricao { get; set; }
}