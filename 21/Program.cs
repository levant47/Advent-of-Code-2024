using System.Text;

public static class Program
{
    public static void Main()
    {
        var startTime = DateTime.Now;
        Console.WriteLine($"Starting problem 21 at {startTime:HH:mm:ss}...");

        var input = INPUT;
        var codes = input.Split("\n").Select(code => code.Trim()).Where(code => code != "").ToList();
        var part1 = 0;
        foreach (var code in codes)
        {
            var sequences = new List<Chain<string>> { new() { Value = code } };
            for (var i = 0; i < 3; i++)
            {
                sequences = sequences.SelectMany(sequence =>
                        CalculateKeyPresses2(i == 0 ? NUMERIC_KEYPAD : DIRECTIONAL_KEYPAD, i == 0 ? new(2, 3) : new(2, 0), sequence.Value)
                            .Select(nextSequence => new Chain<string> { Value = nextSequence, Prev = sequence }))
                    .ToList();
                var minimalLength = sequences.Min(sequence => sequence.Value.Length);
                sequences = sequences.Where(sequence => sequence.Value.Length == minimalLength).ToList();
            }
            var winningSequence = sequences.MinBy(sequence => sequence.Value.Length)!;
            part1 += int.Parse(code[..^1]) * winningSequence.Value.Length;
            Console.WriteLine($"{code}: {winningSequence.Value}");
        }
        Console.WriteLine($"Part 1 answer: {part1}");

        var endTime = DateTime.Now;
        Console.WriteLine($"Done in {endTime - startTime} ({endTime:HH:mm:ss})");
    }

    public record EncodingIteration(Dictionary<char, Vector2> Keypad, Vector2 InitialPosition);

    public record Vector2(int X, int Y)
    {
        public static Vector2 operator-(Vector2 left, Vector2 right) => new Vector2(left.X - right.X, left.Y - right.Y);
    }

    public record CalculateKeyPresses2Parameters(Dictionary<char, Vector2> keypad, Vector2 initialPosition, string targetSequence);
    public static Dictionary<CalculateKeyPresses2Parameters, List<string>> CalculateKeyPresses2Cache = [];
    public static List<string> CalculateKeyPresses2(Dictionary<char, Vector2> keypad, Vector2 initialPosition, string targetSequence)
    {
        var parameters = new CalculateKeyPresses2Parameters(keypad, initialPosition, targetSequence);
        if (CalculateKeyPresses2Cache.TryGetValue(parameters, out var result)) { return result; }
        if (targetSequence == "")
        {
            result = [];
        }
        else
        {
            result = [];
            var buttonPosition = keypad[targetSequence[0]];
            if (targetSequence.Length == 1)
            {
                foreach (var sequence in FindShortestPathBetweenButtons(keypad, initialPosition, buttonPosition))
                {
                    result.Add(sequence + "A");
                }
            }
            else
            {
                foreach (var restSequence in CalculateKeyPresses2(keypad, buttonPosition, targetSequence[1..]))
                {
                    foreach (var currentSequence in FindShortestPathBetweenButtons(keypad, initialPosition, buttonPosition))
                    {
                        result.Add(currentSequence + "A" + restSequence);
                    }
                }
            }
        }
        CalculateKeyPresses2Cache[parameters] = result;
        return result;
    }

    public class Chain<T>
    {
        public T Value;
        public Chain<T>? Prev;

        public IEnumerable<T> Iterate()
        {
            if (Prev != null)
            {
                foreach (var value in Prev.Iterate()) { yield return value; }
            }
            yield return Value;
        }
    }

    public record FindShortestPathBetweenButtonsParameters(Dictionary<char, Vector2> keypad, Vector2 origin, Vector2 destination);
    public static Dictionary<FindShortestPathBetweenButtonsParameters, List<string>> FindShortestPathBetweenButtonsCache = new();
    public static List<string> FindShortestPathBetweenButtons(Dictionary<char, Vector2> keypad, Vector2 origin, Vector2 destination)
    {
        var parameters = new FindShortestPathBetweenButtonsParameters(keypad, origin, destination);
        if (FindShortestPathBetweenButtonsCache.TryGetValue(parameters, out var result)) { return result; }
        if (origin == destination) { result = [""]; }
        else
        {
            result = [];
            var frontier = new List<Chain<Vector2>> { new() { Value = origin } };
            while (true)
            {
                var newFrontier = new List<Chain<Vector2>>();
                foreach (var path in frontier)
                {
                    var neighbors = new Vector2[]
                    {
                        new(path.Value.X - 1, path.Value.Y + 0),
                        new(path.Value.X + 1, path.Value.Y + 0),
                        new(path.Value.X + 0, path.Value.Y - 1),
                        new(path.Value.X + 0, path.Value.Y + 1),
                    };
                    foreach (var neighbor in neighbors)
                    {
                        if (keypad.Values.Contains(neighbor))
                        {
                            newFrontier.Add(new() { Value = neighbor, Prev = path });
                        }
                    }
                }
                if (newFrontier.Count == 0) { throw new(); }
                var reached = newFrontier.Where(path => path.Value == destination).ToList();
                if (reached.Count != 0)
                {
                    foreach (var path in reached)
                    {
                        var keyPresses = new StringBuilder();
                        var position = origin;
                        foreach (var nextPosition in path.Iterate().Skip(1 /* skip origin */))
                        {
                            if (position.X + 1 == nextPosition.X) { keyPresses.Append('>'); }
                            if (position.X - 1 == nextPosition.X) { keyPresses.Append('<'); }
                            if (position.Y + 1 == nextPosition.Y) { keyPresses.Append('v'); }
                            if (position.Y - 1 == nextPosition.Y) { keyPresses.Append('^'); }
                            position = nextPosition;
                        }
                        result.Add(keyPresses.ToString());
                    }
                    break;
                }
                frontier = newFrontier;
            }
        }
        FindShortestPathBetweenButtonsCache[parameters] = result;
        return result;
    }

    public static Dictionary<char, Vector2> NUMERIC_KEYPAD = new()
    {
        { '7', new(0, 0) },
        { '8', new(1, 0) },
        { '9', new(2, 0) },
        { '4', new(0, 1) },
        { '5', new(1, 1) },
        { '6', new(2, 1) },
        { '1', new(0, 2) },
        { '2', new(1, 2) },
        { '3', new(2, 2) },
        { '0', new(1, 3) },
        { 'A', new(2, 3) },
    };

    public static Dictionary<char, Vector2> DIRECTIONAL_KEYPAD = new()
    {
        { '^', new(1, 0) },
        { 'A', new(2, 0) },
        { '<', new(0, 1) },
        { 'v', new(1, 1) },
        { '>', new(2, 1) },
    };

    public const string EXAMPLE_INPUT = """
    029A
    980A
    179A
    456A
    379A
    """;

    public const string INPUT = """
    463A
    340A
    129A
    083A
    341A
    """;
}
