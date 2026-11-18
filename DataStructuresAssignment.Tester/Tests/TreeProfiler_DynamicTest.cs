using DataStructuresAssignment.Tester.Data;
using DataStructuresAssignment.Tester.Structures.Enums;
using DataStructuresAssignment.Tester.Tools;
using System.Diagnostics;

namespace DataStructuresAssignment.Tester;

public partial class TreeProfiler
{
    public bool DynamicTest(int count, int init, int insert, int delete, int find, double range, int findRange, int findMinimum, int findMaximum, int? seed = null)
    {
        const int totalChance = 100;

        if (insert + delete + find + findRange + findMinimum + findMaximum != totalChance)
        {
            Console.WriteLine("Súčet šancí operácií musí byť 100.");
            return false;
        }

        var generator = new RandomNumberGenerator(seed);
        var stopwatch = new Stopwatch();
        bool isOk = true;

        double minY = -1_000_000_000;
        double maxY = 1_000_000_000;
        double minX = -1_000_000_000;
        double maxX = 1_000_000_000;
        double rangeWidth = (maxX - minX) * range / 100;
        double rangeHeight = (maxY - minY) * range / 100;
        ResetTrees(minX, minY, maxX, maxY);

        InsertedValues.Clear();
        SortedValues.Clear();
        DeletedValues.Clear();
        FindValues.Clear();
        RangeValues.Clear();
        MinimumValues.Clear();
        MaximumValues.Clear();

        int id = 0;
        for (int i = 0; i < init; i++)
        {
            var data = new TestData(++id, generator.GetRandomDouble(minX, maxX), generator.GetRandomDouble(minY, maxY));
            InsertedValues.Add(data);
            SortedValues.Add(data);

            foreach (var (Name, Tree) in TestableTrees)
            {
                if (!Tree.Insert(data))
                {
                    Console.WriteLine($"Chyba pri vkladaní do stromu {Name}, bod {data.Id}. Počiatočné vkladanie.");
                    isOk = false;
                }
            }
        }
        Console.WriteLine($"Vstupné hodnoty načítané správne: {InsertedValues.Count} bodov.");
        Console.WriteLine();

        Operations.Clear();
        for (int i = 0; i < count; i++)
        {
            int operation = generator.GetRandomInteger(100);

            if (operation < insert)
            {
                var data = new TestData(++id, generator.GetRandomDouble(minX, maxX), generator.GetRandomDouble(minY, maxY));

                InsertedValues.Add(data);
                SortedValues.Add(data);

                Operations.Add((EOperation.Insert, data, null, null, 0));
            }
            else if (operation < insert + delete)
            {
                TestData data;

                if (InsertedValues.Count > 0)
                {
                    int index = generator.GetRandomInteger(InsertedValues.Count);
                    data = InsertedValues[index];

                    InsertedValues.RemoveAt(index);
                    SortedValues.Remove(data);
                    DeletedValues.Add(data);
                }
                else
                {
                    data = new TestData(0, generator.GetRandomDouble(minX, maxX), generator.GetRandomDouble(minY, maxY));
                }

                Operations.Add((EOperation.Delete, data, null, null, 0));
            }
            else if (operation < insert + delete + find)
            {
                TestData data;

                if (InsertedValues.Count > 0)
                {
                    int index = generator.GetRandomInteger(InsertedValues.Count);
                    data = InsertedValues[index];
                }
                else
                {
                    data = new TestData(0, generator.GetRandomDouble(minX, maxX), generator.GetRandomDouble(minY, maxY));
                }

                Operations.Add((EOperation.Find, data, null, null, 0));
            }
            else if (operation < insert + delete + find + findRange)
            {
                double x = generator.GetRandomDouble(minX, maxX - rangeWidth);
                double y = generator.GetRandomDouble(minY, maxY - rangeHeight);

                var min = new TestData(0, x, y);
                var max = new TestData(0, x + rangeWidth, y + rangeHeight);

                Operations.Add((EOperation.FindRange, null, min, max, 0));
            }
            else if (operation < insert + delete + find + findRange + findMinimum)
            {
                int dimension = generator.GetRandomInteger(2);
                Operations.Add((EOperation.Minimum, null, null, null, dimension));
            }
            else
            {
                int dimension = generator.GetRandomInteger(2);
                Operations.Add((EOperation.Maximum, null, null, null, dimension));
            }
        }
        Console.WriteLine($"Operácie načítané správne: {Operations.Count} operácií.");
        Console.WriteLine();

        foreach (var (Name, Tree) in TestableTrees)
        {
            stopwatch.Restart();
            foreach (var operation in Operations)
            {
                switch (operation.Operation)
                {
                    case EOperation.Insert:
                        Tree.Insert(operation.Data!);
                        break;

                    case EOperation.Delete:
                        Tree.Delete(operation.Data!);
                        break;

                    case EOperation.Find:
                        Tree.Find(operation.Data!);
                        break;

                    case EOperation.FindRange:
                        Tree.FindRange(operation.Min, operation.Max);
                        break;

                    case EOperation.Minimum:
                        Tree.Minimum(dimension: operation.Dimension);
                        break;

                    case EOperation.Maximum:
                        Tree.Maximum(dimension: operation.Dimension);
                        break;
                }
            }

            stopwatch.Stop();

            Console.WriteLine($"{Name} – dynamické operácie: {stopwatch.Elapsed.TotalMilliseconds:F2} ms");
            Console.WriteLine($"{Name} – (priemerny) dynamické operácie: {(Operations.Count == 0 ? 0 : stopwatch.Elapsed.TotalMilliseconds / Operations.Count):F4} ms");
            Console.WriteLine();

            var nodesFound = Tree.FindRange();
            if (nodesFound.Count != SortedValues.Count || !SortedValues.SetEquals(nodesFound))
            {
                Console.WriteLine($"{Name} – nesprávny konečný obsah. Očakávaných: {SortedValues.Count}, nájdených: {nodesFound.Count}.");
                var nodesMissing = new HashSet<TestData>(SortedValues);
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

        return isOk;
    }
}
