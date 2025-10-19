using Itmo.ObjectOrientedProgramming.Lab1.Entities.ValueObjects;
using Itmo.ObjectOrientedProgramming.Lab1.Segments;

namespace Itmo.ObjectOrientedProgramming.Lab1.ResultTypes;

public abstract record SegmentTraversalResult
{
    private SegmentTraversalResult() { }

    public sealed record Success(Time Time, Speed FinalSpeed) : SegmentTraversalResult;

    public sealed record SpeedLimitExceeded(IRouteSegment Segment) : SegmentTraversalResult;

    public sealed record Failure(IRouteSegment Segment) : SegmentTraversalResult;
}