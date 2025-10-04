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

    public void ApplyForce(Force force)
    {
        if (Force.Abs(force) > MaxForce)
            return;

        // Calculate new acceleration based on F = ma => a = F/m
        Acceleration = force / Mass;
    }

    public void UpdateState(Speed speed, Acceleration acceleration)
    {
        Speed = speed;
        Acceleration = acceleration;
    }

    public TraversalResult CalculateTraversalTime(Distance distance)
    {
        Speed currentSpeed = Speed;
        var totalTime = new Time(0.0);
        Distance remainingDistance = distance;

        while (remainingDistance > Epsilon)
        {
            // Calculate speed after this time step: v = v0 + a*t
            Speed resultingSpeed = currentSpeed + (Acceleration * Precision);

            if (resultingSpeed < Epsilon)
                return new TraversalResult.NegativeSpeed();

            // Calculate distance traveled in this time step: d = v*t
            var distanceTraveled = new Distance(resultingSpeed * Precision);

            if (remainingDistance <= distanceTraveled)
            {
                break;
            }

            remainingDistance -= distanceTraveled;
            currentSpeed = resultingSpeed;
            totalTime += Precision;
        }

        return new TraversalResult.Success(totalTime, currentSpeed);
    }
}