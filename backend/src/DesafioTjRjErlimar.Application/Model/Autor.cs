namespace DesafioTjRjErlimar.Application.Model;

public class Autor
{
    public int AutorId { get; set; }

    public required string Nome { get; set; }

    public List<Livro> Livros { get; set; } = [];
}