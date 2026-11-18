using DataStructuresAssignment.Tester.Data;
using DataStructuresAssignment.Tester.Structures.Enums;
using DataStructuresAssignment.Tester.Structures.Implementations;
using DataStructuresAssignment.Tester.Structures.Interfaces;

namespace DataStructuresAssignment.Tester;

public partial class TreeProfiler
{
    public List<(string Name, ITestableTree<TestData> Tree)> TestableTrees { get; set; } = [];

    public List<TestData> InsertedValues { get; internal set; } = [];

    public SortedSet<TestData> SortedValues { get; internal set; } = new(Comparer<TestData>.Create((a, b) =>
    {
        int compare = 0;
        compare = a.X.CompareTo(b.X);
        if (compare != 0)
            return compare;
        compare = a.Y.CompareTo(b.Y);
        if (compare != 0)
            return compare;
        return a.Id.CompareTo(b.Id);
    }));

    public List<TestData> DeletedValues { get; internal set; } = [];
    public List<TestData> FindValues { get; internal set; } = [];
    public List<(long Id, TestData Min, TestData Max)> RangeValues { get; internal set; } = [];
    public List<TestData> MinimumValues { get; internal set; } = [];
    public List<TestData> MaximumValues { get; internal set; } = [];

    public List<(EOperation Operation, TestData? Data, TestData? Min, TestData? Max, int Dimension)> Operations { get; internal set; } = [];

    private void ResetTrees(double minX, double minY, double maxX, double maxY, int maxDepth = 50)
    {
        TestableTrees =
        [
            ("KD strom", new TestableKdTree<TestData>(2)),
            ("QD strom", new TestableQdTree<TestData>(minX, minY, maxX, maxY, maxDepth)),
            ("QD + KD strom", new TestableQdKdTree<TestData>(minX, minY, maxX, maxY, maxDepth)),

            ("NetTopology KD strom", new NetTopologyKdTree()),
            ("NetTopology QD strom", new NetTopologyQdTree()),

            ("Omni KD strom", new OmniKdTree()),
            ("Omni QD strom", new OmniQdTree(minX, minY, maxX + 1, maxY + 1, maxDepth)),

            ("Themis KD strom", new ThemisKdTree()),
            ("Themis QD strom", new ThemisQdTree(minX, minY, maxX, maxY))
        ];
    }
}
