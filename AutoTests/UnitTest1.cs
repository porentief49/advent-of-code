using Microsoft.VisualStudio.TestTools.UnitTesting;
using Puzzles;

namespace _Year2022
{
    [TestClass]
    public static class Helpers
    {
        public static void SolveOfficial(DayBase aPuzzle, bool aPart1, string aExpect)
        {
            aPuzzle.BareOutput = true;
            aPuzzle.Init();
            Assert.AreEqual(aExpect, aPuzzle.SolvePuzzle(aPart1));
        }
    }

    [TestClass]
    public class _Day01
    {
        [TestMethod]
        public void _Part1_Official() => Helpers.SolveOfficial(new Year2022.Day01(), true, "69795");

        [TestMethod]
        public void _Part2_Official() => Helpers.SolveOfficial(new Year2022.Day01(), false, "208437");
    }

    [TestClass]
    public class _Day02
    {
        [TestMethod]
        public void _Part1_Official() => Helpers.SolveOfficial(new Year2022.Day02(), true, "13675");

        [TestMethod]
        public void _Part2_Official() => Helpers.SolveOfficial(new Year2022.Day02(), false, "14184");
    }
}
