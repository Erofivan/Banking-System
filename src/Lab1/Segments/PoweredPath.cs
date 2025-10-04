using Itmo.ObjectOrientedProgramming.Lab1.Entities;

namespace Itmo.ObjectOrientedProgramming.Lab1.Segments;

public sealed class PoweredPath : IRouteSegment
{
    public PoweredPath(double distance, double force)
    {
        if (distance <= 0)
            throw new ArgumentException("Distance must be positive", nameof(distance));

        Distance = distance;
        Force = force;
    }

    public double Distance { get; }

    public double Force { get; }

    public TraversalContext Traverse(Train train)
    {
        Train trainWithForce = train.ApplyForce(Force);

        if (trainWithForce == train && Math.Abs(Force) > train.MaxForce)
        {
            return TraversalContext.Create(TraversalResult.Failure(), train);
        }

        TraversalResult result = trainWithForce.CalculateTraversalTime(Distance);

        if (!result.IsSuccess)
        {
            return TraversalContext.Create(result, train);
        }

        Train updatedTrain = trainWithForce.UpdateSpeed(result.FinalSpeed).UpdateAcceleration(0);

        return TraversalContext.Create(result, updatedTrain);
    }
}