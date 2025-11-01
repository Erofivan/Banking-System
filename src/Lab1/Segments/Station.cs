using Itmo.ObjectOrientedProgramming.Lab1.Entities;
using Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;
using Itmo.ObjectOrientedProgramming.Lab1.ResultTypes;

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

    public SegmentTraversalResult Traverse(Train train)
    {
        if (train.Speed > SpeedLimit)
        {
            return new SegmentTraversalResult.SpeedLimitExceeded(this);
        }

        Time stationTime = LoadingFactor + UnloadingFactor;

        return new SegmentTraversalResult.Success(stationTime, train.Speed);
    }
}