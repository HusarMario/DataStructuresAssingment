using DataStructuresAssignment.Tester.Data;
using DataStructuresAssignment.Data.Interfaces;
using DataStructuresAssignment.Structures.Implementations;
using DataStructuresAssignment.Tester.Structures.Interfaces;

namespace DataStructuresAssignment.Tester.Structures.Implementations;

public class TestableQdTree<TData>(double minX, double minY, double maxX, double maxY, int maxDepth) : ITestableTree<TData> where TData : class, IQdData<DoubleBoundary>
{
    internal readonly QdTree<TData, DoubleBoundary> _tree = new(new(minX), new(minY), new(maxX), new(maxY), maxDepth);
    public IList<TData> Minimum(TData? data = null, int dimension = 0) => _tree.Minimum(data, dimension);
    public IList<TData> Maximum(TData? data = null, int dimension = 0) => _tree.Maximum(data, dimension);
    public IList<TData> Find(TData data) => _tree.Find(data);
    public IList<TData> FindRange(TData? minData = null, TData? maxData = null) => _tree.FindRange(minData, maxData);
    public bool Insert(TData data) => _tree.Insert(data);
    public bool Delete(TData data) => _tree.Delete(data);
}



