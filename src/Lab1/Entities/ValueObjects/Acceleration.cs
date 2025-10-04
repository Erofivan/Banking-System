namespace Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;

public readonly struct Acceleration
{
    public Acceleration(double value)
    {
        Value = value;
    }

    public double Value { get; }

    public static Speed operator *(Acceleration left, double timeStep) => new Speed(left.Value * timeStep);
}