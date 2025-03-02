using DesafioTjRjErlimar.Application.ManutencaoDeLivros;
using DesafioTjRjErlimar.Application.ManutencaoDeLivros.Model;
using DesafioTjRjErlimar.WebApi.Model;

using Microsoft.AspNetCore.Mvc;

namespace DesafioTjRjErlimar.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AutorController : ControllerBase
{
    private readonly ILogger<AutorController> _logger;
    private readonly ManutencaoLivroAppService _manutencaoLivroAppService;

    public AutorController(ILogger<AutorController> logger, ManutencaoLivroAppService manutencaoLivroAppService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _manutencaoLivroAppService = manutencaoLivroAppService ?? throw new ArgumentNullException(nameof(manutencaoLivroAppService));
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
    public async Task<ActionResult<AutorViewModel>> CadastrarNovoAutor(CadastrarNovoAutorViewModel model)
    {
        _ = model ?? throw new ArgumentNullException(nameof(model));
        _ = model.Nome ?? throw new ArgumentNullException(nameof(model.Nome));

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
}