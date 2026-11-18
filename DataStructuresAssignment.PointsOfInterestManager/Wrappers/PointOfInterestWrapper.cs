using DataStructuresAssignment.Data.Interfaces;
using DataStructuresAssignment.PointsOfInterestManager.DBOs;

namespace DataStructuresAssignment.PointsOfInterestManager.Wrappers;

public class PointOfInterestWrapper(PointOfInterestDbo data) : IQdKdData<PointOfInterestWrapper, PointOfInterestBoundary>
{
    public PointOfInterestDbo Data { get; } = data;

    internal double X { get; } = data.Latitude;
    internal double Y { get; } = data.Longitude;

    private readonly PointOfInterestBoundary _boundaryX = new(data.Latitude);
    private readonly PointOfInterestBoundary _boundaryY = new(data.Longitude);

    PointOfInterestBoundary IQdData<PointOfInterestBoundary>.X => _boundaryX;
    PointOfInterestBoundary IQdData<PointOfInterestBoundary>.Y => _boundaryY;

    public int Compare(PointOfInterestWrapper other, int dimension = 0)
    {
        return dimension switch
        {
            0 => X.CompareTo(other.X),
            1 => Y.CompareTo(other.Y),
            _ => throw new ArgumentException("Invalid dimension!")
        };
    }
}