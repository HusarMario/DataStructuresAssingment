using DataStructuresAssignment.Data.Interfaces;

namespace DataStructuresAssignment.PointsOfInterestManager.Wrappers;

public class PointOfInterestBoundary(double value) : IQdBoundary<PointOfInterestBoundary>
{
    public double Value { get; } = value;

    public PointOfInterestBoundary Middle(PointOfInterestBoundary other)
    {
        return new((Value + other.Value) / 2);
    }

    public int Compare(PointOfInterestBoundary other)
    {
        return Value.CompareTo(other.Value);
    }
}