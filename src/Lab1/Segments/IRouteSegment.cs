using Itmo.ObjectOrientedProgramming.Lab1.Entities;
using Itmo.ObjectOrientedProgramming.Lab1.ResultTypes;

namespace Itmo.ObjectOrientedProgramming.Lab1.Segments;

public interface IRouteSegment
{
    SegmentTraversalResult Traverse(Train train);
}