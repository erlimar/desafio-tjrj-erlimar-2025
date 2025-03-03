using DesafioTjRjErlimar.Application.ManutencaoDeLivros;
using DesafioTjRjErlimar.Application.ManutencaoDeLivros.Exceptions;
using DesafioTjRjErlimar.Application.ManutencaoDeLivros.Model;

using Moq;

namespace DesafioTjRjErlimar.ApplicationTests.ManutencaoDeLivros;

[Trait("target", nameof(ManutencaoLivroAppService))]
public class ManutencaoLivroAppServiceTest
{
    [Fact(DisplayName = "Um repositório é obrigatório")]
    public void UmRepositorioEhObrigatorio()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => _ = new ManutencaoLivroAppService(null!)
        );

        Assert.Equal("repository", exception.ParamName);
    }

    [Fact(DisplayName = "Adicionar autor não aceita nulo")]
    public async Task AdicionarAutorNaoAceitaNulo()
    {
        var service = new ManutencaoLivroAppService(new Mock<IManutencaoLivroAppRepository>().Object);

        var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            () => _ = service.AdicionarAutorAsync(null!)
        );

        Assert.Equal("autor", exception.ParamName);
    }

    [Fact(DisplayName = "Não se pode mais de um autor com mesmo identificador")]
    public async Task NaoSePodeMaisDeUmAutorComMesmoId()
    {
        var mock = new Mock<IManutencaoLivroAppRepository>();

        mock.Setup(m => m.ExisteAutorComIdAsync(1)).ReturnsAsync(true);

        var service = new ManutencaoLivroAppService(mock.Object);

        var exception = await Assert.ThrowsAsync<AutorRepetidoException>(
            () => _ = service.AdicionarAutorAsync(new Autor
            {
                AutorId = 1,
                Nome = "Autor 1"
            })
        );

        Assert.Equal("Autor com identificador 1 já existe", exception.Message);

        mock.Verify(m => m.ExisteAutorComIdAsync(1), Times.Once);
    }

    [Fact(DisplayName = "Não se pode mais de um autor com mesmo nome")]
    public async Task NaoSePodeMaisDeUmAutorComMesmoNome()
    {
        var mock = new Mock<IManutencaoLivroAppRepository>();

        mock.Setup(m => m.ExisteAutorComNomeAsync("Nome já existente")).ReturnsAsync(true);

        var service = new ManutencaoLivroAppService(mock.Object);

        var exception = await Assert.ThrowsAsync<AutorRepetidoException>(
            () => _ = service.AdicionarAutorAsync(new Autor
            {
                AutorId = 1,
                Nome = "Nome já existente"
            })
        );

        Assert.Equal("Autor com nome 'Nome já existente' já existe", exception.Message);

        mock.Verify(m => m.ExisteAutorComNomeAsync("Nome já existente"), Times.Once);
    }

    /// <summary>
    /// Quando se cadastra um autor informando um Id diferente de zero, e o cadastro é permitido,
    /// o autor deve ser cadastrado com o Id informado.
    /// </summary>
    [Fact(DisplayName = "Cadastro permitido com id não zero permanece inauterado")]
    public async Task CadastroPermitidoComIdNaoZeroPermaneceInauterado()
    {
        var mock = new Mock<IManutencaoLivroAppRepository>();

        mock.Setup(m => m.CadastraNovoAutorAsync(It.IsAny<Autor>()))
            .ReturnsAsync(new Autor
            {
                AutorId = 1,
                Nome = "Cadastro permitido"
            });

        var service = new ManutencaoLivroAppService(mock.Object);

        var cadastro = await service.AdicionarAutorAsync(new Autor
        {
            AutorId = 1,
            Nome = "Cadastro permitido"
        });

        Assert.NotNull(cadastro);
        Assert.Equal(1, cadastro.AutorId);
        Assert.Equal("Cadastro permitido", cadastro.Nome);

        mock.Verify(m => m.CadastraNovoAutorAsync(It.IsAny<Autor>()), Times.Once);
    }

    /// <summary>
    /// Quando se cadastra um autor informando um Id zero, e o cadastro é permitido,
    /// o autor deve ser cadastrado com o Id gerado automaticamente diferente de zero.
    /// </summary>
    [Fact(DisplayName = "Cadastro permitido com id zero gera um novo identificador")]
    public async Task CadastroPermitidoComIdZeroGeraUmNovoId()
    {
        var mock = new Mock<IManutencaoLivroAppRepository>();

        mock.Setup(m => m.CadastraNovoAutorAsync(It.IsAny<Autor>()))
            .ReturnsAsync(new Autor
            {
                AutorId = 1,
                Nome = "Cadastro permitido"
            });

        var service = new ManutencaoLivroAppService(mock.Object);

        var cadastro = await service.AdicionarAutorAsync(new Autor
        {
            AutorId = 0,
            Nome = "Cadastro permitido"
        });

        Assert.NotNull(cadastro);
        Assert.NotEqual(0, cadastro.AutorId);

        mock.Verify(m => m.CadastraNovoAutorAsync(It.IsAny<Autor>()), Times.Once);
    }

    [Fact(DisplayName = "Lista de autores sempre está ordenada por nome")]
    public async Task ListaDeAutoresSempreEstaOrdenadaPorNome()
    {
        var mock = new Mock<IManutencaoLivroAppRepository>();

        mock.Setup(m => m.ListarAutoresAsync())
            .ReturnsAsync(new List<Autor>()
            {
                new Autor { AutorId = 1, Nome = "Z" },
                new Autor { AutorId = 2, Nome = "A" },
                new Autor { AutorId = 3, Nome = "B" }
            });

        var service = new ManutencaoLivroAppService(mock.Object);

        var autoresOrdenados = await service.ObterAutoresAsync();

        Assert.NotNull(autoresOrdenados);
        Assert.NotEmpty(autoresOrdenados);
        Assert.Equal(3, autoresOrdenados.Count());

        Assert.Equal(2, autoresOrdenados.ElementAt(0).AutorId);
        Assert.Equal(3, autoresOrdenados.ElementAt(1).AutorId);
        Assert.Equal(1, autoresOrdenados.ElementAt(2).AutorId);

        mock.Verify(m => m.ListarAutoresAsync(), Times.Once);
    }

    [Fact(DisplayName = "Não se pode excluir um autor não cadastrado")]
    public async Task NaoSePodeExcluirUmAutorNaoCadastrado()
    {
        var mock = new Mock<IManutencaoLivroAppRepository>();

        mock.Setup(m => m.ExisteAutorComIdAsync(100)).ReturnsAsync(false);

        var service = new ManutencaoLivroAppService(mock.Object);

        var exception = await Assert.ThrowsAsync<RegistroInexistenteException>(
            () => _ = service.RemoverAutorPorIdAsync(100)
        );

        Assert.Equal("Autor com identificador 100 não existe para ser excluído", exception.Message);

        mock.Verify(m => m.ExisteAutorComIdAsync(100), Times.Once);
    }

    [Fact(DisplayName = "Se pode excluir um autor cadastrado")]
    public async Task SePodeExcluirUmAutorCadastrado()
    {
        var mock = new Mock<IManutencaoLivroAppRepository>();

        mock.Setup(m => m.ExisteAutorComIdAsync(100)).ReturnsAsync(true);

        var service = new ManutencaoLivroAppService(mock.Object);

        await service.RemoverAutorPorIdAsync(100);

        Assert.True(true);

        mock.Verify(m => m.ExisteAutorComIdAsync(100), Times.Once);
    }

    [Fact(DisplayName = "Atualizar autor não aceita nulo")]
    public async Task AtualizarAutorNaoAceitaNulo()
    {
        var service = new ManutencaoLivroAppService(new Mock<IManutencaoLivroAppRepository>().Object);

        var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            () => _ = service.AtualizarAutorAsync(null!)
        );

        Assert.Equal("autor", exception.ParamName);
    }

    [Fact(DisplayName = "Não se pode alterar nome de autor para nome já existente")]
    public async Task NaoSePodeAlterarNomeDeAutorParaNomeJaExistente()
    {
        var mock = new Mock<IManutencaoLivroAppRepository>();

        mock.Setup(m => m.ExisteAutorComNomeExcetoIdAsync("Nome já existente", 200)).ReturnsAsync(true);

        var service = new ManutencaoLivroAppService(mock.Object);

        var exception = await Assert.ThrowsAsync<AutorRepetidoException>(
            () => _ = service.AtualizarAutorAsync(new Autor
            {
                AutorId = 200,
                Nome = "Nome já existente"
            })
        );

        Assert.Equal("Já existe um autor com o novo nome 'Nome já existente' pretendido", exception.Message);

        mock.Verify(m => m.ExisteAutorComNomeExcetoIdAsync("Nome já existente", 200), Times.Once);
    }

    [Fact(DisplayName = "Se pode atualizar um autor com nome não utilizado")]
    public async Task SePodeAtualizarUmAutorComNomeNaoUtilizado()
    {
        var mock = new Mock<IManutencaoLivroAppRepository>();

        mock.Setup(m => m.ExisteAutorComNomeExcetoIdAsync("Nome não existente", 23)).ReturnsAsync(false);
        mock.Setup(m => m.AtualizarAutorAsync(It.IsAny<Autor>()))
            .ReturnsAsync(new Autor
            {
                AutorId = 23,
                Nome = "Nome não existente"
            });

        var service = new ManutencaoLivroAppService(mock.Object);

        var autorAtualizado = await service.AtualizarAutorAsync(new Autor
        {
            AutorId = 23,
            Nome = "Nome não existente"
        });

        Assert.NotNull(autorAtualizado);

        mock.Verify(m => m.ExisteAutorComNomeExcetoIdAsync("Nome não existente", 23), Times.Once);
        mock.Verify(m => m.AtualizarAutorAsync(It.IsAny<Autor>()), Times.Once);
    }
}