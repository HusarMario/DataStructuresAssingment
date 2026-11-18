namespace DataStructuresAssignment.Data.Interfaces;

public interface IKdData<TData>
{
    int Compare(TData data, int dimension = 0);
}
