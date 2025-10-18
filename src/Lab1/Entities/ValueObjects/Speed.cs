namespace Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;

public readonly struct Speed
{
    // private const double Epsilon = 1e-10;
    public Speed(double value)
    {
        Value = value;
    }

    public double Value { get; }

    public static Speed operator +(Speed left, double right) => new Speed(left.Value + right);

    public static Speed operator +(Speed left, Speed right) => new Speed(left.Value + right.Value);

    public static bool operator >(Speed left, Speed right) => left.Value > right.Value;

    public static bool operator <(Speed left, Speed right) => left.Value < right.Value;

    public static bool operator <(Speed left, double right) => left.Value < right;

    public static bool operator >(Speed left, double right) => left.Value > right;

    public static bool operator <=(Speed left, Speed right) => left.Value <= right.Value;

    public static bool operator >=(Speed left, Speed right) => left.Value >= right.Value;

    public static Speed Create(Acceleration left, double timeStep) => new Speed(left.Value * timeStep);
}