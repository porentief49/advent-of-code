using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Puzzles;

namespace _Year2022
{
    [TestClass]
    public static class Helpers
    {
        private const string _inputExtension = ".txt";
        public static DayBase? Puzzle;
        public static string InputPrefix= string.Empty;

        public static void RunTest(string InputFile, bool Part1, string Expect)
        {
            if (Puzzle is not null)
            {
                Puzzle.BareOutput = true;
                Puzzle.Init(InputPrefix + InputFile + _inputExtension);
                Assert.AreEqual(Expect, Puzzle.Solve(Part1));
            }
            else Assert.Fail("Puzzle not instantiated");
        }
    }

    [TestClass]
    public class _Day01
    {
        [TestInitialize]
        public void _Init()
        { 
            Helpers.Puzzle = new Year2022.Day01();
            Helpers.InputPrefix = @"2022\01_";
        }

        [TestMethod]
        public void _Part1_Example() => Helpers.RunTest("Example", true, "24000");

        [TestMethod]
        public void _Part2_Example() => Helpers.RunTest("Example", false, "45000");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "69795");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "208437");
    }

    [TestClass]
    public class _Day02
    {
        [TestInitialize]
        public void _Init()
        {
            Helpers.Puzzle = new Year2022.Day02();
            Helpers.InputPrefix = @"2022\02_";
        }

        [TestMethod]
        public void _Part1_Example() => Helpers.RunTest("Example", true, "15");

        [TestMethod]
        public void _Part2_Example() => Helpers.RunTest("Example", false, "12");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "13675");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "14184");
    }

    [TestClass]
    public class _Day03
    {
        [TestInitialize]
        public void _Init()
        {
            Helpers.Puzzle = new Year2022.Day03();
            Helpers.InputPrefix = @"2022\03_";
        }

        [TestMethod]
        public void _Part1_Example() => Helpers.RunTest("Example", true, "157");

        [TestMethod]
        public void _Part2_Example() => Helpers.RunTest("Example", false, "70");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "7917");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "2585");
    }

    [TestClass]
    public class _Day04
    {
        [TestInitialize]
        public void _Init()
        {
            Helpers.Puzzle = new Year2022.Day04();
            Helpers.InputPrefix = @"2022\04_";
        }

        [TestMethod]
        public void _Part1_Example() => Helpers.RunTest("Example", true, "2");

        [TestMethod]
        public void _Part2_Example() => Helpers.RunTest("Example", false, "4");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "576");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "905");
    }
}

