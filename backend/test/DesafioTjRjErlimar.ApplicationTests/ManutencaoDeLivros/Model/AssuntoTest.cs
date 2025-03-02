using DesafioTjRjErlimar.Application.ManutencaoDeLivros.Model;

namespace DesafioTjRjErlimar.ApplicationTests.ManutencaoDeLivros.Model;

[Trait("target", nameof(Assunto))]
public class AssuntoTest
{
    [Fact(DisplayName = "Assunto deve ser instanciável")]
    public void AssuntoDeveSerInstanciavel()
    {
        var instance = new Assunto
        {
            Descricao = "Teste"
        };

        Assert.NotNull(instance);
    }
}