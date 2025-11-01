using Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab1.ResultTypes;

public abstract record RouteTraversalResult
{
    private RouteTraversalResult() { }

    public sealed record Success(Time Time, Speed FinalSpeed) : RouteTraversalResult;

    public sealed record Failure() : RouteTraversalResult;
}