using DataStructuresAssignment.Data.Interfaces;
using DataStructuresAssignment.Structures.Implementations.Abstractions;

namespace DataStructuresAssignment.Structures.Implementations;

public class QdKdTree<TData, TBoundary>(TBoundary minX, TBoundary minY, TBoundary maxX, TBoundary maxY, int maxDepth = 50) : AbstractQdKdTree<QdKdNode<TData, TBoundary>, TData, TBoundary>(minX, minY, maxX, maxY, maxDepth)
    where TData : class, IQdKdData<TData, TBoundary>
    where TBoundary : IQdBoundary<TBoundary>
{
    internal override QdKdNode<TData, TBoundary> CreateNode(TData data) => new(MinX, MinY, MaxX, MaxY);
}
