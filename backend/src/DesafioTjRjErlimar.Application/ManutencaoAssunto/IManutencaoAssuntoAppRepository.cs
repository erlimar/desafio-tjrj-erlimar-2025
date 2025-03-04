namespace DesafioTjRjErlimar.Application.ManutencaoAssunto;

/// <summary>
/// Repositório para manutenção da persistência de assuntos
/// </summary>
public interface IManutencaoAssuntoAppRepository
{
    /// <summary>
    /// Verifica se já existe um assunto com identificador informado
    /// </summary>
    /// <param name="assuntoId">Identificador do <see cref="Assunto"/></param>
    /// <returns><see cref="true"> se já existe, ou <see cref="false"/> se não</returns>
    Task<bool> ExisteAssuntoComIdAsync(int assuntoId);

    /// <summary>
    /// Verifica se já existe um assunto com descrição informada
    /// </summary>
    /// <param name="descricao">Descrição do <see cref="Assunto"/></param>
    /// <returns><see cref="true"> se já existe, ou <see cref="false"/> se não</returns>
    Task<bool> ExisteAssuntoComDescricaoAsync(string descricao);

    /// <summary>
    /// Verifica se já existe um assunto com descrição informada, exceto o identificador informado
    /// </summary>
    /// <param name="nome">Nome do <see cref="Assunto"/></param>
    /// <param name="idExcecao">Identificador a desconsiderar na comparação</param>
    Task<bool> ExisteAssuntoComDescricaoExcetoIdAsync(string nome, int idExcecao);

    /// <summary>
    /// Cadastra um novo assunto
    /// </summary>
    /// <param name="assunto">Dados do assunto para cadastro</param>
    /// <returns>Instância do assunto cadastrado</returns>
    Task<Assunto> CadastraNovoAssuntoAsync(Assunto assunto);

    /// <summary>
    /// Lista todos os assuntos
    /// </summary>
    Task<IEnumerable<Assunto>> ListarAssuntosAsync();

    /// <summary>
    /// Remove um assunto por identificador
    /// </summary>
    /// <param name="assuntoId">Identificador do assunto a excluir</param>
    Task RemoveAssuntoPorIdAsync(int assuntoId);

    /// <summary>
    /// Atualiza dados de um assunto
    /// </summary>
    /// <param name="assunto">Dados do assunto a atualizar</param>
    /// <returns>Instância do assunto atualizado</returns>
    Task<Assunto> AtualizarAssuntoAsync(Assunto assunto);
}