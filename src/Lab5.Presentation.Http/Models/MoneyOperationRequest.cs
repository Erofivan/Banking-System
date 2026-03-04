using System.ComponentModel.DataAnnotations;

namespace Lab5.Presentation.Http.Models;

public sealed class MoneyOperationRequest
{
    [Range(minimum: 1, maximum: int.MaxValue)]
    public int Amount { get; set; }
}
