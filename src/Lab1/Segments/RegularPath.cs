using Itmo.ObjectOrientedProgramming.Lab1.Entities;
using Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;
using Itmo.ObjectOrientedProgramming.Lab1.ResultTypes;

namespace Itmo.ObjectOrientedProgramming.Lab1.Segments;

public sealed class RegularPath : IRouteSegment
{
    public RegularPath(Distance distance)
    {
        Distance = distance;
    }

    public Distance Distance { get; }

    public SegmentTraversalResult Traverse(Train train)
    {
        TrainTraversalResult result = train.Traverse(Distance);

        if (result is TrainTraversalResult.Success success)
        {
            train.TryApplyForce(new Force(0));

            return new SegmentTraversalResult.Success(success.Time, success.FinalSpeed);
        }

        return new SegmentTraversalResult.Failure(this);
    }
}