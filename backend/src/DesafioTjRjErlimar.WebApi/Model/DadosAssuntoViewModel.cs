using System.ComponentModel.DataAnnotations;

namespace DesafioTjRjErlimar.WebApi.Model;

/// <summary>
/// Dados para cadastro de assunto
/// </summary>
public class DadosAssuntoViewModel : IValidatableObject
{
    /// <summary>
    /// Descrição do assunto, com no mínimo 3 e no máximo 20 caracteres
    /// </summary>
    [Required(ErrorMessage = "A descrição do assunto é obrigatória")]
    [MinLength(3, ErrorMessage = "A descrição deve ter no mínimo 3 caracteres")]
    [MaxLength(20, ErrorMessage = "A descrição só pode conter no máximo 20 caracteres")]
    public string? Descricao { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var descricao = Descricao?.Trim();

        if (descricao?.Length < 3)
        {
            yield return new ValidationResult("A descrição deve ter no mínimo 3 caracteres (espaços não contam)", [nameof(Descricao)]);
        }
    }
}