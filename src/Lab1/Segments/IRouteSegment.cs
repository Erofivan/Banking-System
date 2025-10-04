using Itmo.ObjectOrientedProgramming.Lab1.Entities;

namespace Itmo.ObjectOrientedProgramming.Lab1.Segments;

public interface IRouteSegment
{
    TraversalContext Traverse(Train train);
}