using System.ComponentModel.DataAnnotations;

using DesafioTjRjErlimar.Application.ManutencaoAutor;
using DesafioTjRjErlimar.WebApi.Model;

using Microsoft.AspNetCore.Mvc;

namespace DesafioTjRjErlimar.WebApi.Controllers;

[ApiController]
[Route("autores")]
public class AutorController : ControllerBase
{
    private readonly ILogger<AutorController> _logger;
    private readonly ManutencaoAutorAppService _manutencaoLivroAppService;

    public AutorController(ILogger<AutorController> logger, ManutencaoAutorAppService manutencaoLivroAppService)
    {
        _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));

        _manutencaoLivroAppService = manutencaoLivroAppService
            ?? throw new ArgumentNullException(nameof(manutencaoLivroAppService));
    }

    /// <summary>
    /// Cadastra um novo autor
    /// </summary>
    /// <param name="model">Dados para cadastro</param>
    /// <returns>Dados do autor cadatrado</returns>
    /// <response code="201">Autor cadastrado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AutorViewModel>> CadastrarNovoAutor(DadosAutorViewModel model)
    {
        _ = model ?? throw new ArgumentNullException(nameof(model));
        _ = model.Nome ?? throw new ArgumentNullException(nameof(model.Nome));

        try
        {
            var autorCadastrado = await _manutencaoLivroAppService.AdicionarAutorAsync(new Autor
            {
                Nome = model.Nome.Trim()
            });

            return Created("", new AutorViewModel
            {
                Codigo = autorCadastrado.AutorId,
                Nome = autorCadastrado.Nome
            });
        }
        catch (AutorRepetidoException ex)
        {
            ModelState.AddModelError("nome", ex.Message);

            return ValidationProblem(modelStateDictionary: ModelState, title: "Não foi possível cadastrar o autor");
        }
    }

    /// <summary>
    /// Atualiza cadastro de um autor
    /// </summary>
    /// <param name="model">Dados para cadastro</param>
    /// <returns>Dados do autor cadatrado</returns>
    /// <response code="200">Autor atualizado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    [HttpPost("{autorId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AutorViewModel>> AtualizarAutor(
        [Range(1, int.MaxValue, ErrorMessage = "O identificador precisa ser maior que zero")]
        int autorId,
        DadosAutorViewModel model)
    {
        _ = model ?? throw new ArgumentNullException(nameof(model));
        _ = model.Nome ?? throw new ArgumentNullException(nameof(model.Nome));

        try
        {
            var autorAtualizado = await _manutencaoLivroAppService.AtualizarAutorAsync(new Autor
            {
                AutorId = autorId,
                Nome = model.Nome.Trim()
            });

            return Ok(new AutorViewModel
            {
                Codigo = autorAtualizado.AutorId,
                Nome = autorAtualizado.Nome
            });
        }
        catch (AutorRepetidoException ex)
        {
            ModelState.AddModelError("nome", ex.Message);

            return ValidationProblem(modelStateDictionary: ModelState, title: "Não foi possível atualizar o autor");
        }
    }

    /// <summary>
    /// Lista todos os autores
    /// </summary>
    /// <response code="200">Lista de autores</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AutorViewModel>>> ListarAutores()
    {
        var autores = await _manutencaoLivroAppService.ObterAutoresAsync();

        var autoresViewModel = autores.Select(a => new AutorViewModel
        {
            Codigo = a.AutorId,
            Nome = a.Nome
        });

        return Ok(autoresViewModel);
    }

    /// <summary>
    /// Remove um autor
    /// </summary>
    /// <param name="autorId">Identificador do autor para excluir</param>
    [HttpDelete("{autorId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> RemoverAutor(
        [Range(1, int.MaxValue, ErrorMessage = "O identificador precisa ser maior que zero")]
        int autorId)
    {
        try
        {
            await _manutencaoLivroAppService.RemoverAutorPorIdAsync(autorId);
        }
        catch (RegistroInexistenteException ex)
        {
            ModelState.AddModelError("autorId", ex.Message);

            return ValidationProblem(modelStateDictionary: ModelState, title: "Não foi possível remover o autor");
        }

        return NoContent();
    }
}