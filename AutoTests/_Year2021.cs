using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Puzzles;

namespace _Year2021 {
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
            Helpers.Puzzle = new Year2021.Day01();
            Helpers.InputPrefix = @"2021\01_";
        }

        [TestMethod]
        public void _Part1_Example() => Helpers.RunTest("Example", true, "7");

        [TestMethod]
        public void _Part2_Example() => Helpers.RunTest("Example", false, "5");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "1292");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "1262");
    }

    [TestClass]
    public class _Day02 {
        [TestInitialize]
        public void _Init() {
            Helpers.Puzzle = new Year2021.Day02();
            Helpers.InputPrefix = @"2021\02_";
        }

        [TestMethod]
        public void _Part1_Example() => Helpers.RunTest("Example", true, "150");

        [TestMethod]
        public void _Part2_Example() => Helpers.RunTest("Example", false, "900");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "1868935");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "1965970888");
    }
}

