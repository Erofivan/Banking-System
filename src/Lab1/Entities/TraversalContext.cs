namespace Itmo.ObjectOrientedProgramming.Lab1.Entities;

public sealed class TraversalContext
{
    private TraversalContext(TraversalResult result, Train train)
    {
        Result = result;
        Train = train;
    }

    public TraversalResult Result { get; }

    public Train Train { get; }

    public static TraversalContext Create(TraversalResult result, Train train)
    {
        return new TraversalContext(result, train);
    }
}