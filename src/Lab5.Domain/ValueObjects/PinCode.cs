using System.Text.RegularExpressions;

namespace Lab5.Domain.ValueObjects;

public readonly partial record struct PinCode
{
    private static readonly Regex Pattern = MyRegex();

    public PinCode(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        if (Pattern.IsMatch(value) is false)
        {
            throw new ArgumentException("Pin code must be exactly 4 digits", nameof(value));
        }

        Value = value;
    }

    public string Value { get; }

    [GeneratedRegex(@"^\d{4}$", RegexOptions.Compiled)]
    private static partial Regex MyRegex();
}
