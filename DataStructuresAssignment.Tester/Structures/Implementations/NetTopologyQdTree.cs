using DataStructuresAssignment.Tester.Data;
using DataStructuresAssignment.Tester.Structures.Interfaces;
using NetTopologySuite.Geometries;
using NetTopologySuite.Index.Quadtree;

namespace DataStructuresAssignment.Tester.Structures.Implementations;

internal class NetTopologyQdTree : ITestableTree<TestData>
{
    private const double Extent = 0.00001;
    private readonly Quadtree<TestData> _tree = new();

    public bool Insert(TestData data)
    {
        _tree.Insert(new Envelope(data.X, data.X + Extent, data.Y, data.Y + Extent), data);
        return true;
    }

    public IList<TestData> Find(TestData data)
    {
        return FindRange(data, data);
    }

    public IList<TestData> FindRange(TestData? minData = null, TestData? maxData = null)
    {
        var query = new Envelope(minData!.X, maxData!.X, minData.Y, maxData.Y);
        var results = new List<TestData>();

        foreach (var point in _tree.Query(query))
        {
            if (query.Contains(point.X, point.Y))
            {
                results.Add(point);
            }
        }

        return results;
    }

    public bool Delete(TestData data)
    {
        return _tree.Remove(new Envelope(data.X, data.X + Extent, data.Y, data.Y + Extent), data);
    }

    public IList<TestData> Minimum(TestData? data = null, int dimension = 0) => throw new NotSupportedException("Minimum is not supported!");
    public IList<TestData> Maximum(TestData? data = null, int dimension = 0) => throw new NotSupportedException("Maximum is not supported!");
}