using System.ComponentModel.DataAnnotations;

namespace DesafioTjRjErlimar.WebApi.Model;

/// <summary>
/// Dados para cadastro de autor
/// </summary>
public class DadosAutorViewModel : IValidatableObject
{
    /// <summary>
    /// Nome do autor, com no mínimo 3 e no máximo 40 caracteres
    /// </summary>
    [Required(ErrorMessage = "O nome do autor é obrigatório")]
    [MinLength(3, ErrorMessage = "O nome deve ter no mínimo 3 caracteres")]
    [MaxLength(40, ErrorMessage = "O nome só pode conter no máximo 40 caracteres")]
    public string? Nome { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var nome = Nome?.Trim();

        if (nome?.Length < 3)
        {
            yield return new ValidationResult("O nome deve ter no mínimo 3 caracteres (espaços não contam)", [nameof(Nome)]);
        }
    }
}