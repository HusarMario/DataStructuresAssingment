using DataStructuresAssignment.Data.Interfaces;
using DataStructuresAssignment.Structures.Implementations;
using DataStructuresAssignment.Tester.Structures.Interfaces;

namespace DataStructuresAssignment.Tester.Structures.Implementations;

public class TestableKdTree<TData>(int dimensions) : ITestableTree<TData> where TData : class, IKdData<TData>
{
    internal readonly KdTree<TData> _tree = new(dimensions);
    public IList<TData> Minimum(TData? data = null, int dimension = 0) => _tree.Minimum(data, dimension);
    public IList<TData> Maximum(TData? data = null, int dimension = 0) => _tree.Maximum(data, dimension);
    public IList<TData> Find(TData data) => _tree.Find(data);
    public IList<TData> FindRange(TData? minData = null, TData? maxData = null) => _tree.FindRange(minData, maxData);
    public bool Insert(TData data) => _tree.Insert(data);
    public bool Delete(TData data) => _tree.Delete(data);
}
