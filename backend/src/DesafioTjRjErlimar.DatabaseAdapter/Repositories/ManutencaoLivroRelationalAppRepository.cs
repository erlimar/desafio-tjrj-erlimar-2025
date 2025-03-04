using DesafioTjRjErlimar.Application.ManutencaoAssunto;
using DesafioTjRjErlimar.Application.ManutencaoAutor;
using DesafioTjRjErlimar.Application.ManutencaoLivro;

using Microsoft.EntityFrameworkCore;

namespace DesafioTjRjErlimar.DatabaseAdapter.Repositories;

public class ManutencaoLivroRelationalAppRepository : IManutencaoLivroAppRepository
{
    private readonly DatabaseContext _context;

    public ManutencaoLivroRelationalAppRepository(DatabaseContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Atualiza dados de um livro
    /// </summary>
    /// <param name="livro">Dados do livro a atualizar</param>
    /// <returns>Instância do livro atualizado</returns>
    public async Task<Livro> AtualizarLivroAsync(Livro livro)
    {
        _ = livro ?? throw new ArgumentNullException(nameof(livro));

        await _context.Set<Livro>()
            .Where(w => w.LivroId == livro.LivroId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(p => p.Titulo, livro.Titulo)
                .SetProperty(p => p.Editora, livro.Editora)
                .SetProperty(p => p.Edicao, livro.Edicao)
                .SetProperty(p => p.AnoPublicacao, livro.AnoPublicacao));

#pragma warning disable CS8603 // Possible null reference return.
        return await _context.Set<Livro>().FirstOrDefaultAsync(f => f.LivroId == livro.LivroId);
#pragma warning restore CS8603 // Possible null reference return.
    }

    /// <summary>
    /// Atualiza autores de um livro
    /// </summary>
    /// <param name="livroId">Identificador do livro</param>
    /// <param name="autores">Código dos autores vinculados</param>
    public async Task AtualizarAutoresDoLivro(int livroId, IEnumerable<int> autores)
    {
        var livro = await _context.Set<Livro>().AsTracking()
            .Include(i => i.Autores)
            .SingleAsync(s => s.LivroId == livroId);

        if (livro is not null)
        {
            // Descarta autores removidos da nova lista
            List<Autor> autoresDoLivro = [.. livro.Autores.Where(w => autores.Contains(w.AutorId))];

            // Vincula qualquer novo autor da lista
            var codigoNovosAutores = autores.Where(w => !autoresDoLivro.Any(a => a.AutorId == w));
            var novosAutores = _context.Set<Autor>()
                .Where(w => codigoNovosAutores.Contains(w.AutorId))
                .ToList();

            autoresDoLivro.AddRange(novosAutores);

            livro.Autores = autoresDoLivro;

            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Atualiza assuntos de um livro
    /// </summary>
    /// <param name="livroId">Identificador do livro</param>
    /// <param name="assuntos">Códigos dos assuntos vinculados</param>
    public async Task AtualizarAssuntosDoLivro(int livroId, IEnumerable<int> assuntos)
    {
        var livro = await _context.Set<Livro>().AsTracking()
            .Include(i => i.Assuntos)
            .SingleAsync(s => s.LivroId == livroId);

        if (livro is not null)
        {
            // Descarta assuntos removidos da nova lista
            List<Assunto> assuntosDoLivro = [.. livro.Assuntos.Where(w => assuntos.Contains(w.AssuntoId))];

            // Vincula qualquer novo assunto da lista
            var codigoNovosAssuntos = assuntos.Where(w => !assuntosDoLivro.Any(a => a.AssuntoId == w));
            var novosAssuntos = _context.Set<Assunto>()
                .Where(w => codigoNovosAssuntos.Contains(w.AssuntoId))
                .ToList();

            assuntosDoLivro.AddRange(novosAssuntos);

            livro.Assuntos = assuntosDoLivro;

            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Cadastra um novo livro
    /// </summary>
    /// <param name="livro">Dados do livro</param>
    /// <returns>Nova instância de <see cref="Livro"/></returns>
    public async Task<Livro> CadastraNovoLivroAsync(Livro livro)
    {
        _ = livro ?? throw new ArgumentNullException(nameof(livro));

        var livroCadastrado = new Livro
        {
            LivroId = livro.LivroId,
            Titulo = livro.Titulo,
            Editora = livro.Editora,
            Edicao = livro.Edicao,
            AnoPublicacao = livro.AnoPublicacao
        };

        _context.Entry(livroCadastrado).State = EntityState.Added;

        await _context.SaveChangesAsync();

        _context.Entry(livroCadastrado).State = EntityState.Detached;

        return livroCadastrado;
    }

    /// <summary>
    /// Verifica se um livro já existe com o identificador informado
    /// </summary>
    /// <param name="livroId">Identificador do livro</param>
    public async Task<bool> ExisteLivroComIdAsync(int livroId)
    {
        return await _context.Set<Livro>().AnyAsync(a => a.LivroId == livroId);
    }

    /// <summary>
    /// Verifica se um livro já existe com o título informado
    /// </summary>
    /// <param name="titulo">Título do livro</param>
    public async Task<bool> ExisteLivroComTituloAsync(string titulo)
    {
        return await _context.Set<Livro>().AnyAsync(a => a.Titulo.Equals(titulo));
    }

    /// <summary>
    /// Verifica se já existe um livro com título informado, exceto o identificador informado
    /// </summary>
    /// <param name="titulo">Título do <see cref="Livro"/></param>
    /// <param name="idExcecao">Identificador a desconsiderar na comparação</param>
    public async Task<bool> ExisteLivroComTituloExcetoIdAsync(string titulo, int idExcecao)
    {
        return await _context.Set<Livro>().AnyAsync(a => a.Titulo.Equals(titulo) && a.LivroId != idExcecao);
    }

    /// <summary>
    /// Obté a lista de todos os livros cadastrados
    /// </summary>
    public async Task<IEnumerable<Livro>> ListarLivrosAsync()
    {
        return await _context.Set<Livro>().AsNoTracking().ToListAsync();
    }

    /// <summary>
    /// Obter autores de um livro
    /// </summary>
    /// <param name="livroId">Identificador do livro</param>
    /// <returns>Lista de <see cref="Autor"/></returns>
    public async Task<IEnumerable<Autor>> ObterAutoresDoLivroAsync(int livroId)
    {
        return await _context.Set<Autor>()
            .Where(w => w.Livros.Any(a => a.LivroId == livroId))
            .ToListAsync();
    }

    /// <summary>
    /// Obter assuntos de um livro
    /// </summary>
    /// <param name="livroId">Identificador do livro</param>
    /// <returns>Lista de <see cref="Assunto"/></returns>
    public async Task<IEnumerable<Assunto>> ObterAssuntosDoLivroAsync(int livroId)
    {
        return await _context.Set<Assunto>()
            .Where(w => w.Livros.Any(a => a.LivroId == livroId))
            .ToListAsync();
    }

    /// <summary>
    /// Remove um livro por identificador
    /// </summary>
    /// <param name="livroId">Identificador do livro a excluir</param>
    public async Task RemoveLivroPorIdAsync(int livroId)
    {
        await _context.Set<Livro>()
            .Where(w => w.LivroId == livroId)
            .ExecuteDeleteAsync();
    }
}