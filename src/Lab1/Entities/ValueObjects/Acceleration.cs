namespace Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;

public readonly struct Acceleration
{
    public Acceleration(double value)
    {
        Value = value;
    }

    public double Value { get; }

    public static double operator *(Acceleration left, double right)
    {
        return left.Value * right;
    }
}