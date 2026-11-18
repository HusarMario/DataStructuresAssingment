using DataStructuresAssignment.Tester;
using System.Globalization;

namespace DataStructuresAssignment.Tests;

public class TreeProfilerTests
{
    public TreeProfilerTests()
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
    }

    [Fact]
    public void StaticTest()
    {
        var profiler = new TreeProfiler();

        Assert.True(profiler.StaticTest(1_000_000, 50_000, 100_000, 2.5, 1000, 50, 50));

        foreach (var (_, tree) in profiler.TestableTrees)
        {
            TreeTests.CheckTree(tree);
        }
    }

    [Fact]
    public void DynamicTestWithoutData()
    {
        var profiler = new TreeProfiler();

        Assert.True(profiler.DynamicTest(100_000, 0, 30, 40, 18, 5, 10, 1, 1));

        foreach (var (_, tree) in profiler.TestableTrees)
        {
            TreeTests.CheckTree(tree);
        }
    }

    [Fact]
    public void DynamicTestWithData()
    {
        var profiler = new TreeProfiler();

        Assert.True(profiler.DynamicTest(100_000, 10_000, 30, 30, 28, 5, 10, 1, 1));

        foreach (var (_, tree) in profiler.TestableTrees)
        {
            TreeTests.CheckTree(tree);
        }
    }

    [Fact]
    public void DataTest()
    {
        var profiler = new TreeProfiler();

        Assert.True(profiler.DataTest(print: true));

        foreach (var (_, tree) in profiler.TestableTrees)
        {
            TreeTests.CheckTree(tree);
        }
    }
}