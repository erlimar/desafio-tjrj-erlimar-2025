namespace DesafioTjRjErlimar.Application.Model;

public class Assunto
{
    public int AssuntoId { get; set; }

    public required string Descricao { get; set; }

    public List<Livro> Livros { get; set; } = [];
}