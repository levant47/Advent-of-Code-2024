﻿public static class Program
{
    public static void Main()
    {
        Console.WriteLine("Starting problem 10...");

        var input = INPUT;
        var map = ParseGameMap(input);
        var part1 = 0;
        var part2 = 0;
        for (var y = 0; y < map.Height; y++)
        {
            for (var x = 0; x < map.Width; x++)
            {
                if (map.Get(x, y) == 0)
                {
                    var nines = new HashSet<Position>();
                    part2 += GetScore(x, y, map, nines);
                    part1 += nines.Count;
                }
            }
        }
        Console.WriteLine($"Part 1 answer: {part1}");
        Console.WriteLine($"Part 2 answer: {part2}");

        Console.WriteLine("Done");
    }

    public record Position(int X, int Y);

    public class GameMap
    {
        public int Width;
        public int Height;
        public List<string> Rows;

        public int Get(int x, int y) => x >= 0 && x < Width && y >= 0 && y < Height ? int.Parse(Rows[y][x..(x + 1)]) : -1;
    }

    public static GameMap ParseGameMap(string source)
    {
        var rows = source.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(line => line.Trim()).ToList();
        return new()
        {
            Width = rows[0].Length,
            Height = rows.Count,
            Rows = rows,
        };
    }

    public static int GetScore(int x, int y, GameMap map, HashSet<Position> nines)
    {
        var current = map.Get(x, y);
        if (current == 9) { nines.Add(new(x, y)); return 1; }
        var part2 = 0;
        if (map.Get(x + 1, y) == current + 1) { part2 += GetScore(x + 1, y, map, nines); }
        if (map.Get(x - 1, y) == current + 1) { part2 += GetScore(x - 1, y, map, nines); }
        if (map.Get(x, y + 1) == current + 1) { part2 += GetScore(x, y + 1, map, nines); }
        if (map.Get(x, y - 1) == current + 1) { part2 += GetScore(x, y - 1, map, nines); }
        return part2;
    }

    public const string EXAMPLE_INPUT = """
    89010123
    78121874
    87430965
    96549874
    45678903
    32019012
    01329801
    10456732
    """;

    public const string EXAMPLE_INPUT_2 = """
    0123
    1234
    8765
    9876
    """;

    public const string INPUT = """
    987672345210988321089487678943210101985430123012901212349876
    890581876347809451276596521058923432876543276529874301236765
    081490932156918760345603430667898943109801983434565321245125
    112387893045329654565012348766087652234712123403125430104034
    201236794532134503456983289632128141045623010512036765295543
    310145687621032012987674108543459031012654107696543894387654
    498456546001221093474565432432569122101965298787645976578761
    567307632120332387565478901201678233212874345676534589469450
    569218986787454456010785832178876544589012321201225676354321
    478123678796561234521896541069987235678987430390310101256910
    312054569657320899634567892454100124017986543485436543267871
    203765012348210798749656876363211345623477012576567654106565
    104894343239345687658775965278901210786568943689658987265430
    985787210101012210343189034107349809894329876756567456892121
    276321895696523421221076121001256756765018549843498305743012
    123450734789434322834565432890162125321567678732565214654012
    043545643298545618965436901781878034450410589721056543204323
    652634102187656709878927801652989876567323434874347890116454
    701783214074327812567213452343567865018430128965236921327985
    879898543265018903450102169603498984329543012340121023438876
    930569856104345676543221078712321075655432121050123214589565
    321478987645430189876330789610165034746583043269874303678456
    430326786554321234565445698543278129837895650178965452102365
    545415898545210765676324582344569100121298782348766567801476
    696504305676905894889213001053213256780367091059654321945987
    787413218789876103990102128967104345091452187768345670236896
    898310129650145212850167034878545432187873677851210980167345
    790101212341232101763258949889436782106924578960102398798201
    889212303216987232654345656776521093345014467017681432120132
    974322454307896342363214780345810894214323454178590541098743
    865401965416787651654303291236989765201412963289487672347652
    765432876545619650189432100387870652106507875670343781656761
    765410987814308761276541087498761543017890124501289890967890
    898321678903216654365650196501252102120987033215670767856981
    456912565214327612984787767102343256761856144256781056743234
    367803454323438703673298898234358543892349856105892343212101
    219804456554549654560143567895569212876548761234987454101234
    008512367567632103067654410766978103975432810125676655670543
    127601898458903452198103328957860198786901912089985765781632
    234534567321012560198712367046043245697850105678789854398701
    103421673450523871237601452132154032106543214543210710239678
    011210982567658964321546543012965124321432343058988720106549
    320125671098576545690037012129876765010541012167349634218932
    430134040123489836789128903456745895432672123453234545347451
    549232132123498721654100190109832106701089098500104456956320
    678943433001567890193289283254108987892128987612245437875410
    217654456712346521089374374363201076985434376543336521056587
    103450349809454434678765465478912345876548985965447899867898
    212761212778765894501250104567656210230123476876534038769876
    429843203989987765410343215698567340145696545123410125612345
    343456117801256784320354210785458756968787034014567234501654
    652107006901343098901235341812329647879674123456798012612703
    567898215414332187612356756901410138984565012343898543563812
    238987312323278076543349867812567221345694321032187623438910
    109813408954109189801210789003498210210782107153098018542123
    216701567867898276764325650101567367823473498544567129656034
    345652101210567345123434543234989456910565567639875678798945
    456543458325430410089545696545678321045876543320564549567876
    434567869450321523679656787230109878236905452011254235650165
    321018978761234334578765698101234569107812301232340123543234
    """;
}