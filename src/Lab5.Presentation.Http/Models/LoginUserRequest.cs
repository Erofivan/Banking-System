using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Lab5.Presentation.Http.Models;

public sealed class LoginUserRequest
{
    [Range(minimum: 1, maximum: long.MaxValue)]
    public long AccountId { get; set; }

    [NotNull]
    [Required]
    [RegularExpression(@"^\d{4}$", ErrorMessage = "Pin code must be exactly 4 digits")]
    public string? PinCode { get; set; }
}
