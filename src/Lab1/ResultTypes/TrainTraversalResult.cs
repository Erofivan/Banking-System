using Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab1.ResultTypes;

public abstract record TrainTraversalResult
{
    private TrainTraversalResult() { }

    public sealed record Success(Time Time, Speed FinalSpeed) : TrainTraversalResult;

    public sealed record NegativeSpeed : TrainTraversalResult;
}