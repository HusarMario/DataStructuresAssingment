using DataStructuresAssignment.Structures.Interfaces;

namespace DataStructuresAssignment.Structures.Implementations.Abstractions;

public abstract class AbstractBaseNode<TNode, TData> : IBaseNode<TNode, TData>
    where TNode : AbstractBaseNode<TNode, TData>
    where TData : class
{
    private TNode? _parent;
    public TNode? Parent { get => _parent; internal set => _parent = value; }

    internal List<(TData Data, bool Valid)> Data { get; } = [];
    public virtual IList<TData> Read() => [.. Data.Where(data => data.Valid).Select(data => data.Data)];
    internal virtual TData? First => Data.Count == 0 ? null : Data[0].Data;

    internal virtual bool Insert(TData insert)
    {
        foreach (var data in Data)
        {
            if (data.Valid && ReferenceEquals(data.Data, insert))
            {
                return false;
            }
        }

        Data.Add((insert, true));
        return true;
    }

    internal virtual bool Delete(TData delete)
    {
        for (int i = 0; i < Data.Count; i++)
        {
            if (Data[i].Valid && ReferenceEquals(Data[i].Data, delete))
            {
                Data[i] = (Data[i].Data, false);
                return true;
            }
        }

        return false;
    }
}
