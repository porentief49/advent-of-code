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
}

