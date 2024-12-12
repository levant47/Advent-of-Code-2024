public static class Program
{
    public static void Main()
    {
        Console.WriteLine("Starting problem 11...");

        var input = INPUT;
        var stones = input.Split(' ').Select(long.Parse).ToList();
        Console.WriteLine($"Part 1 answer: {BlinkRecursively(stones, 25)}");
        Console.WriteLine($"Part 2 answer: {BlinkRecursively(stones, 75)}");

        Console.WriteLine("Done");
    }

    public record BlinkCacheKey(long Stone, int Iterations);
    public static Dictionary<BlinkCacheKey, long> BlinkCache = new();

    public static long BlinkRecursively(List<long> stones, int iterations) => stones.Sum(stone => BlinkRecursively(iterations, stone));

    public static long BlinkRecursively(int iterations, long stone)
    {
        var cacheKey = new BlinkCacheKey(stone, iterations);
        if (BlinkCache.TryGetValue(cacheKey, out var cachedResult)) { return cachedResult; }
        long result;
        if (iterations == 0) { result = 1; }
        else if (stone == 0) { result = BlinkRecursively(iterations - 1, 1); }
        else
        {
            var stringValue = stone.ToString();
            var digits = stringValue.Length;
            if (digits % 2 == 0)
            {
                result = BlinkRecursively(iterations - 1, long.Parse(stringValue[..(stringValue.Length / 2)]))
                    + BlinkRecursively(iterations - 1, long.Parse(stringValue[(stringValue.Length / 2)..]));
            }
            else { result = BlinkRecursively(iterations - 1, stone * 2024); }
        }
        BlinkCache[cacheKey] = result;
        return result;
    }

    public const string EXAMPLE_INPUT = "125 17";

    public const string INPUT = "4610211 4 0 59 3907 201586 929 33750";
}
