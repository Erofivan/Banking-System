namespace Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;

public readonly struct Distance
{
    private const double EpsilonValue = 1e-10;

    public Distance(double value)
    {
        if (value < EpsilonValue)
        {
            throw new ArgumentException("Distance must be positive", nameof(value));
        }

        Value = value;
    }

    public double Value { get; }

    public static Distance operator -(Distance left, Distance right)
    {
        if (right > left)
        {
            throw new InvalidOperationException("Distance cannot be negative");
        }

        return new Distance(left.Value - right.Value);
    }

    public static bool operator <(Distance left, Distance right) => left.Value < right.Value;

    public static bool operator >(Distance left, Distance right) => left.Value > right.Value;

    public static bool operator <=(Distance left, Distance right) => left.Value <= right.Value;

    public static bool operator >=(Distance left, Distance right) => left.Value >= right.Value;

    public static Distance Create(Speed speed, Time time) => new Distance(speed.Value * time.Value);

    public static Distance Epsilon() => new Distance(EpsilonValue);
}