using DataStructuresAssignment.Data.Interfaces;
using DataStructuresAssignment.Structures.Interfaces;

namespace DataStructuresAssignment.Structures.Implementations.Abstractions;

public abstract class AbstractKdNode<TNode, TData> : AbstractBaseNode<TNode, TData>, IKdNode<TNode, TData>
    where TNode : AbstractKdNode<TNode, TData>
    where TData : class, IKdData<TData>
{
    public int Dimension { get; internal set; }

    private TNode? _left;
    internal ref TNode? LeftRef => ref _left;
    public TNode? Left { get => _left; internal set => _left = value; }

    private TNode? _right;
    internal ref TNode? RightRef => ref _right;
    public TNode? Right { get => _right; internal set => _right = value; }
}
