using DataStructuresAssignment.Tester.Data;
using DataStructuresAssignment.Tester.Structures.Interfaces;
using Omni.Collections.Spatial;

namespace DataStructuresAssignment.Tester.Structures.Implementations;

internal class OmniQdTree(double minX, double minY, double maxX, double maxY, int maxDepth) : ITestableTree<TestData>
{
    private readonly QuadTree<TestData> _tree = new(new Rectangle(minX, minY, maxX - minX, maxY - minY), maxDepth: maxDepth, spatialThreshold: 0);

    public bool Insert(TestData data)
    {
        _tree.Insert(new Point(data.X, data.Y), data);
        return true;
    }

    public IList<TestData> Find(TestData data)
    {
        return FindRange(data, data);
    }

    public IList<TestData> FindRange(TestData? minData = null, TestData? maxData = null)
    {
        var query = new Rectangle(minData!.X, minData.Y, maxData!.X - minData.X, maxData.Y - minData.Y);
        return _tree.Query(query);
    }

    public bool Delete(TestData data)
    {
        return _tree.Remove(new Point(data.X, data.Y), data);
    }

    public IList<TestData> Minimum(TestData? data = null, int dimension = 0) => throw new NotSupportedException("Minimum is not supported!");
    public IList<TestData> Maximum(TestData? data = null, int dimension = 0) => throw new NotSupportedException("Maximum is not supported!");
}