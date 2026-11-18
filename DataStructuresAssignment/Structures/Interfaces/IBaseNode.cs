namespace DataStructuresAssignment.Structures.Interfaces;

public interface IBaseNode<TNode, TData>
    where TNode : IBaseNode<TNode, TData>
    where TData : class
{
    TNode? Parent { get; }
    IList<TData> Read();
}
