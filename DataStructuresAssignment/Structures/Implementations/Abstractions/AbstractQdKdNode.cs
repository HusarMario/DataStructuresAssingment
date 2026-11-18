using DataStructuresAssignment.Data.Interfaces;

namespace DataStructuresAssignment.Structures.Implementations.Abstractions;

public abstract class AbstractQdKdNode<TNode, TData, TBoundary>(TBoundary minX, TBoundary minY, TBoundary maxX, TBoundary maxY) : AbstractQdNode<TNode, TData, TBoundary>(minX, minY, maxX, maxY)
    where TNode : AbstractQdKdNode<TNode, TData, TBoundary>
    where TData : class, IQdKdData<TData, TBoundary>
    where TBoundary : IQdBoundary<TBoundary>
{
    internal new KdTree<TData> Data { get; set; } = new(2);

    public override IList<TData> Read() => Data.FindRange();
    internal override TData? First => Data.Root?.First;

    internal override IList<TData> Find(TData find)
    {
        return Data.Find(find);
    }

    internal override void FindRange(TData? min, TData? max, IList<TData> results)
    {
        foreach (var data in Data.FindRange(min, max))
        {
            results.Add(data);
        }
    }

    internal override bool Insert(TData insert)
    {
        return Data.Insert(insert);
    }

    internal override bool Delete(TData delete)
    {
        return Data.Delete(delete);
    }

    internal override void Copy(TNode node)
    {
        (node.Data, Data) = (Data, node.Data);
    }
}