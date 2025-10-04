namespace Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;

public readonly struct Force
{
    public Force(double value)
    {
        Value = value;
    }

    public double Value { get; }

    public static bool operator >(Force left, Force right) => left.Value > right.Value;

    public static bool operator <(Force left, Force right) => left.Value < right.Value;

    public static Force operator -(Force force) => new Force(-force.Value);

    public static Force Abs(Force force)
    {
        return new Force(Math.Abs(force.Value));
    }

    public static Acceleration operator /(Force force, Mass mass) => new Acceleration(force.Value / mass.Value);
}