using DesafioTjRjErlimar.Application;
using DesafioTjRjErlimar.Application.ManutencaoAssunto;

using Moq;

namespace DesafioTjRjErlimar.ApplicationTests.ManutencaoAssunto;

[Trait("target", nameof(ManutencaoAssuntoAppService))]
public class ManutencaoAssuntoAppServiceTest
{
    [Fact(DisplayName = "Um repositório é obrigatório")]
    public void UmRepositorioEhObrigatorio()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => _ = new ManutencaoAssuntoAppService(null!)
        );

        Assert.Equal("repository", exception.ParamName);
    }

    [Fact(DisplayName = "Adicionar assunto não aceita nulo")]
    public async Task AdicionarAssuntoNaoAceitaNulo()
    {
        var service = new ManutencaoAssuntoAppService(new Mock<IManutencaoAssuntoAppRepository>().Object);

        var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            () => _ = service.AdicionarAssuntoAsync(null!)
        );

        Assert.Equal("assunto", exception.ParamName);
    }

    [Fact(DisplayName = "Não se pode mais de um assunto com mesmo identificador")]
    public async Task NaoSePodeMaisDeUmAssuntoComMesmoId()
    {
        var mock = new Mock<IManutencaoAssuntoAppRepository>();

        mock.Setup(m => m.ExisteAssuntoComIdAsync(1)).ReturnsAsync(true);

        var service = new ManutencaoAssuntoAppService(mock.Object);

        var exception = await Assert.ThrowsAsync<RegistroRepetidoException>(
            () => _ = service.AdicionarAssuntoAsync(new Assunto
            {
                AssuntoId = 1,
                Descricao = "Assunto 1"
            })
        );

        Assert.Equal("Assunto com identificador 1 já existe", exception.Message);

        mock.Verify(m => m.ExisteAssuntoComIdAsync(1), Times.Once);
    }

    [Fact(DisplayName = "Não se pode mais de um assunto com mesma descrição")]
    public async Task NaoSePodeMaisDeUmAssuntoComMesmaDescricao()
    {
        var mock = new Mock<IManutencaoAssuntoAppRepository>();

        mock.Setup(m => m.ExisteAssuntoComDescricaoAsync("Descrição já existente")).ReturnsAsync(true);

        var service = new ManutencaoAssuntoAppService(mock.Object);

        var exception = await Assert.ThrowsAsync<RegistroRepetidoException>(
            () => _ = service.AdicionarAssuntoAsync(new Assunto
            {
                AssuntoId = 1,
                Descricao = "Descrição já existente"
            })
        );

        Assert.Equal("Assunto com descrição 'Descrição já existente' já existe", exception.Message);

        mock.Verify(m => m.ExisteAssuntoComDescricaoAsync("Descrição já existente"), Times.Once);
    }

    /// <summary>
    /// Quando se cadastra um assunto informando um Id diferente de zero, e o cadastro é permitido,
    /// o assunto deve ser cadastrado com o Id informado.
    /// </summary>
    [Fact(DisplayName = "Cadastro permitido com id não zero permanece inalterado")]
    public async Task CadastroPermitidoComIdNaoZeroPermaneceInalterado()
    {
        var mock = new Mock<IManutencaoAssuntoAppRepository>();

        mock.Setup(m => m.CadastraNovoAssuntoAsync(It.IsAny<Assunto>()))
            .ReturnsAsync(new Assunto
            {
                AssuntoId = 1,
                Descricao = "Cadastro permitido"
            });

        var service = new ManutencaoAssuntoAppService(mock.Object);

        var cadastro = await service.AdicionarAssuntoAsync(new Assunto
        {
            AssuntoId = 1,
            Descricao = "Cadastro permitido"
        });

        Assert.NotNull(cadastro);
        Assert.Equal(1, cadastro.AssuntoId);
        Assert.Equal("Cadastro permitido", cadastro.Descricao);

        mock.Verify(m => m.CadastraNovoAssuntoAsync(It.IsAny<Assunto>()), Times.Once);
    }

    /// <summary>
    /// Quando se cadastra um assunto informando um Id zero, e o cadastro é permitido,
    /// o assunto deve ser cadastrado com o Id gerado automaticamente diferente de zero.
    /// </summary>
    [Fact(DisplayName = "Cadastro permitido com id zero gera um novo identificador")]
    public async Task CadastroPermitidoComIdZeroGeraUmNovoId()
    {
        var mock = new Mock<IManutencaoAssuntoAppRepository>();

        mock.Setup(m => m.CadastraNovoAssuntoAsync(It.IsAny<Assunto>()))
            .ReturnsAsync(new Assunto
            {
                AssuntoId = 1,
                Descricao = "Cadastro permitido"
            });

        var service = new ManutencaoAssuntoAppService(mock.Object);

        var cadastro = await service.AdicionarAssuntoAsync(new Assunto
        {
            AssuntoId = 0,
            Descricao = "Cadastro permitido"
        });

        Assert.NotNull(cadastro);
        Assert.NotEqual(0, cadastro.AssuntoId);

        mock.Verify(m => m.CadastraNovoAssuntoAsync(It.IsAny<Assunto>()), Times.Once);
    }

    [Fact(DisplayName = "Lista de assuntos sempre está ordenada por descrição")]
    public async Task ListaDeAssuntosSempreEstaOrdenadaPorDescricao()
    {
        var mock = new Mock<IManutencaoAssuntoAppRepository>();

        mock.Setup(m => m.ListarAssuntosAsync())
            .ReturnsAsync(new List<Assunto>()
            {
                new Assunto { AssuntoId = 1, Descricao = "Z" },
                new Assunto { AssuntoId = 2, Descricao = "A" },
                new Assunto { AssuntoId = 3, Descricao = "B" }
            });

        var service = new ManutencaoAssuntoAppService(mock.Object);

        var assuntosOrdenados = await service.ObterAssuntosAsync();

        Assert.NotNull(assuntosOrdenados);
        Assert.NotEmpty(assuntosOrdenados);
        Assert.Equal(3, assuntosOrdenados.Count());

        Assert.Equal(2, assuntosOrdenados.ElementAt(0).AssuntoId);
        Assert.Equal(3, assuntosOrdenados.ElementAt(1).AssuntoId);
        Assert.Equal(1, assuntosOrdenados.ElementAt(2).AssuntoId);

        mock.Verify(m => m.ListarAssuntosAsync(), Times.Once);
    }

    [Fact(DisplayName = "Não se pode excluir um assunto não cadastrado")]
    public async Task NaoSePodeExcluirUmAssuntoNaoCadastrado()
    {
        var mock = new Mock<IManutencaoAssuntoAppRepository>();

        mock.Setup(m => m.ExisteAssuntoComIdAsync(100)).ReturnsAsync(false);

        var service = new ManutencaoAssuntoAppService(mock.Object);

        var exception = await Assert.ThrowsAsync<RegistroInexistenteException>(
            () => _ = service.RemoverAssuntoPorIdAsync(100)
        );

        Assert.Equal("Assunto com identificador 100 não existe para ser excluído", exception.Message);

        mock.Verify(m => m.ExisteAssuntoComIdAsync(100), Times.Once);
    }

    [Fact(DisplayName = "Se pode excluir um assnto cadastrado")]
    public async Task SePodeExcluirUmAssuntoCadastrado()
    {
        var mock = new Mock<IManutencaoAssuntoAppRepository>();

        mock.Setup(m => m.ExisteAssuntoComIdAsync(100)).ReturnsAsync(true);

        var service = new ManutencaoAssuntoAppService(mock.Object);

        await service.RemoverAssuntoPorIdAsync(100);

        Assert.True(true);

        mock.Verify(m => m.ExisteAssuntoComIdAsync(100), Times.Once);
    }

    [Fact(DisplayName = "Atualizar assunto não aceita nulo")]
    public async Task AtualizarAssuntoNaoAceitaNulo()
    {
        var service = new ManutencaoAssuntoAppService(new Mock<IManutencaoAssuntoAppRepository>().Object);

        var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            () => _ = service.AtualizarAssuntoAsync(null!)
        );

        Assert.Equal("assunto", exception.ParamName);
    }

    [Fact(DisplayName = "Não se pode alterar descrição de assunto para desçrição já existente")]
    public async Task NaoSePodeAlterarDescricaoDeAssuntoParaDescricaoJaExistente()
    {
        var mock = new Mock<IManutencaoAssuntoAppRepository>();

        mock.Setup(m => m.ExisteAssuntoComIdAsync(200)).ReturnsAsync(true);
        mock.Setup(m => m.ExisteAssuntoComDescricaoExcetoIdAsync("Descrição já existente", 200)).ReturnsAsync(true);

        var service = new ManutencaoAssuntoAppService(mock.Object);

        var exception = await Assert.ThrowsAsync<RegistroRepetidoException>(
            () => _ = service.AtualizarAssuntoAsync(new Assunto
            {
                AssuntoId = 200,
                Descricao = "Descrição já existente"
            })
        );

        Assert.Equal("Já existe um assunto com a nova descrição 'Descrição já existente' pretendida", exception.Message);

        mock.Verify(m => m.ExisteAssuntoComIdAsync(200), Times.Once);
        mock.Verify(m => m.ExisteAssuntoComDescricaoExcetoIdAsync("Descrição já existente", 200), Times.Once);
    }

    [Fact(DisplayName = "Não se pode atualizar um assunto não cadastrado")]
    public async Task NaoSePodeAtualizarUmAssuntoNaoCadastrado()
    {
        var mock = new Mock<IManutencaoAssuntoAppRepository>();

        mock.Setup(m => m.ExisteAssuntoComIdAsync(70)).ReturnsAsync(false);

        var service = new ManutencaoAssuntoAppService(mock.Object);

        var exception = await Assert.ThrowsAsync<RegistroInexistenteException>(
            () => _ = service.AtualizarAssuntoAsync(new Assunto
            {
                AssuntoId = 70,
                Descricao = "Descrição Ok"
            })
        );

        Assert.Equal("Assunto com identificador 70 não existe para ser atualizado", exception.Message);

        mock.Verify(m => m.ExisteAssuntoComIdAsync(70), Times.Once);
    }

    [Fact(DisplayName = "Se pode atualizar um assunto com descrição não utilizada")]
    public async Task SePodeAtualizarUmAssuntoComDescricaoNaoUtilizada()
    {
        var mock = new Mock<IManutencaoAssuntoAppRepository>();

        mock.Setup(m => m.ExisteAssuntoComIdAsync(23)).ReturnsAsync(true);
        mock.Setup(m => m.ExisteAssuntoComDescricaoExcetoIdAsync("Descrição não existente", 23)).ReturnsAsync(false);
        mock.Setup(m => m.AtualizarAssuntoAsync(It.IsAny<Assunto>()))
            .ReturnsAsync(new Assunto
            {
                AssuntoId = 23,
                Descricao = "Descrição não existente"
            });

        var service = new ManutencaoAssuntoAppService(mock.Object);

        var assuntoAtualizado = await service.AtualizarAssuntoAsync(new Assunto
        {
            AssuntoId = 23,
            Descricao = "Descrição não existente"
        });

        Assert.NotNull(assuntoAtualizado);

        mock.Verify(m => m.ExisteAssuntoComIdAsync(23), Times.Once);
        mock.Verify(m => m.ExisteAssuntoComDescricaoExcetoIdAsync("Descrição não existente", 23), Times.Once);
        mock.Verify(m => m.AtualizarAssuntoAsync(It.IsAny<Assunto>()), Times.Once);
    }
}