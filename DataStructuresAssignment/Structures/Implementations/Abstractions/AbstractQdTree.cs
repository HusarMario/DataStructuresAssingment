using DataStructuresAssignment.Data.Interfaces;
using DataStructuresAssignment.Structures.Interfaces;

namespace DataStructuresAssignment.Structures.Implementations.Abstractions;

public abstract class AbstractQdTree<TNode, TData, TBoundary>(TBoundary minX, TBoundary minY, TBoundary maxX, TBoundary maxY, int maxDepth) : AbstractBaseTree<TNode, TData>, IQdTree<TNode, TData, TBoundary>
    where TNode : AbstractQdNode<TNode, TData, TBoundary>
    where TData : class, IQdData<TBoundary>
    where TBoundary : IQdBoundary<TBoundary>
{
    public TBoundary MinX { get; } = minX;
    public TBoundary MinY { get; } = minY;
    public TBoundary MaxX { get; } = minX.Compare(maxX) <= 0 ? maxX : throw new ArgumentException("Invalid boundaries!");
    public TBoundary MaxY { get; } = minY.Compare(maxY) <= 0 ? maxY : throw new ArgumentException("Invalid boundaries!");
    public int MaxDepth { get; } = maxDepth >= 0 ? maxDepth : throw new ArgumentException("Negative maximum depth!");

    protected internal override IList<TData> FindInternal(TData data)
    {
        if (MinX.Compare(data.X) > 0 || MaxX.Compare(data.X) < 0 || MinY.Compare(data.Y) > 0 || MaxY.Compare(data.Y) < 0)
        {
            return [];
        }

        var current = Root;

        while (current is not null && current.HasChildren)
        {
            var centerX = current.MinX.Middle(current.MaxX);
            var centerY = current.MinY.Middle(current.MaxY);

            if (centerY.Compare(data.Y) < 0)
            {
                if (centerX.Compare(data.X) < 0)
                {
                    current = current.UpperRight;
                }
                else
                {
                    current = current.UpperLeft;
                }
            }
            else
            {
                if (centerX.Compare(data.X) < 0)
                {
                    current = current.LowerRight;
                }
                else
                {
                    current = current.LowerLeft;
                }
            }
        }

        if (current is null)
        {
            return [];
        }

        return current.Find(data);
    }

    protected internal override IList<TData> FindRangeInternal(TData? min, TData? max)
    {
        var results = new List<TData>();

        if (min is not null && max is not null)
        {
            if (min.X.Compare(max.X) > 0)
            {
                throw new ArgumentException("Invalid boundaries!");
            }

            if (min.Y.Compare(max.Y) > 0)
            {
                throw new ArgumentException("Invalid boundaries!");
            }
        }

        var stack = new Stack<TNode>();
        stack.Push(Root!);

        while (stack.Count > 0)
        {
            var current = stack.Pop();

            if ((max is not null && (current.MinX.Compare(max.X) > 0 || current.MinY.Compare(max.Y) > 0)) || (min is not null && (current.MaxX.Compare(min.X) < 0 || current.MaxY.Compare(min.Y) < 0)))
            {
                continue;
            }

            if (!current.HasChildren)
            {
                current.FindRange(min, max, results);
            }
            else
            {
                if (current.UpperLeft is not null)
                {
                    stack.Push(current.UpperLeft);
                }

                if (current.UpperRight is not null)
                {
                    stack.Push(current.UpperRight);
                }

                if (current.LowerLeft is not null)
                {
                    stack.Push(current.LowerLeft);
                }

                if (current.LowerRight is not null)
                {
                    stack.Push(current.LowerRight);
                }
            }
        }

        return results;
    }

    protected internal override IList<TData> MinimumInternal(TData? data, int dimension)
    {
        if (dimension >= 2)
        {
            throw new ArgumentException("Invalid dimension!");
        }

        var current = Root;

        if (data is not null)
        {
            if (MinX.Compare(data.X) > 0 || MaxX.Compare(data.X) < 0 || MinY.Compare(data.Y) > 0 || MaxY.Compare(data.Y) < 0)
            {
                return [];
            }

            while (current is not null && current.HasChildren)
            {
                var centerX = current.MinX.Middle(current.MaxX);
                var centerY = current.MinY.Middle(current.MaxY);

                if (centerY.Compare(data.Y) < 0)
                {
                    if (centerX.Compare(data.X) < 0)
                    {
                        current = current.UpperRight;
                    }
                    else
                    {
                        current = current.UpperLeft;
                    }
                }
                else
                {
                    if (centerX.Compare(data.X) < 0)
                    {
                        current = current.LowerRight;
                    }
                    else
                    {
                        current = current.LowerLeft;
                    }
                }
            }
        }

        if (current is null || (data is not null && current.Find(data).Count == 0))
        {
            return [];
        }

        var results = new List<TData>();
        TData? minimum = null;

        var stack = new Stack<TNode>();
        stack.Push(current);

        while (stack.Count > 0)
        {
            current = stack.Pop();

            if (minimum is not null && ((dimension == 0 && current.MinX.Compare(minimum.X) > 0) || (dimension == 1 && current.MinY.Compare(minimum.Y) > 0)))
            {
                continue;
            }

            foreach (var existing in current.Read())
            {
                var compare = minimum is null ? -1 : dimension == 0 ? existing.X.Compare(minimum.X) : existing.Y.Compare(minimum.Y);

                if (compare < 0)
                {
                    minimum = existing;
                    results.Clear();
                    results.Add(existing);
                }
                else if (compare == 0)
                {
                    results.Add(existing);
                }
            }

            if (current.UpperLeft is not null)
            {
                stack.Push(current.UpperLeft);
            }

            if (current.UpperRight is not null)
            {
                stack.Push(current.UpperRight);
            }

            if (current.LowerLeft is not null)
            {
                stack.Push(current.LowerLeft);
            }

            if (current.LowerRight is not null)
            {
                stack.Push(current.LowerRight);
            }
        }

        return results;
    }

    protected internal override IList<TData> MaximumInternal(TData? data, int dimension)
    {
        if (dimension >= 2)
        {
            throw new ArgumentException("Invalid dimension!");
        }

        var current = Root;

        if (data is not null)
        {
            if (MinX.Compare(data.X) > 0 || MaxX.Compare(data.X) < 0 || MinY.Compare(data.Y) > 0 || MaxY.Compare(data.Y) < 0)
            {
                return [];
            }

            while (current is not null && current.HasChildren)
            {
                var centerX = current.MinX.Middle(current.MaxX);
                var centerY = current.MinY.Middle(current.MaxY);

                if (centerY.Compare(data.Y) < 0)
                {
                    if (centerX.Compare(data.X) < 0)
                    {
                        current = current.UpperRight;
                    }
                    else
                    {
                        current = current.UpperLeft;
                    }
                }
                else
                {
                    if (centerX.Compare(data.X) < 0)
                    {
                        current = current.LowerRight;
                    }
                    else
                    {
                        current = current.LowerLeft;
                    }
                }
            }
        }

        if (current is null || (data is not null && current.Find(data).Count == 0))
        {
            return [];
        }

        var results = new List<TData>();
        TData? maximum = null;

        var stack = new Stack<TNode>();
        stack.Push(current);

        while (stack.Count > 0)
        {
            current = stack.Pop();

            if (maximum is not null && ((dimension == 0 && current.MaxX.Compare(maximum.X) < 0) || (dimension == 1 && current.MaxY.Compare(maximum.Y) < 0)))
            {
                continue;
            }

            foreach (var record in current.Read())
            {
                var compare = maximum is null ? 1 : dimension == 0 ? record.X.Compare(maximum.X) : record.Y.Compare(maximum.Y);

                if (compare > 0)
                {
                    maximum = record;
                    results.Clear();
                    results.Add(record);
                }
                else if (compare == 0)
                {
                    results.Add(record);
                }
            }

            if (current.UpperLeft is not null)
            {
                stack.Push(current.UpperLeft);
            }

            if (current.UpperRight is not null)
            {
                stack.Push(current.UpperRight);
            }

            if (current.LowerLeft is not null)
            {
                stack.Push(current.LowerLeft);
            }

            if (current.LowerRight is not null)
            {
                stack.Push(current.LowerRight);
            }
        }

        return results;
    }

    protected internal override bool InsertInternal(TData data)
    {
        if (MinX.Compare(data.X) > 0 || MaxX.Compare(data.X) < 0 || MinY.Compare(data.Y) > 0 || MaxY.Compare(data.Y) < 0)
        {
            throw new ArgumentException("Point out of tree boundary!");
        }

        if (Root is null)
        {
            Root = CreateNode(data);
        }

        var current = Root;

        for (int depth = 0; depth < MaxDepth; depth++)
        {
            var centerX = current.MinX.Middle(current.MaxX);
            var centerY = current.MinY.Middle(current.MaxY);

            if (!current.HasChildren)
            {
                var first = current.First;

                if (first is null)
                {
                    break;
                }

                if (first.X.Compare(data.X) == 0 && first.Y.Compare(data.Y) == 0)
                {
                    break;
                }

                if (centerX.Compare(current.MinX) <= 0 || centerX.Compare(current.MaxX) >= 0)
                {
                    break;
                }

                if (centerY.Compare(current.MinY) <= 0 || centerY.Compare(current.MaxY) >= 0)
                {
                    break;
                }

                ref TNode? previous = ref current.LowerLeftRef;

                if (centerY.Compare(first.Y) < 0)
                {
                    if (centerX.Compare(first.X) < 0)
                    {
                        previous = ref current.UpperRightRef;
                    }
                    else
                    {
                        previous = ref current.UpperLeftRef;
                    }
                }
                else
                {
                    if (centerX.Compare(first.X) < 0)
                    {
                        previous = ref current.LowerRightRef;
                    }
                    else
                    {
                        previous = ref current.LowerLeftRef;
                    }
                }

                previous = CreateNode(first);
                previous.Parent = current;
                previous.MinX = current.MinX;
                previous.MinY = current.MinY;
                previous.MaxX = current.MaxX;
                previous.MaxY = current.MaxY;

                if (centerX.Compare(first.X) < 0)
                {
                    previous.MinX = centerX;
                }
                else
                {
                    previous.MaxX = centerX;
                }

                if (centerY.Compare(first.Y) < 0)
                {
                    previous.MinY = centerY;
                }
                else
                {
                    previous.MaxY = centerY;
                }

                current.Copy(previous);
            }

            ref TNode? child = ref current.LowerLeftRef;

            if (centerY.Compare(data.Y) < 0)
            {
                if (centerX.Compare(data.X) < 0)
                {
                    child = ref current.UpperRightRef;
                }
                else
                {
                    child = ref current.UpperLeftRef;
                }
            }
            else
            {
                if (centerX.Compare(data.X) < 0)
                {
                    child = ref current.LowerRightRef;
                }
                else
                {
                    child = ref current.LowerLeftRef;
                }
            }

            if (child is null)
            {
                child = CreateNode(data);
                child.Parent = current;
                child.MinX = current.MinX;
                child.MinY = current.MinY;
                child.MaxX = current.MaxX;
                child.MaxY = current.MaxY;

                if (centerX.Compare(data.X) < 0)
                {
                    child.MinX = centerX;
                }
                else
                {
                    child.MaxX = centerX;
                }

                if (centerY.Compare(data.Y) < 0)
                {
                    child.MinY = centerY;
                }
                else
                {
                    child.MaxY = centerY;
                }
            }

            current = child;
        }

        return current.Insert(data);
    }

    protected internal override bool DeleteInternal(TData data)
    {
        if (MinX.Compare(data.X) > 0 || MaxX.Compare(data.X) < 0 || MinY.Compare(data.Y) > 0 || MaxY.Compare(data.Y) < 0)
        {
            return false;
        }

        var current = Root;

        while (current is not null && current.HasChildren)
        {
            var centerX = current.MinX.Middle(current.MaxX);
            var centerY = current.MinY.Middle(current.MaxY);

            if (centerY.Compare(data.Y) < 0)
            {
                if (centerX.Compare(data.X) < 0)
                {
                    current = current.UpperRight;
                }
                else
                {
                    current = current.UpperLeft;
                }
            }
            else
            {
                if (centerX.Compare(data.X) < 0)
                {
                    current = current.LowerRight;
                }
                else
                {
                    current = current.LowerLeft;
                }
            }
        }

        if (current is null)
        {
            return false;
        }

        return current.Delete(data);
    }
}