using Itmo.ObjectOrientedProgramming.Lab1.Entities;

namespace Itmo.ObjectOrientedProgramming.Lab1.Segments;

public sealed class Station : IRouteSegment
{
    public Station(double speedLimit, double loadingFactor, double unloadingFactor)
    {
        if (speedLimit <= 0)
            throw new ArgumentException("Speed limit must be positive", nameof(speedLimit));

        if (loadingFactor < 0)
            throw new ArgumentException("Loading factor cannot be negative", nameof(loadingFactor));

        if (unloadingFactor < 0)
            throw new ArgumentException("Unloading factor cannot be negative", nameof(unloadingFactor));

        SpeedLimit = speedLimit;
        LoadingFactor = loadingFactor;
        UnloadingFactor = unloadingFactor;
    }

    public double SpeedLimit { get; }

    public double LoadingFactor { get; }

    public double UnloadingFactor { get; }

    public TraversalContext Traverse(Train train)
    {
        if (train.Speed > SpeedLimit)
        {
            return TraversalContext.Create(TraversalResult.Failure(), train);
        }

        double stationTime = LoadingFactor + UnloadingFactor;
        double initialSpeed = train.Speed;

        Train updatedTrain = train.UpdateSpeed(initialSpeed).UpdateAcceleration(0);

        return TraversalContext.Create(TraversalResult.Success(stationTime, initialSpeed), updatedTrain);
    }
}