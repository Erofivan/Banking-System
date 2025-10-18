namespace Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;

public readonly struct Acceleration
{
    public Acceleration(double value)
    {
        Value = value;
    }

    public double Value { get; }

    public static bool TryCreate(Force force, Mass mass)
    {
        if (mass.Value == 0)
        {
            return false;
        }

        return true;
    }

    public static Acceleration Create(Force force, Mass mass)
    {
        if (mass.Value == 0)
        {
            throw new ArgumentException("Mass cannot be zero");
        }

        return new Acceleration(force.Value / mass.Value);
    }
}