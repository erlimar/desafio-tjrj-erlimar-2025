using DesafioTjRjErlimar.Application.ManutencaoDeLivros.Model;

namespace DesafioTjRjErlimar.Application.ManutencaoDeLivros;

public interface IManutencaoLivroAppRepository
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
    /// Cadastra um novo autor
    /// </summary>
    /// <param name="autor">Dados do autor para cadastro</param>
    /// <returns>Instância do autor cadastrado</returns>
    Task<Autor> CadastraNovoAutorAsync(Autor autor);
}