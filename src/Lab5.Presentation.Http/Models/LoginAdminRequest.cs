using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Lab5.Presentation.Http.Models;

public sealed class LoginAdminRequest
{
    [NotNull]
    [Required]
    public string? Password { get; set; }
}
