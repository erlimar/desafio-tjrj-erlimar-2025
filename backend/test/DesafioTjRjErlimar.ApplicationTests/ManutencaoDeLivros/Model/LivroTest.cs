using DesafioTjRjErlimar.Application.ManutencaoDeLivros.Model;

namespace DesafioTjRjErlimar.ApplicationTests.ManutencaoDeLivros.Model;

[Trait("target", nameof(Livro))]
public class LivroTest
{
    [Fact(DisplayName = "Livro deve ser instanciável")]
    public void LivroDeveSerInstanciavel()
    {
        var instance = new Livro
        {
            Titulo = "Teste",
            Editora = "Teste",
        };

        Assert.NotNull(instance);
    }
}