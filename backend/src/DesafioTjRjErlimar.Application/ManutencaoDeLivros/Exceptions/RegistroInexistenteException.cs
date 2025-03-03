namespace DesafioTjRjErlimar.Application.ManutencaoDeLivros.Exceptions;

/// <summary>
/// Exceção para quando se tenta manipular um registro inexistente
/// </summary>
public class RegistroInexistenteException : Exception
{
    public RegistroInexistenteException(string? message) : base(message)
    {
    }
}