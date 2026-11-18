using DataStructuresAssignment.Data.Interfaces;
using DataStructuresAssignment.Structures.Implementations.Abstractions;

namespace DataStructuresAssignment.Structures.Implementations;

public class QdTree<TData, TBoundary>(TBoundary minX, TBoundary minY, TBoundary maxX, TBoundary maxY, int maxDepth = 50) : AbstractQdTree<QdNode<TData, TBoundary>, TData, TBoundary>(minX, minY, maxX, maxY, maxDepth)
    where TData : class, IQdData<TBoundary>
    where TBoundary : IQdBoundary<TBoundary>
{
    internal override QdNode<TData, TBoundary> CreateNode(TData data) => new(MinX, MinY, MaxX, MaxY);
}
