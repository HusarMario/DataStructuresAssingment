using DataStructuresAssignment.Data.Interfaces;
using DataStructuresAssignment.Structures.Implementations.Abstractions;

namespace DataStructuresAssignment.Structures.Implementations;

public class QdKdNode<TData, TBoundary>(TBoundary minX, TBoundary minY, TBoundary maxX, TBoundary maxY) : AbstractQdKdNode<QdKdNode<TData, TBoundary>, TData, TBoundary>(minX, minY, maxX, maxY)
    where TData : class, IQdKdData<TData, TBoundary>
    where TBoundary : IQdBoundary<TBoundary>
{

}
