using DataStructuresAssignment.Structures.Interfaces;

namespace DataStructuresAssignment.Structures.Implementations.Abstractions;

public abstract class AbstractBaseTree<TNode, TData> : IBaseTree<TNode, TData>
    where TNode : AbstractBaseNode<TNode, TData>
    where TData : class
{
    private TNode? _root;
    internal ref TNode? RootRef => ref _root;
    public TNode? Root { get => _root; internal set => _root = value; }

    protected internal abstract IList<TData> FindInternal(TData data);
    protected internal abstract IList<TData> FindRangeInternal(TData? min, TData? max);

    protected internal abstract IList<TData> MinimumInternal(TData? data, int dimension);
    protected internal abstract IList<TData> MaximumInternal(TData? data, int dimension);

    protected internal abstract bool InsertInternal(TData data);
    protected internal abstract bool DeleteInternal(TData data);

    internal abstract TNode CreateNode(TData data);

    public IList<TData> Find(TData data)
    {
        if (data is null || Root is null)
            return [];

        return FindInternal(data);
    }

    public IList<TData> FindRange(TData? min = null, TData? max = null)
    {
        if (Root is null)
            return [];

        return FindRangeInternal(min, max);
    }

    public IList<TData> Minimum(TData? data = null, int dimension = 0)
    {
        if (dimension < 0)
        {
            throw new ArgumentException("Negative dimension!");
        }

        if (Root is null)
            return [];

        return MinimumInternal(data, dimension);
    }
    public IList<TData> Maximum(TData? data = null, int dimension = 0)
    {
        if (dimension < 0)
        {
            throw new ArgumentException("Negative dimension!");
        }

        if (Root is null)
            return [];

        return MaximumInternal(data, dimension);
    }

    public bool Insert(TData data)
    {
        if (data is null)
            return false;

        return InsertInternal(data);
    }

    public bool Delete(TData data)
    {
        if (data is null || Root is null)
            return false;

        return DeleteInternal(data);
    }
}
