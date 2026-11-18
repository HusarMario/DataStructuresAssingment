using DataStructuresAssignment.Tester.Data;
using DataStructuresAssignment.Tester.Tools;
using System.Diagnostics;

namespace DataStructuresAssignment.Tester;

public partial class TreeProfiler
{
    public bool StaticTest(int insert, int delete, int find, double range, int findRange, int findMinimum, int findMaximum, int? seed = null)
    {
        var generator = new RandomNumberGenerator(seed);
        var stopwatch = new Stopwatch();
        bool isOk = true;

        double minY = -1_000_000_000;
        double maxY = 1_000_000_000;
        double minX = -1_000_000_000;
        double maxX = 1_000_000_000;
        ResetTrees(minX, minY, maxX, maxY);

        InsertedValues.Clear();
        SortedValues.Clear();
        for (int i = 0; i < insert; i++)
        {
            var data = new TestData(i + 1, generator.GetRandomDouble(minX, maxX), generator.GetRandomDouble(minY, maxY));
            InsertedValues.Add(data);
            SortedValues.Add(data);
        }
        Console.WriteLine($"Vstupné hodnoty načítané správne: {InsertedValues.Count} bodov.");
        Console.WriteLine();

        foreach (var (Name, Tree) in TestableTrees)
        {
            stopwatch.Restart();
            foreach (var data in from data in InsertedValues where !Tree.Insert(data) select data)
            {
                Console.WriteLine($"Chyba pri vkladaní do stromu {Name}, bod {data.Id}.");
                isOk = false;
            }

            stopwatch.Stop();
            Console.WriteLine($"{Name} – vkladanie: {stopwatch.Elapsed.TotalMilliseconds:F2} ms");
            Console.WriteLine($"{Name} – (priemerny) vkladanie: {stopwatch.Elapsed.TotalMilliseconds / InsertedValues.Count:F4} ms");
            Console.WriteLine();

            var nodesFound = Tree.FindRange();
            if (nodesFound.Count != SortedValues.Count || !SortedValues.SetEquals(nodesFound))
            {
                Console.WriteLine($"{Name} – nesprávny obsah po vložení. Očakávaných: {SortedValues.Count}, nájdených: {nodesFound.Count}.");
                isOk = false;
            }
        }

        DeletedValues.Clear();
        for (int i = 0; i < delete; i++)
        {
            int index = generator.GetRandomInteger(InsertedValues.Count);
            var data = InsertedValues[index];
            DeletedValues.Add(data);
            InsertedValues.RemoveAt(index);
            SortedValues.Remove(data);
        }

        foreach (var (Name, Tree) in TestableTrees)
        {
            stopwatch.Restart();
            foreach (var data in from data in DeletedValues where !Tree.Delete(data) select data)
            {
                Console.WriteLine($"Chyba pri mazaní zo stromu {Name}, bod {data.Id}.");
                isOk = false;
            }

            stopwatch.Stop();
            Console.WriteLine($"{Name} – mazanie: {stopwatch.Elapsed.TotalMilliseconds:F2} ms");
            Console.WriteLine($"{Name} – (priemerny) mazanie: {stopwatch.Elapsed.TotalMilliseconds / DeletedValues.Count:F4} ms");
            Console.WriteLine();

            var nodesFound = Tree.FindRange();
            if (nodesFound.Count != SortedValues.Count || !SortedValues.SetEquals(nodesFound))
            {
                Console.WriteLine($"{Name} – nesprávny obsah po mazaní. Očakávaných: {SortedValues.Count}, nájdených: {nodesFound.Count}.");
                isOk = false;
            }
        }

        FindValues.Clear();
        for (int i = 0; i < find; i++)
        {
            FindValues.Add(InsertedValues[generator.GetRandomInteger(InsertedValues.Count)]);
        }

        foreach (var (Name, Tree) in TestableTrees)
        {
            stopwatch.Restart();
            foreach (var data in FindValues)
            {
                Tree.Find(data);
            }

            stopwatch.Stop();
            Console.WriteLine($"{Name} – hľadanie: {stopwatch.Elapsed.TotalMilliseconds:F2} ms");
            Console.WriteLine($"{Name} – (priemerny) hľadanie: {stopwatch.Elapsed.TotalMilliseconds / FindValues.Count:F4} ms");
            Console.WriteLine();

            foreach (var data in FindValues)
            {
                var nodesFound = Tree.Find(data);
                if (!nodesFound.Contains(data))
                {
                    Console.WriteLine($"Chyba pri hľadaní v strome {Name}, bod {data.Id}.");
                    isOk = false;
                }
            }
        }

        double rangeWidth = (maxX - minX) * range / 100;
        double rangeHeight = (maxY - minY) * range / 100;
        RangeValues.Clear();
        for (int i = 0; i < findRange; i++)
        {
            double x = generator.GetRandomDouble(minX, maxX - rangeWidth);
            double y = generator.GetRandomDouble(minY, maxY - rangeHeight);
            var min = new TestData(long.MinValue, x, y);
            var max = new TestData(long.MaxValue, x + rangeWidth, y + rangeHeight);
            RangeValues.Add((i + 1, min, max));
        }

        foreach (var (Name, Tree) in TestableTrees)
        {
            stopwatch.Restart();
            foreach (var (_, Min, Max) in RangeValues)
            {
                Tree.FindRange(Min, Max);
            }

            stopwatch.Stop();
            Console.WriteLine($"{Name} – hľadanie v rozsahu: {stopwatch.Elapsed.TotalMilliseconds:F2} ms");
            Console.WriteLine($"{Name} – (priemerny) hľadanie v rozsahu: {(RangeValues.Count == 0 ? 0 : stopwatch.Elapsed.TotalMilliseconds / RangeValues.Count):F4} ms");
            Console.WriteLine();

            foreach (var (Id, Min, Max) in RangeValues)
            {
                var minimum = new TestData(long.MinValue, Min.X, minY);
                var maximum = new TestData(long.MaxValue, Max.X, maxY);
                var nodesExpected = SortedValues.GetViewBetween(minimum, maximum).Where(data => data.Y >= Min.Y && data.Y <= Max.Y).ToHashSet();
                var nodesFound = Tree.FindRange(Min, Max);

                if (nodesFound.Count != nodesExpected.Count || !nodesExpected.SetEquals(nodesFound))
                {
                    Console.WriteLine($"{Name} – nesprávny obsah v oblasti {Id}. Očakávaných: {nodesExpected.Count}, nájdených: {nodesFound.Count}.");

                    var nodesMissing = new HashSet<TestData>(nodesExpected);
                    var nodesContain = new HashSet<TestData>();

                    foreach (var data in nodesFound)
                    {
                        bool added = nodesContain.Add(data);
                        if (!added)
                        {
                            Console.WriteLine($"Duplicitný bod: ID {data.Id}");
                            continue;
                        }

                        bool expected = nodesMissing.Remove(data);
                        if (!expected)
                        {
                            Console.WriteLine($"Bod navyše: ID {data.Id}");
                        }
                    }

                    foreach (var data in nodesMissing)
                    {
                        Console.WriteLine($"Chýbajúci bod: ID {data.Id}");
                    }

                    isOk = false;
                }
            }

        }

        MinimumValues.Clear();
        for (int i = 0; i < findMinimum; i++)
        {
            MinimumValues.Add(InsertedValues[generator.GetRandomInteger(InsertedValues.Count)]);
        }

        foreach (var (Name, Tree) in TestableTrees)
        {
            stopwatch.Restart();
            for (int i = 0; i < MinimumValues.Count; i++)
            {
                Tree.Minimum(MinimumValues[i], dimension: i % 2);
            }

            stopwatch.Stop();
            Console.WriteLine($"{Name} – minimum: {stopwatch.Elapsed.TotalMilliseconds:F2} ms");
            Console.WriteLine($"{Name} – (priemerny) minimum: {(MinimumValues.Count == 0 ? 0 : stopwatch.Elapsed.TotalMilliseconds / MinimumValues.Count):F4} ms");
            Console.WriteLine();

            stopwatch.Restart();
            Tree.Minimum();
            stopwatch.Stop();
            Console.WriteLine($"{Name} – minimum nad koreňom: {stopwatch.Elapsed.TotalMilliseconds:F4} ms");
            Console.WriteLine();

            for (int dimension = 0; dimension < 2; dimension++)
            {
                var nodesExpected = new HashSet<TestData>();
                if (SortedValues.Count > 0)
                {
                    double value = dimension == 0 ? SortedValues.Min(data => data.X) : SortedValues.Min(data => data.Y);
                    nodesExpected = SortedValues.Where(data => (dimension == 0 ? data.X : data.Y) == value).ToHashSet();
                }

                var nodesFound = Tree.Minimum(dimension: dimension);
                if (nodesFound.Count != nodesExpected.Count || !nodesExpected.SetEquals(nodesFound))
                {
                    Console.WriteLine($"{Name} – nesprávne minimum celého stromu v dimenzii {dimension}. Očakávaných: {nodesExpected.Count}, nájdených: {nodesFound.Count}.");

                    var nodesMissing = new HashSet<TestData>(nodesExpected);
                    var nodesContain = new HashSet<TestData>();

                    foreach (var data in nodesFound)
                    {
                        if (!nodesContain.Add(data))
                        {
                            Console.WriteLine($"Duplicitný bod: ID {data.Id}");
                            continue;
                        }

                        if (!nodesMissing.Remove(data))
                        {
                            Console.WriteLine($"Bod navyše: ID {data.Id}");
                        }
                    }

                    foreach (var data in nodesMissing)
                    {
                        Console.WriteLine($"Chýbajúci bod: ID {data.Id}");
                    }

                    isOk = false;
                }
            }
        }

        MaximumValues.Clear();
        for (int i = 0; i < findMaximum; i++)
        {
            MaximumValues.Add(InsertedValues[generator.GetRandomInteger(InsertedValues.Count)]);
        }

        foreach (var (Name, Tree) in TestableTrees)
        {
            stopwatch.Restart();
            for (int i = 0; i < MaximumValues.Count; i++)
            {
                Tree.Maximum(MaximumValues[i], dimension: i % 2);
            }

            stopwatch.Stop();
            Console.WriteLine($"{Name} – maximum: {stopwatch.Elapsed.TotalMilliseconds:F2} ms");
            Console.WriteLine($"{Name} – (priemerny) maximum: {(MaximumValues.Count == 0 ? 0 : stopwatch.Elapsed.TotalMilliseconds / MaximumValues.Count):F4} ms");
            Console.WriteLine();

            stopwatch.Restart();
            Tree.Maximum();
            stopwatch.Stop();
            Console.WriteLine($"{Name} – maximum nad koreňom: {stopwatch.Elapsed.TotalMilliseconds:F4} ms");
            Console.WriteLine();

            for (int dimension = 0; dimension < 2; dimension++)
            {
                var nodesExpected = new HashSet<TestData>();
                if (SortedValues.Count > 0)
                {
                    double value = dimension == 0 ? SortedValues.Max(data => data.X) : SortedValues.Max(data => data.Y);
                    nodesExpected = SortedValues.Where(data => (dimension == 0 ? data.X : data.Y) == value).ToHashSet();
                }

                var nodesFound = Tree.Maximum(dimension: dimension);
                if (nodesFound.Count != nodesExpected.Count || !nodesExpected.SetEquals(nodesFound))
                {
                    Console.WriteLine($"{Name} – nesprávne maximum celého stromu v dimenzii {dimension}. Očakávaných: {nodesExpected.Count}, nájdených: {nodesFound.Count}.");

                    var nodesMissing = new HashSet<TestData>(nodesExpected);
                    var nodesContain = new HashSet<TestData>();

                    foreach (var data in nodesFound)
                    {
                        if (!nodesContain.Add(data))
                        {
                            Console.WriteLine($"Duplicitný bod: ID {data.Id}");
                            continue;
                        }

                        if (!nodesMissing.Remove(data))
                        {
                            Console.WriteLine($"Bod navyše: ID {data.Id}");
                        }
                    }

                    foreach (var data in nodesMissing)
                    {
                        Console.WriteLine($"Chýbajúci bod: ID {data.Id}");
                    }

                    isOk = false;
                }
            }
        }

        return isOk;
    }
}