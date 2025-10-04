using Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;
using Itmo.ObjectOrientedProgramming.Lab1.Segments;

namespace Itmo.ObjectOrientedProgramming.Lab1.Entities;

public sealed class Route
{
    private readonly IReadOnlyCollection<IRouteSegment> _segments;

    public Route(IReadOnlyCollection<IRouteSegment> segments, SpeedLimit endSpeedLimit)
    {
        _segments = segments;
        EndSpeedLimit = endSpeedLimit;
    }

    public SpeedLimit EndSpeedLimit { get; }

    public TraversalResult Traverse(Train train)
    {
        Train currentTrain = train;
        var totalTime = new Time(0);

        foreach (IRouteSegment segment in _segments)
        {
            TraversalContext context = segment.Traverse(currentTrain);

            if (context.Result is not TraversalResult.Success success)
                return context.Result;

            totalTime += success.Time;
            currentTrain = context.Train;
        }

        if (currentTrain.Speed > EndSpeedLimit)
            return new TraversalResult.SpeedLimitExceeded();

        return new TraversalResult.Success(totalTime, currentTrain.Speed);
    }
}