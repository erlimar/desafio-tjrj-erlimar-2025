using DesafioTjRjErlimar.Application.ManutencaoDeLivros.Model;

namespace DesafioTjRjErlimar.ApplicationTests.ManutencaoDeLivros.Model;

[Trait("target", nameof(Autor))]
public class AutorTest
{
    [Fact(DisplayName = "Autor deve ser instanciável")]
    public void AutorDeveSerInstanciavel()
    {
        var instance = new Autor
        {
            Nome = "Teste"
        };

        Assert.NotNull(instance);
    }
}