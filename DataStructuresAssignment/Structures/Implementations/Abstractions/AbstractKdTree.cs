using DataStructuresAssignment.Data.Interfaces;
using DataStructuresAssignment.Structures.Interfaces;

namespace DataStructuresAssignment.Structures.Implementations.Abstractions;

public abstract class AbstractKdTree<TNode, TData>(int dimensions = 0) : AbstractBaseTree<TNode, TData>, IKdTree<TNode, TData>
    where TNode : AbstractKdNode<TNode, TData>
    where TData : class, IKdData<TData>
{
    public int Dimensions { get; } = dimensions > 0 ? dimensions : throw new ArgumentException("Negative or 0 dimension!");

    protected internal override IList<TData> FindInternal(TData data)
    {
        var current = Root;

        while (current is not null)
        {
            if (data.Compare(current.First!, current.Dimension) <= 0)
            {
                if (MatchData(data, current.First!))
                {
                    break;
                }

                current = current.Left;
            }
            else
            {
                current = current.Right;
            }
        }

        if (current is null)
        {
            return [];
        }

        return current.Read();
    }

    protected internal override IList<TData> FindRangeInternal(TData? min, TData? max)
    {
        var results = new List<TData>();

        var stack = new Stack<TNode>();
        stack.Push(Root!);

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            var first = current.First!;

            bool insideRange = true;

            for (int dimension = 0; dimension < Dimensions; dimension++)
            {
                if ((min is not null && first.Compare(min, dimension) < 0) || (max is not null && first.Compare(max, dimension) > 0))
                {
                    insideRange = false;
                    break;
                }
            }

            if (insideRange)
            {
                results.AddRange(current.Read());
            }

            if (current.Left is not null && (min is null || first.Compare(min, current.Dimension) >= 0))
            {
                stack.Push(current.Left);
            }

            if (current.Right is not null && (max is null || first.Compare(max, current.Dimension) < 0))
            {
                stack.Push(current.Right);
            }
        }

        return results;
    }

    protected internal override IList<TData> MinimumInternal(TData? data, int dimension)
    {
        if (dimension >= Dimensions)
        {
            throw new ArgumentException("Invalid dimension!");
        }

        var current = Root;

        if (data is not null)
        {
            while (current is not null)
            {
                if (data.Compare(current.First!, current.Dimension) <= 0)
                {
                    if (MatchData(data, current.First!))
                    {
                        break;
                    }

                    current = current.Left;
                }
                else
                {
                    current = current.Right;
                }
            }
        }

        if (current is null)
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

            var first = current.First!;
            var records = current.Read();

            if (records.Count > 0)
            {
                var compare = minimum is null ? -1 : first.Compare(minimum, dimension);

                if (compare < 0)
                {
                    minimum = first;
                    results.Clear();
                    results.AddRange(records);
                }
                else if (compare == 0)
                {
                    results.AddRange(records);
                }
            }

            if (current.Left is not null)
            {
                stack.Push(current.Left);
            }

            if (current.Right is not null && (current.Dimension != dimension || minimum is null || first.Compare(minimum, dimension) < 0))
            {
                stack.Push(current.Right);
            }
        }

        return results;
    }

    protected internal override IList<TData> MaximumInternal(TData? data, int dimension)
    {
        if (dimension >= Dimensions)
        {
            throw new ArgumentException("Invalid dimension!");
        }

        var current = Root;

        if (data is not null)
        {
            while (current is not null)
            {
                if (data.Compare(current.First!, current.Dimension) <= 0)
                {
                    if (MatchData(data, current.First!))
                    {
                        break;
                    }

                    current = current.Left;
                }
                else
                {
                    current = current.Right;
                }
            }
        }

        if (current is null)
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

            var first = current.First!;
            var records = current.Read();

            if (records.Count > 0)
            {
                var compare = maximum is null ? 1 : first.Compare(maximum, dimension);

                if (compare > 0)
                {
                    maximum = first;
                    results.Clear();
                    results.AddRange(records);
                }
                else if (compare == 0)
                {
                    results.AddRange(records);
                }
            }

            if (current.Left is not null && (current.Dimension != dimension || maximum is null || first.Compare(maximum, dimension) >= 0))
            {
                stack.Push(current.Left);
            }

            if (current.Right is not null)
            {
                stack.Push(current.Right);
            }
        }

        return results;
    }

    protected internal override bool InsertInternal(TData data)
    {
        ref TNode? current = ref RootRef;
        TNode? parent = current;
        int dimension = 0;

        while (current is not null)
        {
            parent = current;

            var compare = data.Compare(current.First!, current.Dimension);

            if (compare == 0)
            {
                if (MatchData(data, current.First!))
                {
                    return current.Insert(data);
                }

                current = ref current.LeftRef;
            }
            else if (compare < 0)
            {
                current = ref current.LeftRef;
            }
            else
            {
                current = ref current.RightRef;
            }

            dimension = (dimension + 1) % Dimensions;
        }

        current = CreateNode(data);
        current.Parent = parent;
        current.Dimension = dimension;

        return current.Insert(data);
    }

    protected internal override bool DeleteInternal(TData data)
    {
        var current = Root;

        while (current is not null)
        {
            if (data.Compare(current.First!, current.Dimension) <= 0)
            {
                if (MatchData(data, current.First!))
                {
                    break;
                }

                current = current.Left;
            }
            else
            {
                current = current.Right;
            }
        }

        if (current is null)
        {
            return false;
        }

        return current.Delete(data);
    }

    private bool MatchData(TData first, TData second)
    {
        for (int dimension = 0; dimension < Dimensions; dimension++)
        {
            if (first.Compare(second, dimension) != 0)
            {
                return false;
            }
        }

        return true;
    }
}