namespace Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;

public readonly struct Precision
{
    private const double Epsilon = 1e-10;

    public Precision(double value)
    {
        if (value <= Epsilon)
        {
            throw new ArgumentException("Precision must be positive", nameof(value));
        }

        Value = value;
    }

    public double Value { get; }
}