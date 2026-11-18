namespace DataStructuresAssignment.Structures.Interfaces;

public interface IBaseTree<TNode, TData>
    where TNode : IBaseNode<TNode, TData>
    where TData : class
{
    TNode? Root { get; }

    IList<TData> Find(TData data);
    IList<TData> FindRange(TData? min = null, TData? max = null);

    IList<TData> Minimum(TData? data = null, int dimension = 0);
    IList<TData> Maximum(TData? data = null, int dimension = 0);

    bool Insert(TData data);
    bool Delete(TData data);
}
