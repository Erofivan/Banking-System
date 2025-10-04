namespace Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;

public readonly struct TimeFactor
{
    public TimeFactor(double value)
    {
        if (value < 0)
        {
            throw new ArgumentException("Time factor cannot be negative", nameof(value));
        }

        Value = value;
    }

    public double Value { get; }

    public static TimeFactor operator +(TimeFactor left, TimeFactor right)
    {
        return new TimeFactor(left.Value + right.Value);
    }
}