using Microsoft.VisualStudio.TestTools.UnitTesting;
using Puzzles;

namespace _Year2022
{
    [TestClass]
    public static class Helpers
    {
        public static void SolveOfficial(DayBase Puzzle, string InputFile, bool Part1, string Expect)
        {
            Puzzle.BareOutput = true;
            Puzzle.Init(InputFile);
            Assert.AreEqual(Expect, Puzzle.Solve(Part1));
        }
    }

    [TestClass]
    public class _Day01
    {
        [TestMethod]
        public void _Part1_Official() => Helpers.SolveOfficial(new Year2022.Day01(), @"2022\01_rAiner.txt", true, "69795");

        [TestMethod]
        public void _Part2_Official() => Helpers.SolveOfficial(new Year2022.Day01(), @"2022\01_rAiner.txt", false, "208437");
    }

    [TestClass]
    public class _Day02
    {
        [TestMethod]
        public void _Part1_Official() => Helpers.SolveOfficial(new Year2022.Day02(), @"2022\02_rAiner.txt", true, "13675");

        [TestMethod]
        public void _Part2_Official() => Helpers.SolveOfficial(new Year2022.Day02(), @"2022\02_rAiner.txt", false, "14184");
    }
}
