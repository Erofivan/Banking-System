namespace Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;

public readonly struct Mass
{
    private const double EpsilonValue = 1e-10;

    public Mass(double value)
    {
        if (value <= EpsilonValue)
        {
            throw new ArgumentException("Mass must be positive", nameof(value));
        }

        Value = value;
    }

    public double Value { get; }
}