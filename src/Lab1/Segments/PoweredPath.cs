using Itmo.ObjectOrientedProgramming.Lab1.Entities;
using Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab1.Segments;

public sealed class PoweredPath : IRouteSegment
{
    public PoweredPath(Distance distance, Force force)
    {
        Distance = distance;
        Force = force;
    }

    public Distance Distance { get; }

    public Force Force { get; }

    public TraversalResult Traverse(Train train)
    {
        train.ApplyForce(Force);

        if (Force.Abs(Force) > train.MaxForce)
        {
            return new TraversalResult.InvalidTraversal();
        }

        TraversalResult result = train.CalculateTraversalTime(Distance);

        if (result is TraversalResult.Success success)
        {
            train.UpdateState(success.FinalSpeed, new Acceleration(0));
        }

        return result;
    }
}