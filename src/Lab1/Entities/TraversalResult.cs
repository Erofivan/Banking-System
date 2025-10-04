using Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab1.Entities;

public abstract record TraversalResult
{
    private TraversalResult()
    {
    }

    public sealed record Success(Time Time, Speed FinalSpeed) : TraversalResult;

    public sealed record SpeedLimitExceeded : TraversalResult;

    public sealed record InvalidTraversal : TraversalResult;

    public sealed record NegativeSpeed : TraversalResult;
}