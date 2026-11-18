namespace DataStructuresAssignment.Tester.Structures.Interfaces;

public interface ITestableTree<TData> where TData : class
{
    IList<TData> Minimum(TData? data = null, int dimension = 0);
    IList<TData> Maximum(TData? data = null, int dimension = 0);
    IList<TData> Find(TData data);
    IList<TData> FindRange(TData? minData = null, TData? maxData = null);
    bool Insert(TData data);
    bool Delete(TData data);
}
