﻿public static class Program
{
    public static void Main()
    {
        var startTime = DateTime.Now;
        Console.WriteLine($"Starting problem 20 at {startTime:HH:mm:ss}...");

        var input = INPUT;
        var parsedInput = ParseInput(input);

        var fromStart = GetDistanceMap(parsedInput.Start, parsedInput);
        var fromEnd = GetDistanceMap(parsedInput.End, parsedInput);

        var benchmark = fromStart[parsedInput.End];
        var cheats2 = EvaluateCheats(maxCheatDistance: 2, parsedInput, fromStart, fromEnd);
        Console.WriteLine($"Part 1 answer: {cheats2.Count(pair => benchmark - pair.Value >= 100)}"); // 1286

        var cheats20 = EvaluateCheats(maxCheatDistance: 20, parsedInput, fromStart, fromEnd);
        Console.WriteLine($"Part 2 answer: {cheats20.Count(pair => benchmark - pair.Value >= 100)}"); // 989316

        var endTime = DateTime.Now;
        Console.WriteLine($"Done in {endTime - startTime} ({endTime:HH:mm:ss})");
    }

    public record Cheat(Vector2 Start, Vector2 End);

    public record Vector2(int X, int Y);

    public static Dictionary<Vector2, int> GetDistanceMap(Vector2 target, ParsedInput parsedInput)
    {
        var distanceMap = new Dictionary<Vector2, int>();
        var frontier = new List<Vector2> { target };
        var distance = 0;
        while (true)
        {
            var newFrontier = new List<Vector2>();
            foreach (var frontierPosition in frontier)
            {
                distanceMap[frontierPosition] = distance;
                foreach (var surroundingPosition in Surroundings(frontierPosition))
                {
                    if (distanceMap.ContainsKey(surroundingPosition)
                        || parsedInput.Walls.Contains(surroundingPosition)
                        || surroundingPosition.X == 0
                        || surroundingPosition.Y == 0
                        || surroundingPosition.X == parsedInput.MapWidth - 1
                        || surroundingPosition.Y == parsedInput.MapHeight - 1
                    ) { continue; }
                    newFrontier.Add(surroundingPosition);
                }
            }
            if (newFrontier.Count == 0) { break; }
            distance++;
            frontier = newFrontier;
        }
        return distanceMap;
    }

    public static Dictionary<Cheat, int> EvaluateCheats(
        int maxCheatDistance,
        ParsedInput parsedInput,
        Dictionary<Vector2, int> fromStart,
        Dictionary<Vector2, int> fromEnd
    )
    {
        var cheats = new Dictionary<Cheat, int>();
        for (var x = 1; x < parsedInput.MapWidth - 1; x++)
        {
            for (var y = 1; y < parsedInput.MapHeight - 1; y++)
            {
                var cheatStartPosition = new Vector2(x, y);
                if (parsedInput.Walls.Contains(cheatStartPosition)) { continue; }

                for (var stepX = -maxCheatDistance; stepX <= maxCheatDistance; stepX++)
                {
                    for (var stepY = -maxCheatDistance; stepY <= maxCheatDistance; stepY++)
                    {
                        if (Math.Abs(stepX) + Math.Abs(stepY) > maxCheatDistance) { continue; }
                        var cheatEndPosition = new Vector2(x + stepX, y + stepY);
                        if (parsedInput.Walls.Contains(cheatEndPosition)
                            || cheatEndPosition.X <= 0
                            || cheatEndPosition.Y <= 0
                            || cheatEndPosition.X >= parsedInput.MapWidth - 1
                            || cheatEndPosition.Y >= parsedInput.MapHeight -1
                        ) { continue; }
                        var cheat = new Cheat(cheatStartPosition, cheatEndPosition);
                        if (cheats.ContainsKey(cheat)) { continue; }
                        cheats[cheat] = fromStart[cheatStartPosition]
                            + Math.Abs(stepX) + Math.Abs(stepY)
                            + fromEnd[cheatEndPosition];
                    }
                }
            }
        }
        return cheats;
    }

    public static List<Vector2> Surroundings(Vector2 position) =>
    [
        new(position.X + 1, position.Y),
        new(position.X - 1, position.Y),
        new(position.X, position.Y + 1),
        new(position.X, position.Y - 1),
    ];

    public class ParsedInput
    {
        public int MapWidth;
        public int MapHeight;
        public HashSet<Vector2> Walls;
        public Vector2 Start;
        public Vector2 End;
    }

    public static ParsedInput ParseInput(string source)
    {
        var lines = source.Split('\n').Select(line => line.Trim()).ToList();
        var result = new ParsedInput
        {
            MapWidth = lines[0].Length,
            MapHeight = lines.Count,
            Walls = [],
        };
        for (var y = 0; y < lines.Count; y++)
        {
            var line = lines[y];
            for (var x = 0; x < line.Length; x++)
            {
                var p = new Vector2(x, y);
                var c = line[x];
                if (c == '#') { result.Walls.Add(p); }
                else if (c == 'S') { result.Start = p; }
                else if (c == 'E') { result.End = p; }
            }
        }
        return result;
    }

    public const string EXAMPLE_INPUT = """
    ###############
    #...#...#.....#
    #.#.#.#.#.###.#
    #S#...#.#.#...#
    #######.#.#.###
    #######.#.#...#
    #######.#.###.#
    ###..E#...#...#
    ###.#######.###
    #...###...#...#
    #.#####.#.###.#
    #.#...#.#.#...#
    #.#.#.#.#.#.###
    #...#...#...###
    ###############
    """;

    public const string INPUT = """
    #############################################################################################################################################
    ###...#.....#...###...#...###...###.....#...............#...###...#...###.......#...#...........#...#.....#.....#...#...#.....#.......###...#
    ###.#.#.###.#.#.###.#.#.#.###.#.###.###.#.#############.#.#.###.#.#.#.###.#####.#.#.#.#########.#.#.#.###.#.###.#.#.#.#.#.###.#.#####.###.#.#
    #...#...#...#.#.#...#...#.#...#...#.#...#.........#...#.#.#.....#...#.#...#.....#.#.#.....#.....#.#.#.#...#.#...#.#.#.#.#...#...#.....#...#.#
    #.#######.###.#.#.#######.#.#####.#.#.###########.#.#.#.#.###########.#.###.#####.#.#####.#.#####.#.#.#.###.#.###.#.#.#.###.#####.#####.###.#
    #.......#...#.#.#.#.......#.....#.#.#.......#...#...#.#...#.....#...#.#.#...###...#...#...#...#...#.#.#.#...#...#.#.#.#.###...#...#...#...#.#
    #######.###.#.#.#.#.###########.#.#.#######.#.#.#####.#####.###.#.#.#.#.#.#####.#####.#.#####.#.###.#.#.#.#####.#.#.#.#.#####.#.###.#.###.#.#
    ###...#...#...#...#.....###...#.#...#.......#.#.#...#.....#.###...#.#.#.#.....#.....#.#...#...#...#...#.#.#...#.#.#.#.#...#...#.###.#.#...#.#
    ###.#.###.#############.###.#.#.#####.#######.#.#.#.#####.#.#######.#.#.#####.#####.#.###.#.#####.#####.#.#.#.#.#.#.#.###.#.###.###.#.#.###.#
    #...#.###...........#...#...#.#.#...#.....#...#...#.#...#...#...#...#.#.#.....#.....#.#...#.....#.#.....#...#.#...#.#.#...#...#...#.#.#.#...#
    #.###.#############.#.###.###.#.#.#.#####.#.#######.#.#.#####.#.#.###.#.#.#####.#####.#.#######.#.#.#########.#####.#.#.#####.###.#.#.#.#.###
    #.#...#...###.......#...#.#...#.#.#.#.....#.....#...#.#.###...#...#...#.#.#...#.....#.#...#.....#.#.......#...#...#...#.....#...#.#.#.#.#...#
    #.#.###.#.###.#########.#.#.###.#.#.#.#########.#.###.#.###.#######.###.#.#.#.#####.#.###.#.#####.#######.#.###.#.#########.###.#.#.#.#.###.#
    #.#.....#...#.........#.#.#.###.#.#...#.......#.#...#.#...#.......#.....#.#.#.#...#.#.#...#.....#.#.......#.....#.#.........#...#...#.#...#.#
    #.#########.#########.#.#.#.###.#.#####.#####.#.###.#.###.#######.#######.#.#.#.#.#.#.#.#######.#.#.#############.#.#########.#######.###.#.#
    #.......#...#...#...#.#.#.#.#...#.#...#.....#.#.#...#...#...#...#...#.....#.#.#.#.#.#.#.#.......#.#.#...###...#...#.......###...#...#.....#.#
    #######.#.###.#.#.#.#.#.#.#.#.###.#.#.#####.#.#.#.#####.###.#.#.###.#.#####.#.#.#.#.#.#.#.#######.#.#.#.###.#.#.#########.#####.#.#.#######.#
    ###...#.#.#...#.#.#.#.#.#.#.#...#...#...#...#.#.#.#...#...#...#.....#.#...#.#.#.#.#.#...#.#.......#...#...#.#.#...#...#...#...#.#.#.....#...#
    ###.#.#.#.#.###.#.#.#.#.#.#.###.#######.#.###.#.#.#.#.###.###########.#.#.#.#.#.#.#.#####.#.#############.#.#.###.#.#.#.###.#.#.#.#####.#.###
    #...#.#.#.#...#.#.#...#...#.#...#.......#.#...#.#.#.#...#.#...#.....#...#.#.#...#.#.#.....#.#...........#.#.#.....#.#...#...#.#.#.....#...###
    #.###.#.#.###.#.#.#########.#.###.#######.#.###.#.#.###.#.#.#.#.###.#####.#.#####.#.#.#####.#.#########.#.#.#######.#####.###.#.#####.#######
    #...#.#.#.....#.#.........#.#...#.###.....#...#.#.#.#...#...#...###...#...#.....#...#...#...#.......#...#.#.......#...###...#.#.#.....#...###
    ###.#.#.#######.#########.#.###.#.###.#######.#.#.#.#.###############.#.#######.#######.#.#########.#.###.#######.###.#####.#.#.#.#####.#.###
    #...#...#.....#.#...#...#.#.###.#...#...#.....#.#.#.#.....###...#...#.#...#.....#.......#...#.......#.....#...#...#...#...#.#...#.......#...#
    #.#######.###.#.#.#.#.#.#.#.###.###.###.#.#####.#.#.#####.###.#.#.#.#.###.#.#####.#########.#.#############.#.#.###.###.#.#.###############.#
    #...#.....###.#...#...#...#.....#...#...#...#...#...#.....#...#.#.#.#.#...#.....#...#...#...#.#...#...###...#.#...#...#.#.#.....#...........#
    ###.#.#######.###################.###.#####.#.#######.#####.###.#.#.#.#.#######.###.#.#.#.###.#.#.#.#.###.###.###.###.#.#.#####.#.###########
    ###...#.......#.........#...#...#...#.#.....#...#.....#...#...#...#.#.#...#.....#...#.#.#.#...#.#.#.#...#...#.....#...#.#...#...#...#...#...#
    #######.#######.#######.#.#.#.#.###.#.#.#######.#.#####.#.###.#####.#.###.#.#####.###.#.#.#.###.#.#.###.###.#######.###.###.#.#####.#.#.#.#.#
    #...###.#...#...#.......#.#...#...#.#.#.###...#.#.#...#.#...#.....#.#.#...#.....#.....#.#.#...#.#.#.#...###...#.....#...#...#.#.....#.#.#.#.#
    #.#.###.#.#.#.###.#######.#######.#.#.#.###.#.#.#.#.#.#.###.#####.#.#.#.#######.#######.#.###.#.#.#.#.#######.#.#####.###.###.#.#####.#.#.#.#
    #.#...#...#...###...#.....#.......#.#.#.#...#...#.#.#.#.#...###...#.#.#.#.....#.....#...#...#...#.#.#.#...#...#.......#...#...#.......#...#.#
    #.###.#############.#.#####.#######.#.#.#.#######.#.#.#.#.#####.###.#.#.#.###.#####.#.#####.#####.#.#.#.#.#.###########.###.###############.#
    #...#...###...#...#...#...#...#...#.#.#.#...#.....#.#.#.#.#...#.#...#.#.#.#...#...#.#.#...#...#...#.#.#.#.#.#...........#...#...#...#...#...#
    ###.###.###.#.#.#.#####.#.###.#.#.#.#.#.###.#.#####.#.#.#.#.#.#.#.###.#.#.#.###.#.#.#.#.#.###.#.###.#.#.#.#.#.###########.###.#.#.#.#.#.#.###
    #...#...#...#...#.#.....#...#...#.#...#.....#...#...#.#.#.#.#.#.#.....#...#.#...#.#.#.#.#.#...#.#...#...#...#...#...###...#...#.#.#.#.#...###
    #.###.###.#######.#.#######.#####.#############.#.###.#.#.#.#.#.###########.#.###.#.#.#.#.#.###.#.#############.#.#.###.###.###.#.#.#.#######
    #...#...#.......#.#.......#.#...#...........#...#.#...#.#...#.#...#.........#.###.#.#...#.#...#...#.....#.......#.#.#...#...###...#...#...###
    ###.###.#######.#.#######.#.#.#.###########.#.###.#.###.#####.###.#.#########.###.#.#####.###.#####.###.#.#######.#.#.###.#############.#.###
    ###...#.#.......#.........#...#.............#.#...#.#...#...#.#...#.........#...#.#.#.....#...#...#...#...###...#.#.#.#...###...........#...#
    #####.#.#.###################################.#.###.#.###.#.#.#.###########.###.#.#.#.#####.###.#.###.#######.#.#.#.#.#.#####.#############.#
    #.....#...#.....#...........................#.#.#...#.#...#...#.........###...#.#...#.#.....###.#...#.........#...#...#.....#.#...........#.#
    #.#########.###.#.#########################.#.#.#.###.#.###############.#####.#.#####.#.#######.###.#######################.#.#.#########.#.#
    #.#...#...#.#...#.#...........#...#.....#...#...#...#.#...#...#.......#.#.....#.....#.#.#.....#...#...............#...#...#...#.#.......#...#
    #.#.#.#.#.#.#.###.#.#########.#.#.#.###.#.#########.#.###.#.#.#.#####.#.#.#########.#.#.#.###.###.###############.#.#.#.#.#####.#.#####.#####
    #...#...#...#.....#...#.....#.#.#.#...#.#...###...#.#.#...#.#.#.....#...#.....#...#.#...#...#.....#...........###...#.#.#.....#...#...#.....#
    #####################.#.###.#.#.#.###.#.###.###.#.#.#.#.###.#.#####.#########.#.#.#.#######.#######.#########.#######.#.#####.#####.#.#####.#
    #...#.....###...#...#...#...#...#.....#...#.#...#.#.#.#...#.#...#...###.......#.#...###...#...#...#.#.......#.....#...#.....#.......#.......#
    #.#.#.###.###.#.#.#.#####.###############.#.#.###.#.#.###.#.###.#.#####.#######.#######.#.###.#.#.#.#.#####.#####.#.#######.#################
    #.#.#...#.#...#...#.......#...#.....#...#...#...#.#...###.#.###.#.....#.......#.......#.#.###...#...#.#...#.#.....#.#.....#...#...#...#...###
    #.#.###.#.#.###############.#.#.###.#.#.#######.#.#######.#.###.#####.#######.#######.#.#.###########.#.#.#.#.#####.#.###.###.#.#.#.#.#.#.###
    #.#.#...#.#...#...#.......#.#...###...#.........#...###...#...#.#...#.###...#.#...#...#.#...........#...#.#.#.#...#...###...#...#...#...#...#
    #.#.#.###.###.#.#.#.#####.#.#######################.###.#####.#.#.#.#.###.#.#.#.#.#.###.###########.#####.#.#.#.#.#########.###############.#
    #.#.#.#...###...#...#...#...###.........#.........#.#...#...#.#...#.#...#.#...#.#.#.###...........#.......#...#.#.....#...#.#.......#...#...#
    #.#.#.#.#############.#.#######.#######.#.#######.#.#.###.#.#.#####.###.#.#####.#.#.#############.#############.#####.#.#.#.#.#####.#.#.#.###
    #.#...#...#...#...#...#.......#.......#...#.....#.#.#...#.#.#.#.....#...#.#...#.#.#.....###...###...............#.....#.#.#.#.....#...#...###
    #.#######.#.#.#.#.#.#########.#######.#####.###.#.#.###.#.#.#.#.#####.###.#.#.#.#.#####.###.#.###################.#####.#.#.#####.###########
    #...#...#.#.#.#.#.#.........#.#.......###...#...#...#...#.#...#...#...###.#.#.#.#.......#...#...#.................#...#.#.#.#...#.#.........#
    ###.#.#.#.#.#.#.#.#########.#.#.#########.###.#######.###.#######.#.#####.#.#.#.#########.#####.#.#################.#.#.#.#.#.#.#.#.#######.#
    #...#.#.#...#.#.#.#.........#...#...#...#...#.#...###...#.....###.#.....#.#.#.#.#.........#...#.#...................#...#.#.#.#.#...#...#...#
    #.###.#.#####.#.#.#.#############.#.#.#.###.#.#.#.#####.#####.###.#####.#.#.#.#.#.#########.#.#.#########################.#.#.#.#####.#.#.###
    #...#.#.....#...#.#.......#.....#.#...#.#...#...#...#...#...#.#...#...#.#.#.#.#.#.#...#...#.#.#.................#...#.....#...#.......#...###
    ###.#.#####.#####.#######.#.###.#.#####.#.#########.#.###.#.#.#.###.#.#.#.#.#.#.#.#.#.#.#.#.#.#################.#.#.#.#######################
    ###...###...#...#.......#...###...#...#.#.#.......#.#.....#.#.#.#...#...#.#.#...#...#...#...#...............###...#...###...........#...#...#
    #########.###.#.#######.###########.#.#.#.#.#####.#.#######.#.#.#.#######.#.###############################.#############.#########.#.#.#.#.#
    #...#.....#...#.......#.###...#.....#...#.#.#...#...#...###...#...#...###...###.............................#.......#...#.#.........#.#...#.#
    #.#.#.#####.#########.#.###.#.#.#########.#.#.#.#####.#.###########.#.#########.#############################.#####.#.#.#.#.#########.#####.#
    #.#...#.....#...###...#.....#...#.........#...#...#...#.#...###.....#.....#.....#.....#.....#...............#.#...#...#...#...#...#...#...#.#
    #.#####.#####.#.###.#############.###############.#.###.#.#.###.#########.#.#####.###.#.###.#.#############.#.#.#.###########.#.#.#.###.#.#.#
    #.#...#.#.....#.....#...#...#...#.#...............#.#...#.#.....#.........#.......###...###...#...#.........#.#.#...#...#...#.#.#.#.#...#...#
    #.#.#.#.#.###########.#.#.#.#.#.#.#.###############.#.###.#######.#############################.#.#.#########.#.###.#.#.#.#.#.#.#.#.#.#######
    #...#.#.#.#.........#.#...#...#...#.............###.#.....#######....E#...#...#...#.............#...#.........#...#.#.#.#.#.#...#...#.......#
    #####.#.#.#.#######.#.#########################.###.###################.#.#.#.#.#.#.#################.###########.#.#.#.#.#.###############.#
    ###...#.#.#.#.....#...###.................#S###.....###################.#.#.#...#.#...............#...###...#.....#...#...#...#.............#
    ###.###.#.#.#.###.#######.###############.#.###########################.#.#.#####.###############.#.#####.#.#.###############.#.#############
    #...#...#...#...#.###...#...#...#.......#.#.#######################...#.#.#.....#...............#...#.....#.#...........#.....#.............#
    #.###.#########.#.###.#.###.#.#.#.#####.#.#.#######################.#.#.#.#####.###############.#####.#####.###########.#.#################.#
    #.#...#...#.....#.#...#.###...#...#.....#...###########...#########.#.#.#.#...#...........#.....#...#.#...#.............#.....#...#...#...#.#
    #.#.###.#.#.#####.#.###.###########.###################.#.#########.#.#.#.#.#.###########.#.#####.#.#.#.#.###################.#.#.#.#.#.#.#.#
    #...###.#.#.....#.#...#.#...###...#.#.....#...#...#...#.#.....#.....#...#...#.......#.....#...#...#.#...#...........#...#...#.#.#.#.#.#.#...#
    #######.#.#####.#.###.#.#.#.###.#.#.#.###.#.#.#.#.#.#.#.#####.#.###################.#.#######.#.###.###############.#.#.#.#.#.#.#.#.#.#.#####
    ###...#.#.......#...#.#...#.....#...#.#...#.#.#.#...#.#.#.....#.........#.....#...#...#.....#.#...#.................#.#...#.#...#...#.#.....#
    ###.#.#.###########.#.###############.#.###.#.#.#####.#.#.#############.#.###.#.#.#####.###.#.###.###################.#####.#########.#####.#
    #...#...#...#.....#...#...............#.....#.#...#...#.#.......###...#...###...#.....#.#...#...#.........#...#...###.#.....#.......#...#...#
    #.#######.#.#.###.#####.#####################.###.#.###.#######.###.#.###############.#.#.#####.#########.#.#.#.#.###.#.#####.#####.###.#.###
    #.........#...###...#...###.............#.....#...#.....#.......#...#...###...#.....#.#.#.....#.#...#...#...#...#.....#.......#...#...#.#...#
    ###################.#.#####.###########.#.#####.#########.#######.#####.###.#.#.###.#.#.#####.#.#.#.#.#.#######################.#.###.#.###.#
    ###...#.......#####...#.....#.........#.#.......#.....#...#.....#.....#...#.#...#...#...###...#...#...#.....#...#...#...........#.#...#.....#
    ###.#.#.#####.#########.#####.#######.#.#########.###.#.###.###.#####.###.#.#####.#########.###############.#.#.#.#.#.###########.#.#########
    #...#.#.....#...###...#.....#.#.......#.#.....#...###...###...#...###...#.#...#...#...#...#.........#.....#...#...#...#...#...###.#...#...###
    #.###.#####.###.###.#.#####.#.#.#######.#.###.#.#############.###.#####.#.###.#.###.#.#.#.#########.#.###.#############.#.#.#.###.###.#.#.###
    #...#.....#...#.#...#.#...#...#.......#.#...#.#.........#...#.#...#.....#.#...#.#...#...#...#...###...###...........###.#...#...#...#...#...#
    ###.#####.###.#.#.###.#.#.###########.#.###.#.#########.#.#.#.#.###.#####.#.###.#.#########.#.#.###################.###.#######.###.#######.#
    #...#.....###.#.#...#...#...#...#.....#...#.#.#.........#.#...#...#.....#.#...#.#.#.......#...#.#.....###...#...#...#...#...#...###.#...#...#
    #.###.#######.#.###.#######.#.#.#.#######.#.#.#.#########.#######.#####.#.###.#.#.#.#####.#####.#.###.###.#.#.#.#.###.###.#.#.#####.#.#.#.###
    #...#.###...#.#.###.....#...#.#.#.......#...#...###...###...#...#.....#.#...#.#.#...#.....#.....#...#.....#...#...#...#...#...#...#...#.#...#
    ###.#.###.#.#.#.#######.#.###.#.#######.###########.#.#####.#.#.#####.#.###.#.#.#####.#####.#######.###############.###.#######.#.#####.###.#
    #...#.....#...#.........#.....#.......#.....#.......#...###...#...#...#.#...#.#.#.....#.....#.....#...#...........#...#.#...#...#...###...#.#
    #.###################################.#####.#.#########.#########.#.###.#.###.#.#.#####.#####.###.###.#.#########.###.#.#.#.#.#####.#####.#.#
    #.#...#.....#.........#...#...#.....#.......#.....#.....#...#...#.#.#...#...#.#.#.....#...###.#...###...#.........#...#...#...#...#.#...#...#
    #.#.#.#.###.#.#######.#.#.#.#.#.###.#############.#.#####.#.#.#.#.#.#.#####.#.#.#####.###.###.#.#########.#########.###########.#.#.#.#.#####
    #.#.#.#.###...#...#...#.#...#.#.#...#.....#...#...#.#.....#.#.#.#.#.#...#...#.#...#...#...#...#...#.....#.....#.....#.........#.#.#...#.....#
    #.#.#.#.#######.#.#.###.#####.#.#.###.###.#.#.#.###.#.#####.#.#.#.#.###.#.###.###.#.###.###.#####.#.###.#####.#.#####.#######.#.#.#########.#
    #.#.#.#.#.....#.#...#...#.....#.#...#...#.#.#.#.#...#...#...#.#.#.#.###.#.###...#.#.#...###...#...#...#.#...#...#...#.......#...#.........#.#
    #.#.#.#.#.###.#.#####.###.#####.###.###.#.#.#.#.#.#####.#.###.#.#.#.###.#.#####.#.#.#.#######.#.#####.#.#.#.#####.#.#######.#############.#.#
    #.#.#...#...#...#...#.#...#...#.#...#...#.#.#.#.#.#...#.#.#...#...#.#...#...#...#...#.#.....#.#...#...#.#.#.#...#.#.#.....#.#.........#...#.#
    #.#.#######.#####.#.#.#.###.#.#.#.###.###.#.#.#.#.#.#.#.#.#.#######.#.#####.#.#######.#.###.#.###.#.###.#.#.#.#.#.#.#.###.#.#.#######.#.###.#
    #...###...#.....#.#.#.#.#...#.#.#...#...#...#.#.#.#.#.#.#.#.....#...#.###...#.#.......#.#...#.#...#...#.#.#.#.#.#.#.#...#.#...#.......#.....#
    #######.#.#####.#.#.#.#.#.###.#.###.###.#####.#.#.#.#.#.#.#####.#.###.###.###.#.#######.#.###.#.#####.#.#.#.#.#.#.#.###.#.#####.#############
    #.......#.......#.#.#.#...#...#.###...#.....#.#.#...#.#.#.....#.#...#...#.....#...#...#.#...#.#.......#...#.#.#...#.#...#.......###.........#
    #.###############.#.#.#####.###.#####.#####.#.#.#####.#.#####.#.###.###.#########.#.#.#.###.#.#############.#.#####.#.#############.#######.#
    #.....#...#...#...#.#.....#...#.#.....#...#.#...###...#...#...#...#.#...#...#.....#.#.#.#...#.........#...#.#.....#...#...#.........#.......#
    #####.#.#.#.#.#.###.#####.###.#.#.#####.#.#.#######.#####.#.#####.#.#.###.#.#.#####.#.#.#.###########.#.#.#.#####.#####.#.#.#########.#######
    #.....#.#.#.#...#...#...#.#...#.#.....#.#...#.......#.....#.#...#.#.#.....#.#.#...#.#.#.#.#.........#...#.#.#...#.......#...#.......#.......#
    #.#####.#.#.#####.###.#.#.#.###.#####.#.#####.#######.#####.#.#.#.#.#######.#.#.#.#.#.#.#.#.#######.#####.#.#.#.#############.#####.#######.#
    #...#...#.#...###...#.#...#...#...###.#.....#...#...#.#.....#.#.#.#...#...#.#...#.#.#...#.#.......#...#...#...#.#...#...#...#.....#.........#
    ###.#.###.###.#####.#.#######.###.###.#####.###.#.#.#.#.#####.#.#.###.#.#.#.#####.#.#####.#######.###.#.#######.#.#.#.#.#.#.#####.###########
    #...#...#...#.#.....#.....#...#...#...#.....###.#.#.#.#.....#.#...#...#.#.#.#.....#.#.....#...#...#...#.......#...#.#.#...#.#...#...#.......#
    #.#####.###.#.#.#########.#.###.###.###.#######.#.#.#.#####.#.#####.###.#.#.#.#####.#.#####.#.#.###.#########.#####.#.#####.#.#.###.#.#####.#
    #.#.....###...#.....#.....#...#.#...#...#.......#.#.#.#.....#.....#...#.#...#.#...#.#...#...#.#...#...#.......#...#.#.#.....#.#...#...#...#.#
    #.#.###############.#.#######.#.#.###.###.#######.#.#.#.#########.###.#.#####.#.#.#.###.#.###.###.###.#.#######.#.#.#.#.#####.###.#####.#.#.#
    #.#...#...#.........#...#.....#.#.#...###.#...#...#.#.#.#...#...#...#.#.....#...#.#.#...#.###...#.#...#.......#.#...#.#...#...###...#...#...#
    #.###.#.#.#.###########.#.#####.#.#.#####.#.#.#.###.#.#.#.#.#.#.###.#.#####.#####.#.#.###.#####.#.#.#########.#.#####.###.#.#######.#.#######
    #...#...#.#.#.....#...#.#.#.....#.#.#...#...#.#.#...#.#...#.#.#...#.#...#...#.....#.#...#.....#...#.#...#.....#...#...#...#.###...#.#.......#
    ###.#####.#.#.###.#.#.#.#.#.#####.#.#.#.#####.#.#.###.#####.#.###.#.###.#.###.#####.###.#####.#####.#.#.#.#######.#.###.###.###.#.#.#######.#
    #...#.....#.#...#.#.#.#.#...#.....#...#...###...#.....#.....#.#...#...#...###.......#...#...#.....#...#.#.......#.#...#...#.....#.#.#...#...#
    #.###.#####.###.#.#.#.#.#####.###########.#############.#####.#.#####.###############.###.#.#####.#####.#######.#.###.###.#######.#.#.#.#.###
    #...#.....#.#...#.#.#...#...#.....#.......#.....#.......#...#.#...#...###...#...#...#.#...#.#...#.....#...#...#.#.#...#...#.......#.#.#.#...#
    ###.#####.#.#.###.#.#####.#.#####.#.#######.###.#.#######.#.#.###.#.#####.#.#.#.#.#.#.#.###.#.#.#####.###.#.#.#.#.#.###.###.#######.#.#.###.#
    ###.#...#.#.#.###.#.#.....#.......#.........#...#...#...#.#.#...#.#.#.....#...#...#...#.#...#.#.#...#...#.#.#...#.#.#...#...#...#...#.#...#.#
    ###.#.#.#.#.#.###.#.#.#######################.#####.#.#.#.#.###.#.#.#.#################.#.###.#.#.#.###.#.#.#####.#.#.###.###.#.#.###.###.#.#
    #...#.#.#.#.#.#...#.#.......#.......#.......#...###.#.#.#.#...#.#.#.#.....#...#.....#...#...#.#.#.#.#...#.#...#...#.#...#.....#.#...#...#.#.#
    #.###.#.#.#.#.#.###.#######.#.#####.#.#####.###.###.#.#.#.###.#.#.#.#####.#.#.#.###.#.#####.#.#.#.#.#.###.###.#.###.###.#######.###.###.#.#.#
    #.#...#.#.#.#.#.#...###.....#.#.....#.....#.#...#...#.#.#.#...#.#.#.....#...#.#...#.#...#...#.#.#.#.#...#...#.#.#...###...#...#.###.#...#.#.#
    #.#.###.#.#.#.#.#.#####.#####.#.#########.#.#.###.###.#.#.#.###.#.#####.#####.###.#.###.#.###.#.#.#.###.###.#.#.#.#######.#.#.#.###.#.###.#.#
    #.#...#.#.#.#.#.#.#.....#.....#.......#...#.#.###...#.#...#.....#.#...#.....#.#...#.#...#...#.#.#.#.#...#...#.#.#...###...#.#.#...#.#...#...#
    #.###.#.#.#.#.#.#.#.#####.###########.#.###.#.#####.#.###########.#.#.#####.#.#.###.#.#####.#.#.#.#.#.###.###.#.###.###.###.#.###.#.###.#####
    #.#...#.#.#.#.#.#.#.#...#.#...........#...#.#.....#.#...#.........#.#...#...#.#...#.#.....#.#.#.#.#.#...#...#.#...#...#...#.#.#...#...#.....#
    #.#.###.#.#.#.#.#.#.#.#.#.#.#############.#.#####.#.###.#.#########.###.#.###.###.#.#####.#.#.#.#.#.###.###.#.###.###.###.#.#.#.#####.#####.#
    #...###...#...#...#...#...#...............#.......#.....#...........###...###.....#.......#...#...#.....###...###.....###...#...#####.......#
    #############################################################################################################################################
    """;
}
