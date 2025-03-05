using System.ComponentModel.DataAnnotations;

using DesafioTjRjErlimar.Application;
using DesafioTjRjErlimar.Application.ManutencaoAssunto;
using DesafioTjRjErlimar.WebApi.Model;

using Microsoft.AspNetCore.Mvc;

namespace DesafioTjRjErlimar.WebApi.Controllers;

[ApiController]
[Route("assuntos")]
public class AssuntoController : ControllerBase
{
    private readonly ManutencaoAssuntoAppService _manutencaoAssuntoAppService;

    public AssuntoController(ManutencaoAssuntoAppService manutencaoAssuntoAppService)
    {
        _manutencaoAssuntoAppService = manutencaoAssuntoAppService
            ?? throw new ArgumentNullException(nameof(manutencaoAssuntoAppService));
    }

    /// <summary>
    /// Cadastra um novo assunto
    /// </summary>
    /// <param name="model">Dados para cadastro</param>
    /// <returns>Dados do assunto cadatrado</returns>
    /// <response code="201">Assunto cadastrado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AssuntoViewModel>> CadastrarNovoAssunto(DadosAssuntoViewModel model)
    {
        _ = model ?? throw new ArgumentNullException(nameof(model));
        _ = model.Descricao ?? throw new ArgumentNullException(nameof(model.Descricao));

        try
        {
            var assuntoCadastrado = await _manutencaoAssuntoAppService.AdicionarAssuntoAsync(new Assunto
            {
                Descricao = model.Descricao.Trim()
            });

            return Created("", new AssuntoViewModel
            {
                Codigo = assuntoCadastrado.AssuntoId,
                Descricao = assuntoCadastrado.Descricao
            });
        }
        catch (RegistroRepetidoException ex)
        {
            ModelState.AddModelError("descricao", ex.Message);

            return ValidationProblem(modelStateDictionary: ModelState, title: "Não foi possível cadastrar o assunto");
        }
    }

    /// <summary>
    /// Atualiza cadastro de um assunto
    /// </summary>
    /// <param name="model">Dados para cadastro</param>
    /// <returns>Dados do assunto cadatrado</returns>
    /// <response code="200">Assunto atualizado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    [HttpPost("{assuntoId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AssuntoViewModel>> AtualizarAssunto(
        [Range(1, int.MaxValue, ErrorMessage = "O identificador precisa ser maior que zero")]
        int assuntoId,
        DadosAssuntoViewModel model)
    {
        _ = model ?? throw new ArgumentNullException(nameof(model));
        _ = model.Descricao ?? throw new ArgumentNullException(nameof(model.Descricao));

        try
        {
            var autorAtualizado = await _manutencaoAssuntoAppService.AtualizarAssuntoAsync(new Assunto
            {
                AssuntoId = assuntoId,
                Descricao = model.Descricao.Trim()
            });

            return Ok(new AssuntoViewModel
            {
                Codigo = autorAtualizado.AssuntoId,
                Descricao = autorAtualizado.Descricao
            });
        }
        catch (RegistroRepetidoException ex)
        {
            ModelState.AddModelError("descricao", ex.Message);

            return ValidationProblem(modelStateDictionary: ModelState, title: "Não foi possível atualizar o assunto");
        }
        catch (RegistroInexistenteException ex)
        {
            ModelState.AddModelError(nameof(assuntoId), ex.Message);
            return ValidationProblem(modelStateDictionary: ModelState, title: "Não foi possível atualizar o assunto");
        }
    }

    /// <summary>
    /// Lista todos os assuntos
    /// </summary>
    /// <response code="200">Lista de assuntos</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AssuntoViewModel>>> ListarAssuntos()
    {
        var assuntos = await _manutencaoAssuntoAppService.ObterAssuntosAsync();

        var assuntoViewModel = assuntos.Select(a => new AssuntoViewModel
        {
            Codigo = a.AssuntoId,
            Descricao = a.Descricao
        });

        return Ok(assuntoViewModel);
    }

    /// <summary>
    /// Remove um assunto
    /// </summary>
    /// <param name="assuntoId">Identificador do assunto para excluir</param>
    [HttpDelete("{assuntoId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> RemoverAssunto(
        [Range(1, int.MaxValue, ErrorMessage = "O identificador precisa ser maior que zero")]
        int assuntoId)
    {
        try
        {
            await _manutencaoAssuntoAppService.RemoverAssuntoPorIdAsync(assuntoId);
        }
        catch (RegistroInexistenteException ex)
        {
            ModelState.AddModelError(nameof(assuntoId), ex.Message);

            return ValidationProblem(modelStateDictionary: ModelState, title: "Não foi possível remover o assunto");
        }

        return NoContent();
    }
}