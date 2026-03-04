using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Lab5.Presentation.Http.Models;

public sealed class CreateAccountRequest
{
    [NotNull]
    [Required]
    [RegularExpression(@"^\d{4}$", ErrorMessage = "Pin code must be exactly 4 digits")]
    public string? PinCode { get; set; }
}
