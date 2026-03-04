namespace Lab5.Domain.ValueObjects;

public readonly record struct Amount : IComparable<Amount>
{
    public Amount(int value)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(value);
        Value = value;
    }

    public int Value { get; }

    public int CompareTo(Amount other)
    {
        return Value.CompareTo(other.Value);
    }

    public static Amount operator +(Amount left, Amount right) => new(left.Value + right.Value);

    public bool TrySubtract(Amount other, out Amount result)
    {
        if (Value < other.Value)
        {
            result = default;
            return false;
        }

        result = new Amount(Value - other.Value);
        return true;
    }

    public static bool operator <(Amount left, Amount right) => left.Value < right.Value;

    public static bool operator >(Amount left, Amount right) => left.Value > right.Value;

    public static bool operator <=(Amount left, Amount right) => left.Value <= right.Value;

    public static bool operator >=(Amount left, Amount right) => left.Value >= right.Value;
}