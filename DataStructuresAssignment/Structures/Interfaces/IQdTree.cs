using DataStructuresAssignment.Data.Interfaces;
namespace DataStructuresAssignment.Structures.Interfaces;

public interface IQdTree<TNode, TData, TBoundary> : IBaseTree<TNode, TData>
    where TNode : IQdNode<TNode, TData, TBoundary>
    where TBoundary : IQdBoundary<TBoundary>
    where TData : class
{
    int MaxDepth { get; }
    TBoundary MinX { get; }
    TBoundary MinY { get; }
    TBoundary MaxX { get; }
    TBoundary MaxY { get; }
}
