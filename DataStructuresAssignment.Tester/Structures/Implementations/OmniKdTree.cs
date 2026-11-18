using DataStructuresAssignment.Tester.Data;
using DataStructuresAssignment.Tester.Structures.Interfaces;
using Omni.Collections.Spatial.KDTree;

namespace DataStructuresAssignment.Tester.Structures.Implementations;

internal class OmniKdTree : ITestableTree<TestData>
{
    private readonly KdTree<TestData> _tree = KdTree<TestData>.Create2D(data => data.X, data => data.Y);

    public bool Insert(TestData data)
    {
        _tree.Insert(data);
        return true;
    }

    public IList<TestData> Find(TestData data)
    {
        return FindRange(data, data);
    }

    public IList<TestData> FindRange(TestData? minData = null, TestData? maxData = null)
    {
        return _tree.FindInRange([minData!.X, minData.Y], [maxData!.X, maxData.Y]);
    }

    public IList<TestData> Minimum(TestData? data = null, int dimension = 0) => throw new NotSupportedException("Minimum is not supported!");
    public IList<TestData> Maximum(TestData? data = null, int dimension = 0) => throw new NotSupportedException("Maximum is not supported!");
    public bool Delete(TestData data) => throw new NotSupportedException("Delete is not supported!");
}