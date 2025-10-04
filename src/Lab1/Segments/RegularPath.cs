using Itmo.ObjectOrientedProgramming.Lab1.Entities;

namespace Itmo.ObjectOrientedProgramming.Lab1.Segments;

public sealed class RegularPath : IRouteSegment
{
    public RegularPath(double distance)
    {
        if (distance <= 0)
            throw new ArgumentException("Distance must be positive", nameof(distance));

        Distance = distance;
    }

    public double Distance { get; }

    public TraversalContext Traverse(Train train)
    {
        TraversalResult result = train.CalculateTraversalTime(Distance);

        if (!result.IsSuccess)
        {
            return TraversalContext.Create(result, train);
        }

        Train updatedTrain = train.UpdateSpeed(result.FinalSpeed);

        return TraversalContext.Create(result, updatedTrain);
    }
}