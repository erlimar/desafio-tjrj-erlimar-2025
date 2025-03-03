namespace DesafioTjRjErlimar.Application.ManutencaoAutor;

public class ManutencaoAutorAppService
{
    private readonly IManutencaoAutorAppRepository _repository;

    public ManutencaoAutorAppService(IManutencaoAutorAppRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <summary>
    /// Adiciona um autor
    /// </summary>
    /// <param name="autor">Dados do autor</param>
    /// <returns>Instância de <see cref="Autor"/> quando consegue adicionar</returns>
    /// <exception cref="ArgumentNullException">Quando <paramref name="autor"/> é nulo</exception>
    public async Task<Autor> AdicionarAutorAsync(Autor autor)
    {
        _ = autor ?? throw new ArgumentNullException(nameof(autor));

        if (await _repository.ExisteAutorComIdAsync(autor.AutorId))
        {
            throw new AutorRepetidoException($"Autor com identificador {autor.AutorId} já existe");
        }

        if (await _repository.ExisteAutorComNomeAsync(autor.Nome))
        {
            throw new AutorRepetidoException($"Autor com nome '{autor.Nome}' já existe");
        }

        // TODO: Validar retorno do repositório e tratar situações inesperadas de banco

        return await _repository.CadastraNovoAutorAsync(autor);
    }

    /// <summary>
    /// Lista todos os autores ordenados por nome
    /// </summary>
    public async Task<IEnumerable<Autor>> ObterAutoresAsync()
    {
        IEnumerable<Autor> autores = await _repository.ListarAutoresAsync();

        var autoresOrdenados = autores.OrderBy(a => a.Nome);

        return autoresOrdenados;
    }

    /// <summary>
    /// Remove um autor por identificador
    /// </summary>
    /// <param name="autorId">Identificador do autor a excluir</param>
    /// <exception cref="RegistroInexistenteException"></exception>
    public async Task RemoverAutorPorIdAsync(int autorId)
    {
        if (!await _repository.ExisteAutorComIdAsync(autorId))
        {
            throw new RegistroInexistenteException($"Autor com identificador {autorId} não existe para ser excluído");
        }

        await _repository.RemoveAutorPorIdAsync(autorId);
    }

    /// <summary>
    /// Atualiza dados do autor
    /// </summary>
    /// <param name="autor"></param>
    /// <returns>Instância do <see cref="Autor"/> com os dados atualizados</returns>
    /// <exception cref="AutorRepetidoException">Quando o novo nome do autor já estiver cadastrado</exception>
    public async Task<Autor> AtualizarAutorAsync(Autor autor)
    {
        _ = autor ?? throw new ArgumentNullException(nameof(autor));

        if (await _repository.ExisteAutorComNomeExcetoIdAsync(autor.Nome, autor.AutorId))
        {
            throw new AutorRepetidoException($"Já existe um autor com o novo nome '{autor.Nome}' pretendido");
        }

        return await _repository.AtualizarAutorAsync(autor);
    }
}