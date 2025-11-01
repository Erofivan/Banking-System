using Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;
using Itmo.ObjectOrientedProgramming.Lab1.ResultTypes;
using Itmo.ObjectOrientedProgramming.Lab1.Segments;

namespace Itmo.ObjectOrientedProgramming.Lab1.Entities;

public sealed class Route
{
    private readonly IReadOnlyCollection<IRouteSegment> _segments;

    public Route(IReadOnlyCollection<IRouteSegment> segments, Speed endSpeedLimit)
    {
        _segments = segments;
        EndSpeedLimit = endSpeedLimit;
    }

    public Speed EndSpeedLimit { get; }

    public RouteTraversalResult Traverse(Train train)
    {
        var totalTime = new Time(0);

        foreach (IRouteSegment segment in _segments)
        {
            SegmentTraversalResult result = segment.Traverse(train);

            if (result is SegmentTraversalResult.Success success)
            {
                totalTime += success.Time;
            }
            else
            {
                return new RouteTraversalResult.Failure();
            }
        }

        if (train.Speed > EndSpeedLimit)
            return new RouteTraversalResult.Failure();

        return new RouteTraversalResult.Success(totalTime, train.Speed);
    }
}