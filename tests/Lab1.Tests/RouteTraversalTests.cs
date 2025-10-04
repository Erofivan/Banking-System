using Itmo.ObjectOrientedProgramming.Lab1.Entities;
using Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;
using Itmo.ObjectOrientedProgramming.Lab1.Segments;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab1.Tests;

public class RouteTraversalTests
{
    private static readonly Mass TrainMass = new Mass(1000);
    private static readonly Force TrainMaxForce = new Force(50000);
    private static readonly Precision TrainPrecision = new Precision(0.1);
    private static readonly Distance PathDistance = new Distance(100);
    private static readonly TimeFactor StationLoadingFactor = new TimeFactor(10);
    private static readonly TimeFactor StationUnloadingFactor = new TimeFactor(10);

    [Fact]
    public void Scenario1_AccelerateToRouteSpeedLimitWithRegularPath_Success()
    {
        var routeSpeedLimit = new SpeedLimit(50);
        double targetSpeed = 48;
        double requiredForce = CalculateForceForSpeed(TrainMass.Value, targetSpeed, PathDistance.Value);

        var segments = new List<IRouteSegment>
        {
            new PoweredPath(PathDistance, requiredForce),
            new RegularPath(PathDistance),
        };

        var route = new Route(segments, routeSpeedLimit);
        var train = new Train(TrainMass, TrainMaxForce, TrainPrecision);

        TraversalResult result = route.Traverse(train);

        Assert.IsType<TraversalResult.Success>(result);
        var success = (TraversalResult.Success)result;
        Assert.True(success.Time.Value > 0);
    }

    [Fact]
    public void Scenario2_AccelerateAboveRouteSpeedLimit_Failure()
    {
        var routeSpeedLimit = new SpeedLimit(30);
        double targetSpeed = 50;
        double requiredForce = CalculateForceForSpeed(TrainMass.Value, targetSpeed, PathDistance.Value);

        var segments = new List<IRouteSegment>
        {
            new PoweredPath(PathDistance, requiredForce),
            new RegularPath(PathDistance),
        };

        var route = new Route(segments, routeSpeedLimit);
        var train = new Train(TrainMass, TrainMaxForce, TrainPrecision);

        TraversalResult result = route.Traverse(train);

        Assert.IsType<TraversalResult.SpeedLimitExceeded>(result);
    }

    [Fact]
    public void Scenario3_AccelerateToStationSpeedLimitWithStation_Success()
    {
        var routeSpeedLimit = new SpeedLimit(50);
        var stationSpeedLimit = new SpeedLimit(30);
        double targetSpeed = 28;
        double requiredForce = CalculateForceForSpeed(TrainMass.Value, targetSpeed, PathDistance.Value);

        var segments = new List<IRouteSegment>
        {
            new PoweredPath(PathDistance, requiredForce),
            new RegularPath(PathDistance),
            new Station(stationSpeedLimit, StationLoadingFactor, StationUnloadingFactor),
            new RegularPath(PathDistance),
        };

        var route = new Route(segments, routeSpeedLimit);
        var train = new Train(TrainMass, TrainMaxForce, TrainPrecision);

        TraversalResult result = route.Traverse(train);

        Assert.IsType<TraversalResult.Success>(result);
        var success = (TraversalResult.Success)result;
        Assert.True(success.Time.Value > 0);
    }

    [Fact]
    public void Scenario4_AccelerateAboveStationSpeedLimit_Failure()
    {
        var routeSpeedLimit = new SpeedLimit(50);
        var stationSpeedLimit = new SpeedLimit(30);
        double targetSpeed = 50;
        double requiredForce = CalculateForceForSpeed(TrainMass.Value, targetSpeed, PathDistance.Value);

        var segments = new List<IRouteSegment>
        {
            new PoweredPath(PathDistance, requiredForce),
            new Station(stationSpeedLimit, StationLoadingFactor, StationUnloadingFactor),
            new RegularPath(PathDistance),
        };

        var route = new Route(segments, routeSpeedLimit);
        var train = new Train(TrainMass, TrainMaxForce, TrainPrecision);

        TraversalResult result = route.Traverse(train);

        Assert.IsType<TraversalResult.SpeedLimitExceeded>(result);
    }

    [Fact]
    public void Scenario5_AccelerateAboveRouteLimitButBelowStationLimit_Failure()
    {
        var routeSpeedLimit = new SpeedLimit(30);
        var stationSpeedLimit = new SpeedLimit(50);
        double targetSpeed = 40;
        double requiredForce = CalculateForceForSpeed(TrainMass.Value, targetSpeed, PathDistance.Value);

        var segments = new List<IRouteSegment>
        {
            new PoweredPath(PathDistance, requiredForce),
            new RegularPath(PathDistance),
            new Station(stationSpeedLimit, StationLoadingFactor, StationUnloadingFactor),
            new RegularPath(PathDistance),
        };

        var route = new Route(segments, routeSpeedLimit);
        var train = new Train(TrainMass, TrainMaxForce, TrainPrecision);

        TraversalResult result = route.Traverse(train);

        Assert.IsType<TraversalResult.SpeedLimitExceeded>(result);
    }

    [Fact]
    public void Scenario6_AccelerateAndDecelerateToMeetLimits_Success()
    {
        var routeSpeedLimit = new SpeedLimit(25);
        var stationSpeedLimit = new SpeedLimit(40);

        double initialAccelForce = 6000;
        double decelerateToStationForce = -250;
        double accelerateFromStationForce = 100;
        double finalDecelerateForce = -30;

        var segments = new List<IRouteSegment>
        {
            new PoweredPath(PathDistance, initialAccelForce),
            new RegularPath(PathDistance),
            new PoweredPath(new Distance(PathDistance.Value * 12), decelerateToStationForce),
            new Station(stationSpeedLimit, StationLoadingFactor, StationUnloadingFactor),
            new RegularPath(PathDistance),
            new PoweredPath(PathDistance, accelerateFromStationForce),
            new RegularPath(PathDistance),
            new PoweredPath(new Distance(PathDistance.Value * 1.75), finalDecelerateForce),
        };

        var route = new Route(segments, routeSpeedLimit);
        var train = new Train(TrainMass, TrainMaxForce, TrainPrecision);

        TraversalResult result = route.Traverse(train);

        Assert.IsType<TraversalResult.Success>(result);
        var success = (TraversalResult.Success)result;
        Assert.True(success.Time.Value > 0);
    }

    [Fact]
    public void Scenario7_NoAccelerationOnRegularPath_Failure()
    {
        var routeSpeedLimit = new SpeedLimit(50);

        var segments = new List<IRouteSegment>
        {
            new RegularPath(PathDistance),
        };

        var route = new Route(segments, routeSpeedLimit);
        var train = new Train(TrainMass, TrainMaxForce, TrainPrecision);

        TraversalResult result = route.Traverse(train);

        Assert.IsType<TraversalResult.NegativeSpeed>(result);
    }

    [Fact]
    public void Scenario8_AccelerateThenDecelerateToNegativeSpeed_Failure()
    {
        var routeSpeedLimit = new SpeedLimit(50);
        double forceY = 5000;
        double distanceX = 50;

        var segments = new List<IRouteSegment>
        {
            new PoweredPath(new Distance(distanceX), forceY),
            new PoweredPath(new Distance(distanceX), -2 * forceY),
        };

        var route = new Route(segments, routeSpeedLimit);
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