namespace Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;

public readonly struct Distance
{
    private const double Epsilon = 1e-10;

    public Distance(double value)
    {
        if (value <= Epsilon)
        {
            throw new ArgumentException("Distance must be positive", nameof(value));
        }

        Value = value;
    }

    public double Value { get; }

    public static bool operator >(Distance left, double right) => left.Value > right;

    public static bool operator <(Distance left, double right) => left.Value < right;

    public static bool operator <=(Distance left, double right) => left.Value <= right;

    public static bool operator >=(Distance left, double right) => left.Value >= right;

    public static Distance operator -(Distance left, double right) => new Distance(left.Value - right);

    public static Distance operator -(Distance left, Distance right)
    {
        if (left.Value - right.Value <= 0)
        {
            throw new InvalidOperationException("Distance cannot be negative");
        }

        return new Distance(left.Value - right.Value);
    }

    public static bool operator <=(Distance left, Distance right) => left.Value <= right.Value;

    public static bool operator >=(Distance left, Distance right) => left.Value >= right.Value;
}