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

        [TestMethod]
        public void _Part1_SEGCC() => Helpers.RunTest("SEGCC", true, "70509");

        [TestMethod]
        public void _Part2_SEGCC() => Helpers.RunTest("SEGCC", false, "208567");
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

        [TestMethod]
        public void _Part1_SEGCC() => Helpers.RunTest("SEGCC", true, "9651");

        [TestMethod]
        public void _Part2_SEGCC() => Helpers.RunTest("SEGCC", false, "10560");
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

        [TestMethod]
        public void _Part1_SEGCC() => Helpers.RunTest("SEGCC", true, "7850");

        [TestMethod]
        public void _Part2_SEGCC() => Helpers.RunTest("SEGCC", false, "2581");
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

        [TestMethod]
        public void _Part1_SEGCC() => Helpers.RunTest("SEGCC", true, "562");

        [TestMethod]
        public void _Part2_SEGCC() => Helpers.RunTest("SEGCC", false, "924");
    }

    [TestClass]
    public class _Day05
    {
        [TestInitialize]
        public void _Init()
        {
            Helpers.Puzzle = new Year2022.Day05();
            Helpers.InputPrefix = @"2022\05_";
        }

        [TestMethod]
        public void _Part1_Example() => Helpers.RunTest("Example", true, "CMZ");

        [TestMethod]
        public void _Part2_Example() => Helpers.RunTest("Example", false, "MCD");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "SPFMVDTZT");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "ZFSJBPRFP");

        [TestMethod]
        public void _Part1_SEGCC() => Helpers.RunTest("SEGCC", true, "QNNTGTPFN");

        [TestMethod]
        public void _Part2_SEGCC() => Helpers.RunTest("SEGCC", false, "GGNPJBTTR");
    }

    [TestClass]
    public class _Day06
    {
        [TestInitialize]
        public void _Init()
        {
            Helpers.Puzzle = new Year2022.Day06();
            Helpers.InputPrefix = @"2022\06_";
        }

        [TestMethod]
        public void _Part1_Example1() => Helpers.RunTest("Example1", true, "7");

        [TestMethod]
        public void _Part1_Example2() => Helpers.RunTest("Example2", true, "5");

        [TestMethod]
        public void _Part1_Example3() => Helpers.RunTest("Example3", true, "6");

        [TestMethod]
        public void _Part1_Example4() => Helpers.RunTest("Example4", true, "10");

        [TestMethod]
        public void _Part1_Example5() => Helpers.RunTest("Example5", true, "11");

        [TestMethod]
        public void _Part2_Example1() => Helpers.RunTest("Example1", false, "19");

        [TestMethod]
        public void _Part2_Example2() => Helpers.RunTest("Example2", false, "23");

        [TestMethod]
        public void _Part2_Example3() => Helpers.RunTest("Example3", false, "23");

        [TestMethod]
        public void _Part2_Example4() => Helpers.RunTest("Example4", false, "29");

        [TestMethod]
        public void _Part2_Example5() => Helpers.RunTest("Example5", false, "26");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "1542");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "3153");

        [TestMethod]
        public void _Part1_SEGCC() => Helpers.RunTest("SEGCC", true, "1361");

        [TestMethod]
        public void _Part2_SEGCC() => Helpers.RunTest("SEGCC", false, "3263");
    }

    [TestClass]
    public class _Day07
    {
        [TestInitialize]
        public void _Init()
        {
            Helpers.Puzzle = new Year2022.Day07();
            Helpers.InputPrefix = @"2022\07_";
        }

        [TestMethod]
        public void _Part1_Example() => Helpers.RunTest("Example", true, "95437");

        [TestMethod]
        public void _Part2_Example() => Helpers.RunTest("Example", false, "24933642");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "1490523");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "12390492");

        [TestMethod]
        public void _Part1_SEGCC() => Helpers.RunTest("SEGCC", true, "1908462");

        [TestMethod]
        public void _Part2_SEGCC() => Helpers.RunTest("SEGCC", false, "3979145");
    }

    [TestClass]
    public class _Day08
    {
        [TestInitialize]
        public void _Init()
        {
            Helpers.Puzzle = new Year2022.Day08();
            Helpers.InputPrefix = @"2022\08_";
        }

        [TestMethod]
        public void _Part1_Example() => Helpers.RunTest("Example", true, "21");

        [TestMethod]
        public void _Part2_Example() => Helpers.RunTest("Example", false, "8");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "1695");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "287040");

        [TestMethod]
        public void _Part1_SEGCC() => Helpers.RunTest("SEGCC", true, "1669");

        [TestMethod]
        public void _Part2_SEGCC() => Helpers.RunTest("SEGCC", false, "331344");
    }

    [TestClass]
    public class _Day09
    {
        [TestInitialize]
        public void _Init()
        {
            Helpers.Puzzle = new Year2022.Day09();
            Helpers.InputPrefix = @"2022\09_";
        }

        [TestMethod]
        public void _Part1_Example1() => Helpers.RunTest("Example1", true, "13");

        [TestMethod]
        public void _Part2_Example1() => Helpers.RunTest("Example1", false, "1");

        [TestMethod]
        public void _Part1_Example2() => Helpers.RunTest("Example2", true, "88");

        [TestMethod]
        public void _Part2_Example2() => Helpers.RunTest("Example2", false, "36");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "6391");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "2593");

        [TestMethod]
        public void _Part1_SEGCC() => Helpers.RunTest("SEGCC", true, "6642");

        [TestMethod]
        public void _Part2_SEGCC() => Helpers.RunTest("SEGCC", false, "2765");
    }

    [TestClass]
    public class _Day10
    {
        [TestInitialize]
        public void _Init()
        {
            Helpers.Puzzle = new Year2022.Day10();
            Helpers.InputPrefix = @"2022\10_";
        }

        [TestMethod]
        public void _Part1_Example2() => Helpers.RunTest("Example2", true, "13140");

        [TestMethod]
        public void _Part2_Example2() => Helpers.RunTest("Example2", false, "        ");

        [TestMethod]
        public void _Part1_rAiner() => Helpers.RunTest("rAiner", true, "11720");

        [TestMethod]
        public void _Part2_rAiner() => Helpers.RunTest("rAiner", false, "ERCREPCJ");

        [TestMethod]
        public void _Part1_SEGCC() => Helpers.RunTest("SEGCC", true, "13860");

        [TestMethod]
        public void _Part2_SEGCC() => Helpers.RunTest("SEGCC", false, "RZHFGJCB");
    }
}

