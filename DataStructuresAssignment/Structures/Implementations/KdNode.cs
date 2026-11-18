using DataStructuresAssignment.Data.Interfaces;
using DataStructuresAssignment.Structures.Implementations.Abstractions;

namespace DataStructuresAssignment.Structures.Implementations;

public class KdNode<TData> : AbstractKdNode<KdNode<TData>, TData>
    where TData : class, IKdData<TData>
{

}
