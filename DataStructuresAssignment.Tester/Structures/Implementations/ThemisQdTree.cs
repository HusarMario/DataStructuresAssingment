using DataStructuresAssignment.Tester.Data;
using DataStructuresAssignment.Tester.Structures.Interfaces;
using Themis.Geometry.Boundary;
using Themis.Index.QuadTree;

namespace DataStructuresAssignment.Tester.Structures.Implementations;

internal class ThemisQdTree(double minX, double minY, double maxX, double maxY) : ITestableTree<TestData>
{
    private const double Margin = BoundingBox.SinglePointBuffer;
    private readonly QuadTree<TestData> _tree = new(minX - Margin, minY - Margin, maxX + Margin, maxY + Margin, 8);

    public bool Insert(TestData data)
    {
        _tree.Add(data, BoundingBox.FromPoint(data.X, data.Y, Margin));
        return true;
    }

    public IList<TestData> Find(TestData data)
    {
        return FindRange(data, data);
    }

    public IList<TestData> FindRange(TestData? minData = null, TestData? maxData = null)
    {
        var query = BoundingBox.From(minData!.X, minData.Y, maxData!.X, maxData.Y);
        return _tree.QueryDistinct(query).Where(point => point.X >= minData.X && point.X <= maxData.X && point.Y >= minData.Y && point.Y <= maxData.Y).ToList();
    }

    public bool Delete(TestData data)
    {
        int count = _tree.Count;
        _tree.Remove(data);
        return _tree.Count < count;
    }

    public IList<TestData> Minimum(TestData? data = null, int dimension = 0) => throw new NotSupportedException("Minimum is not supported!");
    public IList<TestData> Maximum(TestData? data = null, int dimension = 0) => throw new NotSupportedException("Maximum is not supported!");
}