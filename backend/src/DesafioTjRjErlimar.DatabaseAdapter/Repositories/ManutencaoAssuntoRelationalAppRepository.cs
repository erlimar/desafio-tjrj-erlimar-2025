using DesafioTjRjErlimar.Application.ManutencaoAssunto;
using DesafioTjRjErlimar.Application.ManutencaoAutor;

using Microsoft.EntityFrameworkCore;

namespace DesafioTjRjErlimar.DatabaseAdapter.Repositories;

public class ManutencaoAssuntoRelationalAppRepository : IManutencaoAssuntoAppRepository
{
    private readonly DatabaseContext _context;

    public ManutencaoAssuntoRelationalAppRepository(DatabaseContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Cadastra um novo assunto
    /// </summary>
    /// <param name="assunto">Dados do assunto</param>
    /// <returns>Nova instância de <see cref="Assunto"/></returns>
    public async Task<Assunto> CadastraNovoAssuntoAsync(Assunto assunto)
    {
        _ = assunto ?? throw new ArgumentNullException(nameof(assunto));

        var assuntoCadastrado = new Assunto
        {
            AssuntoId = assunto.AssuntoId,
            Descricao = assunto.Descricao
        };

        _context.Entry(assuntoCadastrado).State = EntityState.Added;

        await _context.SaveChangesAsync();

        _context.Entry(assuntoCadastrado).State = EntityState.Detached;

        return assuntoCadastrado;
    }

    /// <summary>
    /// Verifica se um assunto já existe com o identificador informado
    /// </summary>
    /// <param name="assuntoId">Identificador do assunto</param>
    public async Task<bool> ExisteAssuntoComIdAsync(int assuntoId)
    {
        return await _context.Set<Assunto>().AnyAsync(a => a.AssuntoId == assuntoId);
    }

    /// <summary>
    /// Verifica se um assunto já existe com a descrição informada
    /// </summary>
    /// <param name="descricao">Descrição do assunto</param>
    public async Task<bool> ExisteAssuntoComDescricaoAsync(string descricao)
    {
        return await _context.Set<Assunto>().AnyAsync(a => a.Descricao.Equals(descricao));
    }

    /// <summary>
    /// Verifica se já existe um assunto com descrição informada, exceto o identificador informado
    /// </summary>
    /// <param name="descricao">Descrição do <see cref="Assunto"/></param>
    public async Task<bool> ExisteAssuntoComDescricaoExcetoIdAsync(string descricao, int idExcecao)
    {
        return await _context.Set<Assunto>().AnyAsync(a => a.Descricao.Equals(descricao) && a.AssuntoId != idExcecao);
    }

    /// <summary>
    /// Obté a lista de todos os assuntos cadastrados
    /// </summary>
    public async Task<IEnumerable<Assunto>> ListarAssuntosAsync()
    {
        return await _context.Set<Assunto>().AsNoTracking().ToListAsync();
    }

    /// <summary>
    /// Remove um assunto por identificador
    /// </summary>
    /// <param name="assuntoId">Identificador do assunto a excluir</param>
    public async Task RemoveAssuntoPorIdAsync(int assuntoId)
    {
        await _context.Set<Assunto>()
            .Where(w => w.AssuntoId == assuntoId)
            .ExecuteDeleteAsync();
    }

    /// <summary>
    /// Atualiza dados de um assunto
    /// </summary>
    /// <param name="assunto">Dados do assunto a atualizar</param>
    /// <returns>Instância do assunto atualizado</returns>
    public async Task<Assunto> AtualizarAssuntoAsync(Assunto assunto)
    {
        _ = assunto ?? throw new ArgumentNullException(nameof(assunto));

        await _context.Set<Assunto>()
            .Where(w => w.AssuntoId == assunto.AssuntoId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(p => p.Descricao, assunto.Descricao));

#pragma warning disable CS8603 // Possible null reference return.
        return await _context.Set<Assunto>().FirstOrDefaultAsync(f => f.AssuntoId == assunto.AssuntoId);
#pragma warning restore CS8603 // Possible null reference return.
    }
}