using Itmo.ObjectOrientedProgramming.Lab1.Entities;
using Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;
using Itmo.ObjectOrientedProgramming.Lab1.ResultTypes;

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

    public SegmentTraversalResult Traverse(Train train)
    {
        if (train.TryApplyForce(Force) is false)
        {
            return new SegmentTraversalResult.Failure(this);
        }

        TrainTraversalResult result = train.Traverse(Distance);

        if (result is TrainTraversalResult.Success success)
        {
            train.TryApplyForce(new Force(0));

            return new SegmentTraversalResult.Success(success.Time, success.FinalSpeed);
        }

        return new SegmentTraversalResult.Failure(this);
    }
}