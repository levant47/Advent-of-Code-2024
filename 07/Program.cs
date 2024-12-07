﻿public static class Program
{
    public static void Main()
    {
        Console.WriteLine("Starting problem 07...");

        var equations = new List<Equation>();
        foreach (var line in INPUT.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(line => line.Trim()))
        {
            equations.Add(Equation.Parse(line));
        }

        Console.WriteLine($"Part 1 answer: {equations.Where(e => Part1(e.Target, e.Operands)).Sum(e => e.Target)}");
        Console.WriteLine($"Part 2 answer: {equations.Where(e => Part2(e.Target, e.Operands)).Sum(e => e.Target)}");

        Console.WriteLine("Done");
    }

    public static bool Part1(long target, List<long> operands, int i = 0, long result = 0)
    {
        if (i == 0) { return Part1(target, operands, 1, operands[0]); }
        if (i == operands.Count) { return result == target; }
        return Part1(target, operands, i + 1, result + operands[i]) || Part1(target, operands, i + 1, result * operands[i]);
    }

    public static bool Part2(long target, List<long> operands, int i = 0, long result = 0)
    {
        if (i == 0) { return Part2(target, operands, 1, operands[0]); }
        if (i == operands.Count) { return result == target; }
        return Part2(target, operands, i + 1, result + operands[i])
            || Part2(target, operands, i + 1, result * operands[i])
            || Part2(target, operands, i + 1, long.Parse(result.ToString() + operands[i].ToString()));
    }

    public class Equation
    {
        public long Target;
        public List<long> Operands;

        public static Equation Parse(string source)
        {
            var parts = source.Split(':');
            return new()
            {
                Target = long.Parse(parts[0]),
                Operands = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList(),
            };
        }
    }

    public const string EXAMPLE_INPUT = """
    190: 10 19
    3267: 81 40 27
    83: 17 5
    156: 15 6
    7290: 6 8 6 15
    161011: 16 10 13
    192: 17 8 14
    21037: 9 7 18 13
    292: 11 6 16 20
    """;

    public const string INPUT = """
    558536: 8 5 8 2 3 958 69 1 4 54 8
    62085452: 2 95 32 2 54 51
    363816188802: 5 601 3 603 2 2 93 6 3 5
    88542069: 88 133 355 4 70
    773056: 7 593 4 3 1 1 8 91 2 5 7 8
    4552710: 9 544 8 32 2 95 58
    18479401846: 577 48 1 160 20 923 2
    6568553517: 593 9 65 4 787 83 57
    6666: 2 1 1 9 6 8 338 1 2 8 58
    27806: 8 5 28 522 89 18 28 2
    73259120000: 6 96 4 8 2 83 5 80 44 59
    5913: 95 47 811 562 77
    1796326: 1 701 6 427 362
    73527795: 75 755 5 2 832 55 801
    1722: 7 23 778 690 93
    640700: 162 8 8 9 1 29 545 5 10
    27248639573: 918 306 5 81 970
    198699370: 3 662 9 9 3 70
    1155255891: 299 23 517 9 398
    376195091: 7 76 1 707 8 3 265 45 1
    2376899: 2 5 97 44 900
    55842419: 1 203 5 232 1 2 5 6 5
    417928: 7 9 3 2 651 4 149 38 94
    771348695: 7 887 4 5 1 489 9 2
    22344255519: 80 6 7 95 3 650 1 2 7
    1289584: 5 9 295 52 3 86 426 2
    716327841: 223 42 3 5 33 6 3 3 3 5 5
    16133: 3 5 843 31 259
    67718: 85 796 25 24 9
    32460: 270 53 74 4 81
    48960: 1 402 957 4 9
    3597: 9 5 3 6 4 81 38 1 237 1
    2279424: 2 5 723 6 8 56 8 64 4 2
    688: 2 6 4 6 3 5 658
    5137095277203: 9 89 41 89 20 641 2
    581028: 84 5 2 912 5 7 32 2 7 8
    865177732: 96 3 9 14 67 5 8 4 6 2 6 3
    6445848: 426 80 8 2 6 186 2 982
    3391644218205: 556 61 44 218 208
    64191161: 836 43 33 1 73
    803537526699: 80 35 37 522 4 699
    229618560: 431 75 59 6 9 24 590
    9617: 82 686 12 341 60
    25113667: 2 4 1 15 436 2 6 5 1 8 59
    3207456: 2 42 2 8 9 86 592
    194143: 925 557 131
    488757: 244 20 7 50 5
    3329684: 16 2 826 789 5 9 95
    3824723448: 3 3 572 9 30 666 3 364
    6267696: 6 97 44 760 9 9
    14606754376: 3 69 543 729 76
    7478801: 37 7 36 551 5 626
    429: 1 71 36 4
    25272164: 4 81 6 65 8 2 1
    210821591147: 68 9 131 366 2 941 79
    28009: 6 97 3 4 5 7
    271028246: 54 519 71 983 10 7 7
    25206: 3 9 93 9 87
    4363: 4 67 3 3 5 9 9 4 804 5 2 2
    342354: 126 9 6 23 2 92 1 1 1 3
    7241601587: 368 41 13 177 5 90
    49866: 1 2 39 334 57 454 9 4 6
    1750852: 378 6 36 17 6 7 35 82
    129243383: 7 3 2 4 2 67 2 8 4 533 3 8
    138856: 567 2 3 67 2 4 2 2 5 4 1 2
    357115: 534 95 140 146 7
    27529: 2 4 15 2 437 8 8 7 1 30 2
    1278639350964: 4 7 5 7 6 710 389 92 4 9
    67962716: 4 8 74 8 5 18 8 9 5 92 9 7
    8815: 2 15 41
    5241075840: 858 707 4 60 36
    2886996: 58 929 975 7 1 3
    1999479: 4 671 94 26 81
    230742: 443 520 302 14 66
    30385619972: 288 15 85 61 997 2
    5058738: 966 330 93 6 607
    490164217: 6 127 8 34 7 68 2 39 9
    8686: 75 508 270 9 6 4
    172900: 5 21 7 70 38 25
    357125: 12 8 186 5 4 5
    10283240: 93 575 2 165 6 9 2 7 2 4
    259071398691: 43 8 8 5 71 3 98 683 8
    365: 5 5 8 40 5
    191544: 4 344 12 3 46
    9853: 7 898 9 71 1
    747: 1 9 83
    851551: 8 2 8 1 33 1 6 6 5 593 4 3
    309530: 97 27 1 476 30
    15483: 83 7 1 86 2
    3154970: 1 67 27 218 9 8 2
    494589: 6 7 9 42 181 38 774 3
    2209216: 8 261 89 7 9 84 2 424 4
    28974: 2 83 8 5 1 10 3 17 7 631
    1512048: 4 905 944 816
    164884: 2 96 9 1 68 6 1 3 2 8 4 34
    3242385708: 8 105 8 1 62 4 8 3 6 8 7
    39494: 3 50 5 345 98
    2247: 3 83 6 748 5
    138938644467: 9 7 177 6 209 1 567 69
    5693516: 5 692 6 83 833
    2074937264: 367 31 4 829 55 4
    42144296: 421 44 18 7 97 15
    7495776: 976 80 6 1 2 1 2 7 8 2 2 4
    14384647207754: 81 7 2 6 190 88 774 3 8
    6199: 728 52 764 4 8 15
    5390: 7 6 4 9 98
    7823592: 65 6 8 5 379 88 67 1 6 6
    281328049586: 78 146 662 18 44 6 6
    50205462: 5 2 94 763 5 7 6
    74003300: 1 1 34 69 2 10 2 72 45 9
    39396: 3 1 6 503 876 11 7 28
    126238: 64 28 208 91 322 336
    1156: 8 3 5 3 3 3 9 4 40
    7143534: 8 562 34 3 438 7 7 56 9
    991536: 68 8 8 91 2
    9033543629: 1 5 4 471 54 3 1 2 30 3 7
    10532505360: 4 5 84 6 362 93 58 8 1 3
    125763517005: 94 275 50 667 3 2
    734213456446: 734 21 345 64 4 4
    12315361137569: 246 800 824 4 499 9
    1481959: 5 9 8 1 873 4 80
    26004636: 966 11 54 87 1 6 252
    61948096: 291 473 1 634 7 8 8
    128705: 2 9 6 6 58 202 1 9 36 5
    328819545: 7 8 74 9 4 4 7 7 29 5 9 6
    6197401308: 37 555 5 165 36 4 946
    33687039: 110 34 3 5 7 4 321 15
    791951381: 7 919 513 1 7 1
    542832660: 3 7 213 55 402 618
    245719: 5 989 50 49 86 878
    2049: 559 675 88 71 656
    888: 5 8 36 333 83 4
    221227200: 62 9 9 1 6 6 5 6 3 2 72 5
    1034939: 921 8 6 533 705
    1614816868: 168 96 20 16 865
    3324090: 851 9 62 7 84
    41394: 859 48 52 44 66
    7674864: 929 688 417 1 6 4 8 2
    431199726: 6 9 8 3 55 45 82 78 8 4 9
    84348935: 305 3 97 91 3 12 5
    67617038: 92 5 6 6 60 4 170 38 3
    3787281: 15 6 475 6 686 38 557
    28847: 8 9 14 490 19 385
    2058: 3 3 7 5 6 9 3 37 74
    164010: 584 3 492 152
    187416: 2 16 7 4 2 411 2
    11254793: 9 5 677 85 86 758 9
    51624553217: 7 455 1 9 8 9 3 2 4 4 3 17
    95965731: 848 3 3 188 8 258 3 17
    3478627: 8 1 35 1 7 956 69 6 2 2
    20376955: 57 1 471 759 83
    47117324796: 1 891 83 691 921
    642897196082: 191 3 3 8 45 7 20 65 48
    90632: 6 598 2 15 4
    1549951200: 1 4 666 918 30 84
    72286978: 3 577 720 58 418
    2081024608: 7 45 243 6 704 6 1 1
    925: 5 6 2 6 50 4 1 9 31 4 224
    1542524: 4 3 45 9 5 93 484 6 62
    264919974: 705 193 59 40 33 99
    26231083: 4 828 18 1 44
    799624751: 7 792 624 74 9
    93334395: 93 3 34 351 41
    15894: 95 2 4 53 70 18
    171418354: 58 8 606 5 83 39 386
    1014: 95 328 6 490 91 4
    59103851: 738 27 43 11 917 53 8
    2735073067: 706 737 6 645 877
    689930489: 76 8 7 1 6 2 35 4 1 8 89
    32055467: 1 505 7 9 8 169 4 52 7 5
    1360: 6 55 942 88
    15481331721: 8 2 36 6 9 2 707 44 8 5 4
    100572: 35 32 22 97 8 3 243 58
    7355290: 7 9 931 50 341
    2058112: 12 180 50 87 896
    22264: 215 6 81 77 6
    778379496: 7 144 4 844 85 7 70 6
    167449492: 8 91 23 941 1 79
    805817185: 416 24 578 31 1 59 23
    5060882: 6 946 4 891 2 1
    1694525770: 4 1 748 95 1 9 2 29 8 6 2
    30079103: 363 98 8 986 64
    2200: 93 3 449 5 4
    467460: 39 8 926 87 441
    453: 89 280 84
    544244844: 5 17 748 214 1 4 43
    5244: 9 83 57
    1212: 153 297 733 9 20
    1244100: 63 2 22 110 5 26
    657444299: 40 3 938 163 99
    2728679: 8 91 5 5 3 8 62 837 2 59
    542332: 854 635 42
    846: 35 8 51 47 6
    725644227: 4 5 879 122 2 8 6 9 7 61
    145022102431: 8 9 508 3 4 1 9 5 1 6 43 1
    876: 8 6 1 8 6
    5851: 8 71 529 80 6 734 989
    31468537: 5 8 5 5 6 6 7 17 11 7 841
    242326066: 603 28 7 50 990 41 76
    107684254: 197 820 78 51 544
    561595: 618 35 86 9 5
    351895936: 17 5 62 46 365 8 7 8 2
    3541795977209: 62 13 6 7 71 5 30 57
    634922: 2 5 50 66 5 416 607
    342313964: 59 736 5 3 6 3 2 6 239 2
    116467400: 951 6 17 53 6 7 5 40
    779053: 3 8 896 79 420 9
    133094200: 4 51 533 8 69 6 38 410
    4602816: 206 6 3 4 4 9 91 9 7 8 9 8
    2065661: 71 82 570 2 42 34 773
    729849: 8 9 935 4 498
    15336697950: 783 619 146 2 81 925
    11517: 238 2 3 16
    367458: 687 186 4 3 9 5 8 5 4 7
    2460: 135 6 17 61 2
    223887: 554 27 815 8 5 260 2
    61632673: 616 16 7 8 82 69 70 3
    4471037: 558 88 5 16
    164475: 2 75 63 69 21
    975555: 95 7 6 441 665
    866033: 865 167 56 812
    1748: 93 2 4 35 4 1 85 3 1 1 2
    234720: 519 268 298 188 6
    6280276: 523 2 6 4 27 6
    19449359: 51 68 9 454 40
    12933880: 8 78 7 4 732 7 55 849
    419840: 3 3 30 8 4 1 8 8 444 5 8 4
    23091403567: 6 613 125 761 66 67
    155: 5 3 13 65 5 64
    20003820115: 7 2 4 4 881 5 2 683 8
    1575: 15 7 1 2 63
    67146432128: 87 888 764 1 27
    23009814818: 348 87 8 577 95
    340122101: 1 8 7 7 535 7 2 7 72 54 4
    263688: 65 76 8 2 127 5 3
    34826527085: 8 9 9 2 7 9 1 1 801 2 59 8
    503687: 10 76 1 3 195
    4006545: 446 212 835 7 42
    40606909: 3 3 7 95 49 8 8 3 829
    179784737: 6 193 76 9 736
    450585: 48 9 644 691 255
    2579195852: 95 77 69 35 2 73
    103839: 210 8 6 208 8 87 1 5 4
    4647465: 6 47 5 44 6 56 10 7 695
    184464: 9 35 21 9 61
    3398589: 9 1 18 5 41 157 48 10 6
    12702854517: 33 8 6 428 48 55 62 1
    1294278: 7 7 13 82 6 5 5 3 4 78
    105135: 17 88 1 3 3
    1188: 5 10 2 16 36
    1916745: 4 776 69 917 35 953
    16871276342: 863 6 644 2 2 6 5 3 606
    279844: 9 3 98 4 4
    1192: 6 3 30 431 4 709
    4301908: 74 58 9 878 30
    18189: 5 259 4 27 846 6 3 83 9
    66663: 66 110 1 8 3 539
    20208867: 302 9 361 180 87
    168959727: 129 5 81 362 58 90 43
    2372: 7 5 5 4 2 3 16 1 2 23 5 6
    569188: 6 904 7 815 7 91 165
    3588317: 265 8 15 2 9
    3252950453: 7 208 6 890 18 25 3
    188632637: 314 38 770 6 7 3 4
    912636066: 1 32 6 9 7 8 88 97 54 16
    1622: 6 15 2 451 960
    252690189: 9 7 45 1 256 6 77 205 8
    468570960: 73 3 4 1 229 6 814 6 5 6
    62343938113: 5 60 3 127 5 974 6 1 2 5
    222096608: 56 6 510 2 33 4 5
    139398: 80 6 53 386 9
    529034797: 9 1 744 643 79
    1925: 8 2 839 2 866 113 4 91
    518176: 6 71 7 914 76
    9513895162: 9 51 38 95 162
    354673368: 1 770 8 903 2 278 8 7 9
    131759: 200 9 63 1 88
    6002565488: 1 33 645 6 47 195 486
    39905349: 9 1 7 58 8 6 433 3 5 8 3
    188888: 3 6 355 2 5 1 4 92 3 4 56
    631687680: 37 80 1 9 247 4 24
    571480: 2 7 7 4 2 5 30 70 6 2 7 65
    1214052: 4 8 133 31 718
    9116: 2 428 651 6 74
    251092952: 374 765 5 1 67
    433: 6 67 9 301 50
    6492: 8 2 399 73 35
    159309: 4 99 402 70 47
    1914: 2 20 35 808 336
    174193: 3 3 43 33 512 4 43 64 6
    854: 8 8 195 3 5
    240671: 1 5 225 667 5
    1511739: 1 22 5 901 739
    215358242: 84 70 217 7 174 29 4 4
    3255992449: 82 261 82 18 947
    47565: 1 5 5 52 8 1 1 1 43 5 7 5
    77832: 91 20 8 804 46
    2092627: 22 35 640 4 5 2 7 3 7
    154193: 191 9 2 77 39
    3768353918: 80 92 8 530 64
    240521544: 463 738 1 3 2 6 147 7 1
    8258090: 81 8 4 3 716 4 5 4 4 877
    192792668640: 244 9 92 95 33 89 302
    59859299204747: 91 110 9 4 730 46 7 7 5
    2986437680630: 78 4 9 566 36 806 27
    5235048: 1 7 1 753 555 238 3
    460: 30 358 55 17
    428285: 92 7 1 665 25
    6524446204: 7 1 8 8 76 4 44 615 5 4
    16586695: 909 749 4 2 69 5
    353580: 21 678 73 458 4
    41869915: 410 5 34 83 6 34 3 75
    1527369: 841 686 277 8 85
    131043: 3 4 364 3 5
    4367857: 3 4 653 8 9 1 2 8 7 515 1
    71148: 3 2 9 847 6
    6160682: 3 2 2 182 7 772 8 7 4 6 8
    17335: 9 5 3 972 129 2 7 31
    5831208237015: 949 52 433 229 516
    4096256: 997 8 7 32 332 228 16
    18038316: 52 86 237 519 13 3 34
    1332859027602: 7 5 9 415 806 5 1 1 3 35
    99055: 948 71 97 3 205 3
    2671500369: 68 5 75 4 65 2 3 70
    96103: 96 999 70 84 37 8
    17312100: 38 730 6 26 584 4 4
    1121982660: 8 6 2 53 7 727 9 2 5 5 3 4
    286705: 724 251 147 2 52
    11213870: 92 394 56 1 2 206 466
    5215: 4 2 6 8 6 6 1 829 6 1 21 9
    464941700: 5 8 7 3 60 369 68 5 5
    73447292: 734 4 656 5 729
    201484327: 53 38 58 26 326
    110639: 1 783 319 4 38
    34072231: 2 16 1 4 9 2 652 7 97 4 8
    208967493: 94 8 512 4 71 48 5 9
    18402386: 232 2 988 940 78
    88451: 54 1 384 34 187 3 1
    302591: 21 4 8 7 1 374 91
    1386: 95 8 13 20 27
    21572830: 468 379 1 749 92 34
    7192718: 29 6 8 144 58 38 916 1
    478056832: 5 5 3 4 659 8 8 9 3 7 2 32
    58474780: 371 3 79 5 665
    35991020: 4 400 9 67 5 9 63 260
    1187174: 148 9 6 2 126
    1053: 91 860 29 67 6
    33326462472: 127 2 262 546 78 72
    9661: 198 8 1 42 31 2 878 56
    4602175921846: 10 78 795 6 537 184 6
    64944099677: 3 7 648 5 6 9 9 678
    572460: 6 565 6 81 870
    5149482: 74 30 77 9 490
    54775512: 3 2 2 8 5 2 8 5 98 93 97 8
    32629718730: 75 1 8 9 5 7 2 370 591 7
    33896: 8 4 1 769 75 907 19
    131134: 28 120 39 94
    92568007: 56 1 4 1 7 4 1 5 5 580 5 2
    41876787: 797 8 310 52 664
    1329345990: 6 2 112 9 8 75 490 333
    53530: 974 1 8 9 9 3 1 2 3 2 10 1
    7423097364: 28 55 26 97 366
    197453704737: 98 687 52 21 6 94 91
    31616: 2 76 4 52
    1280: 68 13 11 466 722
    1279619816: 2 32 55 3 783 7 3 96 3 5
    13326: 678 8 2 9 978
    1059491794: 6 1 22 6 14 3 7 3 9 7 91
    82796537: 1 8 1 6 3 3 306 4 1 5 643
    668: 8 73 61 7 14
    517900: 259 4 85 7 66
    113143843: 598 2 7 31 869 7 36
    27826064: 28 133 8 934 4 247 85
    444473008: 7 92 8 20 5 4 3 8 1 74 7 1
    37266: 6 55 75 91 411
    20019226: 9 5 4 45 1 4 4 7 3 9 481 6
    6086155955: 7 38 9 685 814 5 3
    65375606: 6 3 9 91 95 68 9 8 9 5 1
    7875331: 97 279 291
    764: 3 46 25 598 3
    47411676: 5 867 27 45 938 9 9
    2476892: 91 43 9 14 1 8 735 902
    39847: 45 212 155 7 7
    309070: 10 74 612 6 622
    3104: 97 4 8
    17381279597081: 258 649 9 94 3 5 8 5 8 7
    13214520: 62 6 3 846 1 110 2
    181330: 3 9 2 6 8 91 5 3 92 4 274
    13504: 88 2 7 868 9 44 670 8
    3759640: 895 21 5 8 8 8 5
    309436051: 661 136 241 9 179 4
    10746705: 9 6 9 302 5 438 73 1 9
    3732939: 4 72 6 8 8 3 8 392 7 9 7 3
    6888960: 9 6 1 7 3 996 6 260 4 1
    180042454: 12 476 32 1 985 209 5
    58281268740: 41 31 207 52 5 5 852
    633926778132: 2 63 8 9 5 759 6 267
    10526516: 55 6 911 5 638 7
    180858154: 603 98 6 5 86 67 90
    19793: 3 77 21 65 9 3 6 65
    133941: 2 6 73 19 98 26 3 8 3 8
    9234432: 50 1 718 24 501
    305850: 1 8 9 6 92 46 9 986 5 1 6
    488424846: 4 5 60 5 37 75 3 33
    8741: 82 94 72 794 167
    15132260: 5 7 21 32 72 1 5 4 37
    262850772: 1 9 1 6 1 6 585 3 4 5 395
    80943398: 3 887 293 239 3 1 845
    251045824: 8 8 5 667 3 95 8 7 8 1 7 1
    478106902: 56 900 2 5 2 4 6 2 3 2 5 3
    41058004: 576 891 9 80 4
    756: 1 3 62 299 2 179 210
    60812076: 83 15 57 423 72 207 9
    113272754: 8 7 1 516 4 4 7 9 3 35 14
    2701: 5 28 79 135 7 971 1
    43971093: 39 73 26 2 22 31 9 3
    38131237: 576 662 36
    1728758: 1 560 54 87 57 119
    298883: 70 322 13 1 736 67
    830166975: 38 4 1 3 5 4 5 5 9 99 909
    11478647: 3 625 5 4 8 9 33 34 77
    20997896: 5 784 7 8 7 2 2 76 821
    72473747: 69 5 81 209 814 71
    13554369736: 5 480 2 2 151 9 91
    6865: 380 2 98 8 1
    3950250: 5 96 46 7 750
    51674: 4 463 49 73
    1058932300: 6 2 95 3 510 418 5 908
    991: 4 827 59 4 97
    766839727: 22 1 5 59 218 68 3 727
    404736298: 3 89 866 5 508
    932424909: 47 6 677 74 66 8 925
    25246464: 78 274 7 845 568 2
    14471: 56 85 3 7 1
    110088: 60 715 2 90 28 66
    534247788: 2 4 243 9 3 3 8 6 813 9 5
    840608: 20 42 51 9 7
    5706186885: 21 6 5 17 5 5 4 8 5 5 87
    78308685314: 6 2 6 853 3 1 6 3 5 9 3 12
    1419464852: 4 61 6 1 2 2 8 81 4 949 2
    108735656: 628 8 301 568 8
    7696: 9 1 673 925 1 41
    46943362: 6 814 8 47 47 6 2
    40247145: 9 6 6 4 9 2 8 7 2 163 93
    239272062521: 946 3 34 843 522
    5262989265: 216 877 9 49 63 9
    68610525757: 820 7 836 575 6
    3395083: 23 87 7 311 732 2 64 3
    21036672: 23 8 5 606 6 88 878 96
    1957996: 15 7 30 13 619 95 2 2
    585808: 2 2 776 36 7 425 4 41
    1013: 4 916 93 1
    6531840: 5 760 189 2 3
    96324058882: 41 1 779 7 70 46 64
    726356: 8 892 807 49 7
    232961: 5 842 13 3 59 3 7 425 3
    54308163716: 332 791 47 48 44 2 4
    2047257600: 61 64 368 95 15
    1191: 55 7 7 4 648 46 9 40 7 3
    3735396: 8 6 82 2 65 95 7 9 972
    8188434: 5 6 6 330 1 4 429 5 101
    534147283: 661 808 8 584 80
    562736: 17 58 570 1 716
    3049191: 254 6 596 2
    3201219: 49 7 183 1 51
    3505519351: 130 802 96 268 21
    19744: 3 85 56 8 4
    5029416: 6 8 4 9 457 22 7 53 7 63
    56992366: 55 9 257 7 64 46
    17168816: 778 922 3 9 1 2 5 4 6 8 7
    8667304436: 1 8 801 461 36 7 4 26 8
    464: 1 1 9 22 8 92 8 2 8 26 4 4
    3668788: 6 7 2 9 6 3 9 6 9 50 786
    7632436: 3 49 45 6 62 24 6 8 4
    1180748756: 396 8 8 7 71 656 580
    108318735: 4 7 4 623 945
    3810070547: 81 4 8 367 2 74 56 1
    67298: 6 4 26 2 930 338
    177673: 7 4 240 7 6
    67819383: 391 73 88 45 27
    564439116: 5 7 489 1 63 4 2 1 9 11 3
    162563277: 794 9 799 839 79 843
    56512: 92 612 1 8 6 170 23
    106965900550: 1 3 5 44 85 60 3 3 550
    670248: 198 35 88 261 8
    314357754081: 8 93 7 411 943 852 40
    801360: 1 7 6 5 9 53 7
    384611: 4 661 2 2 6 9 387 7 2 4 3
    317011: 829 9 1 7 9 8 6 26 6 3 3 1
    1663350: 233 460 7 5 53 78 91 6
    1537513: 8 480 1 315
    810688: 42 9 75 503 1 848
    14004: 2 6 30 377 748 12
    1656: 75 279 323 51 928
    7060: 50 1 6 66 9 5 2 336 18
    5640209: 28 20 4 2 432 34 4 13
    108277765447677: 638 9 426 8 80 954 58
    360644234: 891 216 97 651 5
    262529: 60 8 89 7 4 2 17 1 7 2 4 2
    5700339: 5 693 45 689 1
    1774141600: 347 35 88 830 2
    46740: 6 3 348 5
    1060456: 206 6 269 704 5 931
    596222: 94 532 35 1 902
    12851840701: 2 3 3 367 5 4 8 1 6 85 8 8
    583379075232: 720 221 81 3 8 27 232
    1843150881: 9 8 73 347 538
    234787: 6 1 2 3 3 88 195 6 1 3 1 7
    1561994: 40 152 6 25 7 996
    96350: 166 83 96 4 5 379 350
    570419: 30 54 248 314 883
    61860960: 684 952 95
    4746195762: 6 4 3 73 1 23 915 5 7 5 3
    8925407: 383 186 253 62 40 33
    2594272537: 9 249 1 361 63 3 253 5
    30120688: 8 5 96 2 4 62 4 55 892 4
    11376: 99 24 8 782 662
    15552279095: 8 40 972 4 44 7 8 1 5 9 2
    33363: 2 54 3 552 85 710
    25612217: 4 680 21 8 42 448 57
    5878247: 84 82 6 79 843 8
    93617036447: 5 7 78 170 36 44 7
    3176208: 5 179 7 2 3 26 1 5 6 4 3 7
    4112: 4 35 646 4 968
    4663127: 80 757 77 2 5 1
    37874279866: 58 1 653 279 866
    8882: 6 16 659 112 21 2
    1963040627: 6 45 3 3 5 767 1 625 2
    7243900: 96 572 9 214 50
    176143974: 2 4 790 6 929
    3278: 6 13 7 6 2
    32679125: 3 27 2 711 2 766 30 3
    2728516644: 5 1 535 16 644
    247050: 1 2 5 85 2 8 8 5 5 1 9 225
    835: 37 754 44
    2193052513512: 3 124 78 50 1 50 9 1 5
    18236: 1 1 9 975 683
    112: 3 94 3 6 6
    63974400: 8 3 2 1 87 82 51 8 8 5 20
    307314: 626 3 49 1 430
    86455: 8 1 9 8 494 7 5
    20075924: 935 7 2 7 24 5 51 9 3 3
    382900981062: 68 6 8 825 35 566 59
    648: 63 8 2 6 5
    196863812: 38 993 4 612 7 78
    606489088: 9 760 1 2 953 9 93 4
    266676: 98 33 8 4 83 427 71
    86634399403: 855 11 343 16 83 401
    312256: 3 128 6 211 304 30 2 8
    764832184: 8 756 8 3 21 83
    30193681: 6 972 4 9 35 5 4 55 4 1
    566844936: 9 7 3 425 1 9 2 3 1 7 21 8
    854404: 20 1 613 2 434 502 3
    37342063: 511 534 73 78
    672190: 6 9 1 96 7 72 6 11 59 3
    47508: 3 9 1 3 9 69 4 667 4 9 7 6
    136525: 8 40 69 7 28
    911101433: 8 133 9 9 56 4 6 9 1 2 2 7
    12013: 3 5 740 454 82 2 375
    334185: 1 696 480 8 97
    612: 6 63 21 5 155 7
    885660525: 46 5 7 98 74 66 194 8 2
    45892311291: 845 162 27 543 8 25
    8380066972: 62 9 298 86 2 73 69 1
    16857174: 8 2 4 64 2 2 1 701 7 9 4
    3972772: 2 4 29 5 62 12 346
    911186: 18 34 4 3 379 915 29 3
    13758559: 3 24 5 1 71 2 5
    20069132314: 61 8 3 411 231 1
    1120011235200: 82 3 40 297 440 871
    3447: 193 4 89 45 51 9 9
    1306308: 46 19 2 6 309
    4530260: 3 3 9 11 5 88 6 11 2 6 2 6
    1084857: 2 50 3 80 420
    998072: 49 90 2 7 1
    221: 5 87 7 36 86
    16601636: 4 8 92 3 249 8 1 630 6
    230173: 827 76 28 291 9
    3823537: 284 98 3 537
    1352: 70 1 80 9
    29412: 2 29 5 817
    79075317: 2 5 907 5 232 88
    11048659: 4 57 5 310 27 4 5 259
    20779223: 592 104 6 74 400 9 6 8
    287342: 7 7 85 8 4 3 38 4 4 78
    149190: 1 4 8 9 1 6 2 3 760 56 2 3
    116777190: 8 181 112 9 8 3 133 5 6
    90: 1 9 5 76
    75629: 9 74 91 5 6 41
    934: 52 7 3 9 57 319
    57835: 98 25 86 989 5
    64846479167: 1 38 5 91 5 548 3 9 985
    17854211: 91 8 75 327 8
    5339: 43 7 4 962
    476872: 578 823 7 34 730 407
    356955120: 9 9 6 3 1 5 6 1 9 3 40 34
    485460733: 610 260 558 721 12
    107100270855: 6 7 5 280 3 10 9 5 3 53
    428: 13 3 57 326 6
    20333685: 706 36 1 8 85
    32305: 145 7 763 8 35
    168929: 53 1 6 5 523
    44129553: 1 3 5 992 6 9 7 7 4 8 3 3
    722718109: 7 49 727 44 5 979 729
    115920: 36 3 3 9 966
    3022055838: 29 828 874 8 87 18
    1799107: 17 972 9 95 910
    1939650: 70 9 3 612 534 5
    2238162: 565 330 6 6 37 338 2
    245773680: 74 84 45 865 328
    7593418: 4 283 4 8 4 1 4 1 1 2 59 7
    502208: 28 18 6 1 3 9 944
    136790904: 218 581 72 15 6 258
    254580: 216 190 779 81 530 6
    17371738: 7 8 3 268 1 8 9 1 523 4
    64161748: 1 7 969 8 63 45 1 23 5
    7281279: 94 5 7 477 16 39 9
    11157500: 5 35 1 8 9 1 64 6 43 500
    154800: 8 5 707 5 43
    708110316: 1 5 907 596 4 383 51 6
    7732754: 13 8 8 679 7
    82069: 1 80 58 6 482
    427450: 89 7 9 7 44 5 538 8 228
    4826: 7 3 4 47 878
    471: 10 5 5 8 408
    227405: 53 326 3 20 3
    235299607: 8 2 8 4 3 6 4 2 7 1 540 8
    490175645: 364 7 1 4 8 8 2 241 7 2 3
    835915107977: 971 994 86 26 797 8 1
    5884964: 1 8 6 2 6 488 7 7 7 5 61
    550094388: 707 5 472 683 7 201
    318710669: 5 5 1 5 314 1 7 3 28 468
    5600: 513 1 787 4 96 4
    46391024: 724 4 1 83 193
    59: 8 4 2 8 8 2
    4083840: 506 9 92 879 5 1
    439434: 8 13 697 6 169 576 6
    2811024666: 10 24 7 813 898 9
    583273440000: 24 45 136 90 3 5 10 40
    286136214: 6 9 9 3 2 774 2 54 3 1 3 7
    1333880907: 6 47 473 2 10 910
    2355280: 376 93 3 998 5
    20321002: 762 6 5 8 577 26
    423945: 3 88 498 671 4 59 5 1 5
    16426431: 30 7 9 62 145 57 3 7
    281737257: 9 395 7 14 306
    6174: 5 4 3 11 9 9 7 4 401 1 1 6
    783807: 8 9 3 149 6 2 466 50 7
    269982900: 863 711 1 8 7 19 2 55
    1299872160: 3 45 8 664 185 4 31
    18984303: 862 883 998 9 769
    105694400: 1 3 16 761 310 957 3 7
    2672191: 6 69 7 2 461 235
    8763983355: 871 5 398 335 5
    491143421: 151 75 278 2 3 78 587
    899436: 39 1 959 1 70 5 3 8 4 7 1
    2549160: 4 84 1 16 45 2 219 8
    7134: 625 954 4 1 817
    4988: 61 3 5 207 4 2 2 488 6 2
    50360800: 27 617 340 115 2
    2505: 1 4 9 9 85 8 7 5 6 18 591
    191711: 778 528 84 527 11
    5201: 8 5 57 2 586 55
    352: 70 11 7 79 183
    712265: 16 874 30 3 8
    221101568: 43 3 5 1 81 594 1 8 2 32
    141215995: 27 397 6 982 15 1 994
    5561270: 706 8 80 212 97 66
    376656: 72 4 708 7
    51467130: 2 67 9 2 2 8 7 62 645
    6137780: 996 6 17 3 87 6 5 553
    4898: 98 4 4 956 6
    158190697436: 87 8 5 3 3 9 2 6 93 4 435
    67914357: 7 18 77 7 355
    41768000: 5 537 55 19 7 46 2 4 5 2
    7275836828657: 7 2 75 83 6 825 3 64 5 9
    71485491: 8 5 2 627 85 4 74 6 5 3 2
    9305: 9 67 5 1 1 653 9 8
    324356356: 7 8 6 2 3 587 36 3 4 7 5 6
    731: 605 7 9 69 41
    66026: 6 60 2 1 5
    21310: 59 38 93 7 26 9
    1058: 2 50 328 75 62 1 487 6
    5146656374: 9 3 1 95 46 3 6 156 583
    909854393: 83 7 98 5 439 3
    217008: 1 25 69 5 370 5 2 54
    4140: 99 270 4 7 406
    98898799: 3 71 8 665 40
    10173803: 27 662 8 238 1 62 71
    9122475107: 7 8 937 5 7 8 55 2 649 8
    501300030: 3 696 75 5 48 3 1
    92524836: 871 44 27 997 833
    1083: 9 18 223 29 183 486
    145104: 511 94 3 919 83
    830020: 43 15 227 11 940
    1885846089378: 43 855 21 460 893 78
    2761: 2 2 707 4 84 82 6 2 3 94
    4972838478: 62 159 51 8 40 736 78
    9444670: 943 428 963 4 755
    143451: 42 110 3 40 9 93 7
    1320903: 80 5 37 420 3
    59975991: 2 737 7 5 2 9 2 2 8 9 9 9
    410495: 689 6 4 46 551
    6650280: 1 259 9 406 7
    9702: 1 6 27 3 61 99
    1672706355: 1 8 6 8 484 2 4 2 9 1 5 6
    4712167: 37 61 17 8 3 4 2
    10268874330: 25 2 93 56 717 1 5 5 11
    31183391086: 2 71 5 5 23 68 9 10 1 8 4
    528769: 65 713 1 95 605
    158899: 9 73 76 899
    8528100: 9 1 7 80 62 917 1
    6555444948: 46 7 8 3 5 8 9 56 28 9 45
    243: 51 68 36 56 32
    1374665: 774 2 8 6 2 5 1 1 88 8 1 9
    225346: 56 3 635 6 554
    26839: 4 5 259 40
    7727390828551: 1 373 976 67 3 607 95
    642: 4 7 2 469 58 78 7
    44820: 50 3 7 3 249
    142834138347: 253 9 33 784 79 9 5 8 2
    19706: 4 3 544 35 421
    5680028645: 3 3 720 4 4 8 3 913 4 5 5
    2215710294: 443 5 683 27 294
    184606: 3 92 5 47 4 6 2 7 266 1 2
    7581185: 1 14 4 95 2 45 696 182
    10407906: 25 42 7 59 1 6 62 4 27 7
    10289341332864: 717 82 93 88 594 2 9 2
    21168000: 7 7 6 5 96 30 5
    18823: 4 34 138 4 51
    606225: 5 2 4 1 5 5 177 287 4 4 8
    694051696: 4 4 258 8 74 4 9 21 71 8
    18958064: 7 73 5 5 36 7 6 59 3 302
    154971: 87 5 2 92 15 4 7 75 96 2
    76662250: 412 205 5 71 2 175
    251: 22 6 9 9 97 4
    35044644: 80 438 3 43 45
    678586: 9 18 6 7 5 50 41 806 4
    148471723: 74 248 5 809
    29901058008: 8 8 25 8 1 4 60 2 23 5 1 6
    352071: 8 74 2 33 9 423
    138236449814: 67 2 69 23 6 446 3 816
    62755842240: 117 86 48 5 46 39 718
    107589: 79 7 417 3 3
    379915351797: 87 687 738 87 392 99
    2705125499277: 2 290 583 6 874 8 75
    15080580235: 83 78 10 18 23 5
    3242624: 47 1 736 88 47
    193250: 1 2 66 4 9 3 5 10 72 4 8 2
    698608616: 698 608 60 1 6
    194180612: 275 9 8 7 95 612
    8334: 1 4 4 9 4 9 994 790
    350: 1 12 39 299
    127343: 4 37 3 60 363 6 6 99
    96123936: 480 61 67 28 5 2 27
    7487302: 39 195 653 49 3
    2183345: 26 611 684 2 893 66 5
    6341012623742: 63 9 86 991 23 742
    1263771: 970 293 102 662 7
    5737147: 3 839 53 43 93 811
    9709: 7 136 6 2 178
    3312135616: 303 9 45 38 6 8 8 4 2 97
    37623264374: 440 13 4 246 5 347 77
    364224862: 76 8 5 6 66 9 731 8 58
    4051741742: 404 6 5 741 73 6 9
    56207834: 1 5 8 521 5 7 5 4 55 5 5 9
    436755751: 60 13 919 631 63
    236744640: 583 47 45 96 2
    1605850269: 1 1 8 5 9 4 4 2 78 9 6 6
    1000: 72 44 68 45 771
    1907409383: 57 82 8 2 23 408
    6820: 515 4 46 4 3 8 2 7 5 4 14
    86833903649: 8 683 3 90 36 4 8
    13235641620: 8 943 74 5 4 4 1 3 1 6 2
    65018880: 68 5 332 9 64
    160976773: 91 196 50 3 9 7 3
    12290852684: 569 4 6 42 96 733 1 7 9
    3347214: 333 5 8 66 355 4
    23508919: 289 481 97 169 5
    1905: 1 9 1 26 4 239 725 1 1 1
    863: 63 3 6 4 664
    432: 1 1 4 72
    48393009627: 93 3 17 8 6 3 38 4 5 85
    310388827: 4 124 32 56 42 88
    301782: 301 24 6 458 81
    785000: 48 406 79 58 40
    1313356: 820 2 9 394 1 918 6 4
    18887792: 8 7 3 2 20 1 206 1 7 8 3 8
    1145415600: 73 977 278 4 981 2
    378775671: 434 1 65 29 463
    1288172657509: 49 9 7 3 67 7 178 92 1 4
    37817: 35 2 1 529 1 256 2
    27275453: 505 9 9 6 53
    254012899: 33 5 15 279 843 6 139
    56178: 4 21 789 4 16 4 50
    62169185: 811 8 1 13 36 3 54 56 9
    53018646161867: 73 637 6 12 6 161 86 7
    54260: 4 85 607 98 130 9
    14719: 7 7 530 5 1 153 5 352 2
    8713193974: 87 13 1 86 1 7 971
    147714408: 32 782 14 9 60 927 24
    7796962715: 77 89 7 9 627 1 7 1 1
    506321: 76 723 88 7 2 118 80 1
    7741: 12 645 2 2 1
    446832061: 291 30 48 290 62
    42481444: 3 9 8 607 3 6 6 5 9 131 8
    9440244: 7 9 4 585 2 2 9 48 9 1 9 2
    179628814: 2 452 105 2 340 32 7 9
    358: 2 3 32 8 54
    658308: 215 9 5 410 52 7 95 7
    132300: 79 6 31 6 9
    1046239482067: 577 634 286 206 7
    2317504586: 7 725 30 45 87
    334470: 9 7 3 7 5 3 7 5 41 178 5
    115342: 69 31 72 670 26 76
    8891711: 934 69 457 137
    29684889: 5 3 70 6 8 89
    8835139819: 66 130 255 139 818
    4835606: 519 23 5 3 81 878
    151001112695: 604 5 5 1 11 251 3 181
    1068: 5 4 2 8 7 8 4
    688: 85 8 8
    1450840: 17 398 92 38
    1548075: 539 33 87 599 7
    13022118: 58 6 6 2 1 9 1 210 3 5 9
    651: 5 9 5 6 2 461 7 8 1 1 59 2
    248: 2 2 72 4 3 8
    306272680: 34 235 7 74 74
    694: 127 3 7 5 9
    5404910426474: 7 50 682 9 8 26 472 5
    804888: 6 8 373 2 28
    3678: 9 4 21 97 380
    778815: 43 4 6 6 9 3 6 2 1 6 243
    1740000370: 7 6 9 5 1 15 50 8 5 368
    1494946: 93 434 2 4 2
    410890572: 14 575 699 3 998
    536777943: 7 567 17 76 8 3 828 55
    173408: 86 647 35 2 43
    275498: 44 1 85 72 98
    410244482214: 54 37 354 9 621 934
    16846096: 83 775 9 2 3 727 52
    396000: 3 6 2 3 242 10 3 3 4 4
    90033: 900 24 2 6 3
    31115538: 357 4 345 747 59
    9150711: 26 9 1 30 2 84 408 87
    13270336: 5 93 5 3 4 2 9 373 4 8 9 7
    71272969: 523 3 7 8 46 518 806 1
    113797338: 76 291 522 4 594 6 1
    153431: 36 3 3 401 7 59 5 5 321
    31622640411: 3 226 80 49 2 2 11 1
    """;
}
