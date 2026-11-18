using DataStructuresAssignment.Tester;
using System.Globalization;

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

var profiler = new TreeProfiler();
bool isOk;

//isOk = profiler.DynamicTest(100_000, 0, 30, 40, 18, 5, 10, 1, 1);
//Console.WriteLine(isOk ? "Dynamický test bez počiatočných bodov skončil bez chyby." : "Dynamický test bez počiatočných bodov skončil s chybou.");
//Console.WriteLine();

//isOk = profiler.DynamicTest(100_000, 10_000, 30, 30, 28, 5, 10, 1, 1);
//Console.WriteLine(isOk ? "Dynamický test s počiatočnými bodmi skončil bez chyby." : "Dynamický test s počiatočnými bodmi skončil s chybou.");
//Console.WriteLine();

//isOk = profiler.StaticTest(1_000_000, 50_000, 100_000, 2.5, 1000, 50, 50);
//Console.WriteLine(isOk ? "Statický test skončil bez chyby." : "Statický test skončil s chybou.");
//Console.WriteLine();

isOk = profiler.DataTest(true, true);
Console.WriteLine(isOk ? "Test oblastí (s výpisom) skončil bez chyby." : "Test oblastí (s výpisom) skončil s chybou.");
Console.WriteLine();

//isOk = profiler.DataTest(false, false);
//Console.WriteLine(isOk ? "Test oblastí skončil bez chyby." : "Test oblastí skončil s chybou.");
//Console.WriteLine();