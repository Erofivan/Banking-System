namespace Itmo.ObjectOrientedProgramming.Lab1.Entities;

public sealed class TraversalResult
{
    private TraversalResult(bool isSuccess, double time, double finalSpeed)
    {
        IsSuccess = isSuccess;
        Time = time;
        FinalSpeed = finalSpeed;
    }

    public bool IsSuccess { get; }

    public double Time { get; }

    public double FinalSpeed { get; }

    public static TraversalResult Success(double time, double finalSpeed)
    {
        return new TraversalResult(true, time, finalSpeed);
    }

    public static TraversalResult Failure()
    {
        return new TraversalResult(false, 0, 0);
    }
}