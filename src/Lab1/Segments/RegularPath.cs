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

    public TrainTraversalResult Traverse(Train train)
    {
        TrainTraversalResult result = train.Traverse(Distance);

        return result;
    }
}