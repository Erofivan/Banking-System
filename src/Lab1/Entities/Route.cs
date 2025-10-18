using Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;
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

    public TrainTraversalResult Traverse(Train train)
    {
        var totalTime = new Time(0);

        foreach (IRouteSegment segment in _segments)
        {
            TrainTraversalResult result = segment.Traverse(train);

            if (result is not TrainTraversalResult.Success success)
                return result;

            totalTime += success.Time;
        }

        if (train.Speed > EndSpeedLimit)
            return new TrainTraversalResult.SpeedLimitExceeded();

        return new TrainTraversalResult.Success(totalTime, train.Speed);
    }
}