using Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;
using Itmo.ObjectOrientedProgramming.Lab1.ResultTypes;

namespace Itmo.ObjectOrientedProgramming.Lab1.Entities;

public sealed class Train
{
    public Train(Mass mass, Force maxForce, Time precision)
    {
        Mass = mass;
        MaxForce = maxForce;
        Precision = precision;
        Speed = new Speed(0);
        Acceleration = new Acceleration(0);
    }

    public Mass Mass { get; }

    public Force MaxForce { get; }

    public Time Precision { get; }

    public Speed Speed { get; private set; }

    public Acceleration Acceleration { get; private set; }

    public bool TryApplyForce(Force force)
    {
        if (Force.Abs(force) > MaxForce)
            return false;

        // Calculate new acceleration based on F = ma => a = F/m
        Acceleration = Acceleration.Create(force, Mass);

        return true;
    }

    public TrainTraversalResult Traverse(Distance distance)
    {
        var totalTime = new Time(0.0);
        Distance remainingDistance = distance;

        while (remainingDistance > Distance.Epsilon())
        {
            // Calculate speed after this time step: v = v0 + a*t
            Speed resultingSpeed = Speed + Speed.Create(Acceleration, Precision);

            if (resultingSpeed < Speed.Epsilon())
                return new TrainTraversalResult.NegativeSpeed();

            // Calculate distance traveled in this time step: d = v*t
            var distanceTraveled = Distance.Create(resultingSpeed, Precision);

            if (remainingDistance <= distanceTraveled)
            {
                break;
            }

            remainingDistance -= distanceTraveled;
            Speed = resultingSpeed;
            totalTime += Precision;
        }

        return new TrainTraversalResult.Success(totalTime, Speed);
    }
}