using System;
using System.Collections.Generic;
using System.Linq;

namespace Puzzles
{
    public partial class Year2021
    {
        public class Day12 : DayBase_OLD
        {
            protected override string Title { get; } = "Day 12 - Passage Pathing";

            public override void Init() => Init(Inputs_2021.Rainer_12);

            public override void Init(string aResource) => Input = Tools.SplitLines(aResource, true);

            public override string SolvePuzzle(bool aPart1)
            { // no LINQ: ~1s
                const string LABEL_START = "start";
                const string LABEL_END = "end";
                var lConnections = new List<(string From, string To)>();
                var lCaves = new List<string>();
                var lCaveCounts = new Dictionary<string, int>();
                for (int i = 0; i < Input.Length; i++)
                {
                    string[] lSplit = Input[i].Split('-');
                    lCaves.AddIfNew(new string[] { lSplit[0], lSplit[1] });
                    if ((lSplit[0] != LABEL_END) && (lSplit[1] != LABEL_START)) lConnections.Add((lSplit[0], lSplit[1]));
                    if (lSplit[1] != LABEL_END && (lSplit[0] != LABEL_START)) lConnections.Add((lSplit[1], lSplit[0]));
                }
                foreach (string lCave in lCaves) if (IsSmall(lCave)) lCaveCounts.Add(lCave, 0);
                var lPath = new List<string>();
                lPath.Add(LABEL_START);
                var lStack = new Stack<(List<string> Path, Dictionary<string, int> CaveCounts)>();
                lStack.Push((lPath, lCaveCounts));
                int lCount = 0;
                while (lStack.Any())
                {
                    (lPath, lCaveCounts) = lStack.Pop();
                    foreach ((string From, string To) lConnection in lConnections)
                    {
                        if (lConnection.From == lPath.Last())
                        {
                            var lNewPath = new List<string>(lPath);
                            var lNewCaveCounts = new Dictionary<string, int>(lCaveCounts);
                            lNewPath.Add(lConnection.To);
                            if (IsSmall(lConnection.To)) lNewCaveCounts[lConnection.To]++;
                            bool lValid = true;
                            int lCount2 = 0;
                            int lCountMore = 0;
                            foreach (int lCaveCount in lNewCaveCounts.Values)
                            {
                                if (lCaveCount == 2) lCount2++;
                                if (lCaveCount > 2) lCountMore++;
                            }
                            if (lCount2 > (aPart1 ? 0 : 1)) lValid = false;
                            if (lCountMore > 0) lValid = false;
                            if (lValid)
                            {
                                if (lNewPath.Last() == LABEL_END)
                                {
                                    lCount++;
                                    if (Verbose) Console.WriteLine($"{lCount} {string.Join(",", lNewPath)}");
                                }
                                else lStack.Push((lNewPath, lNewCaveCounts));
                            }
                        }
                    }
                }
                if (Verbose) Console.WriteLine($"Total Paths: {lCount}");
                return FormatResult(lCount, "total paths");

                bool IsSmall(string aCave) => aCave[0] > 95;
            }

            public string SolvePuzzleLINQ(bool aPart1)
            { // LINQ: ~26s
                const string LABEL_START = "start";
                const string LABEL_END = "end";
                var lConnections = new List<(string From, string To)>();
                var lCaves = new List<string>();
                for (int i = 0; i < Input.Length; i++)
                {
                    string[] lSplit = Input[i].Split('-');
                    lCaves.AddIfNew(new string[] { lSplit[0], lSplit[1] });
                    if ((lSplit[0] != LABEL_END) && (lSplit[1] != LABEL_START)) lConnections.Add((lSplit[0], lSplit[1]));
                    if (lSplit[1] != LABEL_END && (lSplit[0] != LABEL_START)) lConnections.Add((lSplit[1], lSplit[0]));
                }
                var lPath = new List<string>();
                lPath.Add(LABEL_START);
                var lStack = new Stack<List<string>>();
                lStack.Push(lPath);
                int lCount = 0;
                while (lStack.Any())
                {
                    lPath = lStack.Pop();
                    foreach ((string From, string To) lConnection in lConnections)
                    {
                        if (lConnection.From == lPath.Last())
                        {
                            var lNewPath = new List<string>(lPath);
                            lNewPath.Add(lConnection.To);
                            if (lCaves.Count(i => (IsSmall(i)) && (lPath.Count(ii => ii == i) == 2)) <= (aPart1 ? 0 : 1) && lCaves.Count(i => (IsSmall(i)) && (lPath.Count(ii => ii == i) > 2)) == 0)
                            {
                                if (lNewPath.Last() == LABEL_END)
                                {
                                    lCount++;
                                    if (Verbose) Console.WriteLine($"{lCount} {string.Join(",", lNewPath)}");
                                }
                                else lStack.Push(lNewPath);
                            }
                        }
                    }
                }
                return FormatResult(lCount, "total paths");

                bool IsSmall(string aCave) => (aCave == aCave.ToLower());
            }
        }
    }
}
