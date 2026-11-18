using DataStructuresAssignment.Tester.Data;
using System.Diagnostics;

namespace DataStructuresAssignment.Tester;

public partial class TreeProfiler
{
    public bool DataTest(bool print = false, bool verify = true)
    {
        var stopwatch = new Stopwatch();
        bool isOk = true;

        InsertedValues.Clear();
        SortedValues.Clear();

        var path = Path.Combine(AppContext.BaseDirectory, "Input", print ? "slovakia_nodes-test.csv" : "slovakia_nodes.csv");
        foreach (var line in File.ReadLines(path).Skip(1))
        {
            var values = line.Split(';');
            var data = new TestData(long.Parse(values[0]), double.Parse(values[1]), double.Parse(values[2]));
            InsertedValues.Add(data);

            if (verify)
            {
                SortedValues.Add(data);
            }
        }
        Console.WriteLine($"Input načítaný správne: {InsertedValues.Count} bodov.");
        Console.WriteLine();

        RangeValues.Clear();
        path = Path.Combine(AppContext.BaseDirectory, "Input", print ? "testovacie_oblasti-test.csv" : "testovacie_oblasti.csv");
        foreach (var line in File.ReadLines(path).Skip(1))
        {
            var values = line.Split(';');
            var id = long.Parse(values[0]);
            var min = new TestData(long.MinValue, double.Parse(values[1]), double.Parse(values[2]));
            var max = new TestData(long.MaxValue, double.Parse(values[3]), double.Parse(values[4]));
            RangeValues.Add((id, min, max));
        }
        Console.WriteLine($"Input načítaný správne: {RangeValues.Count} oblastí.");
        Console.WriteLine();

        double minY = InsertedValues.Min(data => data.Y);
        double maxY = InsertedValues.Max(data => data.Y);
        double minX = InsertedValues.Min(data => data.X);
        double maxX = InsertedValues.Max(data => data.X);
        ResetTrees(minX, minY, maxX, maxY);

        foreach (var (Name, Tree) in TestableTrees)
        {
            int inserted = 0;
            int nextProgress = 10;

            stopwatch.Restart();
            foreach (var data in InsertedValues)
            {
                if (!Tree.Insert(data))
                {
                    Console.WriteLine($"Chyba pri vkladaní do stromu {Name}, bod {data.Id}.");
                    isOk = false;
                }

                inserted++;

                int progress = (int)((long)inserted * 100 / InsertedValues.Count);

                if (progress >= nextProgress)
                {
                    stopwatch.Stop();

                    while (nextProgress <= 100 && progress >= nextProgress)
                    {
                        Console.WriteLine($"{Name} – vkladanie: {nextProgress}%");
                        nextProgress += 10;
                    }

                    stopwatch.Start();
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"{Name} – vkladanie: {stopwatch.Elapsed.TotalMilliseconds:F2} ms");
            Console.WriteLine($"{Name} – (priemerny) vkladanie: {stopwatch.Elapsed.TotalMilliseconds / InsertedValues.Count:F4} ms");
            Console.WriteLine();

            int searched = 0;
            nextProgress = 10;

            stopwatch.Restart();
            foreach (var (_, Min, Max) in RangeValues)
            {
                Tree.FindRange(Min, Max);

                searched++;

                int progress = (int)((long)searched * 100 / RangeValues.Count);

                if (progress >= nextProgress)
                {
                    stopwatch.Stop();

                    while (nextProgress <= 100 && progress >= nextProgress)
                    {
                        Console.WriteLine($"{Name} – hladanie v rozsahu: {nextProgress}%");
                        nextProgress += 10;
                    }

                    stopwatch.Start();
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"{Name} – hladanie v rozsahu: {stopwatch.Elapsed.TotalMilliseconds:F2} ms");
            Console.WriteLine($"{Name} – (priemerny) hladanie v rozsahu: {stopwatch.Elapsed.TotalMilliseconds / RangeValues.Count:F4} ms");
            Console.WriteLine();

            if (!verify)
            {
                Console.WriteLine();
                continue;
            }

            foreach (var (id, Min, Max) in RangeValues)
            {
                var minimum = new TestData(long.MinValue, Min.X, minY);
                var maximum = new TestData(long.MaxValue, Max.X, maxY);
                var nodesExpected = SortedValues.GetViewBetween(minimum, maximum).Where(data => data.Y >= Min.Y && data.Y <= Max.Y).ToHashSet();
                var nodesFound = Tree.FindRange(Min, Max);

                if (print)
                {
                    var idsFound = string.Join(", ", nodesFound.Select(data => data.Id).Order());
                    Console.WriteLine($"{Name} – oblasť {id}, počet bodov {nodesFound.Count}: {(nodesFound.Count == 0 ? "žiadne body" : idsFound)}");
                }

                if (nodesFound.Count != nodesExpected.Count || !nodesExpected.SetEquals(nodesFound))
                {
                    Console.WriteLine($"Chyba v {Name}, oblasť {id}. " + $"Očakávaných bodov: {nodesExpected.Count}, nájdených: {nodesFound.Count}.");

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

            Console.WriteLine();
        }

        return isOk;
    }
}