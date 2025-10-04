using Itmo.ObjectOrientedProgramming.Lab1.Entities;
using Itmo.ObjectOrientedProgramming.Lab1.Segments;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab1.Tests;

public class RouteTraversalTests
{
    private const double TrainMass = 1000;
    private const double TrainMaxForce = 50000;
    private const double TrainPrecision = 0.1;
    private const double PathDistance = 100;
    private const double StationLoadingFactor = 10;
    private const double StationUnloadingFactor = 10;

    [Fact]
    public void Scenario1_AccelerateToRouteSpeedLimitWithRegularPath_Success()
    {
        double routeSpeedLimit = 50;
        double targetSpeed = 48;
        double requiredForce = CalculateForceForSpeed(TrainMass, targetSpeed, PathDistance);

        var segments = new List<IRouteSegment>
        {
            new PoweredPath(PathDistance, requiredForce),
            new RegularPath(PathDistance),
        };

        var route = new Route(segments, routeSpeedLimit);
        var train = new Train(TrainMass, TrainMaxForce, TrainPrecision);

        TraversalResult result = route.Traverse(train);

        Assert.True(result.IsSuccess);
        Assert.True(result.Time > 0);
    }

    [Fact]
    public void Scenario2_AccelerateAboveRouteSpeedLimit_Failure()
    {
        double routeSpeedLimit = 30;
        double targetSpeed = 50;
        double requiredForce = CalculateForceForSpeed(TrainMass, targetSpeed, PathDistance);

        var segments = new List<IRouteSegment>
        {
            new PoweredPath(PathDistance, requiredForce),
            new RegularPath(PathDistance),
        };

        var route = new Route(segments, routeSpeedLimit);
        var train = new Train(TrainMass, TrainMaxForce, TrainPrecision);

        TraversalResult result = route.Traverse(train);

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void Scenario3_AccelerateToStationSpeedLimitWithStation_Success()
    {
        double routeSpeedLimit = 50;
        double stationSpeedLimit = 30;
        double targetSpeed = 28;
        double requiredForce = CalculateForceForSpeed(TrainMass, targetSpeed, PathDistance);

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

        Assert.True(result.IsSuccess);
        Assert.True(result.Time > 0);
    }

    [Fact]
    public void Scenario4_AccelerateAboveStationSpeedLimit_Failure()
    {
        double routeSpeedLimit = 50;
        double stationSpeedLimit = 30;
        double targetSpeed = 50;
        double requiredForce = CalculateForceForSpeed(TrainMass, targetSpeed, PathDistance);

        var segments = new List<IRouteSegment>
        {
            new PoweredPath(PathDistance, requiredForce),
            new Station(stationSpeedLimit, StationLoadingFactor, StationUnloadingFactor),
            new RegularPath(PathDistance),
        };

        var route = new Route(segments, routeSpeedLimit);
        var train = new Train(TrainMass, TrainMaxForce, TrainPrecision);

        TraversalResult result = route.Traverse(train);

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void Scenario5_AccelerateAboveRouteLimitButBelowStationLimit_Failure()
    {
        double routeSpeedLimit = 30;
        double stationSpeedLimit = 50;
        double targetSpeed = 40;
        double requiredForce = CalculateForceForSpeed(TrainMass, targetSpeed, PathDistance);

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

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void Scenario6_AccelerateAndDecelerateToMeetLimits_Success()
    {
        double routeSpeedLimit = 25;
        double stationSpeedLimit = 40;

        double initialAccelForce = 6000;
        double decelerateToStationForce = -250;
        double accelerateFromStationForce = 100;
        double finalDecelerateForce = -30;

        var segments = new List<IRouteSegment>
        {
            new PoweredPath(PathDistance, initialAccelForce),
            new RegularPath(PathDistance),
            new PoweredPath(PathDistance * 12, decelerateToStationForce),
            new Station(stationSpeedLimit, StationLoadingFactor, StationUnloadingFactor),
            new RegularPath(PathDistance),
            new PoweredPath(PathDistance, accelerateFromStationForce),
            new RegularPath(PathDistance),
            new PoweredPath(PathDistance * 1.75, finalDecelerateForce),
        };

        var route = new Route(segments, routeSpeedLimit);
        var train = new Train(TrainMass, TrainMaxForce, TrainPrecision);

        TraversalResult result = route.Traverse(train);

        Assert.True(result.IsSuccess);
        Assert.True(result.Time > 0);
    }

    [Fact]
    public void Scenario7_NoAccelerationOnRegularPath_Failure()
    {
        double routeSpeedLimit = 50;

        var segments = new List<IRouteSegment>
        {
            new RegularPath(PathDistance),
        };

        var route = new Route(segments, routeSpeedLimit);
        var train = new Train(TrainMass, TrainMaxForce, TrainPrecision);

        TraversalResult result = route.Traverse(train);

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void Scenario8_AccelerateThenDecelerateToNegativeSpeed_Failure()
    {
        double routeSpeedLimit = 50;
        double forceY = 5000;
        double distanceX = 50;

        var segments = new List<IRouteSegment>
        {
            new PoweredPath(distanceX, forceY),
            new PoweredPath(distanceX, -2 * forceY),
        };

        var route = new Route(segments, routeSpeedLimit);
        var train = new Train(TrainMass, TrainMaxForce, TrainPrecision);

        TraversalResult result = route.Traverse(train);

        Assert.False(result.IsSuccess);
    }

    private static double CalculateForceForSpeed(double mass, double targetSpeed, double distance)
    {
        double acceleration = targetSpeed * targetSpeed / (2 * distance);
        return acceleration * mass;
    }
}