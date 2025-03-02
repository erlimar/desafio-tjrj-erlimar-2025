using DesafioTjRjErlimar.Application.Model;

namespace DesafioTjRjErlimar.ApplicationTests;

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