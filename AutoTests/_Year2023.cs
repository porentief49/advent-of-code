using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Puzzles;

namespace _Year2023 {
    [TestClass]
    public static class Helpers {
        private const string _inputExtension = ".txt";
        public static DayBase? Puzzle;
        public static string InputPrefix = string.Empty;

        public static void RunTest(string inputFile, bool part1, string expect) {
            if (Puzzle is not null) {
                Puzzle.BareOutput = true;
                Puzzle.Init(InputPrefix + inputFile + _inputExtension);
                Puzzle.Part1 = part1;
                Assert.AreEqual(expect, Puzzle.Solve());
            } else Assert.Fail("Puzzle not instantiated");
        }
    }

    [TestClass]
    public class _Day01 {
        [TestInitialize]
        public void _Init() {
            Helpers.Puzzle = new Year2023.Day01();
            Helpers.InputPrefix = @"2023\01_";
        }

        [TestMethod]
        public void _Part1_Example() => Helpers.RunTest("Example1", true, "142");

        [TestMethod]
        public void _Part2_Example() => Helpers.RunTest("Example2", false, "281");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "55090");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "54845");
    }

    [TestClass]
    public class _Day02 {
        [TestInitialize]
        public void _Init() {
            Helpers.Puzzle = new Year2023.Day02();
            Helpers.InputPrefix = @"2023\02_";
        }

        [TestMethod]
        public void _Part1_Example() => Helpers.RunTest("Example", true, "8");

        [TestMethod]
        public void _Part2_Example() => Helpers.RunTest("Example", false, "2286");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "2085");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "79315");
    }

    [TestClass]
    public class _Day03 {
        [TestInitialize]
        public void _Init() {
            Helpers.Puzzle = new Year2023.Day03();
            Helpers.InputPrefix = @"2023\03_";
        }

        [TestMethod]
        public void _Part1_Example() => Helpers.RunTest("Example", true, "4361");

        [TestMethod]
        public void _Part2_Example() => Helpers.RunTest("Example", false, "467835");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "560670");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "91622824");
    }

    [TestClass]
    public class _Day04 {
        [TestInitialize]
        public void _Init() {
            Helpers.Puzzle = new Year2023.Day04();
            Helpers.InputPrefix = @"2023\04_";
        }

        [TestMethod]
        public void _Part1_Example() => Helpers.RunTest("Example", true, "13");

        [TestMethod]
        public void _Part2_Example() => Helpers.RunTest("Example", false, "30");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "25651");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "19499881");
    }

    [TestClass]
    public class _Day05 {
        [TestInitialize]
        public void _Init() {
            Helpers.Puzzle = new Year2023.Day05();
            Helpers.InputPrefix = @"2023\05_";
        }

        [TestMethod]
        public void _Part1_Example() => Helpers.RunTest("Example", true, "35");

        [TestMethod]
        public void _Part2_Example() => Helpers.RunTest("Example", false, "46");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "525792406");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "79004094");

        [TestMethod]
        public void _Part1_Jannis() => Helpers.RunTest("Jannis", true, "240320250");

        [TestMethod]
        public void _Part2_Jannis() => Helpers.RunTest("Jannis", false, "28580589");
    }

    [TestClass]
    public class _Day06 {
        [TestInitialize]
        public void _Init() {
            Helpers.Puzzle = new Year2023.Day06();
            Helpers.InputPrefix = @"2023\06_";
        }

        [TestMethod]
        public void _Part1_Example() => Helpers.RunTest("Example", true, "288");

        [TestMethod]
        public void _Part2_Example() => Helpers.RunTest("Example", false, "71503");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "227850");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "42948149");
    }

    [TestClass]
    public class _Day07 {
        [TestInitialize]
        public void _Init() {
            Helpers.Puzzle = new Year2023.Day07();
            Helpers.InputPrefix = @"2023\07_";
        }

        [TestMethod]
        public void _Part1_Example() => Helpers.RunTest("Example", true, "6440");

        [TestMethod]
        public void _Part2_Example() => Helpers.RunTest("Example", false, "5905");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "252052080");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "252898370");
    }

    [TestClass]
    public class _Day08 {
        [TestInitialize]
        public void _Init() {
            Helpers.Puzzle = new Year2023.Day08();
            Helpers.InputPrefix = @"2023\08_";
        }

        [TestMethod]
        public void _Part1_Example() => Helpers.RunTest("Example1", true, "2");

        [TestMethod]
        public void _Part2_Example() => Helpers.RunTest("Example2", false, "6");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "13019");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "13524038372771");
    }

    [TestClass]
    public class _Day09 {
        [TestInitialize]
        public void _Init() {
            Helpers.Puzzle = new Year2023.Day09();
            Helpers.InputPrefix = @"2023\09_";
        }

        [TestMethod]
        public void _Part1_Example() => Helpers.RunTest("Example", true, "114");

        [TestMethod]
        public void _Part2_Example() => Helpers.RunTest("Example", false, "2");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "1938731307");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "948");
    }

    [TestClass]
    public class _Day10 {
        [TestInitialize]
        public void _Init() {
            Helpers.Puzzle = new Year2023.Day10();
            Helpers.InputPrefix = @"2023\10_";
        }

        [TestMethod]
        public void _Part1_Example1() => Helpers.RunTest("Example1", true, "4");

        [TestMethod]
        public void _Part2_Example1() => Helpers.RunTest("Example1", false, "1");

        [TestMethod]
        public void _Part1_Example2() => Helpers.RunTest("Example2", true, "8");

        [TestMethod]
        public void _Part2_Example2() => Helpers.RunTest("Example2", false, "1");

        [TestMethod]
        public void _Part1_Example3() => Helpers.RunTest("Example3", true, "23");

        [TestMethod]
        public void _Part2_Example3() => Helpers.RunTest("Example3", false, "4");

        [TestMethod]
        public void _Part1_Example4() => Helpers.RunTest("Example4", true, "22");

        [TestMethod]
        public void _Part2_Example4() => Helpers.RunTest("Example4", false, "4");

        [TestMethod]
        public void _Part1_Example5() => Helpers.RunTest("Example5", true, "70");

        [TestMethod]
        public void _Part2_Example5() => Helpers.RunTest("Example5", false, "8");

        [TestMethod]
        public void _Part1_Example6() => Helpers.RunTest("Example6", true, "80");

        [TestMethod]
        public void _Part2_Example6() => Helpers.RunTest("Example6", false, "10");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "6968");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "413");
    }

    [TestClass]
    public class _Day11 {
        [TestInitialize]
        public void _Init() {
            Helpers.Puzzle = new Year2023.Day11();
            Helpers.InputPrefix = @"2023\11_";
        }

        [TestMethod]
        public void _Part1_Example() => Helpers.RunTest("Example", true, "374");

        [TestMethod]
        public void _Part2_Example() => Helpers.RunTest("Example", false, "82000210");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "9805264");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "779032247216");
    }

    [TestClass]
    public class _Day12 {
        [TestInitialize]
        public void _Init() {
            Helpers.Puzzle = new Year2023.Day12();
            Helpers.InputPrefix = @"2023\12_";
        }

        [TestMethod]
        public void _Part1_Example() => Helpers.RunTest("Example", true, "21");

        [TestMethod]
        public void _Part2_Example() => Helpers.RunTest("Example", false, "525152");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "7236");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "11607695322318");
    }

    [TestClass]
    public class _Day13 {
        [TestInitialize]
        public void _Init() {
            Helpers.Puzzle = new Year2023.Day13();
            Helpers.InputPrefix = @"2023\13_";
        }

        [TestMethod]
        public void _Part1_Example() => Helpers.RunTest("Example", true, "405");

        [TestMethod]
        public void _Part2_Example() => Helpers.RunTest("Example", false, "400");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "30802");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "37876");
    }

    [TestClass]
    public class _Day14 {
        [TestInitialize]
        public void _Init() {
            Helpers.Puzzle = new Year2023.Day14();
            Helpers.InputPrefix = @"2023\14_";
        }

        [TestMethod]
        public void _Part1_Example() => Helpers.RunTest("Example", true, "136");

        [TestMethod]
        public void _Part2_Example() => Helpers.RunTest("Example", false, "64");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "106378");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "90795");
    }

    [TestClass]
    public class _Day15 {
        [TestInitialize]
        public void _Init() {
            Helpers.Puzzle = new Year2023.Day15();
            Helpers.InputPrefix = @"2023\15_";
        }

        [TestMethod]
        public void _Part1_Example() => Helpers.RunTest("Example", true, "1320");

        [TestMethod]
        public void _Part2_Example() => Helpers.RunTest("Example", false, "145");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "517965");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "267372");
    }

    [TestClass]
    public class _Day16 {
        [TestInitialize]
        public void _Init() {
            Helpers.Puzzle = new Year2023.Day16();
            Helpers.InputPrefix = @"2023\16_";
        }

        [TestMethod]
        public void _Part1_Example() => Helpers.RunTest("Example", true, "46");

        [TestMethod]
        public void _Part2_Example() => Helpers.RunTest("Example", false, "51");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "7562");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "7793");
    }

    [TestClass]
    public class _Day17 {
        [TestInitialize]
        public void _Init() {
            Helpers.Puzzle = new Year2023.Day17();
            Helpers.InputPrefix = @"2023\17_";
        }

        [TestMethod]
        public void _Part1_Example() => Helpers.RunTest("Example", true, "102");

        [TestMethod]
        public void _Part2_Example() => Helpers.RunTest("Example", false, "94");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "855");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "980");
    }

    [TestClass]
    public class _Day18 {
        [TestInitialize]
        public void _Init() {
            Helpers.Puzzle = new Year2023.Day18();
            Helpers.InputPrefix = @"2023\18_";
        }

        [TestMethod]
        public void _Part1_Example() => Helpers.RunTest("Example", true, "62");

        [TestMethod]
        public void _Part2_Example() => Helpers.RunTest("Example", false, "952408144115");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "49578");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "52885384955882");
    }

    [TestClass]
    public class _Day19 {
        [TestInitialize]
        public void _Init() {
            Helpers.Puzzle = new Year2023.Day19();
            Helpers.InputPrefix = @"2023\19_";
        }

        [TestMethod]
        public void _Part1_Example() => Helpers.RunTest("Example", true, "19114");

        [TestMethod]
        public void _Part2_Example() => Helpers.RunTest("Example", false, "167409079868000");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "352052");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "116606738659695");
    }

    [TestClass]
    public class _Day20 {
        [TestInitialize]
        public void _Init() {
            Helpers.Puzzle = new Year2023.Day20();
            Helpers.InputPrefix = @"2023\20_";
        }

        [TestMethod]
        public void _Part1_Example1() => Helpers.RunTest("Example1", true, "32000000");

        //[TestMethod]
        //public void _Part2_Example1() => Helpers.RunTest("Example1", false, "");

        [TestMethod]
        public void _Part1_Example2() => Helpers.RunTest("Example2", true, "11687500");

        //[TestMethod]
        //public void _Part2_Example2() => Helpers.RunTest("Example2", false, "");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "788081152");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "224602011344203");
    }

    [TestClass]
    public class _Day21 {
        [TestInitialize]
        public void _Init() {
            Helpers.Puzzle = new Year2023.Day21();
            Helpers.InputPrefix = @"2023\21_";
        }

        [TestMethod]
        public void _Part1_Example() => Helpers.RunTest("Example", true, "");

        [TestMethod]
        public void _Part2_Example() => Helpers.RunTest("Example", false, "");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "");
    }

    [TestClass]
    public class _Day22 {
        [TestInitialize]
        public void _Init() {
            Helpers.Puzzle = new Year2023.Day22();
            Helpers.InputPrefix = @"2023\22_";
        }

        //[TestMethod]
        //public void _Part1_Example() => Helpers.RunTest("Example", true, "");

        //[TestMethod]
        //public void _Part2_Example() => Helpers.RunTest("Example", false, "");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "3773");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "625628021226274");
    }

    [TestClass]
    public class _Day23 {
        [TestInitialize]
        public void _Init() {
            Helpers.Puzzle = new Year2023.Day23();
            Helpers.InputPrefix = @"2023\23_";
        }

        [TestMethod]
        public void _Part1_Example() => Helpers.RunTest("Example", true, "94");

        [TestMethod]
        public void _Part2_Example() => Helpers.RunTest("Example", false, "154");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "2034");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "6302");
    }

    [TestClass]
    public class _Day24 {
        [TestInitialize]
        public void _Init() {
            Helpers.Puzzle = new Year2023.Day24();
            Helpers.InputPrefix = @"2023\24_";
        }

        [TestMethod]
        public void _Part1_Example() => Helpers.RunTest("Example", true, "2");

        [TestMethod]
        public void _Part2_Example() => Helpers.RunTest("Example", false, "47");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "13910");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "618534564836937");
    }

    [TestClass]
    public class _Day25 {
        [TestInitialize]
        public void _Init() {
            Helpers.Puzzle = new Year2023.Day25();
            Helpers.InputPrefix = @"2023\25_";
        }

        [TestMethod]
        public void _Part1_Example() => Helpers.RunTest("Example", true, "54");

        [TestMethod]
        public void _Part2_Example() => Helpers.RunTest("Example", false, "562912");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "");
    }
}

