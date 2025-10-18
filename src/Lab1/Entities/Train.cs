using Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab1.Entities;

public sealed class Train
{
    private const double Epsilon = 1e-10;

    public Train(Mass mass, Force maxForce, double precision)
    {
        Mass = mass;
        MaxForce = maxForce;
        Precision = precision;
        Speed = new Speed(0);
        Acceleration = new Acceleration(0);
    }

    public Mass Mass { get; }

    public Force MaxForce { get; }

    public double Precision { get; }

    public Speed Speed { get; private set; }

    public Acceleration Acceleration { get; private set; }

    public void TryApplyForce(Force force)
    {
        if (Force.Abs(force) > MaxForce)
            return;

        // Calculate new acceleration based on F = ma => a = F/m
        if (Acceleration.TryCreate(force, Mass))
        {
            Acceleration = Acceleration.Create(force, Mass);
        }
    }

    public TrainTraversalResult Traverse(Distance distance)
    {
        Speed currentSpeed = Speed;
        var totalTime = new Time(0.0);
        Distance remainingDistance = distance;

        while (remainingDistance > Epsilon)
        {
            // Calculate speed after this time step: v = v0 + a*t
            Speed resultingSpeed = currentSpeed + Speed.Create(Acceleration, Precision);

            if (resultingSpeed < Epsilon)
                return new TrainTraversalResult.NegativeSpeed();

            // Calculate distance traveled in this time step: d = v*t
            var distanceTraveled = Distance.Create(resultingSpeed, Precision);

            if (remainingDistance <= distanceTraveled)
            {
                break;
            }

            remainingDistance -= distanceTraveled;
            currentSpeed = resultingSpeed;
            totalTime += Precision;
        }

        Speed = currentSpeed;

        return new TrainTraversalResult.Success(totalTime, currentSpeed);
    }
}