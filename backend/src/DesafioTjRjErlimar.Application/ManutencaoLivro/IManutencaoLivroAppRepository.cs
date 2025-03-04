namespace DesafioTjRjErlimar.Application.ManutencaoLivro;

/// <summary>
/// Repositório para manutenção da persistência de livros
/// </summary>
public interface IManutencaoLivroAppRepository
{
    /// <summary>
    /// Verifica se já existe um livro com identificador informado
    /// </summary>
    /// <param name="livroId">Identificador do <see cref="Livro"/></param>
    /// <returns><see cref="true"> se já existe, ou <see cref="false"/> se não</returns>
    Task<bool> ExisteLivroComIdAsync(int livroId);

    /// <summary>
    /// Verifica se já existe um livro com título informado
    /// </summary>
    /// <param name="titulo">Título do <see cref="Livro"/></param>
    /// <returns><see cref="true"> se já existe, ou <see cref="false"/> se não</returns>
    Task<bool> ExisteLivroComTituloAsync(string titulo);

    /// <summary>
    /// Cadastra um novo livro
    /// </summary>
    /// <param name="livro">Dados do livro para cadastro</param>
    /// <returns>Instância do livro cadastrado</returns>
    Task<Livro> CadastraNovoLivroAsync(Livro livro);

    /// <summary>
    /// Lista todos os livros
    /// </summary>
    Task<IEnumerable<Livro>> ListarLivrosAsync();

    /// <summary>
    /// Remove um livro por identificador
    /// </summary>
    /// <param name="livroId">Identificador do livro a excluir</param>
    Task RemoveLivroPorIdAsync(int livroId);

    /// <summary>
    /// Atualiza dados de um livro
    /// </summary>
    /// <param name="livro">Dados do livro a atualizar</param>
    /// <returns>Instância do livro atualizado</returns>
    Task<Livro> AtualizarLivroAsync(Livro livro);

    /// <summary>
    /// Verifica se já existe um livro com título informado, exceto o identificador informado
    /// </summary>
    /// <param name="titulo">Título do <see cref="Livro"/></param>
    /// <param name="idExcecao">Identificador a desconsiderar na comparação</param>
    Task<bool> ExisteLivroComTituloExcetoIdAsync(string titulo, int idExcecao);
}