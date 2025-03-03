namespace DesafioTjRjErlimar.Application.ManutencaoAssunto;

public class ManutencaoAssuntoAppService
{
    private readonly IManutencaoAssuntoAppRepository _repository;

    public ManutencaoAssuntoAppService(IManutencaoAssuntoAppRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <summary>
    /// Adiciona um assunto
    /// </summary>
    /// <param name="assunto">Dados do assunto</param>
    /// <returns>Instância de <see cref="Assunto"/> quando consegue adicionar</returns>
    /// <exception cref="ArgumentNullException">Quando <paramref name="assunto"/> é nulo</exception>
    public async Task<Assunto> AdicionarAssuntoAsync(Assunto assunto)
    {
        _ = assunto ?? throw new ArgumentNullException(nameof(assunto));

        if (await _repository.ExisteAssuntoComIdAsync(assunto.AssuntoId))
        {
            throw new RegistroRepetidoException($"Assunto com identificador {assunto.AssuntoId} já existe");
        }

        if (await _repository.ExisteAssuntoComDescricaoAsync(assunto.Descricao))
        {
            throw new RegistroRepetidoException($"Assunto com descrição '{assunto.Descricao}' já existe");
        }

        // TODO: Validar retorno do repositório e tratar situações inesperadas de banco

        return await _repository.CadastraNovoAssuntoAsync(assunto);
    }

    /// <summary>
    /// Lista todos os assuntos ordenados por nome
    /// </summary>
    public async Task<IEnumerable<Assunto>> ObterAssuntosAsync()
    {
        IEnumerable<Assunto> assuntos = await _repository.ListarAssuntosAsync();

        var assuntosOrdenados = assuntos.OrderBy(a => a.Descricao);

        return assuntosOrdenados;
    }

    /// <summary>
    /// Remove um assunto por identificador
    /// </summary>
    /// <param name="assuntoId">Identificador do assunto a excluir</param>
    /// <exception cref="RegistroInexistenteException"></exception>
    public async Task RemoverAssuntoPorIdAsync(int assuntoId)
    {
        if (!await _repository.ExisteAssuntoComIdAsync(assuntoId))
        {
            throw new RegistroInexistenteException($"Assunto com identificador {assuntoId} não existe para ser excluído");
        }

        await _repository.RemoveAssuntoPorIdAsync(assuntoId);
    }

    /// <summary>
    /// Atualiza dados do assunto
    /// </summary>
    /// <param name="assunto">Dados do assunto a atualizar</param>
    /// <returns>Instância do <see cref="Assunto"/> com os dados atualizados</returns>
    /// <exception cref="RegistroRepetidoException">Quando a descrição do assunto já estiver cadastrado</exception>
    public async Task<Assunto> AtualizarAssuntoAsync(Assunto assunto)
    {
        _ = assunto ?? throw new ArgumentNullException(nameof(assunto));

        if (await _repository.ExisteAssuntoComDescricaoExcetoIdAsync(assunto.Descricao, assunto.AssuntoId))
        {
            throw new RegistroRepetidoException($"Já existe um assunto com a nova descrição '{assunto.Descricao}' pretendida");
        }

        return await _repository.AtualizarAssuntoAsync(assunto);
    }
}