using DataStructuresAssignment.Tester.Data;
using DataStructuresAssignment.Tester.Structures.Interfaces;
using Themis.Index.KdTree;

namespace DataStructuresAssignment.Tester.Structures.Implementations;

internal class ThemisKdTree : ITestableTree<TestData>
{
    private readonly KdTree<double, List<TestData>> _tree = new(2, new DoubleMath(), AddDuplicateBehavior.Skip);

    public bool Insert(TestData data)
    {
        double[] point = [data.X, data.Y];

        if (_tree.TryFindValueAt(point, out var values))
        {
            values.Add(data);
            return true;
        }

        return _tree.Add(point, [data]);
    }

    public IList<TestData> Find(TestData data)
    {
        return _tree.TryFindValueAt([data.X, data.Y], out var values) ? [.. values] : [];
    }

    public IList<TestData> FindRange(TestData? minData = null, TestData? maxData = null)
    {
        double x = (minData!.X + maxData!.X) / 2;
        double y = (minData.Y + maxData.Y) / 2;
        double dx = Math.Max(x - minData.X, maxData.X - x);
        double dy = Math.Max(y - minData.Y, maxData.Y - y);
        double radius = Math.BitIncrement(Math.Sqrt(dx * dx + dy * dy));

        var results = new List<TestData>();

        foreach (var values in _tree.RadialSearch([x, y], radius))
        {
            foreach (var point in values)
            {
                if (point.X >= minData.X && point.X <= maxData.X && point.Y >= minData.Y && point.Y <= maxData.Y)
                {
                    results.Add(point);
                }
            }
        }

        return results;
    }

    public bool Delete(TestData data) => throw new NotSupportedException("Delete is not supported by this adapter!");
    public IList<TestData> Minimum(TestData? data = null, int dimension = 0) => throw new NotSupportedException("Minimum is not supported!");
    public IList<TestData> Maximum(TestData? data = null, int dimension = 0) => throw new NotSupportedException("Maximum is not supported!");
}