namespace DataStructuresAssignment.Data.Interfaces;

public interface IQdBoundary<TBoundary> where TBoundary : IQdBoundary<TBoundary>
{
    int Compare(TBoundary boundary);
    TBoundary Middle(TBoundary boundary);
}
