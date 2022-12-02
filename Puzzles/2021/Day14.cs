using System;
using System.Collections.Generic;
using System.Linq;

namespace Puzzles
{
    public partial class Year2021
    {
        public class Day14 : DayBase_OLD
        {
            protected override string Title { get; } = "Day 14 - Extended Polymerization";

            public override void Init() => Init(Inputs_2021.Rainer_14);

            public override void Init(string aResource) => Input = Tools.SplitLines(aResource, true);

            public override string SolvePuzzle(bool aPart1)
            {
                //build base
                var lCombinationCounts = new Dictionary<string, ulong>();
                for (int i = 0; i < Input[0].Length - 1; i++) AddCombination(Input[0].Substring(i, 2), ref lCombinationCounts, 1);

                // build inserts
                var lInsertsLeft = new Dictionary<string, string>();
                var lInsertsRight = new Dictionary<string, string>();
                for (int i = 1; i < Input.Length; i++)
                {
                    lInsertsLeft.Add(Input[i].Substring(0, 2), Input[i].Substring(0, 1) + Input[i].Substring(6, 1));
                    lInsertsRight.Add(Input[i].Substring(0, 2), Input[i].Substring(6, 1) + Input[i].Substring(1, 1));
                }

                // loop
                int aSteps = (aPart1 ? 10 : 40);
                for (int i = 1; i <= aSteps; i++)
                {
                    var lNewCounts = new Dictionary<string, ulong>();
                    foreach (string lInsert in lInsertsLeft.Keys) if (lCombinationCounts.ContainsKey(lInsert)) AddCombination(lInsertsLeft[lInsert], ref lNewCounts, lCombinationCounts[lInsert]);
                    foreach (string lInsert in lInsertsRight.Keys) if (lCombinationCounts.ContainsKey(lInsert)) AddCombination(lInsertsRight[lInsert], ref lNewCounts, lCombinationCounts[lInsert]);
                    lCombinationCounts = lNewCounts;
                }

                // determine counts
                var lElements = new List<string>();
                foreach (string lCombination in lCombinationCounts.Keys)
                {
                    lElements.AddIfNew(lCombination.Substring(0, 1));
                    lElements.AddIfNew(lCombination.Substring(1, 1));
                }
                ulong lMost = ulong.MinValue;
                ulong lLeast = ulong.MaxValue;
                foreach (string lElement in lElements)
                {
                    ulong lCountOcurrences = 0;
                    foreach (string lCombination in lCombinationCounts.Keys)
                    {
                        if (lCombination[0] == lElement[0]) lCountOcurrences += lCombinationCounts[lCombination];
                        if (lCombination[1] == lElement[0]) lCountOcurrences += lCombinationCounts[lCombination];
                    }
                    //ulong lCount = (lCountOcurrences + 1) / 2;
                    if (lElement[0] == Input[0].First()) lCountOcurrences++;
                    if (lElement[0] == Input[0].Last()) lCountOcurrences++;
                    ulong lCount = lCountOcurrences / 2;
                    if (lCount > lMost) lMost = lCount;
                    if (lCount < lLeast) lLeast = lCount;
                    if (Verbose) Console.WriteLine($"Element: {lElement} -> {lCount}");
                }
                return FormatResult(lMost - lLeast, "element count most-least");

                void AddCombination(string aKey, ref Dictionary<string, ulong> aCombinationCounts, ulong aCount)
                {
                    if (aCombinationCounts.ContainsKey(aKey)) aCombinationCounts[aKey] += aCount;
                    else aCombinationCounts.Add(aKey, aCount);
                }
            }
        }
    }
}
