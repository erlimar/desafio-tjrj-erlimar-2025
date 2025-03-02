using DesafioTjRjErlimar.Application.ManutencaoDeLivros;
using DesafioTjRjErlimar.Application.ManutencaoDeLivros.Model;

using Microsoft.EntityFrameworkCore;

namespace DesafioTjRjErlimar.DatabaseAdapter.Repositories;

public class ManutencaoLivroRelationalAppRepository : IManutencaoLivroAppRepository
{
    private readonly DatabaseContext _context;

    public ManutencaoLivroRelationalAppRepository(DatabaseContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Autor> CadastraNovoAutorAsync(Autor autor)
    {
        var autorCadastrado = new Autor
        {
            AutorId = autor.AutorId,
            Nome = autor.Nome
        };

        _context.Entry(autorCadastrado).State = EntityState.Added;

        await _context.SaveChangesAsync();

        _context.Entry(autorCadastrado).State = EntityState.Detached;

        // // Em caso de ser possível cadastrar um autor já com livros vnculados
        // if (!autor.Livros.Any())
        // {
        //     autor.Livros.Add(new Livro
        //     {
        //         Titulo = "Livro sem título",
        //         Editora = "Nova",
        //     });
        // }

        // // Cadastro de livros vinculados caso tenha informado
        // if (autor.Livros is not null && autor.Livros.Any())
        // {
        //     var autorComLivros = await _context.Set<Autor>().AsTracking()
        //         .Include(a => a.Livros)
        //         .FirstOrDefaultAsync(a => a.AutorId == autorCadastrado.AutorId);

        //     if (autorComLivros is not null)
        //     {
        //         autorComLivros.Livros.AddRange(autor.Livros);

        //         await _context.SaveChangesAsync();
        //     }
        // }

        return autorCadastrado;
    }

    public async Task<bool> ExisteAutorComIdAsync(int autorId)
    {
        return await _context.Set<Autor>().AnyAsync(a => a.AutorId == autorId);
    }

    public async Task<bool> ExisteAutorComNomeAsync(string nome)
    {
        return await _context.Set<Autor>().AnyAsync(a => a.Nome.Equals(nome));
    }
}