using DataStructuresAssignment.Data.Interfaces;

namespace DataStructuresAssignment.Tester.Data;

public class TestData(long id, double x, double y) : IQdKdData<TestData, DoubleBoundary>
{
    public long Id { get; } = id;
    internal double X { get; } = x;
    internal double Y { get; } = y;

    private readonly DoubleBoundary _boundaryX = new(x);
    private readonly DoubleBoundary _boundaryY = new(y);

    DoubleBoundary IQdData<DoubleBoundary>.X => _boundaryX;
    DoubleBoundary IQdData<DoubleBoundary>.Y => _boundaryY;

    public int Compare(TestData data, int dimension = 0) => dimension switch
    {
        0 => X.CompareTo(data.X),
        1 => Y.CompareTo(data.Y),
        _ => throw new ArgumentException("Invalid dimension!")
    };
}