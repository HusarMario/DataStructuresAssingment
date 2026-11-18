using DataStructuresAssignment.Data.Interfaces;
namespace DataStructuresAssignment.Structures.Interfaces;

public interface IQdNode<TNode, TData, TBoundary> : IBaseNode<TNode, TData>
    where TNode : IQdNode<TNode, TData, TBoundary>
    where TBoundary : IQdBoundary<TBoundary>
    where TData : class
{
    TBoundary MinX { get; }
    TBoundary MinY { get; }
    TBoundary MaxX { get; }
    TBoundary MaxY { get; }
    TNode? LowerLeft { get; }
    TNode? UpperLeft { get; }
    TNode? LowerRight { get; }
    TNode? UpperRight { get; }
}
