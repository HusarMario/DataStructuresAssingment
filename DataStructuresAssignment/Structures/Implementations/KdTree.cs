using DataStructuresAssignment.Data.Interfaces;
using DataStructuresAssignment.Structures.Implementations.Abstractions;

namespace DataStructuresAssignment.Structures.Implementations;

public class KdTree<TData>(int dimensions = 2) : AbstractKdTree<KdNode<TData>, TData>(dimensions)
    where TData : class, IKdData<TData>
{
    internal override KdNode<TData> CreateNode(TData data) => new();
}