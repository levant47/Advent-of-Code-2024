public static class Program
{
    public static void Main()
    {
        Console.WriteLine("Starting problem 15...");

        var input = INPUT;
        var (originalGame, commands) = ParseInput(input);
        var game = originalGame.Copy();
        foreach (var command in commands) { Simulate(command, game); }
        Console.WriteLine($"Part 1 answer: {game.Boxes.Sum(box => box.Position.X + box.Position.Y * 100)}");

        game = originalGame.Copy();
        foreach (var box in game.Boxes)
        {
            box.Position = new(box.Position.X * 2, box.Position.Y);
            box.Size = new(2, 1);
        }
        game.Walls = game.Walls.SelectMany(wall => new[] { new Vector2(wall.X * 2, wall.Y), new Vector2(wall.X * 2 + 1, wall.Y) }).ToHashSet();
        game.Robot = new(game.Robot.X * 2, game.Robot.Y);
        game.MapSize = new(game.MapSize.X * 2, game.MapSize.Y);
        foreach (var command in commands) { Simulate(command, game); }
        Console.WriteLine($"Part 2 answer: {game.Boxes.Sum(box => box.Position.X + box.Position.Y * 100)}");

        Console.WriteLine("Done");
    }

    public record Vector2(int X, int Y)
    {
        public static Vector2 operator+(Vector2 left, Vector2 right) => new(left.X + right.X, left.Y + right.Y);
    }

    public class Game
    {
        public Vector2 MapSize;
        public HashSet<Vector2> Walls;
        public List<Box> Boxes;
        public Vector2 Robot;

        public Game Copy() => new()
        {
            MapSize = MapSize,
            Walls = Walls.ToHashSet(),
            Boxes = Boxes.Select(box => box.Copy()).ToList(),
            Robot = Robot
        };

        public string ToDebug()
        {
            var result = new System.Text.StringBuilder();
            for (var y = 0; y < MapSize.Y; y++)
            {
                for (var x = 0; x < MapSize.X; x++)
                {
                    var position = new Vector2(x, y);
                    if (Robot == position) { result.Append('@'); continue; }
                    if (Walls.Contains(position))
                    {
                        result.Append('#');
                        continue;
                    }
                    var box = GetBoxAtPosition(position, this);
                    if (box != null)
                    {
                        result.Append("[]");
                        x++;
                        continue;
                    }
                    result.Append('.');
                }
                result.Append('\n');
            }
            return result.ToString();
        }
    }

    public class Box
    {
        public Vector2 Position;
        public Vector2 Size;

        public Box Copy() => new() { Position = Position, Size = Size };
    }

    public static Box? GetBoxAtPosition(Vector2 position, Game game)
    {
        foreach (var box in game.Boxes)
        {
            for (var dx = 0; dx < box.Size.X; dx++)
            {
                for (var dy = 0; dy < box.Size.Y; dy++)
                {
                    if (new Vector2(box.Position.X + dx, box.Position.Y + dy) == position)
                    {
                        return box;
                    }
                }
            }
        }
        return null;
    }

    public enum RobotCommand
    {
        Up,
        Right,
        Down,
        Left
    }

    public static (Game, List<RobotCommand>) ParseInput(string source)
    {
        var lines = source.Trim().Split('\n').Select(line => line.Trim()).ToList();
        var game = new Game { Boxes = [], Walls = [] };
        var y = 0;
        for (; lines[y] != ""; y++)
        {
            var line = lines[y];
            for (var x = 0; x < line.Length; x++)
            {
                var c = line[x];
                if (c == '#') { game.Walls.Add(new(x, y)); }
                else if (c == 'O') { game.Boxes.Add(new() { Position = new(x, y), Size = new(1, 1) }); }
                else if (c == '@') { game.Robot = new(x, y); }
            }
        }
        game.MapSize = new(y, lines[0].Length);

        var commands = new List<RobotCommand>();
        foreach (var c in string.Concat(lines[(y + 1)..]))
        {
            if (c == '<') { commands.Add(RobotCommand.Left); }
            else if (c == '>') { commands.Add(RobotCommand.Right); }
            else if (c == '^') { commands.Add(RobotCommand.Up); }
            else if (c == 'v') { commands.Add(RobotCommand.Down); }
            else { throw new(); }
        }
        return (game, commands);
    }

    public static void Simulate(RobotCommand command, Game game)
    {
        var movement = new Vector2(0, 0);
        if (command == RobotCommand.Up) { movement = new(0, -1); }
        else if (command == RobotCommand.Right) { movement = new(1, 0); }
        else if (command == RobotCommand.Down) { movement = new(0, 1); }
        else if (command == RobotCommand.Left) { movement = new(-1, 0); }
        var boxesToMove = new HashSet<Box>();
        var moveValid = Move(game.Robot, new(1, 1), movement, game, boxesToMove);
        if (moveValid)
        {
            game.Robot += movement;
            foreach (var box in boxesToMove) { box.Position += movement; }
        }
    }

    // returns true if move is valid and false otherwise
    public static bool Move(Vector2 origin, Vector2 size, Vector2 movement, Game game, HashSet<Box> boxesToMove, Box? selfReference = null)
    {
        for (var dx = 0; dx < size.X; dx++)
        {
            for (var dy = 0; dy < size.Y; dy++)
            {
                var newPosition = origin + new Vector2(dx, dy) + movement;
                if (game.Walls.Contains(newPosition)) { return false; }
                var box = GetBoxAtPosition(newPosition, game);
                if (box != null && box != selfReference)
                {
                    var moveValid = Move(box.Position, box.Size, movement, game, boxesToMove, box);
                    if (!moveValid) { return false; }
                    boxesToMove.Add(box);
                }
            }
        }
        return true;
    }

    public const string EXAMPLE_INPUT = """
    ##########
    #..O..O.O#
    #......O.#
    #.OO..O.O#
    #..O@..O.#
    #O#..O...#
    #O..O..O.#
    #.OO.O.OO#
    #....O...#
    ##########

    <vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^
    vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v
    ><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<
    <<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^
    ^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><
    ^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^
    >^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^
    <><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>
    ^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>
    v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^
    """;

    public const string EXAMPLE_INPUT_2 = """
    ########
    #..O.O.#
    ##@.O..#
    #...O..#
    #.#.O..#
    #...O..#
    #......#
    ########

    <^^>>>vv<v>>v<<
    """;

    public const string EXAMPLE_INPUT_3 = """
    #######
    #...#.#
    #.....#
    #..OO@#
    #..O..#
    #.....#
    #######

    <vv<<^^<<^^
    """;

    public const string INPUT = """
    ##################################################
    #.O.......O.....#..OOO..O...O#.##.#.O...O...O....#
    ##....##........O#...O#OOOO.O..O.....O.OO..O#.OOO#
    #.....O..O.O.....O......O...OOO....OO...O..#.#...#
    #.O#O.OO.O..#.#......O..OO......#....O#O........O#
    #.OOO.OO...O#......O.OOO.OO....O.....O...........#
    #..#.....OO..O.OOO.......OO#....OOO#O#.O#O.O..O..#
    ##..O..O#.......O.O.O#OO...OO..OO.OO.O.#O....O...#
    #...O...O...O.O..O............O.O#..O.O..OOO.....#
    #......O.O#...OOO......O....O#.........#OO.O.#O..#
    #.O....O....OOO#.#O#O#.........#O..OOO.O....OO.#O#
    #..O.OO.#OO.O#O....O.O.O..O.OOOO.OO..O#O.#OO.OO.O#
    #.O.....O.OO.OO...#.O...O..O..O...OO...O.O....O..#
    #.#OOO...O......O.......O....O#...O..OO.O..O..O..#
    ##..O....#.##OOO......#OO..O.O..OO#...O..OO##O#.##
    #O.OO.#...O#O..O.#.OO..O..###..#.O....OO......O..#
    #.O.O...OO..O.O#O.OO..O...O..#O.....O..#.O.......#
    #.O...#.O..O.O.O..OO.#.O.OO......#.OOO..O..#...#.#
    #..OOO.O..O..OOOO.O.#.....O#...#O......O..O..O..##
    #..O.....O#..OOO.O.......#..#.O.#..OOO..O........#
    ###O#.......#.......#O#.#OO.O...O..........O.....#
    ##...O..O.....O..OO.##....O.OO..#.O#.O..#..O.....#
    ##.O..O.#......O.O.OOO.........#..O...O.#O.O..O.O#
    #.O.O#..#.O......#.O..#O...O...O..#.O...#.O.O#..O#
    #.#O.OOO###.........OO#.@....O...O.O...#O..#OO.O.#
    ##.O.............###...O..O.#....O..O......OO....#
    #.O.O...O...O.#........OO..OOO..OOOO....O.O......#
    #..O.OO.O#.....O.OO..O.OO...OO......#O..O.O....###
    ###.O.O.......#...OO.OO......#.OO.O...O...#.....O#
    #O..O.O.......O......O...O..O.#....O...#O#......O#
    #....O.....O#O.#O...O........OOO.#.OO......OOO#..#
    #...O....O...#..OO..OO...O..O....O....#O.#......##
    #.#....O....##..O..#.#......O......O..O..O.OO#O#.#
    #..O......O.O.O#O....#O..O#OO...O................#
    #.O#..O........OO#O.O#.OO........#....O.O........#
    #O...#.......O##OO..O......#O.#OO.#O.#O#OO..OO..O#
    #O......O#.O.O.....#....OO....#........O..#...#..#
    #O#...##OO.O.O..OO.....OOO....O.O......OO.....OO.#
    ##.....O..O.O.....#O...#.#...OOO#....OO.O..OO..#.#
    #O...O...#OOO..O....#.#..O.O...#O.........OOOO.#O#
    #.#OOO..OO.O.......O.O...OO.O....O#..#..........O#
    #......O.......##..O...O#O..O...O#..O.O..#...OO..#
    #O..O..OO......OO....####......O....O..OOO....O.O#
    #O.......O..O..O...O...O.O#......O..........#...##
    #..#......O.##.O..O.O.OO..O...........O.......O..#
    #........#..#..##O.....OO.....#.#.O..O....O.O....#
    #.....#.......O.....O...#O.O....O...O.........#..#
    ##O#O........#.......OO..O...#OO.O....#.......O..#
    #...#...O..#O.O.....O...OO...OO..O....#O.O#.O..O.#
    ##################################################

    <>>^>vv^^<v^>^>^v>>>^<vvv>^><<<<v>vv^>>v><^v>><vv>v>v>>^^><>^^^^^^<<<^>v<<><>^<<v^>v>><^^<vv^>^v>^>><>^v>^>>^><^^v><v<v<^>v^<>vv>v^v^^^>>vvv<^v>v^<^v><^v>^>^vv>>>vvvv>^<v^vv^v^>^>v>v^>^v<v>^<><><<<>v>><<>>>^v>>><v<>>>^<^>^<<><v^<v>vv>v^<^v^v^<<^^^>^><v<<<^^<v>^^^^>^>v^^^><^<v>^>^>>vv>>^v>>><>v>^<>^^v^<v>^><>^^>v^^<>>><vv^>^v<<<>^<^^<^v^<^^<v>>><v>>^^<^v<^^>v<>v<^v>>>v>><<v^>^<>>>^<v>>>^^v<v<>>^<<v^vv>^>v<v><v^<v><<<<v<^v^>v^<^v>vv^<><<<^><v>>v><^^>v<<vv^^^vvv<<<>>^^<>>>^>^^<><>>vv>v^>v^^>>^>^<<^>^>v>^<>>^^<><v^v<vv^>v<v<<<^>>v^<^^<>v>><<<^>^^<^v>^v^^vvv<^>^>>v<<<^>v><vv<^^<v^><>>>^^v><^v<vv<^>>>v<<v<vv^vvv^><<^<v<<<^<vvvv>v^v>^^v><>^>vv<>>v<vv^>><v>>v^><><^v^^<<^vvv>^><<><>>>^<<^<^<<^<>>^>>v>vvv>v^>>v<^v^<<^>^>^<^<>>^^><<v<<^^<<>v<><>><><>><>v<^^v>^^v>v><v>v^^v<>v>v<<^v<v><v<<<>v><v<vv>>^><><^v>>v>vv<<>^^^^^^^v^>>>>^<^^>>^^<^^>>^>v^<>^vv^<^>>^>v><^^<>vv<><v^v<>^^^^^>^>>v^>>>>^vv>>^>^<<<<>^><v>v^^vv<<<<^vvv^<^^>vv>v>v^v^<v>^v>^v^>>vv<>^<^v><><vv>v<><v>^v^<>v<<^>v><<>vv<>v>>>v><<^^<^v>^>
    <v^>>^>>^v^^v^>v<v>^v><v>>^^vv^>>v^v^><<^v^^<<^>^v<^>>^<^^>><<^<^^^vv^<^>vv<v<^^<^v^<^>vv>><>^^^<v<vv<^^v^^<^vv><^v^<><vv>v><^<^^>><^v>^v<<>>>v<^^v>vvvv^v^v<>^<^^v<v<<<^>>^v<>>>>v<v^<>>^v^v^^^^^><<^v^>^<v><>>>v>>v><v<>^^v<<>v<<<<vv^^<v^<^^v^<><^v<v^>v>^^<<^v>^^><^><^^^v^^<>>><v^<<>vv^v^<<>v>^vv><vv>^v>v>>>>v<<v>>^^<^>>^vv>v<<<><><<^<<v<<<<v<>^^<v<<^^^^^v>v<^^v>>v<<v^v>>^v<<vv><><<>v<<><^<v^><vv^<>v<><v<vvv<>^v>^^^>v>^<>^^>vv^>^v>v<>^vvv><>^><<<<^^<^^^<>^>^^^^<>vv>>^<^>^vv^^^v<<^<^<^vv<vv^^^^^<^<^>^>^v^^^<^>v><>>>>^v<>v^v<<<<>v>^^<^v<v><^<>v<^<<>>^>^<<vvvv<<^v<^v>^v^<<^><<>vv>><^<^^vv<^>>^^><<<>v^^^vvv^<>^<^^^>v>^><v^>><<>>><v>v<^^^<<vvvvvv^vv<>><v><vv^>>>>^v<v><v<>v>^><<<<^>><v>^v^>v<>^><v><^^<>>><^^<<<<vv<^>>>^v<v^^v^v<v<><<v^<<v^<v^<^<^<vv>^v^v>><v>><v^vv<<v^^><><<v^v<^<v><<>><^<<><v<v>v<v<v^>v^^^>^^^^v>^>><^^<vv^<>v>vv^^<v^v<v<vv>><>^<^>v^v^^>^<^>><vvvv><<v^^^^<v<<v^^^^v<>>v<^<^>><<<<vv>^^>>vv^<>^>^><<v><<<^<<v^>^><<<<>><><v<^v^<^^<v<v<>vv><^<^^v><v<^v>^^^v>^>^<<^<vv>>><v^vv<^>>^^<<
    <v<v>><v^vv^v<>^^v<^^<<v^^v<>>vv>^>^^>>v<v^<v<^v<v^v>>>^^<>^>^<<v^^<>vv<vv><^<<^vv>v^v^^<><vvv<^^v>^v<<><>^>>^>^>^v><<v<vv<v><<vv>^^<^<v^v^<vv^vv<v<^^^^<v<<^<<>>>><<<v<vv<<^<v^>v<>^vv>v><<<>vv^>^v<<><>>^>vvv<^v>vv^>><v>^^><^^v<>v<^<v<^vv^<^vv>>><v^v^>^vvvvv^v^^<^><^vv><>v>v<<v<^v<vvvvvv^vvv>>vvv^^<v>><<<><v>^^v<<>>vv<^>v>>vv>>><<>^<<<<^><>v><<>v><>v^>^^><<>><vvv<v>vv^><<>^^>^<^<v>^>v<^<<>v<>vvv<<<<>^vv><>^^^>v><v>^v^<^<^>^><v^<v<^>^>><v<v<v^<v<v><<v>vv^<vv>^<^<<><<vv>vv<<<<vv<v<^^><^vv^^^vv^^>^^>^<>^vv^<<v>v<^><v<v^vv^<^<v>v><<<><^>^<><<^<<<v<><^^^^vv>v<v<>vv^^>><>><vv^^v>v<vv<^<>v>v<v<^>^v>>^<<>^<>v><^v^>v>>>^v<v>vv>^^^<^^<^>^<<<^^>v><v<v>>><<<<^<vv<v<>v><v^^v<^vv>^v<><^v<<<^>^vv^^>^^^>vvv^>>^<^^>v<<<^<<^^<<^>>>^<><><<<<><^^^<<<>>>^>v^>>v^<<<v>v>>^><>>>^^><<<<v^>vvv<>v<v^><<<<<><vv^v<v^v^^>>>>v^v^><^><<^<>v><<>>><^<vvvv^<^^v^>><v^v><^<<<>vv^<^^>>>^><^><<<v<^vvv^vv<>v><>><vv>^<^>^<^^^v<>^<v>^<>v<>>^<^^v>vv^vv^^>^><><^v>^<^^><v^<^><v<^<>><^v>^<>^v<<<<<^<<<>^<^<v^<^^v<<vv<v<<<>^v^v>^vv^^
    v^v<>><<^v><<<>^<^^><<^>>v^v<^v^><<<v>^vv<v>vv>^<v<<v<^>>>^^v^v^<<v<>v>vvv>>^^^<>^<v^vv^vv^vv>>>>v>>v^^v>vvv>^v^^^>^>>^<<<>^<<^<v^>v<><v<^><>^v<^<v^v><v^<^^><^vv^<^v^<^><><^<><>v<^<>>v^><<v<>^vv<>^>>^^<><<v^^<>^<v<v<^>><^vv<<<v<v>^<<>v<^^>>vv<v^^^vv^^><>>^>v<^v><^^^<^vv^vv<<<^<<><^v<^vv^>>>>vv>^vv>>^>^^<^v^v<><v^>v>vv^v><^v>>v^^<^<>>v<<<vv<v^<<>v>><>>^v^<v<><^v^<><>>><v>>v>>vvv<>vv>^>^^>v<<vv><^v<<><v<^>^<v>>^<v<>>>^^<^v<^<<<^<v>v^vv<^<>v<v<^>v>^><^^v^>^^v<>v^><v<<>^^vvv<^^^^v^^v^>vv^><<^^vvvv^^^v<^>>><<v<<v>v><<<^^vvvv<>>vv^<^>v>^vvvv^v^<<^>v<^<^<v^>v^^vv^^<vvv^><v>><v<v^v><vvvvv<>^vv><^vv>>>v>vv>>>vv^^^>><<v>>v>><>v<><vvv><>^^v<<v>v^><^^^^v<<v<vv>><<^<^^v^<<^>^v^>^<><<v^v^^v>^><v^^v>v><^v^<<><>v<<v>v>^<vv<>>>v<^><^v<^^>^v>><vv>>v<<v><^>>^<^<^>>vvvv^<>vv^v^v>vvvv>v^>^vv^^<<><v^>vvv<>>vvv^vv^<^v<^>v<vv>^<<v><^vv^<<<^v<>v<^v<><<vv^<v>v^<><<^^^v<<<><v^<><v<>^^v>v^vv>v><v>>vv>>^v><^<v<v^<v<^><v^<>>>^>^>vv<v>v^vv^>vvvv>v^<>>>^>^vv>v^<><^>>>^^>><<<v<^^<<>>^>><^v>^<<>v^v^^>><^^v^<^<^<v^v<>v<
    ><^>^v>>^^v><>^>vvv<v^^^vv<<v^^^>v^v><v<v>^^vvv>>>v<>><v>^^<^^^vv>><>v>v^<v^vv^>^v>^v><vvvvvvv<<<<^v<^^^v^>^vvv^^>>v<^<vv^<<>^<v>>^<>^<>v^><><v><>vv>^v^^^<^>>>v<^<^<<<v<>>>^v<v^>v<v^^><><><v><^>>^><^v<^<v><>^><>^v<v<v^v<v^^>v^>^<^<>><v<v<^<^>vv<v^^^^<<>vv<^v^v<v>>^v<>><^<<^^^^<v^>^v>><^vv^>^<^^vv>^>^<v>>^<^><><v>v<vv^<^<^v>>^>><^<>><>^<^<^^>^<v^>^<<^v<>v<<<>^><^^>v>><^^<^^<v^vv<v<^<<^^<<<v^^<v>^^v<<^v<<<>^<>>v><v>^<v<>^>^vvv>><>^>v><^<v<><^v^v^^vv>>^<<^<^v<>^vvv>v<^vv<^^v<^v><>><<>>^>v<>v>>v^<><<>^<<^v<>^vv<><>^vvv>v<v<^>^>^vv>>^<>v><^^^^<>v<<vv<<^v<v<>>>v^><>><^<<>v<^v>^v<<^v>v^><>^>^vv>vv>>^v<<v^^^>^<<>>>>>^>vv><^v^<vvv><>v^>><v^^v^^<>v^^<<^^v^>>v^^>v^v<<<>^>><v>v><<v<^><>>v>^^vv^>>v^>v^vvv^^vv<><>>v^^^<<vv^vv^<<>vv<>>>>^v^v^v>>vv<<<>^>v<<>><><<vvv<^<>v>v^<v^^^^vvvvv>><^^>>><^>^vv><^<<vvv^<><^<><>v<vv>>>><>vv<<^>v><<v>^<>^^v>v>v><^^><v^^<>v<>^>^<>vv>>v<>v>^v>>>v<><>^<^v^^<>^>v>v^^>v<<>vv>^v>vv>^<>v>^v<>^><>v<>>><vv<^v<>v><><v<^^^>^^^^v<>vvv^<vv>>^^^>^<vvv>>v<<<<^^>vvvvvvv<v<<>^^<>>v<
    ><v<>><v>^v^^>><<>v<v>^<<vv<<vv<^>^<v^>v>v<vvv>^vv<<^><^>^v<v>vv><^>^>v^<^v^^>>v<<^<<v>>v>>v><><<^<^v<^^v<<<v^>>^<>^^^>>v<v^<^<^^^vvv>v><<^<v>^^>>>>>^><^^>v^<v^>v^^^^^>>>><vv^>v^<><^vv<^^^^^>>^<^^^<<^<v<^vv^<><>^><v>^v<v^vvvv<v<vv<v<v<>v>^^<^^>v<<>v><<>^<>>>>^v>vv^<^v^>v^^v^^v<^^<^^>>v<<<<^^^<v<^vv<^v<v<^>><<>^v^<^<<v>^><^^^v>>>><^<<v^^><^^><^v<^<>^v^<>>v<<v>><<^v<>^<^v^v^<^<><^<>>^>><>^><vv>v<>>>v^v>^>^><vvvvvvv><vv^v^<^>>^>vv^vv<v<<<>>^<^v>>>v<><v<v<>>v>vv<><><><v>^<<<^vv<vv><vv<^v<^>>>vv^v<vv<<<>><v>^><<^vv^^<>vv>^>vv<^v^v>^>^^<<<>^>v>>>^^^v><>^<>^>><vv^<<>^>^v<^>>><>>><^^^vvvv><vv<>>^v<<<^<vv<<^<<^v>^>><<^<v>^v>><><>^<<>v<><>>><vv<^v<v>>vvv><>^<<<^<^v<^^^>v>v>^v>>^<<vv<>>^>v^>>>v>>>^<<<^<v<^vv^>>^^v<^^^^<<>vv<<<v<<v^<>>>^v>><>v>>v^^^<vv>>><<<<<><v><>vv<><v>>v<^vv<>v<vv^v^vv^>>>v>v<<^<^>v<^^<^^vv><^^<<<v>^>^v<vv^>^^<<v^v^^>^^<^^v<^<^^<^>>^vvv>^^v<v>><>^>>^<>^>><<v>vv^<>vv<vv><v^<^vv^<>>v^<^v^<^<v<>>^<v>^v>v>>^>vv^v<<><v<v^<>>>>^^<v><><v>>v<>>>><><>^v^>>>v^<<^<^^vv><><<<^v^<<v>v><vv^
    vv><^v<^<v>vv<v<v<v<v>>^>^^v^<>vv^vvv<v^<<<><^<<^<>><<v<^>>><vv^^>v^<<^>vvvv<<^vv<^^>v>>>^^^>v<>^v<vv<>v<^^vv^^<>^v<>><vv>>vv<>v^<><vv<^^>v^^v><v<^vv>><v^^>>v<<>vv^<^vv^<v>v>vv<>^v<v><<^v^>^^>>v^v<v>>^^>^^>v^^vvv>>^<<vv>v<v><v<^>>^v^<>v<>^>^<<>^<vv^>^^v<<><<^<^<>v<><^^^<v<<<<vv^<>>^^<v<v>^<v<>vv<><v^<^>^>v<v<<v><v^^>v<^>^>>^>^<^^>v^v^v^>^<v^>v<>><<^^>>v>^v^<v<v^<>>>^vvvv^^>vv^v>v<v<<<^<>v^^>^vv<^v^v^><^>>>>>>v^v><>^>><v^^^<v^>>^>>>v^><<vv><<>>v>v<^>>v^<>^v>v^<vv^<v^^<>>v^v<^vv<^v<^v>>^v^<>v<v<<<>^<vv>^<<<<^^>^v^v>vvv^^>v>^>^<><><>^^^v^>v><<<>vv>v^v<v^v<<>v>vv<>>v<v><v<v^^><>>>>>>^>>>v<^>^v><<^vv<v<<^^v>v^v<^<<<^>><^v^v>>v><vv<>>v>>v>v>>^^^>^<^<^>^><<>^v<>^>vv^v^><v^<<v<v<<vv^>>vv><>><^vv>^^v>^v>^<^^><v^^<<<^<^>>^v^<v>v^<><vv<v<<vv>>>^<<^<<>><^<vv<><^>>v<>vv^v<^^<^^>>><^><<v^>v<>>^<^<^<^>>>^v^^<><<vv>v<>vv<><^v^v>><^^>vv^<^^^>^v^<^<vv<vv><v<>^>vv>^^<^>^<v>vv<^>v<>v<v^<^<vv<v<<<>^<<v^v<<<^>>^^<<v>>vvvv^^v<<>><v^^<vvvv>v<v<^>>>^><<^>><>v<^>>^v^<v<<v^^<^>>>^^v<v^>v^^v<v^vvv<vv>^<v><>>^<<><
    >^v<^<v^v>><>v^><^<v^><<v<><<><>v<^<<<<><vv>^^><><^<<v<>^<^>><^^<^^^v^<>><^^vvv>v>>>v^^<<^<<>^^^vvvv^^<v<v^v>>><^v<>v<^v><^<v^v<<>vv<>vv<<>^>v>v<>^vv<<<<<v<<<>^^>v<>>^<^>^<v<>vvv>>^>vv<v^v>^^vvv<v<^^>>>><>vvv><>>^><vvv>v><>^v>>^>v^v>^>>^v<><^^><v>^><>^<<<<^v>v^>^<^v>>^^^^>><><<>><^<^<><<^>>^^<>^v<>^><^^<<^>><v^vv<^<v^>^v<>>v^<^^v<>^><^v<vvv><>v>^<<<^<<<^^<v<vv><^v>v>^v>>>^v>><<>v>^>^<<^<<v^>v^><^<^v<^v<><<>>vv>^^>v>v<><<^v<>v<^^<v>v^vv<<vv<<<<v^<^>vvv<^v>^<>v^v>vv>vv^><<vv<<<^vv^v><><v<v><>^v>v^v>vv>>vv<<vv>^>^^^v>^vvv^v^v<<<v^^^v>>^<><><>v<v<<>^v><v>><>^>^<<v^v<v>>v<v>v^>v>>v^>^v^>><^vvv>><^v^<><<vvv^<^<>^<><><>v>v^^<vv^^^>^<<<v>^>^<<vv>>>^<<^^<>^v>^^vv><>^^<v>><>>><^v>>^><v>v<<^v^<^^<>^<v<v>vv^<v>^v^>v>><^^^vv><<>><>>>>><<><^^>v<<>>v<v<vvv^<>v>^>^vv^<^v<<vv^<<<^^>>><<^<>^><^vv>^>v>v>^v>>v^<>>v>^^v<v>^v>><>v>v<^><^>^v><>^>>v^>v^v<^^v^>>^<v>v^<<^v<v<>^v<<vvv<<v<v^><vv^<><<v><<<vv<v>>^>>^^>^<^<><>>^^v>>v>>v>^v^vv<><^>>^>>^^^>><v^v^<<>>>v^vv<<v^<v<>>><<<>v><^vv><v^<>^v<>v<vv<>>^>v^<>>^v<
    ^<vv<vv^^^v>>>v<v^>v^v<>>^>^>^>^^<<^^>v>vv<^<^>v>>^^<^^^^v<v^^<^v^^^v^>>^<<^<^vv>^vv<v^<vv^^><^^><vv^v>>>v^><<>^^v^>^>v>^^><<v>^<v>>^^^<<^vv>>^v>>v^v>>><^><v<^<^>>>^^^<^<v<v>^<v>^^>vvv^v>^v><vvv<>^<<^<v>vv>><^^^^>vv>><^v><<^^<^^><>><<>><><<^^>^^>^<^v^vv>^^v^^<<^^^>v>>v>>^vv<^<v>v<vv<<^>^^<^^<v^>>^^<>v^<^^>v>^<^><^v<>vv^>>^>>^v>>>^>^v>v>v<^>^><vv^<^>^<^v^v>vvv<^<>v<<><v^^vv^^<^<<vv^<^^^^>^>^^<><<>^v>vv>^^v^^^<>^^v<v<<>>v>^^>v<^vvvvv>v>vv^^>>^<v<<>v^><^^>>^>>^v^<<^^^vv>>><>v<><>^>^^<v<^>^<<^v^>>v<^^>v>^^^^><^^v><v><vv^<^<<v<>>vv<>vv^^<v>^>>^>^<<v<<^>v<v^v^^^<>>vv^>><>^v><>v>v<<v>^>v<<vv>>^<^<<^>v<>^^>^vvv>>vv^v<v>^^>^^><^><><<v<v>^^<<v<><<>v>^^vvvvv^v^<v>>v>>>>><><^^>>v>vv<v^<<>>>vv<^<v>^<>^vv^vv>^v<>^>^v<^>^vv>^><<>>^>v<^>><^v<^^^^^<><v>v><v<>^v>v>^^<^>v>^v>vv^>>v<vv><>^<vv>vv<v><<v>^v^>vv>v^<>v<v<v>>^<^vv<>^><>>vv>^<>vv^^>v><vv>^<><>^v^<<><<vv><vvvv^v<><>v^^<^vvv^^<^>^<^>^v^v><v<vv>vv^^^<^^v^^v>v^<^<v><>^<^<^^>v>^v>>v>>^v^^>v>v>^<v>^<^<>v<<<<<v^<^>^^vv^^vv><><>>>>><<^v>>v<<v<<vv^<v><vv
    <>v>>>vvv<v^<<>>^^^^v<^<<v<v^>^>^v>^>^^v><^><<^^<><<^v^v^vv<<<v^<vv><<^^^^>>^>^<vv^^v>v<<^v<vv>^^v<<^vv>><<>>v<<<>v^>vv>v>><^>v<^<>>v>v^^v<<vv^v>>^<^v^^><>>>>><^<^v^^<v>>v<>v<>^v^>^^><^v>^>v>><^>>^<>vv^^^>^><^<^v<^<^<v^v^v^>><^<^<v^<^^^>^>>^>^<>^>><>^>^>>>>vv><>v^>^^>><v<>>>^>v<>v^<vvv>^<>v>vv^^>vvv<^>^v>^^><v^>vv<<^<<<<v<>^^^v<v>vv<>v>^>v<<>^^^^<>^v^v>v>^>^><>vv^>^^<<>>vv<vv^^><^<<<v>^v^>^<>^<><>>^<v<vv>^v^<<^v>>^>>^><v^v<vvv><^<vv<v>^<^vvv><^vv>^v^<v>>v>^^<v<^<<<^v<>v^^<>^>>>v^^^>vv^v>>v^>^>^<^v><<><v<>v>>^<v^<^<>^v^^v<<v>>^vv<<<^<>v<^>v<^<^vv<>>v>^^^<>>^v><<v^^^>^v^>><>>><<<<>vv><v><>vv^>^>^vv<<<v>v>>vv^><>^>v^v<^><><><>v<<v<>>^v<>vv^>v^<^>^>vvv^<<<v^<><^^>>>>>v>^<>v>^<<^>>>v^v>v<^v^>v<>>vv<<^>v^<^><vv^^>v^v<^<<v>><v<v^<^^<v^>>vv<<^v<vv>^<><v^v^v>v>v<v^v<v^<v^^>><>^^<vvv^<^v>>^^v^><^>><<^^>><^<v^>^v<^>^<^<^v^<v^^<vv>^vvv<^>^^v<<>v<v^<><^^^<><>>v^^v^<<><^>>v>v<<<>^<^>v^^<>^<vv^<<^<<^<^^<<>v<^^>>v^v^v^>v<v<><<>>>><v^v<^v<^<<<vv<<>v<v><>vv><v><vvv^v^^><<^v<<><^vvv^v^^><>>^><>^vv>>^<>><
    v><<<v>v<<<vv><^^<^^^^vv<<v^>^<^^>>v>^>v^^<><v>v<v><v<<v^vv<v>vvv>>^<<<<>v<<><>^>^v><^^><^>vv>>^^^<<>>^>><^^>vvv><<>v>v>>^v<^^><<v>v^vv^v^<^>^>^<v^<vvv^v<^v^>^v<^^^<^>v>^><>v<<>^^>><^<v><^>>>>v<v>vvvvv^^v>^>v^^<^<v>^^^^^vv<<<v^^^vv^<>^<^<^v<vv>^>vvvv><v<<vv<v>v><<v>>^><><^vv>vv^v<<<<v<<<>v>v<>^^<^<>v^v<^>v^>v^<>^^<<^<<v>v^<^><<<^^vv<<v<<>vv>v><>>><^<<<^v>v^>>^^v<^^^>>>^<^^^^<^^vvv^<^>v>^<><>v><<vv<^^>^>^<^<^v>^v>>><>^<<^<><^^<vv<^<><^^<^<v^<<<^^^v<^<v<><^^<^<>v<>>>^^^v^^<^^><vvvv^^v<<<^><^^v<<<vv<v^<<^<v^<^<<^v>v><>>v>v>^<v^^^v<^^<^^<<<>v<v>><^<><<vv>^<^>^v<v^vv>><>^v^^^^v^<><v^>^<>^<^>vv><>v^^><v>>v<<<>vv<<<vv<><^^>>v^>v^<<<<<<><^><<^^^<>><>^<vv><><^v^vv><><v<><<><^>v<<^>v^v>v^vv<v^v>^>^>><^^^v^v^>^<><^^>v>vv<<<vvv^v>>vv<<><>><>>>>^<v>>>><>>vvv<v<<>v><^<^<v<<vv>>v<^v^v>v<vvv^^^>>^v^^>>^^vv<>^vv^<vv>>v<>^>>^^^<v<<vvv<<v^v^>^^^>v>>>^vvv>vv<^<^^^>>vv^>v^>>>^<<<^v<><>^>v^^v>>><vv>>><v^<>^v>>^>^<^v^^>vvv^^vvvvv<^>>^^<vv<>^<vvv<^>^>vv<v^>^>^^<^v<vv<<><>vv<<^^vvvvv^^<^<>v>vv<>vv>>v<vvvv<vvvv
    >^><^>vvv>>^<^<<vv>^v<^^<<v^^<^<<<^v>^^^<^vv><><v<^>^^^><v^^>v^^v<^^^v<<^<^^vv>>^>><^>^^^><vv^vv<>^^>vv>v<^v>^<>><>^>>v<^^>v^v<>v>^vv>>>v<<vv>v^<v>v>>><^^><<<><^<>^>^^v<^>><>^>v><>>>^v>vvv><>>>v>>v>>^<^^v><^<v^>^^^>v>>v<>v<v<<<>vv>v^^v><>^>vv^<>vv^^<<v>^<^<>^<>^v<v<<v^^v<<v^^><^^<v><>>^^^>>>vv><^>^vvv><>>>^^^^^v^v<>>vv>>v^^>^v<^<<<<><><<>v>>>vvv<>^^<^v>^<v>^<<^^<<><>vv^^<v^v^<v<v<v>^vv<^><<><>>vv^v><^<^v<v^>v<^><<><<<<<^<>>^<><><^^v>><<^<>>>>^v^<>><><^<<vv>^vv<><<^>v^^v^>^v^vvv^<<^^v<vvv>><vvv^^>>v^<>^v>^^<^v>>><v<<^<^^vv^^v<<<vv<>^v<^<>^<>^<^v><^v^^^>>><<>><<><v>>>>>v^><<>>>>v>vvvvv>>vv^^><^v>v<v^^v>v^><v>v>v<vv><<<>>>^>><^^<^vv<>>>vvv>v^<>>^^>v^^v<v>^>v<>^v>v>><>>v<^^<<^v<v^<>><><<^^^v<>>v>v^vv><>^>><v^v<<vvv>v^<vv<^v^<v>>vv<^>v^<<v><^^>><>^v^v>^vv<><v><v>>vv^<v><v><<vvv^^>><v<vv><<^<v<<^v<^<v<>>>>^^^^^<<^<v>^^<<^^<^v<^><>v^<>>>^vv>><<v><>v^<^v^v<<^<<<><<<v^>v^v^v^^<v>vv<<>v>v<>vv^<><v^<v>><vv<^<<<>^^<<vv<v<>^>v>v^vv>v<vvvv^^><<v<v^<<^>v<<>v^v<<<<^<<^v<v<>><vv<><^v<^^<v>^<<><v>v<<^^v
    ^v^>v><v>^^v^v^>^^^>^^^v^vv^^>v>>v<<v>>^v<>>v^<^v^v<v<<v^v>^<v>^^<<<<v^^>v><<^<<<v^<^^><>><v^><><v^v^vv><><v<^vvv>><^<<<>>^>><v^v^v>^><>vvv<<^v<>^>vvv^v<>^><>>^^><<^<vv^<>^v^^^<<vv>^>^<<>>v^^>><^^^>>^^<^^<>>>v>v^vv>v<^^^v>>^^v>><>>^vv<><^>>^<vv><<^^v>>v^>vv>v><>vv>>v<v<<^vv<^>^v^^<>v><><vv^vvv><>>^<v<>^^<^>>vv<^<<<<<vvvv>^<><<^<>^v<vv>v<<^^<><<^<^v^>>v><>v<^<<v><^>>>^^v><^v^^vv^<^><^v<><^>^v<^<>^<^v<>^v<><<^>^^<>>>>v^v^><>>^<><^v><v<v^<^^><<<>>^<v<v>v^>^<v><<<<v>><v^^^v<^>v^<vv<<>^>><^<^>>^><<v^>^<<>vv<v<<^v<v>v>^^>v^^<><^v>v>^>v<<^^v<^><>>^>^<^v<vv<>vv>>^<<<>>v<>v^^<><>vvvv><^<>^v<>v><>vv^>^v^<^vv^<^v^<v><^v^<v^v><vv>>>v<>v^vvv^>>^<v>>v<<v<>vv>>>>>>^<^^v<><<><<<<^<<v^v><^>v^<vv<vv^>v^<^><<>^>v<>^<^<<<v>>vv>>>v<v<v>^<>^v<>v^v<><v>>vvv>^>^^^^>^><<<><^vv^vv>^<><<v^<<<<<v^<^v>^^^<vv>^vv^>vv<>^<^>vv^>><^><^<>vv^v^^vv<^v^v><^>v<<^v<v<^<v>><vv^^<<><^^^>>^<^^<^^<<v^v<><<v<><vv<<v>>^<^<<v><>^^>>vvv<v^v<>>^<^<^<>>^>^>^^>^v><^^<<<v^<vv<^<v^<<v><>>v>^>v<v>><<^^^^vvv<>>>^v<^<v<vv^<^<<><v<^^<^v<v>^
    v>^><><<><^v<^<v<><<><^v>><<<<<^^^>vv^^^v^<v^<^v>^^>v<>vv<<^>^<<>^vv^v<v<^^>>^^<v>^<v>^v<v>>><vv><v>v<>v<<^^v^^v<<^v<vv>^>^<>>>^<v><^^>>^<^v^v^^<^<^v>>^<<v>^^^<^<>v^>^<v<<>^^>^<v^>v>^<><>v^<v>v<<>^v<v<v><^v>^>^^^>v>>^<<^^vv>^<v>>>>^>v^><<^vv<^<vv^^<^<<<^^v^vvv^v>^>^<v^vv^^>^<>>v^^v<^^v<<^v>>vvvvv^vvv^v^v>^v<>^<v><<^^<<v^v<v><<<^vv<^^<^vv^v<<^><<^>^>>><v>^vvv>^^v>v^<^^vv<^>>^v<<^>vvv<<>^v^<<<><v<v^^<<>^v>vvv^v^<<<^v^vv^^><>v>^>^>vvvv>><<^v<<><>v^<^<>>v^^v>^^^^>v>>>v><vv<><^>v<<v<>v^><v^><>^v<vv<>v^<v^>vv<<><v<^v^<^vv^^<><^^<^<^<<>^>^^v>>v>vvv^^^v><v>><v<^<v^<v^>><v<<vv^v<>v^^v^^v>vv^<>v<<v^>>^<vv>>v^vv<<v<<><^><v^v<<<^v^v><^><>^vv>>vv>^><^vv^>^^>^<<>^v<^><>>^^<>^<<^v<^^^>v<<><<^>^<^>v<^>v<>>v>vvvv<^>^<v<>><<>v<^^<<>^^^<<vv^<>v^>v^v>v><>^<>v<<^^v><>^<>vv>^^<v<>>^v<^>>>^^^><^>^^>>vvv<>^v>^v<vv<v<>^>^>v>^^>>v<>>v^^^<^<v<><^v^<>^v^>v^<<>^^<^>^<>^^>^<^^>vv^^<<<v>><<<>^>^^v<>vv>v^v^<<<^>vv><v^^v<^^v^^<<<>v<<v^^^>>><^v<<^>>>>^<v><<<v>^vv<^<>>^^vv<>^^>v>^>>>>><^v>>>vv>v^^vv>>^^^<<<>vvv<^<<^v>^^
    >vv<><>v^><v^<^>^vv<^<>^><>>vv^<<v^<<^v>^^^v<<v<^v>><^>v^^>v^<>^><^^v<><v<>>^v^<^^<^<<<v>v<>^<>v<<^^<^>>>vv>vvv>>v>^<^>>vv>>^>^vv^>>>>><<v>v^><v<<>>v^<>^^>>v<<vvvv<v><<><v^^^^^<>>^<v<<<><>>>vv<<^><v<<v^<<^vv>^<^>vv^v>>v<vvvv><<vv<<>^v>v<<<>^^^>v^v^^vvvv<>v>^v>><^><<<vvv>v<>^^v^><v^>^><v>vv>v><>><>>v>^vv^v^^>><<^><>>^>v>v^^^>>>^vv<v^^><v<^^<<v^<>v^<<<^<^<^<<^>v><v<v^<<v<>^v^v^>><^v><^^><<v^<^^><>v>v>>>^^>><>v^^v<<vv>vv^<>>^v^vv<<>^<<v<>>>^<^<<^<^>^^<<^v<^v<>v>^^^^<^v>^>^v^^<^^<<<^<<<>><>^<vv^>^^<<<^>v>v^v>v<v^v<^^v<>><><vv<^v^vv>^<v>^>^v<<>^^<^>^<>>vv^^^>>^<v<v^<><<<^><>>v^^v^<^v<><<><v>v<><v<<^<>^^^>^<>v>>^v^vv<<v^vvv>^^><v^<v^^^<v><>^^>^v>vv<v^^^>><^>^vv>v^>^>^><><<v^^v<^^v>v<<^>>v^>^>^v<>v^><vvvv^<>^<<^vv^vv<v<<>^v>v><^v^>>^^<<v^v^^v<<>><>>v^^><>v>v<v><^>vv><>v^>^><>>>v>^v^^^vv><v<^><<v^v<v>>>^>vvvvvv^^^v>>vvv<>^>v^<^^<><vv><vv^<^v<<^>^>><>><>^>^v^v^>>^vvvvv><v>>>>^v^>>v<<<><>^>^<v<v><^v<^<>vv<>v<v^vv^<v<^v<><vv>><<v>v<<>>>v<^>>>^^v>>><><v>>v^v^v><>>>v<>^^>^<<<^<v><^><>^^^>v^<>^^v>>^
    <vvv^>^v>v^<^><>^^<<<<v>^<^^v<>v^^v>^><<v^^^<^><v^^^>>^^v>^>>vv<>v>^<>^^^vv<v>>><vv^<^v>>v>>^><>v^<^^v>v<^^><<v<>v<>v>vv<vv^<<<v>^<^^^<vv<v<v>^v^>vv<^^v>vvv^<v><v<<v<><v^^>^^^>v<^^>>^>><>^^<<vv^><v^>^^<^<><<>>^vvvvv^>^<v<>>^><>vv>^>^>^><v>>v^<>>^v<^<vv>v><v>v>>>^<^^v<<>>>v^vvv^v^>^^v^^>^^^^^><^v<v><<^v>vvv<vv<<vv>vvv>v><v>>><^v><vvv<<^>v>^>>^^^v^^v><<^><>>^v^<<>^^v<v>^^v><v^<>vv^><>vv><>^v>>^^<v<^^<^v>^>^<^vv>^^<>vv<^><>^<v><>>>>v^>^^>v>vv>^vv^v<>><<><^<vvv^v<v<<<v><v^>vvv<^>v<><v^>^vvv>v<<>><v>>^><>^^^>^vv><vv>v>vv<>^<<<<<<v^<v^<v<^v><>>>^v<>^v><>>>>v^v^<<>>^v<^><>vv<<>^>>v^>>v<>^<>vv><v<^v<vv^<<<vvv^<>>><<^v<v<>>>^^<^v><<v<>^>>v>v<><<^v^vv<><^>>>^><^<<v<^^v<v^vvv<^><<v^v^<^^>v>v<>^>><<^<^vv<^>^v^v^<v><^>>v<^><<v^<^^<^^v^<>><<^<v>><v<v>v<v>>^>><v<^><v>v<v^v^>^^v^^>v<vv^>v^><^>><<>^<^v<v>>><v><v^^<^v^><<>^<<vvvv>^vvv<^<>>^^^>>^v>^><^^<<<^<^v<vv^^vv<v>vv>^>>v>vv^v^vv^<<^^^<>v^^<><<v^v>^v<v><><<v^><^><^>^<<^^<<v^><<^<>^<^>>^<>>><><vv<^^^<><^>^<v<<v^^vvv^^^>^<^<vv>>vv>v>>^^<^><>v>><>>vv^^
    ><>><vv>v^v<v^v^^^^v><<v>>>>>vvv><<v^^<vv^^^<v<>>v><<><>^<<>^><>^<^><^^v<^v^v^^^v<v>vvv>>^<v<^>v<<^<v>vvvv<><v<>>v^><>^<<>v<>>^^^v><<><v<><^>>v>>^>^>v>^<^>><<>v<^<<><vv^^v><vv^>><<^>^>^v^^<>>vv>^<><vv^>><^v^v><<<>v^>>>v^>v<^vv^>>^<>v^<<>vv>vvv<<<><^^v^<><v<vv<>^^<><vvv<^<^><vvv<>>^v>^<>>>v><>v^>v<v<^^<>^><v>v><vvv<v<<><><v<v^^>>>v>^>v>^>v<<^v><<^>vv<<<v>>>^v^>v<v>><<>>><<^v>v<<v>><^^<^>>^<v^v<>v^v>v^v<v^^><v>^v>^v><>>v>vvv^v>^^<<v>v<^v><vvvv^<<<>^v^<v<>><^v^^<^<>vv<>>vv>^^^<<<><>><vv^v><v^v^v^<^v<<>>v>^^v^>v><<^v<<>^^vv^v^v^v>v^v^^vv^>v<<^^>^>>^vv>>>^^v<<>><v><<<^v^>^^v><vv^<vv<^>>>vv^><>>vv^^>>>^>>v>^vvv>>v>v>^vv>v>^><>^<>vv<v<v^<^^>^<<>^v>^<vv^v<^^<<vv<v>><^>>^<<^>^<^v<^vv>>>v>^^vvv^v^^^^^^^>^>^v>>^v<^^^>^vv>v>v<><^^^vv<^vv^>^v<<>^vv<v^>>^<v^^v<<v>v^v<>vv<>>^><<^<>^<v>^^vv^>^v<^<><<<><>v>v<v^^^v^>^>>v>^^^^>^^>><>v>vvv^vvv<^<>>v<><^<^>><<<<<vv<<><^v<vvvv>^^<><<<>v>^>>^v><^^v<^>v>^v^<vv^^<>^<<>^vv>>><v>^v<v<^<<<vv>^<><^v<v^<<v<v^>^v>v>^>^<<^<v>>vvv^v^>>^<^><vvvv>v>v^^<v<>vv^v>^^>>^<^v^
    ^<>vv>^^>v<><>^vvv<v^v^v><^^vv^<<^^<v^<<^^<<<v>>vvv>>><^^^^^v>^v<><<v>>^>^vv<v^<v^v^^>><vv<^<v><<^<v>vv<^v^^v><vv<<^^>>>^>^><v^^v>^v^>vvvv<><<>v>><>>v^>^<^^<>^^<<v^v<^^v^^^v<<v^<<<^<v<<>^v>>>^>^v<vv^<>>v><^<v<>v<><<>^^>v<<<^<><<^>^<v<><^>^<^^^^><v>^>^<^<^<>>^^<vv>^^^^>^<^<v<vv><vvv^^^v<><^^v^v<^^>>><>>v>v><>^<v<>vv<><v^^^>v>v<vv<vv>>>v>^<v<<v><>v>^<^^^^>>^v><<v<<>><>>v^>^v^v^v<v>vv<><^v>v^><v>^^<v^<v>v^><>v>vv<v<>><>v^vv>^>^^^^>^><^<^^>>v><<<<>^v>^^<^^^>v>^v^^v>vv>v<><v>^v^v<>>>>^>^>vv<><>v>v>^v^^<^^v<vv^v<<v^>>vv<>>>^^>>^<^^^<v<v^vv^^>><<v>^<vv>><^v>^>v<<>vv>v><>v^^<<>><<v^v<v>^^vv<>vv<<>v^>^>^^^v<>v>>>^v<>>>^^v<<>^<><v>vv>^<<v>vvvv^v^<><^>^>vvv^><<v<^<><>>^<v>v^^<^^>v<v^<>^v<<^^^^<^<^<v<^v^v><^vv>^<><^<>vv>^v^^<^^>^vv^^>>^<v^<v>^>><<vv^<<<>^>^^v>v>><<^>>^^>>^>>v<^<<v>><<^v>^<^^<<^^<><<<v<<vvvvv^<^>>>v<>^<vv^<^>>vv<^>><<^<^<<<>v^<^v^v>v^vvv<>^^>v<^v^^><v^>>>>v<<^^v^<<vv^<^v<<<^^<^v><vv^^v<<^><v<^<>^^v<^^<vv^vv>><<^^<v>^v^v<v^^^><v>v<><v^>^>><^^^v^>v><<vvv<v>^<v<<<v^<>v>^v>>>>>^>v^>v<>
    <^>><<vv>^^>vv><^^v>>>vv<^^<<^^><<^>><^<v><^<v^v>v^<^^^<><>^>v<^>>v^<^^^<^^<^<^><><^>vv>vv<vv<<v><>>vv>^^>^v><v>>><^v^vvv<><<v^^^>^>v>>^<>v<>^v><v<><v><^<^v^<v^>^^v>vvv<><v^<<<v^<v<v<v^>>vv<vv>v<<<^v^vv><>vv>v^<<^^v^><<<>^<>^vv^>v^><v<v>><v^^<<>v<v>v^^v<<>^>^>vvv>>>>>>^^<>><^vv<v>v^v<<^v><^^<>><>><v>><v^>>^<^^>>^v^^<<v<^>>^^><<>v><vvvvv<<<^<<>v^^^v^v^<>>vv^<>v<^v<^<>>^<<><^>v<<v^>^<><>v>^^>>>v<^v>^^vvv<>^<<<^>v<><v^v<<^>^^>^<^>^<vvv<^v<^^<^v>v^>>v^v><v>>v<^^^^^^^vv^<<^<^<^vvv^^<>>v^v>v^<>vv>v^><v>v^>vv<<^v>v^^v^^><<<>^>^vv>>v<v><<vv^v^v^^v>v<>v<vv^v>v^v^<>^vv<v<v^^>^v>>>^^^<<v><>>>>^vv>v^<>><>>^^>v>><>>>^^<>^^>^vv^^vv^v<^v^>vv<><<<v^^^v>><><^^>v<<<><>^v>^<>v^<^<<<v>><><vv^v<<v^v^^v<v<>>v<^<^^^vvv>v^v>^^<^>v^v^vv>v<<<>v<^^><><>>v^<v>>>v^v<>v><v><^<^v<^^^^v>v^>>v<^vvv<><><^^^><>><<><^<vvv<<^v><v><<>><vv<^>><^>v^>>v><v^<>>^>><>v>>^v><^<<<><>>>>^vv>><v><>>^<vvvvv>v<^>>v><^^v<^^^^^>^^>v>vv<vv^v<^<^^<>v^<^vv>>^>v^v>v<v^<^<^vv^^^><<<>v>^>v^^^v^^>vv<>vv<vv^v>^^>vv<^<v>>^<^>>vvvv<<>><<^v^>>v<>>
    >>vv>vv<^><>><v<^^^>>>^>v>v<vv>vv<^><v^<<>><><v^<^vv^v>><<<vv>^v<^^^^<^^<<^>v>><<<>><<>><v<>^<<<>v<<v^^<vvvv^^>vv<>v<^v^v>>>v<vvv^><v^v><^<^><>v>v^vv>^^>^^vv<<>^v>v^^^<^v<v^>^^^^<<^^^<><<<>>v<><vv>><>^<>v>>^v^>v<<v>^^^v<<><<>v^>>v<>^>v>^^v^^v<>^v<<v<v>vv^v<<>>v<<><>>^>^^>>^<v<><<v<>v>v><<>^>^v<^>>^<>v>>>vv>^>>^^^vv^<>^^>>><^^v<v><><^^vv<>^>^<<<vvvv^<^^<^<><<><^^<<>^>v><><v>>><>^v<<<<^<^<>>^<<>>v<>v^vvv^^^^^v<><<>v^vvv><^<>^vv^v^v>v^^^v^^v>>^<vv<<<<v^v>^<v>><<>>^>><>vv><v><>^vv<^>><<<^>^>v>vv^<>v><<v<<<vv>>>><v^>^>^v^<^>^<<^<^v><><>v<>^^>^^vvv^vv<<<<vv^v<><<^<^^^<<><<^^^^<^<^<>^vv>vv><^^^^<<^v^<>^^^v<>v>^>vvv<v><vv>v^>>>v>^>>^<<v<>^>vv<v^<<^>><<>^<><>>>^^>v><<^^v>>v^<^<^<>>^<<><<>^^^<v^^<>vvv><<<^<><v<>^^v^>vvv^v<>^<v<^>v<>^vv^<>>^<v>>>^<<^<^^<^^^<>><<<^^<^<<>vv>v<>v<^^<vv^>^<>^><v^vv<>vvv>^>^v^vv<^^^^v>^>>^v^>v>^>><^^^>>^>>v^><>v^<<<<<vv^^^<><><v<<>><>^<^<v^v^v>><<<v<v<<v<><<<>>^vv^^>v^^<<vvv^^^^^^^^>v>>>>v<<<v<>>v<v>^>><<>>><>v><v<>v><v<<><^v<v<<v^>v<v^^v<<^v<<<vvv>vvvv^^<vv<v>vvv^<vv
    """;
}
