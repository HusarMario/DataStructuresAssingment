using DataStructuresAssignment.Data.Interfaces;
using DataStructuresAssignment.Structures.Interfaces;

namespace DataStructuresAssignment.Structures.Implementations.Abstractions;

public abstract class AbstractQdNode<TNode, TData, TBoundary>(TBoundary minX, TBoundary minY, TBoundary maxX, TBoundary maxY) : AbstractBaseNode<TNode, TData>, IQdNode<TNode, TData, TBoundary>
    where TNode : AbstractQdNode<TNode, TData, TBoundary>
    where TData : class, IQdData<TBoundary>
    where TBoundary : IQdBoundary<TBoundary>
{
    public TBoundary MinX { get; internal set; } = minX;
    public TBoundary MinY { get; internal set; } = minY;
    public TBoundary MaxX { get; internal set; } = maxX;
    public TBoundary MaxY { get; internal set; } = maxY;

    private TNode? _lowerLeft;
    internal ref TNode? LowerLeftRef => ref _lowerLeft;
    public TNode? LowerLeft { get => _lowerLeft; internal set => _lowerLeft = value; }

    private TNode? _upperLeft;
    internal ref TNode? UpperLeftRef => ref _upperLeft;
    public TNode? UpperLeft { get => _upperLeft; internal set => _upperLeft = value; }

    private TNode? _lowerRight;
    internal ref TNode? LowerRightRef => ref _lowerRight;
    public TNode? LowerRight { get => _lowerRight; internal set => _lowerRight = value; }

    private TNode? _upperRight;
    internal ref TNode? UpperRightRef => ref _upperRight;
    public TNode? UpperRight { get => _upperRight; internal set => _upperRight = value; }

    public bool HasChildren => LowerLeft is not null || UpperLeft is not null || LowerRight is not null || UpperRight is not null;

    internal virtual IList<TData> Find(TData find)
    {
        var results = new List<TData>();

        foreach (var data in Data)
        {
            if (data.Valid && data.Data.X.Compare(find.X) == 0 && data.Data.Y.Compare(find.Y) == 0)
            {
                results.Add(data.Data);
            }
        }

        return results;
    }

    internal virtual void FindRange(TData? min, TData? max, IList<TData> results)
    {
        foreach (var data in Data)
        {
            if (data.Valid && (min is null || (data.Data.X.Compare(min.X) >= 0 && data.Data.Y.Compare(min.Y) >= 0)) && (max is null || (data.Data.X.Compare(max.X) <= 0 && data.Data.Y.Compare(max.Y) <= 0)))
            {
                results.Add(data.Data);
            }
        }
    }

    internal virtual void Copy(TNode node)
    {
        node.Data.AddRange(Data);
        Data.Clear();
    }
}