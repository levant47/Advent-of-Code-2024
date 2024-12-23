﻿using System.Text;

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("Starting problem 04...");

        var input = INPUT();
        var table = LettersTable.Parse(input);
        var target = "XMAS";
        var result1 = 0;
        for (var y0 = 0; y0 < table.Height; y0++)
        {
            for (var x0 = 0; x0 < table.Width; x0++)
            {
                List<string> candidates = [
                    table.GetRange(x0, y0, x0 + target.Length, y0),
                    table.GetRange(x0, y0, x0 - target.Length, y0),
                    table.GetRange(x0, y0, x0, y0 + target.Length),
                    table.GetRange(x0, y0, x0, y0 - target.Length),
                    table.GetRange(x0, y0, x0 + target.Length, y0 + target.Length),
                    table.GetRange(x0, y0, x0 + target.Length, y0 - target.Length),
                    table.GetRange(x0, y0, x0 - target.Length, y0 + target.Length),
                    table.GetRange(x0, y0, x0 - target.Length, y0 - target.Length),
                ];
                result1 += candidates.Count(candidate => candidate.StartsWith(target));
            }
        }
        Console.WriteLine($"Part 1 answer: {result1}");

        var result2 = 0;
        for (var y0 = 0; y0 < table.Height; y0++)
        {
            for (var x0 = 0; x0 < table.Width; x0++)
            {
                if (table.Get(x0, y0) == 'A')
                {
                    List<string> candidates = [
                        table.GetRange(x0 - 1, y0 - 1, x0 + 2, y0 + 2),
                        table.GetRange(x0 - 1, y0 + 1, x0 + 2, y0 - 2),
                        table.GetRange(x0 + 1, y0 + 1, x0 - 2, y0 - 2),
                        table.GetRange(x0 + 1, y0 - 1, x0 - 2, y0 + 2),
                    ];
                    var count = candidates.Count(candidate => candidate == "MAS");
                    if (x0 == table.Width / 2 && y0 == table.Height / 2) { var x = 42; }
                    if (count == 2)
                    {
                        result2++;
                    }
                }
            }
        }
        Console.WriteLine($"Part 2 answer: {result2}");

        Console.WriteLine("Done");
    }

    public class LettersTable
    {
        public List<List<char>> Letters;
        public int Width;
        public int Height;

        public static LettersTable Parse(string input)
        {
            var letters = input.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(line => line.Trim().ToList()).ToList();
            return new() { Letters = letters, Width = letters[0].Count, Height = letters.Count };
        }

        public char Get(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height) { return ' '; }
            return Letters[y][x];
        }

        public string GetRange(int startX, int startY, int endX, int endY)
        {
            var iterations = Math.Max(Math.Abs(startX - endX), Math.Abs(startY - endY));
            var stepX = Math.Sign(endX - startX);
            var stepY = Math.Sign(endY - startY);
            var result = new StringBuilder();
            var x = startX;
            var y = startY;
            for (var i = 0; i < iterations; i++)
            {
                result.Append(Get(x, y));
                x += stepX;
                y += stepY;
            }
            return result.ToString();
        }
    }

    public static string EXAMPLE_INPUT() => """
    MMMSXXMASM
    MSAMXMSMSA
    AMXSXMAAMM
    MSAMASMSMX
    XMASAMXAMM
    XXAMMXXAMA
    SMSMSASXSS
    SAXAMASAAA
    MAMMMXMMMM
    MXMXAXMASX
    """;

    public static string INPUT() => """
    XMASMXMXMAXSMMSSSSXSXMASMSAXMAXAXXMASASMSXSSSSSMMXSXMASASMSMMXSASASXXXXSMSXSMSXMASXXMASMMASXMXSXMSMMMMMSXXMASASMSSSSSMMSMMSSMSMSSMXSAMMSMMMM
    XXMSAMXSXMMSASXAAAAXMMMMAMXSMSMMSSMMMASXMAAAAASASMAAAAMAXXAMMAAXSASMMSAMAAAMASAXAMMSMMMMSAAXSAXASAMMMSAAXSAMXAAAAMMAAAAAAMMAAXMASAAMAMAMXMAM
    SMMMXMASMMASMMMMMMMMMASMMMXAAMXMAAMMMAMAMSMMMMSXMASMMXMAMSASMSXMMAMAAMAMMMSMASAMASMAAAAXMAMMMMSXMASAAMXSXSAMMSMMMMMSMMSSSMSMMMMMSMAMAMASMSSS
    XAAMMMMSAMASASXSAAAXSASAMXSMAMXMSSMXMSMXMAXASXMMAXMXXSMMMMASXXASXAMMMSAMXAXMAMXMAMMSSMMSMMXAAASMMAMMXMMXXSAMMAXASXAAAAXMAASAAXAXMAXSASXSAAAS
    SSMSAAMSMMASMSASXSSMMASMMMMMSSMAAMMSMMASMSSMMAXXMXMXMMAXXMAMASAMXMXXAXXXMSSMMSAMXSAMXMSAMMSSMMSAMMSMSMAMXSAMSMMAXMXSSMXXMMSMMMMMSAMMMMMMMMMM
    MMMMMXMSXMXSXMXMAXMAMXMAAMXAMAXMASAAAMXMAMAMSMMSMAMSMSAMXMASXMAMASMMMSXMAMAAAMASAMXSAMMAAMAAMASXMAAAAXMSASAMMXSSMSAMMMSXXXMASXSAMASMSAMMASAS
    ASXSMSASXXMSAMXMSMSMMMMSASMXSAMXXMXSXMAMAMAMASXAXAXAXMASXSAXASXMMXAAAAXMSMMMMMXMMSAMAXMMMMSMMASXMMMMMMMMAXXMAMAAAMXXAASMSXSAMAMXMAMXMAMSAMAS
    SMAAAMAMMMMMASMMMMSAAMXMSAMMXMSSSMAMAXMSSSXSASXMSMSMSMMSXMSSXMMSXSSMMSSMMXSASAMXMMASMMAAMMMAMXSXSASASASMSMSXSMSMMMMSMMSAMXMASMSAMSXMSSMMXXAM
    MMMMMMMMXAAMXSAMMAXSMSXXAMSSSMAAXMASASAMXMMMASMXAMAXMAASAAXMXAXXAAXXXXXAAXMAXSAAXSAMAASMSAXXMAMXSASASAMAAAXMXXMMAXAAAMMAMMSAMXMMXAAASAAASMSM
    SASASXSSXSMSASAMMMMMMXMMAXAAAMASMMXXXAXXAMAMXMXMMXXMXMSSMASMSXMSXMAMSMSMMSMMMXSMMAMAMMAMMXSAMXSAMMMXMAMMMSMAMXXXSXSSSMSAMAMASXMXMMMMSAMMSAAA
    SASASAXAAXXMXSSMAAAXAAMSXMMSMMMMAXASXSXSASMSXMSMSMSXAXXXMMMMAAMMAXXMXAXAAXASXMAMMAMXXMASASXXMMMASXMMMMMXSAXSASMMXAAAXASXSMSXMXMASXSAXAMSMXMS
    MAMAMAMMMMAMAMXXSMSSMSMMXAAXXSASAMXXAAASAMXMXSAAXAMSMMMXSAAMASASMMMMMSAMXSMMMSASXXMXXMAMXSSMXSMMMAASMMSMXAXXMAAAMMMMMMMAMXMAMSMXSAMAMMMXMXMX
    SAMXMXMAMSSMMSSXMXXAMMASMMMXASAXMMSMMMMMXSAMMSMSMSMXMASAMSMSAMMMXAMXAXASMXMAXMAXAASXSAMXAMXMAXASMMASAAXAMSMSSSMMMXAXXXMXMSSXMAAAMAMSXSASMAMX
    SXSMSASXMAXAAAAXMXMASMMMAAAMMMXSAAMASMAMASASAXMAAAAASMSAXXMMASMMSSMMSSMMMMSSSMSMAMMAMMSMASXMXMAMAMASMMSSMAAAAASMXSMSMXSAMXAASXMXSSXXAMAXXAMX
    XSMASASXAMXMMMMXSMSAMAAXSMMSAAASMMXAMXAMMMAMMMSSSMSMSXSMMXAMAMAAAXAAAXMASAAAXAXAXXMAMAXMXMAAMSMMXMASXMAAXMSMMMAXASXAAXMXSXMXMASAXMAMAMXMSSXM
    XSMXMMMXASAMMSMAXAMMSSMMMMXSMSXXMXMSSSSSSMXMAMXMAXAAXXMASXMMASMMSSMMSMSASMMMMXMXMXMXMMSMXXMMMAAMMMXSAMSSMMMXXXMXMMMMSMSASMAXSAMSMSMMAMXMAXAS
    SMMSAAXSAXAAAAMMMXMMXAMXSAMXXXMASAMXAMAAAASXMSSSMSMSMSMAMMASASAMXMXAAAMASXAXMSMSMAMAAMAXMSMXXMXMAMSSXMAXAASMSAMXMAXSAAMAMAMMAMAXXMASXSXMAMMM
    MAASXMAMSMSMSSSMASMMMSXAXASXMXSAMMSMXMMMMMXAAAAXMAXAAXASXSAMMMAMMMSAMXMAMMMXXAAAMASMSSMMAAAMSXXMAXAMXSASMMXAXAAMMMXSMSMAMSXMMMAMAXMAMSAMSSSX
    SXMSAMXAXXAAXAAMXSAASMMMSAMMAAASXXAXMASMMMSMMMSMMMSXXXMMMMASXXSMXMAMXAMXMAMMSMSMSAMXMAMSSMSMAAMSSXMSAMXSMSXSMSMSSSMMSXMXMXAXXMSMSXMAMSAMAAAM
    SAMXXXMASXMMMXMMASMMMXAXMMMAXXMAXSAMAMXAXMAMAAAAMMMXXMAAXXMMMAXAMSASXSXSSMXAMAMMMMMAXAXXASXMMSMAMXXMMSSMAXXAXXAAAAASAMXXMSMMMMAAXXSXXSAMMSMA
    MAMMMXMXMXAAXSAMXMAXSSXMXMXSAMXMAMAXSSMMSSSMMSSSMAAAMSSMSXMAMXMAMSMMMXMMAAMMSAMXAMSMMMXXMMXXAAMAMASMXMAMMMMMMMMMSMMMXAAXXAMAAXMXMASMMSAMAMMS
    XAAAAMMASMSMSAAXASXMMMXMXSAMAMMXSMMMXAAXXAAMMMMAXMMMMMXASASXSSMXMXAAASMSSMAMMMSMMMAMAMSAASMMSSSXSMXAXSXMXAAAAMXAMASXSMSMSASXSSSSMXMAASXMASAX
    SXSMSAMXSXAXXMAMXXMAXSAMAMXMAASAMAMAXSMMMSMMAASMMXMXAXMASAMAAXMAMXMMMSAAMMSXAAXMXSASAAMSMAXMXAMXSXSSMMMMSSSXSMMMSAMAMSAAMAMXMAXMASXMMSXMXSMS
    MAMXMXMXSXSSMXMXXXSSMAAMAXMXSXMASMMSMMMSAMXSSMSXAAXMXMSAMAMMMMSSSSXMASMMMXMMMSSMASMMMSXMSMSMMSMAMXMAAMMAAXMAXAMMMXMAMAMSMMMMMAMSAMXSASASMMXM
    MXMASAXSMAXAAMSAMXXXXMXSSXSAXMMMMAAXAAMMXSAMMXMXSMSMSXXASMMMMMMAAMSSMXXAXXXAMXAMAMAXXMAXAMAMXMMASASMMMMMSXSASAMSMSSXSMAXXAAMASMXMXXMASAMAAAX
    MAAXSSMAMAMMSXMASMAMMXXXAAMAMMAMXMXMMMXSAMXMSAMXAAAMMXSAMXAAAAMMMMXSASXMSSSXMSXMSSXMASMSMSAMAAMXMMSASXAXAAMASAMAAAMMXXSXSXMMAMXXMMAMXMAMSSXS
    SAXAMXSXMASXXXSSMMXMASMMMMMAMXSSSMXSASMMASXMSASMMSMASMXXMMXMXSSXXAAMXMAAAXAMAMXAASXAXXAAASASMSSSSXMASASMMSMXMASMMMSSMMMXMASMSSSMMAAMASAXXXAM
    AAMSMMMMSASMSASAAASXXAMXMASXSAASAAASXMXSASAASMXMAXXXAAMSMSSMSAMXXMXSXSMMMSAAMMMMASMSAMXMMMXXAAAMMAMAMXXAAMAMSXXXXSAXAAXASXMAXAAAMSASASMSAMXM
    AXAXSAAXMAMAXXMMAMAXMXMAXAMAMMMMMMMMAMASXSMMXXXMAMMXMSASAAAMAASMXAAMXSXMXMXMAXXMXMAMMSAMXXMXMMMMSAMMMASMMSAXSXMMXMASMMSXSMMXMMMMMMAMAMXAXSMS
    AXSXSSSXMXMSMMMXXAMSMSSSSSMSXSAXMXMSSMAXAXMAMXMMMAMMXMXMMMSMSAMXMMMSAMAMXMAXMAXMXMSMAMXXMAMXXASXMMMASXMMASMMMAMAXXAXAXSXMASAMXXMXXMMSSSMASAA
    SAMXMAMXSAMAAAAAMAXAMAAXAXXMAMMXSAAAXMSMMMXMMAAAXAMSXMAXXXMAMXSAMAXMAXXMAMSSMAMMSAMASXAMXSXMMAXAAXSAMAAMAMXASAMSSMMXMXMASMMAMAMMSMAAAXAXAMAM
    MAMXSAMXMASMSMSMSSMMSMSMMMSMAMAAXMMMSMMAAMMASXSXSASXMSSMSASXMASMMAMSAMSSXSAAXAAXMMMAMMASMMSSMMSSMMMSSSMMMMSXSMSXSMAASASMMMSXMASMMMMMXSXMASXX
    MAMXMASASAMMMXMMXMXXAXAAXAASMMMSSMXXMAXSMMAMSAAASMMAXAMXAAMXSXMASXXMXSAAXMXMMSSXAAMASMAXAAAXAAAMASAXXAXXMASAMXAMXMMXSAMXAXXMXXMAAMXMASAMAMAM
    SSMSSXSXMASAMAMMAMXXXSXSMMXSXAAMAMXMSSMASMSXSMMMSASXMMSSMSMAMAMXAMMMMMMSMMMXAAMMSMSAXMASMMMMXMMXAMMSSMMSMMSASAMXMXSAMMMMSMMSSSSSMSMMASAMASAA
    AAAAAXSMSMMMMASMASXSMXAMASMXXMMSAMMXAAXSMMXAMXMXSAMXMXMAXXMASAMXMAAAAMXXAAAXXXSAXMMMXSAMASASMMXSAMXAAMAMAMSAMXXXSAMAMXSAAAXAAXMAMAMMMSXMASAX
    SMMMSMXAAXAXSXSMASAAMSMMAMXAXXASASXMSSMXAXMAMXSAMAMXMASMMXMXSAMXXSSSMSASXMSSMMMASMXMAMASAMXSAAASAMMXMMASAMMAMXSMMMMSXAMSSMMMSMXAXASAMMAMASMS
    XMXMAXMMMSSXMAXXMMXMMXAMXMMMMMMXXMAXAAASXMSXMXMASXMASASXSMSMMXMAXMAMAMMSXSAXAAMXMMAMXSAMXSSSMMMSAMAMXSASASXMSXXAMXAMMXMAMASAAXSSSMSASXXMMMAA
    AMASMSSXXAMASMMMXSSSMSSMSXAAXASMSSSMXXMAXAXMMAMXMMMXMASAMAAASXMXAMAMMSMMMMASMMSXMSMMAMASMSAMXMASAXMAXMASAMXAAAMSMMASAAMSSXMAXMAXAMSAMAMSSMSM
    XSAXAMAXMAMAMAAXXAAAASAAXSSXSMSAXAAXMSSSMSMMSASAAAAMMAMXMSMXMAMMMSXMMAAXXMAMAAAMXAXMASAMXXMXMMXSXMASMMMMMMMMMMMAAMASXMMAXMASMMMSXMXXMAMAAMAX
    XXMSXMMMMMMXXXMSSMSMMXMMMXXXMAMXMSMMXAAXMASASASMSMSXSAMAMMMSSMMMXMASXMSMAMSSMMMSMASXMMMXMASAXXAMXAMAAAMAMAMAAASMSMASAMXXAMXXXXAAAMSXSSSSXMMX
    MAMSXSXSAASXSAXAAXMASXSXSAXMSMSAMXXXMMSMSAMXMXMAMXMMXASMSAAAAAAXXMMMSAMXMSAMXSAMMMMAASXSSSSMSMASMSSSSMSASXXMSXSAAMXSAMSASASXXMXSXMSAMXAXXMXM
    MAMMAXASAMXAAAMSSMSXSASASASMASMXSAAMXAMAXXMXSMMAMMSMSMMXMMXMXMMSXASAMXMASMASAMMMSAMXMMAAXXMAXMAMAAAMAASMMXAAXXMXMSAMXAXAAAMMSMMMMAMSMMMMXSAA
    SMSMSMMMMXMXMXAMXMASMAMAMAXMMMXMMXSASAMXAAXAXAXAMAAXAAASMSSSXMASMMMAMXMSXSAMASXAXMSMSMMMMMMSMAMSSMSMMMXMASMXMAMAXMXXMXMXMXXAAMAAAAAAXAAXXSSS
    XXXAASMASAMMMXXMSAMXMAMAMMMMXMAMSAXXMAXXSSMMSMSSSSMSSSMXAAAAAMASAMSAMSXMXMMMAMMMSMAMMASAXSAMAMXAMXMAMAMMASAMXMSSSMSXSMMAXSMSSSSMSSSMSSSMMMAM
    MXMMMASASAMSXSXAAAXMSASASMAMAMMXMASASAMXAMAMMAAXMAXMMAXMMMSMXMASMMMAXMASMSMMSMSXAMMXSAMXXMASAMMXSXSMMAAMASAMMXAAAXAXMASMSXAXAMAMAMAXAAMAAMAM
    SAMXAMMXMXMSASAMSMMXXXAMXXAMSMSXSASXMASMASMMSMMXSXMSSSMMSAMXXMXSXXSMMSAMMSAAXASXMSMAMASXSMAMAXAASAXMSMSMASAMAMASMMMMMMMSAMXMAMXMASMMMAMSMSAS
    SMSMSSMMSXXMAMXXAAXSXMXMAXMMMAMAMAXXMMXMMMMXMAAASMMXAMAAMASMSMMMXMXAAMAXASMMMXMAXXMASXMAAMAXMMMXSAXXAMAMXSMMSAAAXXXAAXMXSXMSMMMSASMSXMAAXAXX
    XASAMAMMAMXMAMSSSMMXAAAXXMMAMAMXMXMMSSXXMAMXXMMMSAMMMMMMSAMAMAASAMMMMSSMASXXXSMMMMMAMAMAMSSSSSXMMMMSASASMXAAMMSXMASXMMSAMMXAXAAMASASASXSMMMX
    MAMAXAMXAMXSMSAMAXAXSMMSXSMASMMSMMMAAXMASXSMSSXASXMAXXMAMMSASMMSASAXMAMXXXXXASASASMMSAMAXAMXMAXSASXSXMAXAXMMSAMAMXSAAAMAMASMSMSMAMASAMAXXMAX
    SXXSMMXMAXXMMMMSSMMXXAMXAXMAMAAAMAMMXSMXMMSAAXMMMASMSMMXSASXSAAXAAXXMAAMMMSMAMAMASAMSAMXMSXSSMXMAXAMXMASMXSAMAMAMASMMMSAMXAMAMAMXMXMAMXMASXS
    AXAXASMSSSMSAAAXXASASAMMMMSMMMASXSSMXMASXAMSMXSASAMXAXMMAMXAMMSMSMSXSASAXAXMXMAMAMAMMSMXMMAXAXXMSMSXXMXMMAMXMSMSMXXAXMSMMMSXSMXSASMSMSASMMSS
    MSAXAMXAXAAMSMSSMXMAXXMASAAAXXMAMAAMMSAMMXXAXAMXMASXXMXAXXMMMXAXXASAMXMMMMSXXSXSASXMXXXMAMXMMXMAMAAAMSMSXMASAAAXMASXMMMSAAXAXMMSAXAAAMASXMAS
    XAXMAMMMMMXMXMAMXXXSXSXMMMXXMAMSMMMMMAAXAMAMMMXASMMMSSXMMASMAXMMMMMXSXMASMMXXSASXSMSAMASASMSXSXSMXMXMAASAXAXXMMMMAXMAAAXMSMAMAAMSMSMSMXMXMAS
    MAASAMSXXXAMXASXMSAMAMAAASMSXXMXAXMXMMMMMSASAMSMXMAASXAMSASXSSXMAXMXMAMAMAASMMAMXXAMASAMXMASAAAXXAMASMSMMSSSXMMXMASXSMSSMAMASMMMAXXAXXXAAMMS
    ASMAMAMAMSMSAAMAMSAMAMSMMMAAMSASMMSAMAXAAMASMXAAMXMMSMSMMASMXMASMMSASXMXSMAMXMAMXMSSMMMSAMXMSMSMSMSASXMAMAAAASMMMXAMXAMAMXSASAMSSXMMMMXSXSAM
    XXXASAMXMAMMMSMXMMAMXMMASMMMMSASAAMAXSMSSMAMXSMXSAASXAMXMAMAAMAXAASASAXXXMXMASAMAAXAXMMSXXSAMXMMAMMXSXSMMMSMAAAAMSSMMMMMSXMXSAMAXASXSMAXAMXS
    MMSXSMSMSSSMSAAAMSMMXMMAMMMSXMMMMXMSMXAAMMSMXXAAMXMMMMMAMXXSAMXMMMMASMMSXMASASASMMSAMSXMASMMMSMMSSMAXXXASAXXMSMMMAMXAAAXMMAMMXMASXMAAXASASAS
    XAMXMMMXMAAMSSMMMAXMASMMMMAAAXSASXAAAMSXXMAXSAMXSMMSAMSSMAXMASMMMAMXMMAMASAMXSAMMAXMAXASMMSMAMSAXAMXSAMXMASXXMXSMSSSSSSSXMAMXAMMMAMSMMMMAMAS
    XXXXXAMAMMMMMMMSSMXSAMMSAMXMMMSASMMMXMAAMAMASXSAAAAXAXAASXXSMMAMSSSMMMASMMMSAMXMMXMASMMMXAAMASMXSSMXSMMXMAMMMSAMXMAMAAAAMSASXAMMSSMMXMSMAMAM
    ASMXSASASASXXAAMASAMAMAXMMSMSAMXMAMXAMMMMAMMSAMXXMMSSMSXMMMAMXSXAAXAAXXXMAMMMSAMMSASAMXAMSMSXSMMMAMXXAMXMASAXMASXSMMMSMMMXAXMAMMAMAAAAAAMMMS
    XAAASXSAXASMSMMSAMXSXMSXSAMXAMMAXXMMMSAMSXMMSXXMXSAXXAXMSXMMAMXMMXSSMSSMSSMAASASAMXASXMXXMASAXMAMAMASMMAXAMMMSXMAAASMMAMSSSMMAXMAMMMSSXSMAXX
    SMSMSMMXMSMMASMMMMXAAMAAMMSASMSMSMXSAMAXMAAAXMASXMAMMAMAMAMSMMASAAMAMAAAAXMMMSMMASMAAMMMAMAMAMSSSXMMAXXXMMSXXAAMSMMMAMMMAAAMMMSSSMAMAMAAMXMX
    XAAXXAMMXMASMXXAXMMMMMAXMAMXMAAMAAXMAMXMMXMSMMAXMXSXXMMAMAMAASASAXMAMSSMMSAMXSAMASAMXAAMXMSXSAAXMXSMMMXSSMAXXMMAXAMMSMAMMMMMXXXAXMAMMMSMMSMA
    MMMXXSMXASMMXMMMXSAXMXXMMXMAMSMMMXSSSMASASXXXMASMAMAMXSMXMMSXMASAMSSMMMAAXMMASAMAMXSSSSXSMMAMMXXSAXAXAASAMXSSXSMSAMAMMXSSMSMMXMXMSXSXAXMXAXM
    XMAMXASMMSAAXMASASMSSMSMSAMXXAMXXMAAAAXMASAMAMASMXSAAXXAASMMMMXMAXXMASMMMSSMASXMASAMAAMMSAMXMSMAMMSMMMXSAMAAXAMASXMXSXAAAAAMMMMAAAMXMXSASXSM
    XMAXXAMXXSMMMMAMAXXXAAXASMSMSASASMMSMMMMXMAMMMMXAXSMSMMMMMAAXAXSXMASAMXAXSXMASAMAXXMMMMAMAMAMAMMXAAMASASXMASMSMMMASAMMSSMMMSAAMMXMMSMMAMSASX
    SMASMSMMMMMSXXSMSMXSMMMMMAAASAMAAAAAMXSXAMXXXASMMMXMAMXSASMMMSMAXAXMXSSMMXAMAXAMMSSXAAMXMMSSSXSAMXSMASMXXMAXXMAXSAMASAMXXAASMSXSXXAAAXMXMAMS
    AMAMAAAAAAAAXSXAAAMXAAMXMMMMMMMMMMSMSAMMMMMMSASAAAAXASXMAAXAAAMMSSXMXMAXMSMMMMMMSAMXSXSSSMAASAMASMAMAMMSMMAXASAMMXSAMXSSMMXSAXAAXMSSXMXXMAMX
    MSAMSMSSSSSSMXMSMXASMXSAXAMXAXXAXAXXMASASASXSMSXMSMXASXMAMMMSSMXAMXMMMSAXAMASAAMSAMXXAAASMMMMXSAMMAMXMSAMSSMMMAMAXMAXMMXAXAMMMMMSMAMAXXSXXSA
    XMAMXAAMXAAMAAXAAAXXMASMSMSSSMSSMMSASXMASASAXASXMAXMMMMAAXMAXMMXMXMXMAMMSASASMSMXASAMSMSMXMXXAMASXSMSMSAXAAXXMMMMSSMMAAXSMMSASAAAXASXXMAMAMX
    XXMMMMMSMXMXSXSMSMMAMMSXMAMAMAXXAMSMMSMAMMMMMAMXMASMAAMSSSMMSASXXXSAMXSXSAMASAMASMMAMMXMAMSMMXSAMXXXSASMMMSMSMAAXMAXSMMMXAXMASMSMSMSASAAASMS
    SMSAXMAXAAMXMAMAAASMMMSAAAMMMSMSSMMAAXMSMSASMMMSMXSXSXXAMAAAMAMXMASASMMAMXMAMAMAMMAMMMMMAMAAXAMAMAMXSXSMSXMAASXMSMXMMAMASMMMAMMAAXXMAMMSMXAX
    SASXSMSXXAXAMAMSXXXXSXSXSXSXAMMAAASMMXMMXMAMAMXXXXMAXMMSXXMMMAMXMAMXMMAAMMMMSAMASMAMMXMSAMMSMXSAMXSAMMSAMMMSMXSAMXXAXAMMXMAMSSSMMMMMAMXXAXMX
    MAMASAAAMSXSMMMXXMASAAXAMMAMMMSSSMMASMXMXMMMXMXSASMMSAMAASXSMSAXMSSSMSXMMXAAXAXASXXMXAMMAMSAMXMAMAMASAMAMMAXASMMMMSSSSSXSSSMMAAAXMXSASXMSMXM
    MSMMMMMSMMAXXAAXSASXMXMAMAXXMAMAMASAMXAMXMSAAXASMXAASAMMSMAAAXASXAAXAAXSXSMSSXSXMXSSMXMMAMXAMMMSMSXMMMMMMMASXMAXAXXAAXXAMAAASXSMMMASMSMAAMAS
    XXAXAXSAAMAMSMMSAXXXXSMXMSMSSSMAMAMASXXSAAMSSMAMXSMMSXMXAMSMMMAMAMXMMMSXAAAAMMAMAAXXAASXMMSSMSAMMXAXSXASMMMSASXMMMMMMMMMMSXMMAXASMASMSMSMSAS
    XMMSSSSMMMSMXAXAMXMXXXAAMMAXXMXXMASXMXAMAXMAMMXMAMXXMASMMXMXSMSMAXASAMMMMMMMSSMAMSSMSMMASAAMXMASAXMMSMMMASASMMSXAAAAAAAXAMSMMAMAAMASAXAMAMXS
    MSAAMMMAAAMMXSMMXMASMMMMSMSSMMMSMMXMAMXMMMMASXSMMSXSXSAMXAMAXAASXMASASXAXSXAAAXSMAAAAXSAMMXMAMSMXMSAMXXXAMMXAASXSMSMSSSMXXAXMAXSMMASMMMMXMAS
    SMMXXAMSMMMSAMXXMAMMASAMXXAAAAAAAMASMAAAXAMAMXMASAASMAMSXMSAMSMMXMASAMXSMMMMSSMXMSMMSMMXSMXSXSAASXMASMSMMSXSMMSAMXMMAAAASMSXSAXAXXAMXMAAAXXX
    MASXSMXAAXAMMSMASAXSAMAXXMXSSMSSSMAAASASXSSSMMMXMMXMASMMAMMMAXASXXMMXMAXAXXXMAMXMXMAMAMXMAASMSMXMASAMAAAXSXSAAMAMAMAMMMMMAAMXMASAMMSMMAAXMMX
    XAAMASMXSMSSXXMAXMMMMSSMASAMXAXAAMMMXXSXXXAAMSMSAXMMAXASMMAMAMMMXAXSAMXSMMSMXXMAXAMMSAMAMMMSAMXASMMAMSXSMMASMMSSMASMAAXXMMMMAXXMMMAAAMSMMAAM
    MXSASASAAAAMMAMXXSSMAAASAMXSAMMSMMMXMXMMMMSMMAAXASXMASXSASXMXSMSXMMMMSAMMAXAAMSSMXMAMAXAXAMMXMMXMASXMMAMXMAMMAXASXXXSMSMSAXMXSMSSSSSSMAASMMA
    XXAMXAMMMMMSAMXSXAAXMSMMMMXSASAXMAXAMMMXSAMASMSMAAAMXSASAMXSAMMSAMXAAMAMMXSMMXAMMSMMXSSSMSSSSXXASXMAAXSXMMXXMXSXMSMAXAAAXXSAAAASAAAAMMSMMAAS
    MMMMMAMSXSMMMMAAMSMMMXMXAMXSAMASXMSASXAAMASAMXXXMSXMMSXMAMAMMSAXAMSMSSSMSASASMMSAMASAXAMAAAAAASXMSAMXMAAMXMASASMAMMSMSMMMAMMXMXMMMMMMMMMAMXX
    AXAXAAXAASXSAMSSXXAAXAMMASXMXMAXAAMAMMMMSAMASAMXMMAMASXSSMMSAMASMMXMMAMXMASAMXXMASAMMMAMMMMMMMSAMAMXAASXMAAMSAMMAMAAAMAXSAMAXXASAMXASAXMMMMM
    SSSSMMMMXMASMXAMMSMMMAMMAMMSSMSSMSMMMXXXMASAMXASAXAXXSAXAAXMASXXAAAMXSSXMXMMMXMSMMASMMMMAXAXXAXXMAXSAMXASMMXMAMSXSSXSMSMSASXMSAMASXXXMMSAAAX
    MAMAASMSAMAMXMASAXAASASMAXAAAAAXAXXXASMMSXSXMASMMXXSAMMMMMMMAMXMMSMSAAXXMAXXSXMAXMAMAAMSMSMSMSXSSXMAMXSAMASXXAMXAAAXXAMAMAMAMMAXSMMSXSASXXMX
    MAMSMMAXMSASMMAMMSSMSAXMMMSSMMMMSMAMMSAASASMMXMAMASMXMXAASMSMXAAXAMMAMMAMMXMMASXSMASXSMAMMASXAAMMMMAMAMASXAASXMMMMMMMAMSMSMMMMXMAAAXXMASMSSM
    MAMXXMAMXSXSAMXXXAMAMXSAMXXMXMXAASAMXSMMMMSMXXSSMMAAAXMSMSAAMSMSMXSAAMMAAAMMSXMASMMSAAMMXMAMMMSMAASAMXMAMXMMMMASXMAAXAMXAXAXSXSASXMSAMXMMAAM
    MSMSMAAAXMASMMMSMMMAMXSAXSAMASMSMSXMAMXSSXMMSXAASXMSMSMMAMXAMAMAMMXXASXSMXXXAXMXMAXMMMMMAMMXMAXMSMSASAMASAXAAXSMASMMSAAXSMSMAAAXMXAXXSSSMSSM
    AAAAAMXXMMXMMSXAAASMSXXAMAASASXXMSAMMSMMSXMAXMMMSMMAMXAMXSSXSXSASMAMXMAMAMXMAXSASMMMMSSSXSAASAMXAMSMMASASMSSMSAMXMAASXMXXAMAMXMXSXMMAAAMAMXM
    SMSMSXMASXMSAMSMSMSAMMMSMSXMASAMASAMAAAASMMSSSMAMXSAXSXMMAXXXAMAMMXMXSAXMASMXMAAMXASAAAAXMMXSMXXMMMSSXMASAAAMSMAXXMASAMSMSMAXAXMAMMASMMMAMAA
    MAMAXAMASAAMAXSXMAMAMMAMAMMSXMAMMSSMSSSMXASXAAMXSASMMMXSAMMMMSMSMMSMMMXMXSXMMMMMMSSMMSSMSSMXMMSXXMAAXXMSMMMSMSSMXMMMXMMAAXSXMSXAMAXAMASMMSAM
    SAMXSAMASMSMMMXAMXSSMMASAMAXXSXMMXMMXAXXMMMMSMMXMMXSAMXMMXAAAAAXMAMSMSSMXMASAMAMXXAXXXAXXAXXAAMAMMMMSMMMAMMAMXMMASAMSAMXSAMMMMASXSMMXXMAAXAX
    MXSXSXMASAAMAMSXMMMMASASXMASMAASXAXSMMMSSXSXMASMMXXSMSAMASMSXMMMXASXXAMMMSAMSXSMXXAMSSMMSMMSMMXSAAMXXAXSAMXAMAAXAMAMXAMXMMMMASMMMMASXMSMMSXM
    XSAMXMMAMMMSAXMASAMSAMMSAAAMASAMMSSMAAAXXXAASXMASAMMMSASXMAXAXMASXSMMMSAAMMSMXMASMMMXAXAMXXXAAAASASMSSMSASXSXXMMSMMMSMMASAAXXMSAXSAMAAXXAMMS
    XXXAAAAXXSMSMSXMMSMMASXSXMXSAMAMAXMXSMSSMASAMXSXMMAMMSAMXMAXAMMASXMAAXSMSXMAMXMAMASMMMMSSMSMSMSXMXMXAMAMXMAMASXXMAXMAXSMSMSSSXMMMMASMMMMXSAA
    XMSSSSMSXMASAMASMMSSMMMMMMXMXSXMXSMAXMMXMAXAMXSMAMSMMMSMMMSSSMMASASMSASXMMSASXMASXMAAAAXXAAAAMXXXSMMXMMSAMXMAMAASXMXMMSAMAXAMXMXMSAMAASAAMXM
    XMAXMAMMAMAMASAMAAAXXMAMAXAXAXAAAAMXSXXXSASAMAXMAMAAXAAXSAMXMAXAXAMAMMSAMXMASXSMSMSSMMSSMXMXXMMSAAAXMAMXAMAMXSMMMXAASXMAXSMSMSAAMXMXSAMMSSMS
    SMMSSMMSMMSSMMMSMMSSSSSSSMMMMSMMSXMASMSMMAMAMMSSSSSXMSSSMMSMSSMSXAMAMXSAMXMSMAXAMXXAMXAAXXSSSMASXSMMMSMSXMXSMMXAXMSMSAMMMMAAASMSMAXMXXAAXMXA
    XAXAAAAMAAAAXAXXMAXAAAAAXAAAMAXXMXXXXAASMAMSSMAXMAMXMAMMXAMAAAAMXMXXXAMAMXMAMAMMMSSSXMSSMAAAXMAMXMXXXAASXSXAASMMMMXASAMXAMMMXMAAXSMMAXMASMMM
    SSMASMMXMMXMMMSMMMMMMMMMMSSMSMSXMASMMSMSXXXMAMXMMAMXMSXSXXSSMMAMAMMXMMSXMAMMMSSXAAXXAMXAXMMMMMXSXSSMMMSMAASMMMMAASMMSAMSSSMSMMSMMMAXAMMAAXAX
    XMAMMAMXXMASAMAXAAAXMSMXMAXXMAXAMASXAMASMSSSMMMXXMXSAMAXXAMMSSMSASMXSAAXSASXSAMMMMSSXMMSMSAXXSAXAAMASXMMSMXMASMSMMAAXAMMAAAAAMXSAMAMXSXSSSSS
    SAMXMAMSASASASXSSSMMMAAAMASMMMSSMXSMSMAMAAAAXASMMSAMAMAMAXMAAXXMASAAMMSMSASXMAMSAAAMASAXAMXMXMSSMMSAMAMAAXMXSXMXMSMMSXMMSMSSSMSMSSXMASXMXMMM
    MMSMMMMMAMXMMMAMAMXASMXMSXXMAMAXAMMAMMMSSMSMMMAAXMAMXMMMMMMMSSSMXMMMMMAAXASXSAMXMMMSAMASXMAAAAMXAXMAMMMMSMXMMAMXMAMMAMXAXXXAAXXAXMAMXSASMXAM
    MAAMAMAMMMXMASXMAMMXMXSMXASMSMMSMSMAMAAXAXAXAMMSMSAMMSAXAAXXXAXMXXMAMXASMXMXMXSXXMAMMSAMXSMSSSMSSMSMMSAMAAAMSAMAMAMXXAMXSAMXXAMXMMAMXSXMASAS
    MSSSXSAXMAASAMAMASXSAMAMXSXAXXMAMAASMMMSMSMXMSXMXSASASASMSSMMMAMSSSMSXXMASXMMASXMMASAAMMXAAAAXMAMXAMASASAXXMAASASASAMMSMMXSSSMMASMMXXMAMMMMM
    XAAAASAMMSMMASMMASXMASMXMMMMMMSMXMSMMXAXMAXAAXXMASXMMMMMAAXXASAAAAAASMMXSMSAMAMAXMSSXMMMSMMMSXMASXMSXSXMAMSXMXSASMSASXAAMAMMAMSASASXAXAMASXS
    MMMMMMAMAXASXMAAMMASAMXAMXMSMAAAXSAMXMXMSAMMXMXMASMMAAMMMMMXXSMSMSMMMASMXMASMMSMMSASMXMASXXAXXSASXXAMMXMAMAAXMMXMASAMXMXMAXMAMMXSAMMMSMXASAA
    SAMMSSMMMSAMXSXMMSAMAMSASAMAMSSSXMASXAXMAXSAAMMMXSASXSSXMASAAXMAMXAXXXMAXASXSXAXMAXMXMMASMMMMAMASMMMXAAXASMXMAASMMMSMMXMMMXSAMSAMAMXXAXMSMXM
    SASAMAMAMXAAXMASXMAXXMAAMAXXXAMXMMAMMSAAAMSMXSAMSMXMAAXXSAMMAMXXMSSMMSSSMXMAMXMXXSASXSMASMAXMXMXMASAMMSMMSAASXMMAXSAXMASAAXSXMAMSMMXMAXXMASX
    SAMXXMMAXMAMXSAMXSAMSMSSSMMSXMMASMMMAXSSSXMAMAMXAMAMMMMMMASAXXMAMAAAXAAAMXMAMSMXXXAXAAMAMMASXMSASXSXSAAASMXMSAMXXMSAXSASMSMSMXMXAAAXMSMMMMMM
    MAMMSMSMSAMXMMXSMMMMAAAMAAAXAXMAMAXXSXAXXAMMMAMXASMXXXXXSAMXMMAMMSSMMXMAXAMAXAMXMMMMSMMSSMASAAMAMAMAMXXXMASXMAMXAXXXMMXSAAAXMAXSMXMXXXMAMAAX
    SAMXAAAAXXSXSMAMAMSSMMMSMMMSMMMSSMMMMMMMMSXXMAMSXXMMSAMXXMMAMMSAMAXAMMSMSMSSSMSAAAMAXAMXAMSXMMMSMMMAMXMSMMMAMMSXSAMXXXMMMMXASXMAMSMSAMXSSSSS
    SASXMSMSMMXAXMASXMAAAMXSXMMXMXAAXMAASAXMAMMXMAMSMSMASAMASMSXMAMXMMXAMAAMAAMAAXSXSSXSSSMXSMMXSXAXAMSAMXXAAXSSMAMMXASMSSSMSSXXAMXAMAAAAMAAAAXX
    SSMAXXAXAXMSMSAMXMXSMSAMAMMAMMMSXSMSXMSMASMAXSXMAAMMSAMXSAAXMSSMASMMMSSSMSMSMMMMMMMMAAMMXAMAMASMMMMASMSSSMAMMXMAXAMXAAAAAAMXMAMXMMSSMMXSMAMS
    SMSSMMXMXMAMAMMSMSXAXMASXMXASAMXAMXXMAAMASMAXMAMSMSXSMMAMMMMXMAXMAAXXAAMXAXASAAAXAAMMMMAMAMSMAXAMSSMMAAAAMXMXSMSMSMMSMMMMSMMSXMASXMAAXAMMSXA
    MAAMAMSASMXMSAXAAXSXMSAMASXMSXSMSMAMASASASMXXMAMMAXAMAMXSXXMSAMXMXMMMMMMSMXAMSXMMXSSMSMXXAMXMMSSMXAAASMSMMXMMMAXXXAXXXXXAAAXAAMASASXMMMSAMAM
    MMMSAAMAMAAAXSSMSMMMMMAMAMAXXAAAAMAMMMXMMSMXMSMMMMMMSAMSAAXSXMAMSMXXAMAMASMSMMMSSSXXAAASMSXSAMXMXXSMMXXXMSMMAMXMXSXMXMMMXXXMMMMASXMMSSXMAXSX
    AMXMMMMAMXMMMAXXAAMASXMMMSAMMMMXMMAMXXXMAMXAXAAAXMAXXXSAMMMMAMSMAASMMSXXAXXMAXAXSASMSMXMMMAMMXAMSMMSSXSAAMAXMXAXXAXXASASXSMAAXSXMASAAXMSSMMM
    AAMXSXMAXAAAXMXSSXSAMMAMXAAMMAAMSSXSAASMSSSSSXMSSSMSXXAAMSXSAMAMMSMAAAXMSMMMAMSMSMSXXMXSAMXMSMSXXAMASXMXMSSMMSSSMSMSMSASAASXMMMASAMMSSMAAAXA
    MAXASXMAMMMSSXAMMMMMMSAMXSMMSMMSAAAMMMAAXAAXSAAMXMASASMSMAXSASXSXMASMASMAXMMAMMASXMASAASMMMXMAXMSMMASXXAXAAAXAASAAAAMMMMMMMSAXSAMAXSXXMMXMMS
    MMMXXAMSMSAAAMAAAAAXASXSXXSXMAXMMMMAMXMAMMMMXXXSAMSMAMXXMMMSAMXMAMXMMXXMMSXSMSMAMAMAMMXMMAXXMAMXAXMSSMMMMMXSMMSMMMSXSSXSXMAXSMMXSSMMMSSSMSAX
    SASMSSMAAXAMXMSSSSSSMMMMAAMMMXMAMXXMXMXXSXSXMMMMAMXMXMMMAXAMAMMSAMAXMMMSXSMXAAMMSMMXSXXMSMSAMAMSASXXXMAMXMAMXMAXXXMXAMAXSMMSMMMAMXAASAAAAMAS
    SASAAXMMSMSSXXXAXMAMXMAMMMMXAAMSMMXSAMXMMMSAMXASXMSMSASXMMSSSSXMASMMSAAAAXAMSMMXAXMSMMXAAAXMASXSAXAAXMAXXXXMAMXXXAXMMMAMMXMAAASASXSMMMXMMMXM
    MAMMMSSSXAAAMAMMSMAMASASAXXMASXXAXXSASAXXASAMMXSAAAAMMAMXSAAMAASMMMXSXSMSMSXXASXMSXAAXMMMMXMAMXMAMSMSMSXSAMAASXSMXSAAMMMXAXSXMSMSMAMMSXAASMM
    AXXXAAXMMMMSMMAXAXXXXMASMSXAAXAXMMMSAMXMMXSAMXAXMSMSMMMMMMMXMSMMSAMXMAMAMAMXSMMAAXXMXMSXXMASXMSMSMAAXMAAMAMSMMMXMMMXMMASMMMMSXSMXMMMAMSSMSAX
    XMASMMXMAAXAMXSXMSSMAMMMMSMSSSSMAAAMXMAXSASAMMSSMAAAXAXASXMSXXMAXXMAMAMMMMSAMXSAMSMSAAXAMSAMAAAAAXMSMSMXMXMXAXMXMASASMASAAASAMXSXAXMAMXMASMM
    XXXXAXXMXXSASAMSXAAAXXMAMXAMMAAMSXSXSXMXMAMAMXMAMMXMMSXXSAAAASXSMSSXSXSAAMSAMXAXXAAMMSMMMMASMMMMMSSMAAXAAXMMMMSASXXASXASXMMSAXASMMMSXSMMXMAS
    SMMSMMMSAAMXMASAXMSMMMMMMMSMMMMMMMMXSASXMSSSMASXMSAMXXMMMMMMAXAAXAAAXXMMMMSMMSMMSMSMAMAMXSAMXSMSAXAASMSSSMSAAXSASAMSMMMSAMXSAMASXSAMMAMXMSMM
    XAMASXAMMSMSMXMMSMXMASAAAAXMAXSAXAMAMMMAAAMAXASMASXMAXAAXXMSSMSMMMSSSSXSXASMAAAAMXXMMSAMXMASXAAMMSSMMXAAAASMMMMMMMMAAAAMAMAXSAMSMMASXMSAMAAX
    XXMAXMSXXXASMXMASAAXASXSMXSSMSMXSXMASASMMMSSMMSMMMAMMSMMSAAAXMAXAXAMAXAMMMMMSSXSXMMMASAMXMXSMMXMAAMAXXMMMMSAASASAASXSMMSAMXSAMXMAXAMAXSASMXS
    SSMSMAAMXXAMMSMAMXASMSXMAMXAMMMMSXSXXAXXAXAXAXXAASXSAMXAXMMMSSMSSMMSMMMMXMAXAAAXXSAMMSXMAMAXMASMMSSMMSMXSXMMMSASAMAXXMXSASXMMMSSMMSSMMMAMXMA
    XAAMMSMSSMXMXAXMMMXMASAMXXSAMXAAMASAMSMSSSMSXMSSMSAMAXMSSXAAAAMAAAAMXSAAASMSMMMMASMMXXAXAMSSSXXSXMAXAMXAMAMSMMMMMMSMMXAMMXAXXAXAXAAAXAMSMSAM
    SMMMAMAAXAASMMMXASASAMXMSASMXSMXMAXAAAAXMAAAXMAMXMMMSXMMSXMASXMSSMMSASMSMAAAXXAMXMAXSSMMSSMAXXAMSXXASMMXSAMXSAMXXAXAMMMSMSMMMMSMMMSMMMXXASMX
    XXAMXMMMSSMSAMXSMSASMMAASXXXASMSMSMMMXMXXXAMXSASAXAAAASXSXMAMXAAAMXMASAAMMSMSSXSXMXMAAAAMMMMMMSMAMMMMSAMSAMXSMMXMXSAMXMAMAAAXXAXXAAAMMMMAMXA
    SSSSMSSXXAXSAMAMAMAMASMXMMAMMSASAMXSSSMXXXASXSAMMXMMXXMAXAXAMSMMXMXAMXXXXAAXAXMMASAASXMXSAXXXAAXMAAAAMMXMASMMXSXMMSMXAMASMMMMSASMSSSMSAMXMXS
    AAAAASAMSSMSAMXMASXSMXXAAAAAXMAMAMAMAAMAXSAXAMASMSSSMMMMMXMAXAAAMSAMASXMMSSSMSASAMMMMAMMSXMAXSASXSSMSSXXSAMAMXSAMAMASMSASMMSAMAMAXAAASASAMAX
    MMMMMMAMAMMSAMXSAMMAMXSSSMSAMXAMAMMMSMMAMMXMXMMMAAAAAAAAAASMMMMMAAAAAAAAAAAAAAXMASXASAMAXMASMMAAMAAAAMAMMASAMASAMASMXAAMSAMMASAMMMSMMSSMAMAS
    XMSXMSAMXSMSAMXMMSAMXMAAXAMAXSMSXXSAMXMXSSXAXAXMXMMMSSSMMMSXAXXXASAMXSSMMSSMMMSSXMAXSSMMSSSXXMSMSSMMMMXMAXSAMMSMSXSXMSMSSMMSAMMAMXXAXMMSSMAS
    """;
}
