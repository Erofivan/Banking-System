namespace Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;

public readonly struct Acceleration
{
    public Acceleration(double value)
    {
        Value = value;
    }

    public double Value { get; }

    public static Acceleration Create(Force force, Mass mass) => new Acceleration(force.Value / mass.Value);
}