namespace Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;

public readonly struct Mass
{
    private const double Epsilon = 1e-10;

    public Mass(double value)
    {
        if (value <= Epsilon)
        {
            throw new ArgumentException("Mass must be positive", nameof(value));
        }

        Value = value;
    }

    public double Value { get; }
}