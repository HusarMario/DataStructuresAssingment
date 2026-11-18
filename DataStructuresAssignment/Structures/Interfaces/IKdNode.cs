using DataStructuresAssignment.Data.Interfaces;

namespace DataStructuresAssignment.Structures.Interfaces;

public interface IKdNode<TNode, TData> : IBaseNode<TNode, TData>
    where TNode : IKdNode<TNode, TData>
    where TData : class, IKdData<TData>
{
    int Dimension { get; }
    TNode? Left { get; }
    TNode? Right { get; }
}
