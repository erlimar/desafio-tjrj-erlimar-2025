namespace DesafioTjRjErlimar.Application;

/// <summary>
/// Exceção para quando se tenta cadastrar registro repetido de alguma forma
/// </summary>
public class RegistroRepetidoException : Exception
{
    public RegistroRepetidoException(string? message) : base(message)
    {
    }
}