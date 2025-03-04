using System.ComponentModel.DataAnnotations;

using DesafioTjRjErlimar.Application;
using DesafioTjRjErlimar.Application.ManutencaoLivro;
using DesafioTjRjErlimar.WebApi.Model;

using Microsoft.AspNetCore.Mvc;

namespace DesafioTjRjErlimar.WebApi.Controllers;

[ApiController]
[Route("livros")]
public class LivroController : ControllerBase
{
    private readonly ManutencaoLivroAppService _manutencaoLivroAppService;

    public LivroController(ManutencaoLivroAppService manutencaoLivroAppService)
    {
        _manutencaoLivroAppService = manutencaoLivroAppService
            ?? throw new ArgumentNullException(nameof(manutencaoLivroAppService));
    }

    /// <summary>
    /// Cadastra um novo livro
    /// </summary>
    /// <param name="model">Dados para cadastro</param>
    /// <returns>Dados do livro cadatrado</returns>
    /// <response code="201">Livro cadastrado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LivroViewModel>> CadastrarNovoLivro(DadosLivroViewModel model)
    {
        _ = model ?? throw new ArgumentNullException(nameof(model));
        _ = model.Titulo ?? throw new ArgumentNullException(nameof(model.Titulo));
        _ = model.Editora ?? throw new ArgumentNullException(nameof(model.Editora));

        try
        {
            var livroCadastrado = await _manutencaoLivroAppService.AdicionarLivroAsync(new Livro
            {
                Titulo = model.Titulo.Trim(),
                Editora = model.Editora.Trim(),
                Edicao = model.Edicao,
                AnoPublicacao = model.AnoPublicacao
            },
            model.Autores.Select(s => s.Codigo),
            model.Assuntos.Select(s => s.Codigo));

            return Created("", new LivroViewModel
            {
                Codigo = livroCadastrado.LivroId,
                Titulo = livroCadastrado.Titulo,
                Editora = livroCadastrado.Editora,
                Edicao = livroCadastrado.Edicao,
                AnoPublicacao = livroCadastrado.AnoPublicacao
            });
        }
        catch (RegistroRepetidoException ex)
        {
            ModelState.AddModelError("titulo", ex.Message);

            return ValidationProblem(modelStateDictionary: ModelState, title: "Não foi possível cadastrar o livro");
        }
    }

    /// <summary>
    /// Lista todos os livros
    /// </summary>
    /// <response code="200">Lista de livros</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<LivroViewModel>>> ListarLivros()
    {
        var livros = await _manutencaoLivroAppService.ObterLivrosAsync();

        var livrosViewModel = livros.Select(a => new LivroViewModel
        {
            Codigo = a.LivroId,
            Titulo = a.Titulo,
            Editora = a.Editora,
            Edicao = a.Edicao,
            AnoPublicacao = a.AnoPublicacao
        });

        return Ok(livrosViewModel);
    }

    /// <summary>
    /// Lista autores de um livro
    /// </summary>
    /// <response code="200">Lista de autores do livro</response>
    [HttpGet]
    [Route("{livroId}/autores")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AutorViewModel>>> ListarAutoresDoLivro(
        [Range(1, int.MaxValue, ErrorMessage = "O identificador precisa ser maior que zero")]
        int livroId)
    {
        var autores = await _manutencaoLivroAppService.ObterAutoresDoLivroAsync(livroId);

        var autoresViewModel = autores.Select(a => new AutorViewModel
        {
            Codigo = a.AutorId,
            Nome = a.Nome
        });

        return Ok(autoresViewModel);
    }

    /// <summary>
    /// Lista assuntos de um livro
    /// </summary>
    /// <response code="200">Lista de assuntos do livro</response>
    [HttpGet]
    [Route("{livroId}/assuntos")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AssuntoViewModel>>> ListarAssuntosDoLivro(
        [Range(1, int.MaxValue, ErrorMessage = "O identificador precisa ser maior que zero")]
        int livroId)
    {
        var assuntos = await _manutencaoLivroAppService.ObterAssuntosDoLivroAsync(livroId);

        var assuntosViewModel = assuntos.Select(a => new AssuntoViewModel
        {
            Codigo = a.AssuntoId,
            Descricao = a.Descricao
        });

        return Ok(assuntosViewModel);
    }

    /// <summary>
    /// Remove um livro
    /// </summary>
    /// <param name="livroId">Identificador do livro para excluir</param>
    [HttpDelete("{livroId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> RemoverLivro(
        [Range(1, int.MaxValue, ErrorMessage = "O identificador precisa ser maior que zero")]
        int livroId)
    {
        try
        {
            await _manutencaoLivroAppService.RemoverLivroPorIdAsync(livroId);
        }
        catch (RegistroInexistenteException ex)
        {
            ModelState.AddModelError(nameof(livroId), ex.Message);

            return ValidationProblem(modelStateDictionary: ModelState, title: "Não foi possível remover o livro");
        }

        return NoContent();
    }

    /// <summary>
    /// Atualiza cadastro de um livro
    /// </summary>
    /// <param name="model">Dados para cadastro</param>
    /// <returns>Dados do livro cadatrado</returns>
    /// <response code="200">Livro atualizado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    [HttpPost("{livroId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LivroViewModel>> AtualizarLivro(
        [Range(1, int.MaxValue, ErrorMessage = "O identificador precisa ser maior que zero")]
        int livroId,
        DadosLivroViewModel model)
    {
        _ = model ?? throw new ArgumentNullException(nameof(model));
        _ = model.Titulo ?? throw new ArgumentNullException(nameof(model.Titulo));
        _ = model.Editora ?? throw new ArgumentNullException(nameof(model.Editora));

        try
        {
            var livroAtualizado = await _manutencaoLivroAppService.AtualizarLivroAsync(new Livro
            {
                LivroId = livroId,
                Titulo = model.Titulo.Trim(),
                Editora = model.Editora.Trim(),
                Edicao = model.Edicao,
                AnoPublicacao = model.AnoPublicacao
            },
            model.Autores?.Select(s => s.Codigo),
            model.Assuntos?.Select(s => s.Codigo));

            return Ok(new LivroViewModel
            {
                Codigo = livroAtualizado.LivroId,
                Titulo = livroAtualizado.Titulo,
                Editora = livroAtualizado.Editora,
                Edicao = livroAtualizado.Edicao,
                AnoPublicacao = livroAtualizado.AnoPublicacao
            });
        }
        catch (RegistroRepetidoException ex)
        {
            ModelState.AddModelError("titulo", ex.Message);

            return ValidationProblem(modelStateDictionary: ModelState, title: "Não foi possível atualizar o livro");
        }
        catch (RegistroInexistenteException ex)
        {
            ModelState.AddModelError(nameof(livroId), ex.Message);
            return ValidationProblem(modelStateDictionary: ModelState, title: "Não foi possível atualizar o livro");
        }
    }
}