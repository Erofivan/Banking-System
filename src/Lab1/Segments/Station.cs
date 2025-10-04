using Itmo.ObjectOrientedProgramming.Lab1.Entities;
using Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab1.Segments;

public sealed class Station : IRouteSegment
{
    public Station(Speed speedLimit, Time loadingFactor, Time unloadingFactor)
    {
        SpeedLimit = speedLimit;
        LoadingFactor = loadingFactor;
        UnloadingFactor = unloadingFactor;
    }

    public Speed SpeedLimit { get; }

    public Time LoadingFactor { get; }

    public Time UnloadingFactor { get; }

    public TraversalResult Traverse(Train train)
    {
        if (train.Speed > SpeedLimit)
        {
            return new TraversalResult.SpeedLimitExceeded();
        }

        Time stationTime = LoadingFactor + UnloadingFactor;

        return new TraversalResult.Success(stationTime, train.Speed);
    }
}