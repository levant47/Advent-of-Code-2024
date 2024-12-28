﻿public static class Program
{
    public static void Main()
    {
        Console.WriteLine("Starting problem 18...");

        var input = INPUT;
        var allWalls = ParseInput(input);
        var mapSize = MapSize[input];
        {
            var walls = allWalls.Take(WallsLimit[input]).ToHashSet();
            Console.WriteLine($"Part 1 answer: {Solve(walls, new(mapSize, mapSize))}");
        }
        for (var limit = WallsLimit[input] + 1; limit < allWalls.Count; limit++)
        {
            if (Solve(allWalls.Take(limit).ToHashSet(), new(mapSize, mapSize)) == -1)
            {
                Console.WriteLine($"Part 2 answer: {allWalls[limit - 1]}");
                break;
            }
        }

        Console.WriteLine("Done");
    }

    public class Path
    {
        public Vector2 Current;
        public int Length;
    }

    public static int Solve(HashSet<Vector2> walls, Vector2 target)
    {
        var visited = new HashSet<Vector2> { new(0, 0) };
        var paths = new List<Path> { new() { Current = new(0, 0) } };
        while (true)
        {
            var updatedPaths = new List<Path>();
            foreach (var path in paths)
            {
                var nextSteps = new List<Vector2>
                {
                    new(path.Current.X + 1, path.Current.Y),
                    new(path.Current.X - 1, path.Current.Y),
                    new(path.Current.X, path.Current.Y + 1),
                    new(path.Current.X, path.Current.Y - 1),
                };
                foreach (var step in nextSteps)
                {
                    if (step == target) { return path.Length + 1; }
                    if (!visited.Contains(step) && !walls.Contains(step) && step.X >= 0 && step.X <= target.X && step.Y >= 0 && step.Y <= target.Y)
                    {
                        updatedPaths.Add(new() { Current = step, Length = path.Length + 1 });
                        visited.Add(step);
                    }
                }
            }
            if (updatedPaths.Count == 0) { return -1; }
            paths = updatedPaths;
        }
    }

    public record Vector2(int X, int Y);

    public static List<Vector2> ParseInput(string source)
    {
        var lines = source.Trim().Split('\n').Select(line => line.Trim()).ToList();
        var result = new List<Vector2>();
        foreach (var line in lines)
        {
            var words = line.Split(',').Select(int.Parse).ToList();
            result.Add(new(words[0], words[1]));
        }
        return result;
    }

    public static readonly Dictionary<string, int> MapSize = new()
    {
        { EXAMPLE_INPUT, 6 },
        { INPUT, 70 },
    };

    public static readonly Dictionary<string, int> WallsLimit = new()
    {
        { EXAMPLE_INPUT, 12 },
        { INPUT, 1024 },
    };

    public const string EXAMPLE_INPUT = """
    5,4
    4,2
    4,5
    3,0
    2,1
    6,3
    2,4
    1,5
    0,6
    3,3
    2,6
    5,1
    1,2
    5,5
    2,5
    6,5
    1,4
    0,4
    6,4
    1,1
    6,1
    1,0
    0,5
    1,6
    2,0
    """;

    public const string INPUT = """
    54,47
    45,29
    41,65
    55,62
    63,33
    68,27
    29,21
    45,45
    7,1
    43,43
    21,6
    61,55
    59,45
    30,45
    33,49
    45,64
    41,31
    55,33
    19,7
    61,46
    23,19
    52,63
    12,7
    35,42
    58,65
    39,57
    51,51
    49,58
    68,25
    12,5
    41,63
    38,49
    19,23
    3,22
    32,17
    64,33
    41,67
    45,57
    56,41
    59,65
    23,5
    21,19
    29,15
    65,45
    43,40
    67,29
    39,45
    1,5
    69,30
    67,37
    21,26
    40,49
    63,69
    43,61
    29,23
    31,41
    6,31
    37,49
    64,69
    43,64
    61,41
    67,41
    39,70
    3,1
    69,35
    63,29
    41,56
    70,61
    21,13
    58,49
    51,57
    46,57
    69,67
    15,15
    3,8
    8,19
    51,59
    68,69
    37,60
    16,15
    59,44
    55,43
    69,36
    43,53
    39,58
    29,40
    15,14
    65,38
    47,46
    41,41
    55,58
    33,39
    21,7
    41,37
    58,67
    63,59
    57,55
    7,33
    43,67
    39,44
    65,29
    7,25
    17,26
    1,11
    63,26
    65,34
    38,61
    51,53
    39,42
    17,32
    53,57
    20,27
    51,62
    7,3
    31,49
    45,47
    3,11
    37,41
    62,39
    27,20
    26,49
    37,52
    56,69
    21,18
    7,23
    1,2
    15,13
    67,57
    45,49
    53,44
    35,41
    31,11
    45,40
    18,25
    67,63
    34,41
    25,47
    63,47
    5,22
    51,40
    44,35
    55,34
    7,0
    66,35
    13,5
    48,53
    37,44
    43,57
    43,66
    67,46
    57,58
    9,1
    65,37
    19,5
    42,43
    62,43
    47,50
    7,21
    63,61
    69,25
    15,1
    27,25
    37,46
    45,65
    9,2
    48,29
    39,69
    31,46
    43,59
    26,47
    20,9
    69,23
    69,31
    7,27
    13,8
    18,13
    13,3
    63,55
    17,5
    21,27
    57,41
    61,57
    45,35
    53,50
    39,67
    68,33
    43,52
    67,27
    37,34
    43,58
    49,45
    4,1
    15,6
    61,53
    29,39
    40,59
    47,7
    17,16
    21,20
    31,42
    44,45
    46,33
    7,15
    19,15
    48,49
    57,60
    48,61
    44,43
    14,1
    19,16
    59,61
    53,41
    15,21
    53,68
    57,63
    43,31
    53,53
    7,37
    42,67
    39,36
    41,45
    45,66
    41,52
    40,33
    59,64
    5,14
    59,58
    39,61
    64,65
    19,1
    29,3
    1,4
    55,36
    20,13
    56,51
    44,69
    36,49
    63,36
    24,21
    43,37
    68,59
    49,53
    15,36
    33,69
    4,19
    67,60
    22,39
    24,15
    35,49
    25,45
    58,61
    19,18
    23,1
    49,55
    55,56
    3,18
    56,63
    6,15
    36,51
    1,9
    11,7
    60,59
    51,52
    69,34
    49,69
    44,49
    51,67
    43,32
    39,46
    47,41
    41,54
    3,15
    51,49
    45,50
    69,69
    39,35
    59,62
    41,27
    66,69
    43,33
    56,67
    39,65
    48,45
    22,23
    17,1
    47,55
    61,47
    15,19
    42,35
    0,17
    46,43
    15,8
    18,23
    55,42
    36,41
    64,57
    15,17
    8,23
    51,48
    3,5
    13,21
    62,59
    44,37
    33,41
    23,7
    53,61
    55,60
    67,30
    5,13
    3,10
    9,19
    19,11
    27,1
    59,25
    43,30
    31,44
    43,69
    36,45
    53,55
    42,47
    59,67
    51,45
    27,19
    50,67
    63,45
    5,10
    10,27
    40,53
    55,41
    13,1
    62,51
    55,53
    32,39
    38,51
    25,6
    59,39
    52,47
    50,57
    2,3
    34,39
    7,12
    68,41
    9,5
    27,11
    43,56
    17,7
    7,11
    53,63
    63,65
    13,12
    13,24
    55,69
    31,45
    46,53
    37,59
    61,34
    41,32
    32,37
    29,42
    40,29
    63,35
    27,49
    54,67
    45,69
    23,15
    27,21
    67,68
    33,70
    27,13
    11,9
    60,53
    67,55
    9,16
    49,40
    69,29
    21,24
    9,13
    65,69
    69,41
    45,51
    12,23
    47,39
    15,25
    43,65
    30,21
    67,62
    47,33
    17,21
    19,9
    56,47
    51,65
    47,38
    47,37
    47,42
    3,31
    67,61
    16,19
    39,59
    21,4
    13,18
    61,65
    8,9
    35,43
    11,17
    53,51
    66,43
    50,45
    34,43
    63,63
    15,7
    3,19
    42,45
    69,57
    16,3
    69,65
    9,25
    43,48
    45,68
    52,41
    1,21
    6,25
    21,14
    47,53
    36,37
    9,17
    38,37
    37,66
    45,37
    4,13
    65,41
    54,63
    61,49
    50,43
    41,36
    32,49
    65,53
    22,3
    7,17
    37,62
    55,47
    7,5
    50,63
    47,48
    21,23
    27,48
    15,9
    25,25
    59,48
    44,55
    33,45
    37,37
    7,4
    41,62
    63,50
    24,49
    4,29
    1,3
    35,37
    23,4
    17,18
    61,69
    66,41
    17,13
    67,28
    43,35
    2,17
    40,57
    59,46
    61,67
    54,41
    66,25
    35,47
    61,64
    15,20
    57,66
    47,31
    26,41
    49,57
    5,21
    10,5
    3,13
    7,6
    2,5
    14,21
    55,59
    49,49
    65,52
    59,37
    13,9
    19,2
    10,15
    18,21
    19,34
    21,17
    9,11
    65,67
    11,1
    25,22
    58,57
    33,43
    9,7
    46,39
    7,19
    59,49
    18,7
    11,26
    9,6
    67,43
    57,61
    66,29
    65,25
    14,13
    1,7
    1,17
    35,48
    53,58
    51,63
    27,12
    63,60
    65,49
    27,23
    31,39
    30,49
    20,23
    1,13
    22,25
    9,12
    65,35
    62,65
    66,57
    27,47
    20,1
    51,70
    43,51
    41,43
    5,17
    65,59
    53,59
    27,2
    39,49
    16,1
    68,53
    65,39
    17,19
    58,39
    63,41
    45,62
    47,35
    16,23
    46,67
    34,47
    7,13
    14,25
    59,56
    45,53
    33,47
    67,51
    8,15
    63,43
    27,22
    37,45
    57,51
    43,39
    55,65
    49,60
    17,4
    41,39
    49,54
    70,53
    26,3
    67,69
    1,20
    65,31
    64,43
    65,36
    1,1
    11,19
    29,41
    5,24
    68,57
    22,11
    53,38
    69,24
    19,21
    18,11
    42,69
    55,64
    44,31
    23,2
    48,65
    5,16
    13,16
    23,13
    57,53
    40,39
    63,49
    69,66
    27,46
    69,37
    26,9
    21,0
    69,32
    62,49
    57,54
    47,51
    3,7
    63,54
    67,59
    65,47
    55,61
    51,54
    2,15
    67,31
    4,7
    11,4
    24,47
    15,23
    21,3
    53,65
    11,8
    40,65
    45,34
    14,5
    50,51
    63,51
    51,43
    33,46
    5,6
    57,57
    63,27
    7,18
    0,23
    17,28
    69,53
    6,3
    20,21
    65,46
    59,59
    46,55
    41,68
    60,69
    43,55
    9,3
    25,37
    3,9
    56,43
    61,62
    41,38
    6,1
    5,15
    11,27
    24,9
    25,4
    63,30
    47,34
    13,13
    35,39
    55,55
    39,33
    25,7
    25,49
    41,51
    69,55
    7,20
    53,67
    13,7
    9,8
    67,40
    49,59
    8,27
    67,35
    57,70
    65,54
    1,14
    63,57
    64,25
    51,47
    23,12
    17,11
    41,55
    15,3
    23,23
    7,22
    23,11
    41,64
    61,59
    38,47
    67,32
    53,66
    68,63
    68,37
    31,47
    41,59
    59,69
    61,61
    37,64
    61,40
    57,49
    10,33
    57,45
    33,63
    53,47
    65,55
    68,51
    63,28
    62,57
    47,47
    23,25
    39,47
    57,39
    67,66
    59,63
    17,12
    47,67
    25,3
    27,39
    65,50
    33,44
    50,47
    24,13
    37,57
    41,53
    33,0
    45,59
    63,40
    51,28
    39,30
    44,27
    67,65
    21,21
    66,67
    47,43
    60,51
    1,23
    5,5
    69,63
    55,57
    17,23
    3,23
    50,65
    57,69
    5,9
    40,41
    6,11
    65,32
    19,3
    11,3
    26,25
    59,55
    5,19
    46,63
    57,30
    45,61
    41,49
    25,44
    64,61
    21,8
    19,13
    49,43
    49,56
    20,29
    35,21
    19,10
    33,37
    47,59
    51,60
    15,22
    45,33
    17,17
    11,10
    58,41
    47,69
    37,63
    41,33
    11,24
    61,36
    25,27
    67,39
    46,29
    22,21
    10,11
    57,43
    47,62
    49,38
    51,69
    7,9
    9,18
    16,25
    47,61
    57,59
    50,69
    17,29
    45,60
    5,20
    47,57
    5,25
    13,19
    19,6
    1,8
    41,69
    42,55
    1,15
    57,65
    29,8
    53,69
    5,31
    48,69
    53,60
    62,55
    67,64
    42,33
    69,56
    43,63
    37,47
    44,53
    36,63
    8,25
    49,61
    45,58
    59,29
    47,52
    49,47
    55,49
    47,63
    51,50
    63,67
    19,25
    29,25
    21,15
    63,48
    59,38
    60,41
    55,35
    57,46
    69,27
    29,49
    42,61
    63,46
    67,33
    67,50
    70,27
    40,47
    58,35
    39,68
    28,19
    10,1
    12,13
    23,21
    38,39
    1,19
    19,19
    11,13
    5,1
    17,9
    15,11
    39,43
    49,41
    39,51
    61,56
    45,31
    37,39
    45,55
    31,37
    60,67
    6,27
    47,65
    49,65
    16,31
    9,23
    9,24
    23,9
    4,3
    21,16
    7,31
    11,11
    41,61
    47,45
    58,51
    21,1
    63,38
    8,3
    61,44
    69,40
    47,36
    65,58
    6,7
    57,52
    42,41
    65,44
    48,67
    7,10
    13,27
    57,33
    43,60
    56,49
    14,17
    67,54
    61,63
    49,64
    45,28
    5,23
    28,41
    61,51
    12,1
    23,3
    63,62
    69,59
    59,53
    46,49
    70,65
    52,59
    63,52
    47,29
    42,37
    51,61
    13,10
    23,8
    52,53
    39,37
    31,40
    36,39
    21,5
    29,45
    21,45
    53,56
    53,40
    54,53
    60,47
    65,48
    37,69
    12,17
    24,25
    49,51
    18,15
    45,39
    7,7
    32,35
    33,48
    61,31
    59,43
    41,35
    44,41
    35,45
    69,61
    61,45
    64,41
    53,43
    16,9
    49,67
    45,43
    61,43
    45,41
    1,12
    45,46
    5,3
    37,43
    29,22
    47,27
    53,39
    62,69
    15,10
    59,51
    19,17
    13,11
    26,13
    55,67
    65,51
    17,8
    28,25
    60,37
    1,10
    67,38
    39,39
    13,23
    29,47
    26,19
    35,56
    8,31
    52,55
    21,11
    3,3
    67,45
    69,39
    52,67
    51,55
    33,9
    37,67
    63,53
    27,24
    49,39
    47,49
    3,29
    59,54
    57,47
    39,66
    63,39
    31,43
    19,4
    11,25
    63,25
    39,41
    11,14
    5,11
    9,9
    55,51
    13,2
    23,20
    28,45
    27,41
    66,55
    41,50
    25,23
    48,41
    63,37
    30,37
    3,12
    56,55
    41,47
    43,47
    59,41
    57,44
    0,7
    39,34
    36,67
    21,34
    27,26
    22,57
    43,13
    15,35
    28,13
    9,15
    6,45
    67,48
    47,17
    14,65
    23,50
    66,21
    13,59
    13,55
    49,3
    35,30
    27,51
    17,53
    36,19
    47,19
    21,31
    3,63
    68,43
    23,16
    61,20
    61,35
    47,30
    59,32
    51,17
    29,9
    59,7
    17,25
    53,21
    67,7
    4,67
    9,69
    45,10
    34,35
    7,65
    21,50
    19,70
    33,16
    19,63
    57,1
    41,15
    62,17
    69,3
    63,2
    29,28
    55,31
    25,19
    15,27
    17,43
    23,34
    39,55
    30,7
    23,27
    1,25
    31,24
    19,57
    17,61
    29,4
    46,21
    57,7
    31,19
    27,69
    36,3
    23,58
    9,66
    45,27
    15,59
    11,63
    33,55
    5,69
    15,62
    62,11
    5,67
    9,60
    27,5
    13,56
    29,30
    30,61
    22,29
    19,52
    43,1
    67,0
    19,39
    49,34
    17,30
    36,17
    43,20
    43,16
    41,2
    24,55
    62,21
    29,37
    53,45
    25,13
    37,7
    15,60
    32,57
    36,11
    21,46
    21,61
    49,25
    6,55
    27,43
    13,69
    1,55
    18,61
    23,46
    49,35
    23,66
    3,37
    23,39
    55,9
    3,17
    23,53
    11,61
    65,15
    3,27
    31,57
    27,28
    67,2
    35,54
    37,17
    67,11
    23,17
    69,7
    6,43
    3,44
    10,45
    11,39
    34,15
    21,57
    3,41
    15,54
    39,25
    1,63
    23,30
    53,23
    41,13
    21,67
    5,45
    66,61
    37,55
    35,60
    19,67
    11,22
    14,47
    27,27
    7,67
    65,19
    20,61
    33,30
    14,51
    31,61
    1,37
    5,62
    33,7
    8,43
    47,15
    28,65
    66,5
    65,57
    34,23
    59,26
    63,3
    19,37
    1,46
    40,21
    61,13
    59,13
    47,21
    19,65
    11,64
    39,62
    54,51
    13,57
    13,41
    58,5
    33,27
    13,38
    9,68
    54,9
    45,67
    33,52
    25,33
    13,61
    17,57
    24,35
    13,47
    5,59
    57,21
    35,25
    11,29
    31,1
    4,57
    49,13
    17,45
    16,57
    61,26
    7,57
    7,68
    29,31
    56,39
    35,26
    9,43
    5,55
    48,43
    33,26
    35,68
    52,33
    20,55
    4,63
    13,63
    57,27
    65,4
    19,61
    21,43
    60,23
    24,65
    49,26
    45,12
    11,43
    51,3
    49,7
    64,31
    9,57
    63,17
    55,22
    51,37
    8,55
    69,21
    49,18
    67,17
    11,49
    24,37
    31,27
    61,9
    39,16
    66,23
    61,39
    15,42
    1,60
    32,21
    51,8
    43,49
    43,15
    3,61
    9,55
    34,61
    33,13
    1,42
    27,3
    7,45
    49,11
    39,21
    44,13
    31,55
    35,32
    27,52
    13,45
    19,68
    3,49
    35,69
    65,22
    57,13
    42,29
    9,31
    61,28
    46,7
    36,7
    34,11
    65,61
    59,31
    21,37
    47,5
    19,44
    41,17
    5,30
    38,15
    3,25
    31,13
    49,27
    51,13
    44,7
    29,61
    23,52
    55,17
    69,47
    2,57
    12,49
    4,27
    48,13
    40,17
    3,39
    13,35
    25,67
    59,35
    15,67
    15,63
    33,35
    11,69
    44,19
    20,47
    5,57
    9,29
    16,69
    59,18
    68,3
    27,44
    23,29
    68,9
    12,33
    25,59
    27,57
    24,27
    4,61
    29,29
    69,51
    53,25
    33,32
    42,23
    41,44
    21,65
    11,31
    63,9
    18,59
    31,7
    39,9
    37,29
    11,30
    34,51
    43,27
    3,55
    57,23
    49,23
    49,15
    45,15
    25,15
    67,47
    58,3
    43,29
    12,41
    5,54
    46,17
    2,47
    16,51
    3,67
    37,33
    27,65
    54,15
    57,11
    2,67
    69,49
    51,21
    51,34
    45,25
    27,33
    59,34
    16,43
    30,57
    59,4
    25,39
    9,37
    28,51
    5,7
    3,28
    53,10
    3,47
    26,51
    59,1
    6,41
    41,5
    57,19
    5,63
    29,38
    7,55
    7,28
    57,3
    53,8
    29,50
    63,7
    11,36
    45,21
    9,35
    55,13
    33,56
    12,61
    47,25
    27,37
    6,51
    61,37
    1,45
    49,14
    10,53
    1,62
    24,53
    33,20
    12,53
    41,19
    48,19
    9,40
    10,59
    38,69
    5,29
    49,17
    12,65
    18,35
    13,49
    61,2
    36,25
    39,5
    37,11
    33,29
    50,17
    38,13
    19,53
    25,53
    27,29
    49,5
    15,47
    39,29
    3,53
    40,27
    19,31
    35,63
    67,18
    35,31
    63,13
    65,64
    17,36
    42,9
    57,31
    57,9
    25,60
    14,55
    31,68
    50,23
    2,21
    16,67
    33,21
    53,37
    63,66
    21,33
    21,63
    29,13
    61,24
    23,60
    59,23
    42,19
    56,29
    37,1
    69,19
    37,15
    64,7
    37,12
    35,67
    37,23
    61,21
    45,26
    2,29
    13,62
    17,33
    39,53
    44,1
    15,51
    25,63
    5,51
    65,43
    55,24
    47,24
    29,67
    35,14
    41,3
    43,45
    11,5
    57,8
    35,3
    51,15
    69,1
    31,15
    17,40
    63,8
    35,6
    55,38
    61,5
    42,15
    11,65
    23,65
    29,56
    59,11
    67,25
    2,65
    27,36
    52,27
    57,67
    5,37
    59,17
    31,63
    9,61
    35,61
    7,49
    31,29
    38,19
    43,4
    31,33
    21,53
    29,27
    35,4
    34,65
    55,18
    37,31
    39,4
    41,9
    13,65
    31,58
    65,10
    64,23
    39,15
    29,53
    47,22
    11,45
    25,68
    37,25
    29,35
    57,17
    49,1
    16,47
    23,35
    15,4
    17,15
    50,19
    6,69
    39,17
    14,31
    11,55
    36,23
    36,33
    29,1
    67,21
    65,27
    67,67
    17,47
    58,29
    69,46
    52,31
    55,29
    22,41
    49,8
    63,31
    38,31
    30,51
    34,53
    48,3
    35,33
    1,35
    29,33
    37,28
    45,9
    59,57
    1,24
    67,13
    50,3
    11,59
    9,27
    55,32
    57,37
    15,41
    35,27
    24,1
    31,35
    18,55
    65,65
    9,63
    56,25
    61,32
    12,55
    33,1
    14,33
    10,67
    35,55
    53,12
    24,69
    6,61
    37,51
    60,7
    19,51
    61,33
    17,37
    4,49
    19,35
    32,5
    13,53
    31,12
    58,15
    68,21
    68,13
    58,21
    1,47
    33,61
    65,26
    38,55
    37,9
    47,1
    13,17
    32,61
    12,27
    50,21
    22,55
    51,23
    14,37
    25,43
    29,62
    23,18
    62,5
    67,6
    27,63
    55,1
    9,50
    45,14
    63,14
    2,39
    66,15
    55,7
    11,62
    23,55
    52,19
    55,11
    44,5
    23,37
    18,29
    53,2
    67,53
    39,19
    37,27
    1,26
    29,19
    25,55
    5,47
    47,10
    68,11
    9,62
    7,63
    53,4
    31,10
    1,48
    61,15
    67,49
    9,41
    11,32
    17,55
    18,45
    3,59
    38,25
    29,55
    5,32
    34,27
    31,6
    13,15
    41,12
    34,9
    57,29
    55,63
    16,45
    62,7
    67,15
    27,67
    31,31
    1,36
    70,11
    5,38
    21,68
    1,57
    27,10
    32,53
    40,19
    36,57
    32,25
    61,27
    14,29
    28,63
    35,28
    43,21
    51,24
    63,15
    51,39
    9,33
    68,5
    63,21
    29,11
    17,50
    31,23
    5,39
    33,59
    9,47
    7,34
    19,29
    44,17
    57,34
    41,57
    59,3
    19,58
    61,16
    53,5
    55,3
    17,41
    26,65
    1,58
    49,37
    29,43
    52,15
    3,21
    56,11
    49,30
    29,64
    54,17
    51,35
    50,33
    39,22
    4,51
    8,45
    37,36
    37,35
    35,17
    57,36
    55,28
    17,34
    53,49
    39,7
    7,36
    21,41
    28,31
    32,3
    21,39
    27,58
    33,31
    49,21
    39,27
    33,51
    70,45
    55,23
    21,55
    47,58
    45,1
    65,1
    65,21
    61,23
    24,57
    69,15
    27,61
    70,13
    15,55
    66,19
    19,69
    34,19
    34,59
    60,1
    37,3
    17,60
    24,63
    65,3
    13,37
    34,63
    31,59
    45,3
    4,69
    61,3
    45,2
    45,63
    35,51
    57,5
    39,11
    69,13
    30,47
    49,29
    65,2
    8,53
    15,29
    35,65
    5,49
    12,35
    9,30
    37,65
    7,64
    33,33
    11,38
    29,34
    13,33
    43,11
    25,51
    0,55
    32,11
    58,1
    2,69
    9,52
    21,59
    51,19
    39,23
    33,34
    13,67
    25,69
    25,21
    56,9
    23,57
    55,20
    5,34
    61,1
    21,47
    21,25
    21,51
    56,17
    7,58
    15,31
    17,67
    37,58
    9,21
    22,61
    31,25
    11,51
    20,31
    43,25
    41,24
    45,7
    43,5
    27,60
    52,45
    25,1
    62,31
    2,63
    43,10
    59,12
    25,17
    11,20
    37,5
    53,22
    7,66
    47,16
    41,21
    45,24
    40,13
    5,35
    17,69
    20,49
    18,47
    54,25
    25,29
    1,59
    55,19
    53,17
    8,33
    14,49
    30,17
    4,65
    51,31
    69,17
    19,49
    42,17
    49,4
    37,21
    11,57
    57,0
    23,51
    1,39
    35,29
    35,19
    17,65
    43,3
    25,40
    41,11
    53,27
    9,49
    52,5
    53,35
    9,70
    20,53
    14,59
    3,43
    3,24
    11,48
    2,31
    9,65
    37,13
    46,1
    65,5
    65,7
    19,33
    23,33
    42,13
    23,43
    63,5
    13,43
    3,33
    53,13
    59,10
    47,4
    55,5
    52,25
    43,8
    53,31
    31,70
    0,31
    11,50
    45,11
    51,9
    19,47
    25,62
    53,30
    69,5
    9,36
    43,23
    32,31
    3,60
    1,49
    48,21
    11,42
    40,11
    41,8
    48,9
    65,8
    19,38
    38,1
    29,32
    27,56
    30,33
    67,9
    7,29
    53,7
    63,4
    22,43
    66,13
    35,35
    55,37
    28,35
    1,69
    61,17
    23,63
    27,9
    49,9
    47,12
    28,1
    19,59
    21,36
    41,26
    19,41
    41,6
    9,56
    0,43
    69,48
    3,45
    19,43
    19,45
    45,23
    65,63
    39,1
    52,37
    48,15
    27,17
    37,19
    10,57
    53,1
    64,17
    3,40
    66,11
    53,15
    0,33
    49,31
    7,43
    59,33
    27,54
    26,55
    61,29
    26,29
    28,59
    25,41
    31,9
    26,35
    59,16
    11,37
    59,47
    33,57
    13,44
    33,58
    57,6
    64,11
    4,47
    23,44
    11,23
    7,51
    3,42
    12,29
    54,1
    67,23
    51,33
    57,32
    13,66
    46,5
    45,5
    55,21
    55,25
    64,1
    51,14
    61,7
    17,51
    8,63
    29,51
    63,68
    37,61
    30,1
    13,31
    53,11
    59,5
    17,52
    25,31
    33,15
    53,33
    17,49
    17,54
    51,41
    17,59
    67,3
    52,1
    21,44
    67,8
    13,40
    2,55
    29,57
    35,7
    66,17
    60,15
    43,7
    13,51
    51,25
    49,2
    11,15
    47,9
    45,13
    32,63
    64,19
    40,1
    49,19
    39,8
    50,11
    58,11
    25,18
    23,32
    25,30
    64,15
    65,33
    54,13
    9,45
    9,42
    21,35
    41,1
    43,9
    3,26
    26,61
    43,22
    25,38
    33,68
    65,9
    53,3
    11,44
    21,66
    23,45
    3,51
    31,5
    1,43
    15,57
    50,31
    6,49
    25,35
    17,35
    49,63
    54,27
    22,69
    0,65
    48,23
    67,5
    41,23
    2,49
    13,68
    33,2
    0,67
    12,57
    8,59
    49,33
    38,7
    31,21
    57,25
    25,5
    17,63
    51,11
    37,53
    60,5
    23,67
    11,33
    31,3
    23,49
    29,7
    12,69
    7,39
    6,39
    1,67
    15,49
    5,43
    47,0
    54,5
    39,31
    69,33
    3,57
    10,39
    5,41
    25,11
    5,36
    11,35
    15,43
    55,39
    10,21
    9,59
    3,35
    63,11
    62,13
    31,17
    35,5
    27,45
    27,31
    30,53
    43,24
    63,20
    46,19
    30,27
    15,61
    28,69
    33,5
    12,19
    39,3
    33,23
    5,53
    47,32
    35,59
    15,53
    59,19
    39,63
    21,58
    31,69
    51,7
    3,65
    33,67
    51,27
    30,15
    63,1
    63,19
    8,39
    55,2
    43,41
    54,31
    25,57
    1,51
    70,21
    27,35
    11,47
    61,10
    23,28
    17,66
    17,42
    35,1
    56,13
    13,39
    1,53
    34,13
    5,61
    35,53
    17,39
    3,69
    9,51
    5,56
    41,29
    28,9
    1,50
    21,29
    17,64
    31,28
    11,67
    58,27
    41,25
    17,31
    1,41
    31,2
    27,59
    29,63
    22,53
    21,69
    31,54
    29,59
    18,65
    36,15
    37,54
    33,3
    33,22
    38,27
    23,69
    27,7
    61,19
    15,65
    5,65
    25,65
    49,36
    3,52
    23,47
    39,24
    50,27
    17,3
    35,18
    12,59
    27,14
    27,15
    2,35
    42,27
    5,27
    61,11
    53,20
    17,62
    45,17
    55,27
    31,67
    57,14
    20,65
    30,5
    4,45
    33,17
    54,45
    15,33
    51,36
    7,59
    13,29
    61,25
    25,61
    67,44
    69,16
    1,27
    15,69
    3,32
    15,48
    21,49
    30,67
    51,12
    51,16
    27,53
    6,65
    39,13
    9,67
    65,23
    6,59
    7,35
    23,61
    35,13
    21,32
    11,53
    4,37
    20,37
    26,7
    47,23
    70,7
    40,5
    15,38
    47,11
    25,42
    51,29
    39,10
    59,9
    27,55
    70,1
    23,59
    15,5
    11,41
    41,7
    51,1
    18,39
    33,25
    9,53
    32,9
    20,63
    57,35
    14,43
    21,9
    36,1
    17,27
    10,47
    32,29
    48,27
    69,43
    36,21
    54,35
    57,22
    26,33
    23,6
    37,8
    38,5
    69,45
    53,29
    19,55
    60,21
    4,53
    29,69
    7,41
    68,15
    47,3
    7,47
    69,11
    13,25
    29,5
    53,19
    27,16
    29,17
    57,15
    28,39
    28,5
    8,51
    1,38
    10,35
    31,18
    59,21
    15,64
    7,69
    19,27
    19,56
    68,19
    43,17
    33,53
    20,41
    22,63
    55,45
    25,9
    60,13
    11,21
    53,9
    59,27
    35,23
    57,24
    25,10
    7,61
    23,41
    7,53
    42,3
    29,65
    30,25
    38,21
    35,11
    1,31
    15,37
    57,18
    69,9
    45,19
    51,5
    33,11
    67,19
    33,65
    60,29
    31,14
    9,39
    62,23
    31,64
    15,45
    34,3
    15,68
    59,15
    23,36
    4,41
    27,66
    30,11
    5,33
    65,11
    1,61
    65,17
    28,17
    33,66
    47,13
    13,46
    24,41
    59,8
    35,9
    33,19
    26,69
    3,34
    15,58
    43,19
    51,10
    56,3
    35,15
    8,47
    54,7
    15,34
    55,15
    63,23
    33,8
    15,39
    23,31
    39,2
    31,51
    51,6
    67,1
    35,36
    14,27
    16,39
    58,25
    18,49
    1,33
    65,13
    31,66
    25,32
    49,6
    27,68
    31,65
    2,53
    31,53
    7,48
    19,42
    35,57
    21,38
    13,52
    1,29
    1,65
    30,19
    4,58
    22,50
    15,16
    32,34
    58,60
    54,6
    10,30
    44,67
    37,30
    2,14
    24,38
    66,52
    18,27
    20,54
    42,54
    48,54
    26,42
    61,70
    36,53
    30,46
    31,20
    31,32
    23,40
    62,56
    44,26
    12,44
    36,65
    18,43
    24,59
    12,67
    51,38
    0,41
    65,42
    10,19
    15,70
    44,25
    22,64
    24,23
    27,18
    22,54
    28,43
    15,40
    7,50
    26,22
    23,56
    40,32
    64,5
    4,35
    42,53
    10,62
    42,60
    5,26
    18,60
    7,46
    55,8
    40,26
    5,46
    20,14
    10,12
    42,22
    66,14
    65,14
    32,38
    40,58
    30,64
    27,64
    21,54
    18,68
    11,68
    68,49
    10,16
    16,52
    24,43
    12,37
    5,50
    20,22
    20,33
    23,68
    12,66
    33,62
    11,28
    22,37
    38,16
    32,40
    30,69
    64,14
    5,42
    45,36
    40,18
    6,56
    67,14
    25,56
    66,51
    62,8
    4,14
    52,50
    8,44
    42,5
    24,52
    32,2
    30,30
    15,28
    19,20
    30,29
    54,61
    20,15
    55,14
    32,18
    38,8
    37,48
    20,0
    2,68
    19,64
    8,61
    9,32
    13,42
    22,4
    12,14
    24,40
    43,28
    60,48
    3,4
    52,8
    59,66
    18,16
    11,34
    18,58
    36,56
    6,64
    28,70
    38,42
    18,64
    34,31
    11,56
    1,52
    10,54
    54,49
    22,70
    29,36
    2,45
    69,2
    59,0
    26,23
    68,47
    52,16
    38,56
    21,62
    41,40
    48,11
    42,51
    13,36
    16,27
    6,23
    15,24
    1,56
    38,59
    58,66
    22,52
    32,58
    0,49
    0,60
    2,9
    0,30
    8,34
    6,57
    42,38
    8,57
    22,67
    36,55
    56,48
    62,60
    70,30
    10,17
    22,34
    0,16
    12,31
    10,55
    16,14
    6,4
    69,50
    66,46
    6,19
    26,37
    20,10
    28,56
    24,36
    26,31
    38,60
    54,50
    34,62
    26,50
    4,64
    7,42
    6,16
    70,46
    10,29
    2,64
    30,48
    20,30
    63,42
    52,2
    23,42
    56,19
    4,31
    64,46
    5,40
    64,70
    43,6
    31,50
    0,22
    58,68
    24,14
    59,70
    35,62
    18,3
    68,44
    60,44
    21,40
    40,51
    44,22
    70,54
    54,2
    54,24
    17,14
    26,68
    64,45
    26,70
    28,34
    46,37
    32,65
    51,68
    24,20
    42,40
    16,37
    17,22
    14,63
    43,62
    1,22
    27,40
    14,23
    36,46
    69,44
    32,30
    38,52
    16,56
    42,21
    52,7
    31,52
    4,16
    52,62
    47,8
    11,12
    0,20
    28,32
    13,48
    56,24
    36,44
    62,41
    38,67
    28,24
    28,22
    68,54
    36,59
    10,34
    19,26
    39,18
    36,68
    0,51
    18,36
    0,46
    69,70
    43,38
    8,67
    10,26
    48,4
    66,12
    55,70
    41,4
    0,66
    60,19
    36,31
    3,6
    58,19
    70,32
    64,55
    14,20
    51,20
    27,0
    66,65
    36,35
    29,68
    15,46
    4,21
    38,35
    32,50
    68,2
    56,16
    39,12
    34,60
    13,34
    33,18
    16,48
    34,64
    28,36
    24,54
    34,58
    1,30
    50,6
    18,52
    7,70
    2,23
    10,31
    67,22
    36,30
    12,10
    38,11
    7,40
    12,25
    38,36
    42,20
    44,33
    8,52
    70,36
    42,31
    25,34
    49,12
    55,54
    8,35
    64,62
    22,40
    34,57
    32,20
    70,42
    56,50
    6,60
    28,28
    14,32
    52,13
    47,14
    12,11
    20,62
    12,42
    44,28
    13,26
    35,10
    53,54
    38,54
    19,40
    8,18
    18,38
    60,8
    7,30
    52,3
    30,20
    34,69
    4,55
    27,62
    4,32
    57,50
    44,60
    22,12
    32,36
    63,32
    38,9
    4,0
    26,24
    26,16
    32,48
    50,35
    20,35
    42,70
    25,26
    6,20
    29,24
    67,42
    8,30
    20,48
    22,68
    6,9
    61,48
    15,56
    61,68
    34,54
    2,18
    41,58
    30,60
    26,27
    25,50
    12,62
    64,26
    54,54
    3,38
    36,20
    48,16
    6,21
    33,36
    40,61
    6,33
    58,43
    8,68
    20,51
    10,60
    45,54
    39,26
    64,10
    22,32
    60,64
    14,64
    50,64
    34,25
    28,57
    60,12
    7,32
    22,19
    40,24
    20,25
    70,47
    45,6
    38,12
    14,45
    53,18
    44,32
    29,52
    36,43
    40,36
    32,41
    43,36
    5,64
    1,34
    36,48
    59,30
    15,66
    48,20
    2,6
    34,56
    70,31
    50,15
    7,26
    20,46
    66,62
    54,22
    2,11
    64,51
    16,62
    32,62
    62,46
    0,62
    64,64
    14,34
    46,60
    8,58
    14,15
    16,8
    3,48
    70,49
    17,68
    26,48
    35,50
    9,14
    68,70
    17,46
    68,50
    33,64
    24,66
    21,22
    44,0
    37,38
    52,69
    70,43
    60,49
    0,27
    37,26
    33,50
    26,18
    0,3
    52,24
    70,29
    41,34
    29,18
    19,50
    60,46
    28,26
    64,63
    16,60
    2,52
    31,38
    45,56
    44,54
    42,52
    70,52
    61,42
    4,15
    2,16
    2,61
    63,18
    12,26
    36,10
    22,17
    40,23
    0,26
    12,12
    58,42
    1,68
    45,22
    64,48
    63,44
    52,12
    55,6
    69,38
    70,15
    34,45
    12,68
    14,54
    14,41
    59,60
    42,48
    62,68
    62,63
    0,28
    10,37
    25,12
    28,40
    23,64
    64,18
    35,46
    50,20
    10,42
    50,22
    62,19
    68,56
    18,56
    34,16
    2,22
    54,70
    24,51
    56,14
    31,36
    2,24
    24,30
    24,39
    35,40
    66,45
    14,18
    3,46
    31,22
    8,38
    45,70
    13,20
    4,44
    2,48
    4,24
    24,70
    64,42
    26,46
    19,46
    23,38
    16,46
    14,14
    32,51
    44,62
    22,16
    2,66
    18,62
    6,29
    5,68
    26,20
    49,16
    12,16
    35,52
    54,52
    10,23
    58,50
    0,18
    31,62
    13,60
    56,52
    2,46
    32,59
    32,55
    37,24
    21,52
    29,66
    4,43
    4,62
    64,29
    8,24
    61,54
    38,44
    31,60
    28,66
    33,60
    46,68
    10,64
    62,42
    52,57
    70,48
    47,64
    2,10
    32,33
    44,2
    49,70
    48,38
    50,9
    69,68
    69,22
    63,24
    3,70
    22,14
    64,22
    26,6
    56,54
    32,70
    20,69
    45,48
    62,15
    68,65
    3,54
    22,48
    35,38
    9,48
    48,0
    38,50
    56,21
    70,40
    30,12
    68,58
    46,2
    66,27
    53,46
    3,64
    58,47
    36,66
    50,26
    56,42
    14,60
    56,61
    66,60
    68,42
    18,10
    61,52
    24,45
    55,0
    48,24
    46,35
    58,2
    34,12
    32,6
    34,40
    41,28
    38,62
    18,8
    64,50
    68,48
    44,15
    0,34
    6,58
    5,58
    42,66
    58,22
    30,22
    30,28
    16,68
    70,26
    49,28
    20,40
    55,16
    47,20
    13,58
    54,37
    50,59
    6,37
    70,58
    24,12
    40,67
    2,2
    10,4
    55,40
    2,25
    20,6
    58,0
    22,60
    22,59
    55,44
    54,38
    34,10
    60,70
    70,59
    66,1
    33,4
    12,45
    50,32
    58,7
    10,40
    5,44
    54,4
    60,18
    28,60
    28,8
    44,47
    58,13
    11,18
    66,6
    59,24
    32,24
    62,62
    36,61
    64,47
    8,70
    32,16
    55,30
    14,57
    66,22
    39,20
    36,47
    60,14
    54,20
    64,13
    43,70
    27,50
    22,2
    38,0
    66,39
    26,0
    26,64
    66,40
    32,0
    66,0
    58,44
    62,0
    8,0
    25,2
    20,26
    45,42
    64,9
    16,28
    66,70
    42,62
    60,68
    30,66
    53,24
    43,12
    0,56
    44,20
    6,34
    42,24
    14,56
    16,6
    4,17
    42,64
    8,28
    0,63
    62,10
    53,26
    70,28
    40,48
    54,10
    8,41
    18,17
    68,34
    32,69
    8,37
    56,36
    28,33
    26,56
    37,10
    40,50
    67,58
    69,8
    55,10
    20,50
    26,59
    31,8
    64,59
    62,54
    44,64
    34,68
    56,62
    45,8
    28,44
    66,59
    58,33
    33,42
    70,57
    70,69
    30,6
    44,8
    34,38
    32,22
    0,1
    18,18
    18,50
    62,6
    38,28
    2,41
    57,40
    70,4
    38,22
    46,22
    44,10
    62,12
    61,58
    5,70
    53,64
    24,34
    47,28
    48,44
    12,60
    10,0
    16,18
    26,17
    43,18
    70,9
    37,20
    40,46
    12,32
    46,18
    6,5
    14,12
    56,68
    10,22
    10,41
    10,56
    69,4
    46,58
    44,51
    0,70
    17,20
    28,21
    0,4
    0,36
    59,28
    52,46
    34,67
    44,56
    48,48
    6,24
    24,6
    44,44
    59,22
    37,56
    8,65
    7,8
    39,56
    30,13
    26,26
    64,4
    4,2
    40,25
    64,56
    65,56
    16,64
    8,12
    20,68
    66,10
    2,56
    52,35
    54,56
    12,20
    62,37
    2,38
    12,30
    64,12
    42,11
    28,6
    34,17
    0,57
    47,66
    62,24
    48,28
    47,68
    63,6
    60,25
    5,52
    48,50
    13,50
    54,8
    50,68
    31,34
    29,46
    26,36
    7,62
    36,18
    43,14
    34,52
    62,58
    3,58
    28,18
    64,30
    70,38
    65,40
    10,25
    30,50
    62,52
    55,66
    60,57
    47,6
    12,15
    50,28
    18,30
    70,10
    6,6
    69,18
    37,50
    50,61
    61,22
    66,54
    64,68
    44,39
    27,70
    10,2
    16,41
    28,67
    38,26
    67,16
    16,4
    70,23
    2,32
    0,58
    38,64
    38,48
    45,32
    31,26
    14,48
    62,50
    2,51
    24,5
    50,42
    38,23
    16,42
    46,54
    18,33
    1,18
    58,56
    42,36
    36,34
    51,18
    34,49
    11,54
    46,44
    54,55
    44,29
    46,16
    45,0
    56,28
    0,8
    69,64
    52,26
    50,25
    4,11
    56,31
    61,4
    52,28
    22,20
    20,11
    40,63
    32,66
    26,15
    53,0
    64,38
    22,28
    66,34
    66,48
    4,50
    49,22
    38,6
    28,58
    41,60
    1,28
    0,15
    62,30
    66,4
    19,14
    65,62
    67,52
    18,5
    68,28
    6,50
    12,64
    64,39
    57,64
    4,9
    52,42
    49,0
    50,18
    50,24
    56,70
    70,37
    56,35
    46,24
    58,69
    58,14
    22,35
    70,35
    54,68
    66,3
    68,62
    4,22
    19,12
    54,65
    60,54
    """;
}
