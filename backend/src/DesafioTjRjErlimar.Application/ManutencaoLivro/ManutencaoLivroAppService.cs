using DesafioTjRjErlimar.Application.ManutencaoAssunto;
using DesafioTjRjErlimar.Application.ManutencaoAutor;

namespace DesafioTjRjErlimar.Application.ManutencaoLivro;

/// <summary>
/// Serviço de aplicação para manutenção de livros
/// </summary>
public class ManutencaoLivroAppService
{
    private readonly IManutencaoLivroAppRepository _repository;

    public ManutencaoLivroAppService(IManutencaoLivroAppRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <summary>
    /// Adiciona um livro
    /// </summary>
    /// <param name="livro">Dados do livro</param>
    /// <returns>Instância de <see cref="Livro"/> quando consegue adicionar</returns>
    /// <exception cref="ArgumentNullException">Quando <paramref name="livro"/> é nulo</exception>
    public async Task<Livro> AdicionarLivroAsync(
        Livro livro,
        IEnumerable<int>? autores = null,
        IEnumerable<int>? assuntos = null)
    {
        _ = livro ?? throw new ArgumentNullException(nameof(livro));

        if (await _repository.ExisteLivroComIdAsync(livro.LivroId))
        {
            throw new RegistroRepetidoException($"Livro com identificador {livro.LivroId} já existe");
        }

        if (await _repository.ExisteLivroComTituloAsync(livro.Titulo))
        {
            throw new RegistroRepetidoException($"Livro com título '{livro.Titulo}' já existe");
        }

        var livroCadastrado = await _repository.CadastraNovoLivroAsync(livro);

        if (autores is not null)
        {
            await _repository.AtualizarAutoresDoLivro(livroCadastrado.LivroId, autores);
        }

        if (assuntos is not null)
        {
            await _repository.AtualizarAssuntosDoLivro(livroCadastrado.LivroId, assuntos);
        }

        return livroCadastrado;
    }

    /// <summary>
    /// Lista todos os livros ordenados por título
    /// </summary>
    public async Task<IEnumerable<Livro>> ObterLivrosAsync()
    {
        IEnumerable<Livro> livros = await _repository.ListarLivrosAsync();

        var livrosOrdenados = livros.OrderBy(a => a.Titulo);

        return livrosOrdenados;
    }

    /// <summary>
    /// Obter autores de um livro
    /// </summary>
    /// <param name="livroId">Identificador do livro</param>
    public async Task<IEnumerable<Autor>> ObterAutoresDoLivroAsync(int livroId)
    {
        if (!await _repository.ExisteLivroComIdAsync(livroId))
        {
            throw new RegistroInexistenteException($"Livro com identificador {livroId} não existe");
        }

        return await _repository.ObterAutoresDoLivroAsync(livroId);
    }

    /// <summary>
    /// Obter assuntos de um livro
    /// </summary>
    /// <param name="livroId">Identificador do livro</param>
    public async Task<IEnumerable<Assunto>> ObterAssuntosDoLivroAsync(int livroId)
    {
        if (!await _repository.ExisteLivroComIdAsync(livroId))
        {
            throw new RegistroInexistenteException($"Livro com identificador {livroId} não existe");
        }

        return await _repository.ObterAssuntosDoLivroAsync(livroId);
    }

    /// <summary>
    /// Remove um livro por identificador
    /// </summary>
    /// <param name="livroId">Identificador do livro a excluir</param>
    /// <exception cref="RegistroInexistenteException"></exception>
    public async Task RemoverLivroPorIdAsync(int livroId)
    {
        if (!await _repository.ExisteLivroComIdAsync(livroId))
        {
            throw new RegistroInexistenteException($"Livro com identificador {livroId} não existe para ser excluído");
        }

        await _repository.RemoveLivroPorIdAsync(livroId);
    }

    /// <summary>
    /// Atualiza dados do livro
    /// </summary>
    /// <param name="livro">Dados do livro a atualizar</param>
    /// <param name="autores">Identificadores dos autores</param>
    /// <param name="assuntos">Identificadores dos assuntos</param>
    /// <returns>Instância do <see cref="Livro"/> com os dados atualizados</returns>
    /// <exception cref="RegistroRepetidoException">Quando o novo título do livro já estiver cadastrado</exception>
    public async Task<Livro> AtualizarLivroAsync(
        Livro livro,
        IEnumerable<int>? autores = null,
        IEnumerable<int>? assuntos = null)
    {
        _ = livro ?? throw new ArgumentNullException(nameof(livro));

        if (!await _repository.ExisteLivroComIdAsync(livro.LivroId))
        {
            throw new RegistroInexistenteException($"Livro com identificador {livro.LivroId} não existe para ser atualizado");
        }

        if (await _repository.ExisteLivroComTituloExcetoIdAsync(livro.Titulo, livro.LivroId))
        {
            throw new RegistroRepetidoException($"Já existe um livro com o novo título '{livro.Titulo}' pretendido");
        }

        var livroAtualizado = await _repository.AtualizarLivroAsync(livro);

        if (autores is not null)
        {
            await _repository.AtualizarAutoresDoLivro(livroAtualizado.LivroId, autores);
        }

        if (assuntos is not null)
        {
            await _repository.AtualizarAssuntosDoLivro(livroAtualizado.LivroId, assuntos);
        }

        return livroAtualizado;
    }
}