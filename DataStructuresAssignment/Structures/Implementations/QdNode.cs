using DataStructuresAssignment.Data.Interfaces;
using DataStructuresAssignment.Structures.Implementations.Abstractions;

namespace DataStructuresAssignment.Structures.Implementations;

public class QdNode<TData, TBoundary>(TBoundary minX, TBoundary minY, TBoundary maxX, TBoundary maxY) : AbstractQdNode<QdNode<TData, TBoundary>, TData, TBoundary>(minX, minY, maxX, maxY)
    where TData : class, IQdData<TBoundary>
    where TBoundary : IQdBoundary<TBoundary>
{

}
