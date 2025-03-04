using DesafioTjRjErlimar.Application;
using DesafioTjRjErlimar.Application.ManutencaoLivro;

using Moq;

namespace DesafioTjRjErlimar.ApplicationTests.ManutencaoAssunto;

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

    [Fact(DisplayName = "Adicionar livro não aceita nulo")]
    public async Task AdicionarLivroNaoAceitaNulo()
    {
        var service = new ManutencaoLivroAppService(new Mock<IManutencaoLivroAppRepository>().Object);

        var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            () => _ = service.AdicionarLivroAsync(null!)
        );

        Assert.Equal("livro", exception.ParamName);
    }

    [Fact(DisplayName = "Não se pode mais de um livro com mesmo identificador")]
    public async Task NaoSePodeMaisDeUmLivroComMesmoId()
    {
        var mock = new Mock<IManutencaoLivroAppRepository>();

        mock.Setup(m => m.ExisteLivroComIdAsync(1)).ReturnsAsync(true);

        var service = new ManutencaoLivroAppService(mock.Object);

        var exception = await Assert.ThrowsAsync<RegistroRepetidoException>(
            () => _ = service.AdicionarLivroAsync(new Livro
            {
                LivroId = 1,
                Titulo = "Livro 1",
                Editora = "Editora 1",
            })
        );

        Assert.Equal("Livro com identificador 1 já existe", exception.Message);

        mock.Verify(m => m.ExisteLivroComIdAsync(1), Times.Once);
    }

    [Fact(DisplayName = "Não se pode mais de um livro com mesmo título")]
    public async Task NaoSePodeMaisDeUmLivroComMesmoTitulo()
    {
        var mock = new Mock<IManutencaoLivroAppRepository>();

        mock.Setup(m => m.ExisteLivroComTituloAsync("Título já existente")).ReturnsAsync(true);

        var service = new ManutencaoLivroAppService(mock.Object);

        var exception = await Assert.ThrowsAsync<RegistroRepetidoException>(
            () => _ = service.AdicionarLivroAsync(new Livro
            {
                LivroId = 1,
                Titulo = "Título já existente",
                Editora = "Editora 1",
            })
        );

        Assert.Equal("Livro com título 'Título já existente' já existe", exception.Message);

        mock.Verify(m => m.ExisteLivroComTituloAsync("Título já existente"), Times.Once);
    }

    /// <summary>
    /// Quando se cadastra um livro informando um Id diferente de zero, e o cadastro é permitido,
    /// o livro deve ser cadastrado com o Id informado.
    /// </summary>
    [Fact(DisplayName = "Cadastro permitido com id não zero permanece inalterado")]
    public async Task CadastroPermitidoComIdNaoZeroPermaneceInalterado()
    {
        var mock = new Mock<IManutencaoLivroAppRepository>();

        mock.Setup(m => m.CadastraNovoLivroAsync(It.IsAny<Livro>()))
            .ReturnsAsync(new Livro
            {
                LivroId = 1,
                Titulo = "Cadastro permitido",
                Editora = "Editora 1",
            });

        var service = new ManutencaoLivroAppService(mock.Object);

        var cadastro = await service.AdicionarLivroAsync(new Livro
        {
            LivroId = 1,
            Titulo = "Cadastro permitido",
            Editora = "Editora 1",
        });

        Assert.NotNull(cadastro);
        Assert.Equal(1, cadastro.LivroId);
        Assert.Equal("Cadastro permitido", cadastro.Titulo);

        mock.Verify(m => m.CadastraNovoLivroAsync(It.IsAny<Livro>()), Times.Once);
    }

    /// <summary>
    /// Quando se cadastra um livro informando um Id zero, e o cadastro é permitido,
    /// o livro deve ser cadastrado com o Id gerado automaticamente diferente de zero.
    /// </summary>
    [Fact(DisplayName = "Cadastro permitido com id zero gera um novo identificador")]
    public async Task CadastroPermitidoComIdZeroGeraUmNovoId()
    {
        var mock = new Mock<IManutencaoLivroAppRepository>();

        mock.Setup(m => m.CadastraNovoLivroAsync(It.IsAny<Livro>()))
            .ReturnsAsync(new Livro
            {
                LivroId = 1,
                Titulo = "Cadastro permitido",
                Editora = "Editora 1",
            });

        var service = new ManutencaoLivroAppService(mock.Object);

        var cadastro = await service.AdicionarLivroAsync(new Livro
        {
            LivroId = 0,
            Titulo = "Cadastro permitido",
            Editora = "Editora 1",
        });

        Assert.NotNull(cadastro);
        Assert.NotEqual(0, cadastro.LivroId);

        mock.Verify(m => m.CadastraNovoLivroAsync(It.IsAny<Livro>()), Times.Once);
    }

    [Fact(DisplayName = "Lista de livros sempre está ordenada por título")]
    public async Task ListaDeLivrosSempreEstaOrdenadaPorTitulo()
    {
        var mock = new Mock<IManutencaoLivroAppRepository>();

        mock.Setup(m => m.ListarLivrosAsync())
            .ReturnsAsync(new List<Livro>()
            {
                new Livro { LivroId = 1, Editora = "Editora", Titulo = "Z" },
                new Livro { LivroId = 2, Editora = "Editora", Titulo = "A" },
                new Livro { LivroId = 3, Editora = "Editora", Titulo = "B" }
            });

        var service = new ManutencaoLivroAppService(mock.Object);

        var livrosOrdenados = await service.ObterLivrosAsync();

        Assert.NotNull(livrosOrdenados);
        Assert.NotEmpty(livrosOrdenados);
        Assert.Equal(3, livrosOrdenados.Count());

        Assert.Equal(2, livrosOrdenados.ElementAt(0).LivroId);
        Assert.Equal(3, livrosOrdenados.ElementAt(1).LivroId);
        Assert.Equal(1, livrosOrdenados.ElementAt(2).LivroId);

        mock.Verify(m => m.ListarLivrosAsync(), Times.Once);
    }

    [Fact(DisplayName = "Não se pode excluir um livro não cadastrado")]
    public async Task NaoSePodeExcluirUmLivroNaoCadastrado()
    {
        var mock = new Mock<IManutencaoLivroAppRepository>();

        mock.Setup(m => m.ExisteLivroComIdAsync(23)).ReturnsAsync(false);

        var service = new ManutencaoLivroAppService(mock.Object);

        var exception = await Assert.ThrowsAsync<RegistroInexistenteException>(
            () => _ = service.RemoverLivroPorIdAsync(23)
        );

        Assert.Equal("Livro com identificador 23 não existe para ser excluído", exception.Message);

        mock.Verify(m => m.ExisteLivroComIdAsync(23), Times.Once);
    }

    [Fact(DisplayName = "Se pode excluir um livro cadastrado")]
    public async Task SePodeExcluirUmLivroCadastrado()
    {
        var mock = new Mock<IManutencaoLivroAppRepository>();

        mock.Setup(m => m.ExisteLivroComIdAsync(50)).ReturnsAsync(true);

        var service = new ManutencaoLivroAppService(mock.Object);

        await service.RemoverLivroPorIdAsync(50);

        Assert.True(true);

        mock.Verify(m => m.ExisteLivroComIdAsync(50), Times.Once);
    }

    [Fact(DisplayName = "Atualizar livro não aceita nulo")]
    public async Task AtualizarLivroNaoAceitaNulo()
    {
        var service = new ManutencaoLivroAppService(new Mock<IManutencaoLivroAppRepository>().Object);

        var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            () => _ = service.AtualizarLivroAsync(null!)
        );

        Assert.Equal("livro", exception.ParamName);
    }

    [Fact(DisplayName = "Não se pode alterar o título de um livro para um título já existente")]
    public async Task NaoSePodeAlterarTituloDeLivroParaTituloJaExistente()
    {
        var mock = new Mock<IManutencaoLivroAppRepository>();

        mock.Setup(m => m.ExisteLivroComIdAsync(33)).ReturnsAsync(true);
        mock.Setup(m => m.ExisteLivroComTituloExcetoIdAsync("Título já existente", 33)).ReturnsAsync(true);

        var service = new ManutencaoLivroAppService(mock.Object);

        var exception = await Assert.ThrowsAsync<RegistroRepetidoException>(
            () => _ = service.AtualizarLivroAsync(new Livro
            {
                LivroId = 33,
                Titulo = "Título já existente",
                Editora = "Editora 1",
            })
        );

        Assert.Equal("Já existe um livro com o novo título 'Título já existente' pretendido", exception.Message);

        mock.Verify(m => m.ExisteLivroComIdAsync(33), Times.Once);
        mock.Verify(m => m.ExisteLivroComTituloExcetoIdAsync("Título já existente", 33), Times.Once);
    }

    [Fact(DisplayName = "Não se pode atualizar um livro não cadastrado")]
    public async Task NaoSePodeAtualizarUmLivroNaoCadastrado()
    {
        var mock = new Mock<IManutencaoLivroAppRepository>();

        mock.Setup(m => m.ExisteLivroComIdAsync(20)).ReturnsAsync(false);

        var service = new ManutencaoLivroAppService(mock.Object);

        var exception = await Assert.ThrowsAsync<RegistroInexistenteException>(
            () => _ = service.AtualizarLivroAsync(new Livro
            {
                LivroId = 20,
                Titulo = "Título Ok",
                Editora = "Editora 1",
            })
        );

        Assert.Equal("Livro com identificador 20 não existe para ser atualizado", exception.Message);

        mock.Verify(m => m.ExisteLivroComIdAsync(20), Times.Once);
    }

    [Fact(DisplayName = "Se pode atualizar um livro com título não utilizado")]
    public async Task SePodeAtualizarUmLivroComTituloNaoUtilizado()
    {
        var mock = new Mock<IManutencaoLivroAppRepository>();

        mock.Setup(m => m.ExisteLivroComIdAsync(90)).ReturnsAsync(true);
        mock.Setup(m => m.ExisteLivroComTituloExcetoIdAsync("Título não existente", 90)).ReturnsAsync(false);
        mock.Setup(m => m.AtualizarLivroAsync(It.IsAny<Livro>()))
            .ReturnsAsync(new Livro
            {
                LivroId = 90,
                Titulo = "Título não existente",
                Editora = "Editora 1",
            });

        var service = new ManutencaoLivroAppService(mock.Object);

        var livroAtualizado = await service.AtualizarLivroAsync(new Livro
        {
            LivroId = 90,
            Titulo = "Título não existente",
            Editora = "Editora 1",
        });

        Assert.NotNull(livroAtualizado);

        mock.Verify(m => m.ExisteLivroComIdAsync(90), Times.Once);
        mock.Verify(m => m.ExisteLivroComTituloExcetoIdAsync("Título não existente", 90), Times.Once);
        mock.Verify(m => m.AtualizarLivroAsync(It.IsAny<Livro>()), Times.Once);
    }
}