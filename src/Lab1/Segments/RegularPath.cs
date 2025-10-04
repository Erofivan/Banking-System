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

    public TraversalResult Traverse(Train train)
    {
        TraversalResult result = train.CalculateTraversalTime(Distance);

        if (result is TraversalResult.Success success)
        {
            train.UpdateState(success.FinalSpeed, new Acceleration(0));
        }

        return result;
    }
}