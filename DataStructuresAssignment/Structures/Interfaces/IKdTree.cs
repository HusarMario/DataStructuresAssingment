using DataStructuresAssignment.Data.Interfaces;

namespace DataStructuresAssignment.Structures.Interfaces;

public interface IKdTree<TNode, TData> : IBaseTree<TNode, TData>
    where TNode : IKdNode<TNode, TData>
    where TData : class, IKdData<TData>
{
    int Dimensions { get; }
}
