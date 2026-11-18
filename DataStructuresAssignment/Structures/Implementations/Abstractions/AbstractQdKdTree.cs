using DataStructuresAssignment.Data.Interfaces;

namespace DataStructuresAssignment.Structures.Implementations.Abstractions;

public abstract class AbstractQdKdTree<TNode, TData, TBoundary>(TBoundary minX, TBoundary minY, TBoundary maxX, TBoundary maxY, int maxDepth) : AbstractQdTree<TNode, TData, TBoundary>(minX, minY, maxX, maxY, maxDepth)
    where TNode : AbstractQdKdNode<TNode, TData, TBoundary>
    where TData : class, IQdKdData<TData, TBoundary>
    where TBoundary : IQdBoundary<TBoundary>
{
}