public static class Program
{
    public static void Main()
    {
        Console.WriteLine("Starting problem 17...");

        var input = EXAMPLE_INPUT_2;
        var parsedInput = ParseInput(input);
        var code = parsedInput.Code;
        var computer = new Computer
        {
            A = parsedInput.StartA,
            B = parsedInput.StartB,
            C = parsedInput.StartC,
        };
        {
            var output = Run(computer, code)!;
            Console.WriteLine($"Part 1 answer: {string.Join(",", output)}");
        }

        var a = (int)(0.47 * 2_000_000_000);
        var milestone = 1;
        while (true)
        {
            computer.A = a;
            computer.B = parsedInput.StartB;
            computer.C = parsedInput.StartC;
            var output = Run(computer, code, code);
            if (output != null && output.SequenceEqual(code)) { break; }
            if (a / 2_000_000 >= milestone)
            {
                Console.WriteLine($"{milestone / 10d}% done");
                milestone++;
            }
            a++;
        }
        Console.WriteLine($"Part 2 answer: {a}");

        Console.WriteLine("Done");
    }

    public class Computer
    {
        public int A;
        public int B;
        public int C;
    }

    public static List<int>? Run(Computer computer, List<int> code, List<int>? expected = null)
    {
        var ip = 0;
        var output = new List<int>(32);
        while (ip < code.Count)
        {
            var operand = code[ip + 1];
            if (code[ip] == 0) // adv
            {
                computer.A = (int)(computer.A / Math.Pow(2, GetCombo(operand, computer)));
            }
            else if (code[ip] == 1) // bxl
            {
                computer.B ^= operand;
            }
            else if (code[ip] == 2) // bst
            {
                computer.B = GetCombo(operand, computer) % 8;
            }
            else if (code[ip] == 3) // jnz
            {
                if (computer.A != 0)
                {
                    ip = operand;
                    continue;
                }
            }
            else if (code[ip] == 4) // bxc
            {
                computer.B ^= computer.C;
            }
            else if (code[ip] == 5) // out
            {
                output.Add(GetCombo(operand, computer) % 8);
                if (expected != null && (output.Count > expected.Count || output.Last() != expected[output.Count - 1])) { return null; }
            }
            else if (code[ip] == 6) // bdv
            {
                computer.B = (int)(computer.A / Math.Pow(2, GetCombo(operand, computer)));
            }
            else if (code[ip] == 7) // cdv
            {
                computer.C = (int)(computer.A / Math.Pow(2, GetCombo(operand, computer)));
            }
            ip += 2;
        }
        return output;
    }

    public static int GetCombo(int codeByte, Computer computer)
    {
        if (codeByte >= 0 && codeByte < 4) { return codeByte; }
        if (codeByte == 4) { return computer.A; }
        if (codeByte == 5) { return computer.B; }
        if (codeByte == 6) { return computer.C; }
        throw new();
    }

    public class ParsedInput
    {
        public int StartA;
        public int StartB;
        public int StartC;
        public List<int> Code;
    }

    public static ParsedInput ParseInput(string source)
    {
        var lines = source.Trim().Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(line => line.Trim()).ToList();
        var registers = lines[..3].Select(line => int.Parse(line.Split(": ")[1])).ToList();
        var code = lines[4].Split(": ")[1].Split(',').Select(int.Parse).ToList();
        return new()
        {
            StartA = registers[0],
            StartB = registers[1],
            StartC = registers[2],
            Code = code,
        };
    }

    public const string EXAMPLE_INPUT = """
    Register A: 729
    Register B: 0
    Register C: 0

    Program: 0,1,5,4,3,0
    """;

    public const string EXAMPLE_INPUT_2 = """
    Register A: 2024
    Register B: 0
    Register C: 0

    Program: 0,3,5,4,3,0
    """;

    public const string INPUT = """
    Register A: 22817223
    Register B: 0
    Register C: 0

    Program: 2,4,1,2,7,5,4,5,0,3,1,7,5,5,3,0
    """;
}
