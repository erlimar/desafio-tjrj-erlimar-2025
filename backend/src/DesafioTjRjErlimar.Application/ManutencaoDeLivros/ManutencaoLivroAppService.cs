using DesafioTjRjErlimar.Application.ManutencaoDeLivros.Exceptions;
using DesafioTjRjErlimar.Application.ManutencaoDeLivros.Model;

namespace DesafioTjRjErlimar.Application.ManutencaoDeLivros;

public class ManutencaoLivroAppService
{
    private readonly IManutencaoLivroAppRepository _repository;

    public ManutencaoLivroAppService(IManutencaoLivroAppRepository repository)
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
}