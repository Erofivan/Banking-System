using Itmo.ObjectOrientedProgramming.Lab1.Entities;
using Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab1.Segments;

public sealed class Station : IRouteSegment
{
    public Station(SpeedLimit speedLimit, TimeFactor loadingFactor, TimeFactor unloadingFactor)
    {
        SpeedLimit = speedLimit;
        LoadingFactor = loadingFactor;
        UnloadingFactor = unloadingFactor;
    }

    public SpeedLimit SpeedLimit { get; }

    public TimeFactor LoadingFactor { get; }

    public TimeFactor UnloadingFactor { get; }

    public TraversalContext Traverse(Train train)
    {
        if (train.Speed > SpeedLimit)
        {
            return TraversalContext.Create(new TraversalResult.SpeedLimitExceeded(), train);
        }

        TimeFactor stationTime = LoadingFactor + UnloadingFactor;

        return TraversalContext.Create(
            new TraversalResult.Success(new Time(stationTime.Value), train.Speed),
            train);
    }
}