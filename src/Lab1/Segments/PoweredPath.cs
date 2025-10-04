using Itmo.ObjectOrientedProgramming.Lab1.Entities;
using Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab1.Segments;

public sealed class PoweredPath : IRouteSegment
{
    public PoweredPath(Distance distance, double force)
    {
        Distance = distance;
        Force = force;
    }

    public Distance Distance { get; }

    public double Force { get; }

    public TraversalContext Traverse(Train train)
    {
        train.ApplyForce(Force);

        if (Math.Abs(Force) > train.MaxForce.Value)
        {
            return TraversalContext.Create(new TraversalResult.InvalidTraversal(), train);
        }

        TraversalResult result = train.CalculateTraversalTime(Distance);
        return TraversalContext.Create(result, train);
    }
}