namespace Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;

public readonly struct Time
{
    public Time(double value)
    {
        Value = value;
    }

    public double Value { get; }

    public static Time operator +(Time left, Time right) => new(left.Value + right.Value);

    public static Time operator +(Time left, double right) => new(left.Value + right);

    public static bool operator >(Time left, double right) => left.Value > right;

    public static bool operator <(Time left, double right) => left.Value < right;
}