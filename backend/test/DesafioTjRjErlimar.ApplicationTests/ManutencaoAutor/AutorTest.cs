using DesafioTjRjErlimar.Application.ManutencaoAutor;

namespace DesafioTjRjErlimar.ApplicationTests.ManutencaoAutor;

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