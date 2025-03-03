namespace DesafioTjRjErlimar.Application.ManutencaoAutor;

public interface IManutencaoAutorAppRepository
{
    /// <summary>
    /// Verifica se já existe um autor com identificador informado
    /// </summary>
    /// <param name="autorId">Identificador do <see cref="Autor"/></param>
    /// <returns><see cref="true"> se já existe, ou <see cref="false"/> se não</returns>
    Task<bool> ExisteAutorComIdAsync(int autorId);

    /// <summary>
    /// Verifica se já existe um autor com nome informado
    /// </summary>
    /// <param name="nome">Nome do <see cref="Autor"/></param>
    /// <returns><see cref="true"> se já existe, ou <see cref="false"/> se não</returns>
    Task<bool> ExisteAutorComNomeAsync(string nome);

    /// <summary>
    /// Verifica se já existe um autor com nome informado, exceto o identificador informado
    /// </summary>
    /// <param name="nome">Nome do <see cref="Autor"/></param>
    /// <param name="idExcecao">Identificador a desconsiderar na comparação</param>
    Task<bool> ExisteAutorComNomeExcetoIdAsync(string nome, int idExcecao);

    /// <summary>
    /// Cadastra um novo autor
    /// </summary>
    /// <param name="autor">Dados do autor para cadastro</param>
    /// <returns>Instância do autor cadastrado</returns>
    Task<Autor> CadastraNovoAutorAsync(Autor autor);

    /// <summary>
    /// Lista todos os autores
    /// </summary>
    Task<IEnumerable<Autor>> ListarAutoresAsync();

    /// <summary>
    /// Remove um autor por identificador
    /// </summary>
    /// <param name="autorId">Identificador do autor a excluir</param>
    Task RemoveAutorPorIdAsync(int autorId);

    /// <summary>
    /// Atualiza dados de um autor
    /// </summary>
    /// <param name="autor">Dados do autor a atualizar</param>
    /// <returns>Instância do autor atualizado</returns>
    Task<Autor> AtualizarAutorAsync(Autor autor);
}