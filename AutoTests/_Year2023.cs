using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Puzzles;

namespace _Year2023 {
    [TestClass]
    public static class Helpers {
        private const string _inputExtension = ".txt";
        public static DayBase? Puzzle;
        public static string InputPrefix = string.Empty;

        public static void RunTest(string InputFile, bool Part1, string Expect) {
            if (Puzzle is not null) {
                Puzzle.BareOutput = true;
                Puzzle.Init(InputPrefix + InputFile + _inputExtension);
                Assert.AreEqual(Expect, Puzzle.Solve(Part1));
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
        public void _Part1_Example() => Helpers.RunTest("Example", true, "");

        [TestMethod]
        public void _Part2_Example() => Helpers.RunTest("Example", false, "");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "");
    }

    [TestClass]
    public class _Day08 {
        [TestInitialize]
        public void _Init() {
            Helpers.Puzzle = new Year2023.Day08();
            Helpers.InputPrefix = @"2023\08_";
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
    public class _Day09 {
        [TestInitialize]
        public void _Init() {
            Helpers.Puzzle = new Year2023.Day09();
            Helpers.InputPrefix = @"2023\09_";
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
    public class _Day10 {
        [TestInitialize]
        public void _Init() {
            Helpers.Puzzle = new Year2023.Day10();
            Helpers.InputPrefix = @"2023\10_";
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
    public class _Day11 {
        [TestInitialize]
        public void _Init() {
            Helpers.Puzzle = new Year2023.Day11();
            Helpers.InputPrefix = @"2023\11_";
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
    public class _Day12 {
        [TestInitialize]
        public void _Init() {
            Helpers.Puzzle = new Year2023.Day12();
            Helpers.InputPrefix = @"2023\12_";
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
    public class _Day13 {
        [TestInitialize]
        public void _Init() {
            Helpers.Puzzle = new Year2023.Day13();
            Helpers.InputPrefix = @"2023\13_";
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
}

