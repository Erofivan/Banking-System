namespace Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;

public readonly struct Time
{
    public Time(double value)
    {
        Value = value;
    }

    public double Value { get; }

    public static Time operator +(Time left, Time right)
    {
        return new Time(left.Value + right.Value);
    }

    public static Time operator +(Time left, double right)
    {
        return new Time(left.Value + right);
    }

    public static bool operator >(Time left, double right)
    {
        return left.Value > right;
    }

    public static bool operator <(Time left, double right)
    {
        return left.Value < right;
    }
}