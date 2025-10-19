namespace Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;

public readonly struct Speed
{
    public Speed(double value)
    {
        Value = value;
    }

    public double Value { get; }

    public static Speed operator +(Speed left, Speed right) => new Speed(left.Value + right.Value);

    public static bool operator >(Speed left, Speed right) => left.Value > right.Value;

    public static bool operator <(Speed left, Speed right) => left.Value < right.Value;

    public static Speed Create(Acceleration left, Time timeStep) => new Speed(left.Value * timeStep.Value);

    public static Speed Epsilon() => new Speed(1e-10);
}