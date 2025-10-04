namespace Itmo.ObjectOrientedProgramming.Lab1.Entities;

public sealed class Train
{
    private const double Epsilon = 1e-10;

    public Train(double mass, double maxForce, double precision)
    {
        if (mass <= 0)
            throw new ArgumentException("Mass must be positive", nameof(mass));

        if (maxForce <= 0)
            throw new ArgumentException("Max force must be positive", nameof(maxForce));

        if (mass <= 0)
            throw new ArgumentException("Precision must be positive", nameof(precision));

        Mass = mass;
        MaxForce = maxForce;
        Precision = precision;
        Speed = 0;
        Acceleration = 0;
    }

    private Train(double mass, double maxForce, double precision, double speed, double acceleration)
    {
        Mass = mass;
        MaxForce = maxForce;
        Precision = precision;
        Speed = speed;
        Acceleration = acceleration;
    }

    public double Mass { get; }

    public double MaxForce { get; }

    public double Precision { get; }

    public double Speed { get; }

    public double Acceleration { get; }

    public Train ApplyForce(double force)
    {
        if (Math.Abs(force) > MaxForce)
            return this;

        // Calculate new acceleration based on F = ma => a = F/m
        double newAcceleration = force / Mass;

        return new Train(Mass, MaxForce, Precision, Speed, newAcceleration);
    }

    public TraversalResult CalculateTraversalTime(double distance)
    {
        if (distance <= 0)
            return TraversalResult.Failure();

        double remainingDistance = distance;
        double currentSpeed = Speed;
        double totalTime = 0;

        while (remainingDistance > Epsilon)
        {
            // Calculate speed after this time step: v = v0 + a*t
            double resultingSpeed = currentSpeed + (Acceleration * Precision);

            if (resultingSpeed < Epsilon)
                return TraversalResult.Failure();

            // Calculate distance traveled in this time step: d = v*t
            double traveledDistance = resultingSpeed * Precision;
            remainingDistance -= traveledDistance;
            currentSpeed = resultingSpeed;
            totalTime += Precision;
        }

        return TraversalResult.Success(totalTime, currentSpeed);
    }

    public Train UpdateSpeed(double newSpeed)
    {
        return new Train(Mass, MaxForce, Precision, newSpeed, Acceleration);
    }

    public Train UpdateAcceleration(double newAcceleration)
    {
        return new Train(Mass, MaxForce, Precision, Speed, newAcceleration);
    }
}