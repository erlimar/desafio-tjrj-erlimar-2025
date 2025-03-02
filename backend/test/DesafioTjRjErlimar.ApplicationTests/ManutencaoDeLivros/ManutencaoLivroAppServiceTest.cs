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
    }
}