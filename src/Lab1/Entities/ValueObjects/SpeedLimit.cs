namespace Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;

public readonly struct SpeedLimit
{
    public SpeedLimit(double value)
    {
        if (value <= 0)
        {
            throw new ArgumentException("Speed limit must be positive", nameof(value));
        }

        Value = value;
    }

    public double Value { get; }

    public static bool operator >(SpeedLimit left, SpeedLimit right)
    {
        return left.Value > right.Value;
    }

    public static bool operator <(SpeedLimit left, SpeedLimit right)
    {
        return left.Value < right.Value;
    }

    public static bool operator >=(SpeedLimit left, SpeedLimit right)
    {
        return left.Value >= right.Value;
    }

    public static bool operator <=(SpeedLimit left, SpeedLimit right)
    {
        return left.Value <= right.Value;
    }
}