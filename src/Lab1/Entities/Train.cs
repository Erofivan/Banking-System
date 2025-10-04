using Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab1.Entities;

public sealed class Train
{
    private const double Epsilon = 1e-10;

    public Train(Mass mass, Force maxForce, Precision precision)
    {
        Mass = mass;
        MaxForce = maxForce;
        Precision = precision;
        Speed = new Speed(0);
        Acceleration = new Acceleration(0);
    }

    public Mass Mass { get; }

    public Force MaxForce { get; }

    public Precision Precision { get; }

    public Speed Speed { get; private set; }

    public Acceleration Acceleration { get; private set; }

    public void ApplyForce(double force)
    {
        if (Math.Abs(force) > MaxForce.Value)
            return;

        // Calculate new acceleration based on F = ma => a = F/m
        double newAcceleration = force / Mass.Value;

        Acceleration = new Acceleration(newAcceleration);
    }

    public TraversalResult CalculateTraversalTime(Distance distance)
    {
        double remainingDistance = distance.Value;
        double currentSpeed = Speed.Value;
        double totalTime = 0;

        while (remainingDistance > Epsilon)
        {
            // Calculate speed after this time step: v = v0 + a*t
            double resultingSpeed = currentSpeed + (Acceleration.Value * Precision.Value);

            if (resultingSpeed < Epsilon)
                return new TraversalResult.NegativeSpeed();

            // Calculate distance traveled in this time step: d = v*t
            double traveledDistance = resultingSpeed * Precision.Value;
            remainingDistance -= traveledDistance;
            currentSpeed = resultingSpeed;
            totalTime += Precision.Value;
        }

        Speed = new Speed(currentSpeed);
        Acceleration = new Acceleration(0);

        return new TraversalResult.Success(new Time(totalTime), new Speed(currentSpeed));
    }
}