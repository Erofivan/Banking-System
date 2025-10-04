using Itmo.ObjectOrientedProgramming.Lab1.Entities;
using Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab1.Segments;

public sealed class RegularPath : IRouteSegment
{
    public RegularPath(Distance distance)
    {
        Distance = distance;
    }

    public Distance Distance { get; }

    public TraversalContext Traverse(Train train)
    {
        TraversalResult result = train.CalculateTraversalTime(Distance);
        return TraversalContext.Create(result, train);
    }
}