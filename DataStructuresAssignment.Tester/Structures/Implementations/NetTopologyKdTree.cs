using DataStructuresAssignment.Tester.Data;
using DataStructuresAssignment.Tester.Structures.Interfaces;
using NetTopologySuite.Geometries;
using NetTopologySuite.Index.KdTree;

namespace DataStructuresAssignment.Tester.Structures.Implementations;

internal class NetTopologyKdTree : ITestableTree<TestData>
{
    private readonly KdTree<List<TestData>> _tree = new(0);

    public bool Insert(TestData data)
    {
        var coordinate = new Coordinate(data.X, data.Y);
        var node = _tree.Query(coordinate);

        if (node is null)
        {
            _tree.Insert(coordinate, [data]);
        }
        else
        {
            node.Data.Add(data);
        }

        return true;
    }

    public IList<TestData> Find(TestData data)
    {
        var node = _tree.Query(new Coordinate(data.X, data.Y));

        if (node is null)
        {
            return [];
        }

        return [.. node.Data];
    }

    public IList<TestData> FindRange(TestData? minData = null, TestData? maxData = null)
    {
        var query = new Envelope(minData!.X, maxData!.X, minData.Y, maxData.Y);
        var results = new List<TestData>();

        foreach (var node in _tree.Query(query))
        {
            results.AddRange(node.Data);
        }

        return results;
    }

    public IList<TestData> Minimum(TestData? data = null, int dimension = 0) => throw new NotSupportedException("Minimum is not supported!");
    public IList<TestData> Maximum(TestData? data = null, int dimension = 0) => throw new NotSupportedException("Maximum is not supported!");
    public bool Delete(TestData data) => throw new NotSupportedException("Delete is not supported!");
}