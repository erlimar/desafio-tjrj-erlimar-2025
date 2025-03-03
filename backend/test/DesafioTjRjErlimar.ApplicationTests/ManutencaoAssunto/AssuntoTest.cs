using DesafioTjRjErlimar.Application.ManutencaoAssunto;

namespace DesafioTjRjErlimar.ApplicationTests.ManutencaoAssunto;

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