namespace DataStructuresAssignment.Tester.Tools;

public class RandomNumberGenerator(int? seed = null)
{
    private readonly Random _random = seed.HasValue ? new Random(seed.Value) : new Random();
    public bool GetRandomBoolean() => _random.Next(0, 2) == 0;
    public int GetRandomInteger(int max) => _random.Next(0, max);
    public int GetRandomInteger(int min, int max) => _random.Next(min, max);
    public double GetRandomDouble(double min, double max) => _random.NextDouble() * (max - min) + min;
}
