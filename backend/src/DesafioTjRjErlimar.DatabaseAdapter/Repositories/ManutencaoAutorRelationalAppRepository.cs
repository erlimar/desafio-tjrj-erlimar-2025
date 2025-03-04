using DesafioTjRjErlimar.Application.ManutencaoAutor;

using Microsoft.EntityFrameworkCore;

namespace DesafioTjRjErlimar.DatabaseAdapter.Repositories;

public class ManutencaoAutorRelationalAppRepository : IManutencaoAutorAppRepository
{
    private readonly DatabaseContext _context;

    public ManutencaoAutorRelationalAppRepository(DatabaseContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Cadastra um novo autor
    /// </summary>
    /// <param name="autor">Dados do autor</param>
    /// <returns>Nova instância de <see cref="Autor"/></returns>
    public async Task<Autor> CadastraNovoAutorAsync(Autor autor)
    {
        _ = autor ?? throw new ArgumentNullException(nameof(autor));

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

    /// <summary>
    /// Verifica se um autor já existe com o identificador informado
    /// </summary>
    /// <param name="autorId">Identificador do autor</param>
    public async Task<bool> ExisteAutorComIdAsync(int autorId)
    {
        return await _context.Set<Autor>().AnyAsync(a => a.AutorId == autorId);
    }

    /// <summary>
    /// Verifica se um autor já existe com o nome informado
    /// </summary>
    /// <param name="nome">Nome do autor</param>
    public async Task<bool> ExisteAutorComNomeAsync(string nome)
    {
        return await _context.Set<Autor>().AnyAsync(a => a.Nome.Equals(nome));
    }

    /// <summary>
    /// Verifica se já existe um autor com nome informado, exceto o identificador informado
    /// </summary>
    /// <param name="nome">Nome do <see cref="Autor"/></param>
    /// <param name="idExcecao">Identificador a desconsiderar na comparação</param>
    public async Task<bool> ExisteAutorComNomeExcetoIdAsync(string nome, int idExcecao)
    {
        return await _context.Set<Autor>().AnyAsync(a => a.Nome.Equals(nome) && a.AutorId != idExcecao);
    }

    /// <summary>
    /// Obté a lista de todos os autores cadastrados
    /// </summary>
    public async Task<IEnumerable<Autor>> ListarAutoresAsync()
    {
        return await _context.Set<Autor>().AsNoTracking().ToListAsync();
    }

    /// <summary>
    /// Remove um autor por identificador
    /// </summary>
    /// <param name="autorId">Identificador do autor a excluir</param>
    public async Task RemoveAutorPorIdAsync(int autorId)
    {
        await _context.Set<Autor>()
            .Where(w => w.AutorId == autorId)
            .ExecuteDeleteAsync();
    }

    /// <summary>
    /// Atualiza dados de um autor
    /// </summary>
    /// <param name="autor">Dados do autor a atualizar</param>
    /// <returns>Instância do autor atualizado</returns>
    public async Task<Autor> AtualizarAutorAsync(Autor autor)
    {
        _ = autor ?? throw new ArgumentNullException(nameof(autor));

        await _context.Set<Autor>()
            .Where(w => w.AutorId == autor.AutorId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(p => p.Nome, autor.Nome));

#pragma warning disable CS8603 // Possible null reference return.
        return await _context.Set<Autor>().FirstOrDefaultAsync(f => f.AutorId == autor.AutorId);
#pragma warning restore CS8603 // Possible null reference return.
    }
}