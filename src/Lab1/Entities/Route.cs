using Itmo.ObjectOrientedProgramming.Lab1.Segments;

namespace Itmo.ObjectOrientedProgramming.Lab1.Entities;

public sealed class Route
{
    private readonly IReadOnlyCollection<IRouteSegment> _segments;

    public Route(IReadOnlyCollection<IRouteSegment> segments, double endSpeedLimit)
    {
        ArgumentNullException.ThrowIfNull(segments);

        if (segments.Count == 0)
            throw new ArgumentException("Route must have at least one segment", nameof(segments));

        if (endSpeedLimit <= 0)
            throw new ArgumentException("End speed limit must be positive", nameof(endSpeedLimit));

        _segments = segments;
        EndSpeedLimit = endSpeedLimit;
    }

    public double EndSpeedLimit { get; }

    public TraversalResult Traverse(Train train)
    {
        ArgumentNullException.ThrowIfNull(train);

        Train currentTrain = train;
        double totalTime = 0;

        foreach (IRouteSegment segment in _segments)
        {
            TraversalContext context = segment.Traverse(currentTrain);

            if (!context.Result.IsSuccess)
                return TraversalResult.Failure();

            totalTime += context.Result.Time;
            currentTrain = context.Train;
        }

        if (currentTrain.Speed > EndSpeedLimit)
            return TraversalResult.Failure();

        return TraversalResult.Success(totalTime, currentTrain.Speed);
    }
}