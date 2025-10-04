namespace Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;

public readonly struct Force
{
    private const double Epsilon = 1e-10;

    public Force(double value)
    {
        if (value <= Epsilon)
        {
            throw new ArgumentException("Force must be positive", nameof(value));
        }

        Value = value;
    }

    public double Value { get; }

    public static bool operator >(Force left, Force right)
    {
        return left.Value > right.Value;
    }

    public static bool operator <(Force left, Force right)
    {
        return left.Value < right.Value;
    }

    public static Force operator -(Force force)
    {
        return new Force(-force.Value);
    }

    public static double Abs(Force force)
    {
        return Math.Abs(force.Value);
    }
}