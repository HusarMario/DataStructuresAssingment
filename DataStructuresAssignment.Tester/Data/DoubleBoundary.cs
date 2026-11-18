using DataStructuresAssignment.Data.Interfaces;

namespace DataStructuresAssignment.Tester.Data;

public class DoubleBoundary(double value) : IQdBoundary<DoubleBoundary>
{
    public double Value { get; } = value;

    public int Compare(DoubleBoundary other)
    {
        return Value.CompareTo(other.Value);
    }

    public DoubleBoundary Middle(DoubleBoundary other)
    {
        return new DoubleBoundary((Value + other.Value) / 2);
    }
}