namespace DesafioTjRjErlimar.Application.ManutencaoDeLivros.Exceptions;

/// <summary>
/// Exceção para quando se tenta cadastrar autor com mesmo nome ou identificador
/// </summary>
public class AutorRepetidoException : Exception
{
    public AutorRepetidoException(string? message) : base(message)
    {
    }
}