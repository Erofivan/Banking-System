namespace Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;

public readonly struct Speed
{
    // private const double Epsilon = 1e-10;
    public Speed(double value)
    {
        Value = value;
    }

    public double Value { get; }

    public static Speed operator +(Speed left, double right)
    {
        return new Speed(left.Value + right);
    }

    public static bool operator >(Speed left, SpeedLimit right)
    {
        return left.Value > right.Value;
    }

    public static bool operator <(Speed left, SpeedLimit right)
    {
        return left.Value < right.Value;
    }

    public static bool operator <(Speed left, double right)
    {
        return left.Value < right;
    }

    public static bool operator >(Speed left, double right)
    {
        return left.Value > right;
    }

    public static bool operator <=(Speed left, SpeedLimit right)
    {
        return left.Value <= right.Value;
    }

    public static bool operator >=(Speed left, SpeedLimit right)
    {
        return left.Value >= right.Value;
    }

    public static double operator *(Speed left, double right)
    {
        return left.Value * right;
    }
}