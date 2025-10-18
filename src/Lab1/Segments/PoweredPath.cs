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

    public TrainTraversalResult Traverse(Train train)
    {
        train.TryApplyForce(Force);

        TrainTraversalResult result = train.Traverse(Distance);

        train.TryApplyForce(new Force(0));

        return result;
    }
}