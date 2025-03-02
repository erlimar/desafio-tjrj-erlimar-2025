using DesafioTjRjErlimar.Application.Model;

namespace DesafioTjRjErlimar.ApplicationTests;

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