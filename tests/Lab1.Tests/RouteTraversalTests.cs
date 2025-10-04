using Itmo.ObjectOrientedProgramming.Lab1.Entities;
using Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;
using Itmo.ObjectOrientedProgramming.Lab1.Segments;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab1.Tests;

public class RouteTraversalTests
{
    private const double TrainPrecision = 0.1;

    private static readonly Mass TrainMass = new Mass(1000);
    private static readonly Force TrainMaxForce = new Force(50000);
    private static readonly Distance PathDistance = new Distance(100);
    private static readonly Time StationLoadingFactor = new Time(10);
    private static readonly Time StationUnloadingFactor = new Time(10);

    [Fact]
    public void Scenario1_AccelerateToRouteSpeedWithRegularPath_Success()
    {
        var routeSpeed = new Speed(50);
        double targetSpeed = 48;
        double requiredForce = CalculateForceForSpeed(TrainMass.Value, targetSpeed, PathDistance.Value);

        var segments = new List<IRouteSegment>
        {
            new PoweredPath(PathDistance, new Force(requiredForce)),
            new RegularPath(PathDistance),
        };

        var route = new Route(segments, routeSpeed);
        var train = new Train(TrainMass, TrainMaxForce, TrainPrecision);

        TraversalResult result = route.Traverse(train);

        Assert.IsType<TraversalResult.Success>(result);
        var success = (TraversalResult.Success)result;
        Assert.True(success.Time.Value > 0);
    }

    [Fact]
    public void Scenario2_AccelerateAboveRouteSpeed_Failure()
    {
        var routeSpeed = new Speed(30);
        double targetSpeed = 50;
        double requiredForce = CalculateForceForSpeed(TrainMass.Value, targetSpeed, PathDistance.Value);

        var segments = new List<IRouteSegment>
        {
            new PoweredPath(PathDistance, new Force(requiredForce)),
            new RegularPath(PathDistance),
        };

        var route = new Route(segments, routeSpeed);
        var train = new Train(TrainMass, TrainMaxForce, TrainPrecision);

        TraversalResult result = route.Traverse(train);

        Assert.IsType<TraversalResult.SpeedLimitExceeded>(result);
    }

    [Fact]
    public void Scenario3_AccelerateToStationSpeedWithStation_Success()
    {
        var routeSpeed = new Speed(50);
        var stationSpeed = new Speed(30);
        double targetSpeed = 28;
        double requiredForce = CalculateForceForSpeed(TrainMass.Value, targetSpeed, PathDistance.Value);

        var segments = new List<IRouteSegment>
        {
            new PoweredPath(PathDistance, new Force(requiredForce)),
            new RegularPath(PathDistance),
            new Station(stationSpeed, StationLoadingFactor, StationUnloadingFactor),
            new RegularPath(PathDistance),
        };

        var route = new Route(segments, routeSpeed);
        var train = new Train(TrainMass, TrainMaxForce, TrainPrecision);

        TraversalResult result = route.Traverse(train);

        Assert.IsType<TraversalResult.Success>(result);
        var success = (TraversalResult.Success)result;
        Assert.True(success.Time.Value > 0);
    }

    [Fact]
    public void Scenario4_AccelerateAboveStationSpeed_Failure()
    {
        var routeSpeed = new Speed(50);
        var stationSpeed = new Speed(30);
        double targetSpeed = 50;
        double requiredForce = CalculateForceForSpeed(TrainMass.Value, targetSpeed, PathDistance.Value);

        var segments = new List<IRouteSegment>
        {
            new PoweredPath(PathDistance, new Force(requiredForce)),
            new Station(stationSpeed, StationLoadingFactor, StationUnloadingFactor),
            new RegularPath(PathDistance),
        };

        var route = new Route(segments, routeSpeed);
        var train = new Train(TrainMass, TrainMaxForce, TrainPrecision);

        TraversalResult result = route.Traverse(train);

        Assert.IsType<TraversalResult.SpeedLimitExceeded>(result);
    }

    [Fact]
    public void Scenario5_AccelerateAboveRouteLimitButBelowStationLimit_Failure()
    {
        var routeSpeed = new Speed(30);
        var stationSpeed = new Speed(50);
        double targetSpeed = 40;
        double requiredForce = CalculateForceForSpeed(TrainMass.Value, targetSpeed, PathDistance.Value);

        var segments = new List<IRouteSegment>
        {
            new PoweredPath(PathDistance, new Force(requiredForce)),
            new RegularPath(PathDistance),
            new Station(stationSpeed, StationLoadingFactor, StationUnloadingFactor),
            new RegularPath(PathDistance),
        };

        var route = new Route(segments, routeSpeed);
        var train = new Train(TrainMass, TrainMaxForce, TrainPrecision);

        TraversalResult result = route.Traverse(train);

        Assert.IsType<TraversalResult.SpeedLimitExceeded>(result);
    }

    [Fact]
    public void Scenario6_AccelerateAndDecelerateToMeetLimits_Success()
    {
        var routeSpeed = new Speed(25);
        var stationSpeed = new Speed(40);

        double initialAccelForce = 6000;
        double decelerateToStationForce = -250;
        double accelerateFromStationForce = 100;
        double finalDecelerateForce = -30;

        var segments = new List<IRouteSegment>
        {
            new PoweredPath(PathDistance, new Force(initialAccelForce)),
            new RegularPath(PathDistance),
            new PoweredPath(new Distance(PathDistance.Value * 12), new Force(decelerateToStationForce)),
            new Station(stationSpeed, StationLoadingFactor, StationUnloadingFactor),
            new RegularPath(PathDistance),
            new PoweredPath(PathDistance, new Force(accelerateFromStationForce)),
            new RegularPath(PathDistance),
            new PoweredPath(new Distance(PathDistance.Value * 1.75), new Force(finalDecelerateForce)),
        };

        var route = new Route(segments, routeSpeed);
        var train = new Train(TrainMass, TrainMaxForce, TrainPrecision);

        TraversalResult result = route.Traverse(train);

        Assert.IsType<TraversalResult.Success>(result);
        var success = (TraversalResult.Success)result;
        Assert.True(success.Time.Value > 0);
    }

    [Fact]
    public void Scenario7_NoAccelerationOnRegularPath_Failure()
    {
        var routeSpeed = new Speed(50);

        var segments = new List<IRouteSegment>
        {
            new RegularPath(PathDistance),
        };

        var route = new Route(segments, routeSpeed);
        var train = new Train(TrainMass, TrainMaxForce, TrainPrecision);

        TraversalResult result = route.Traverse(train);

        Assert.IsType<TraversalResult.NegativeSpeed>(result);
    }

    [Fact]
    public void Scenario8_AccelerateThenDecelerateToNegativeSpeed_Failure()
    {
        var routeSpeed = new Speed(50);
        double forceY = 5000;
        double distanceX = 50;

        var segments = new List<IRouteSegment>
        {
            new PoweredPath(new Distance(distanceX), new Force(forceY)),
            new PoweredPath(new Distance(distanceX), new Force(-2 * forceY)),
        };

        var route = new Route(segments, routeSpeed);
        var train = new Train(TrainMass, TrainMaxForce, TrainPrecision);

        TraversalResult result = route.Traverse(train);

        Assert.IsType<TraversalResult.NegativeSpeed>(result);
    }

    private static double CalculateForceForSpeed(double mass, double targetSpeed, double distance)
    {
        double acceleration = targetSpeed * targetSpeed / (2 * distance);
        return acceleration * mass;
    }
}