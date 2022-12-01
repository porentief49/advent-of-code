using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puzzles
{
    public partial class Year2021
    {
        public class Day11 : DayBase
        {
            protected override string Title { get; } = "Day 11 - Dumbo Octopus";

            public override void Init() => Init(Inputs_2021.Rainer_11);

            public override void Init(string aResource) => Input = Tools.SplitLines(aResource, true);

            public override string SolvePuzzle(bool aPart1)
            {
                const int STEP_COUNT = 100;
                int lRowCount = Input.Length;
                int lColCount = Input[0].Length;
                var lOctoLevels = new int[lRowCount, lColCount];
                var lFlash = new bool[lRowCount, lColCount];
                for (int lRow = 0; lRow < lRowCount; lRow++) for (int lCol = 0; lCol < lColCount; lCol++) lOctoLevels[lRow, lCol] = int.Parse(Input[lRow][lCol].ToString());
                if (Verbose) PrintMap(lOctoLevels, "Before any steps", 0);
                int lTotalFlashCount = 0;
                int lThisFlashCount;
                int lStep = 1;
                bool lKeepGoing;
                do
                {
                    lThisFlashCount = 0;
                    var lIncs = new Queue<(int lRow, int lCol)>();
                    for (int lRow = 0; lRow < lRowCount; lRow++)
                    {
                        for (int lCol = 0; lCol < lColCount; lCol++)
                        {
                            lIncs.Enqueue((lRow, lCol));
                            lFlash[lRow, lCol] = false;
                        }
                    }
                    while (lIncs.Any())
                    {
                        (int Row, int Col) lThis = lIncs.Dequeue();
                        if (!lFlash[lThis.Row, lThis.Col])
                        {
                            if (lOctoLevels[lThis.Row, lThis.Col] < 9) lOctoLevels[lThis.Row, lThis.Col]++;
                            else
                            {
                                lOctoLevels[lThis.Row, lThis.Col] = 0;
                                lFlash[lThis.Row, lThis.Col] = true;
                                lThisFlashCount++;
                                if (lThis.Row > 0)
                                {
                                    if (lThis.Col > 0) lIncs.Enqueue((lThis.Row - 1, lThis.Col - 1));
                                    lIncs.Enqueue((lThis.Row - 1, lThis.Col));
                                    if (lThis.Col < lColCount - 1) lIncs.Enqueue((lThis.Row - 1, lThis.Col + 1));
                                }
                                if (lThis.Col > 0) lIncs.Enqueue((lThis.Row, lThis.Col - 1));
                                if (lThis.Col < lColCount - 1) lIncs.Enqueue((lThis.Row, lThis.Col + 1));
                                if (lThis.Row < lRowCount - 1)
                                {
                                    if (lThis.Col > 0) lIncs.Enqueue((lThis.Row + 1, lThis.Col - 1));
                                    lIncs.Enqueue((lThis.Row + 1, lThis.Col));
                                    if (lThis.Col < lColCount - 1) lIncs.Enqueue((lThis.Row + 1, lThis.Col + 1));
                                }
                            }
                        }
                    }
                    lTotalFlashCount += lThisFlashCount;
                    if (Verbose) PrintMap(lOctoLevels, $"After step {lStep}", lThisFlashCount);
                    lKeepGoing = (aPart1 ? lStep < STEP_COUNT : lThisFlashCount != lRowCount * lColCount);
                    lStep++;
                } while (lKeepGoing);
                if (aPart1) return FormatResult(lTotalFlashCount, "total flash count");
                return FormatResult(lStep - 1, "first simultaneous flash after");

                void PrintMap(int[,] aMap, string aTitle, int aFlashCount)
                {
                    var lMap = new StringBuilder();
                    for (int lRow = 0; lRow < lRowCount; lRow++) for (int lCol = 0; lCol < lColCount; lCol++) lMap.Append(aMap[lRow, lCol].ToString() + (lCol == lColCount - 1 ? Environment.NewLine : string.Empty));
                    Console.WriteLine($"{aTitle}:\n{lMap}--> Flash Count: {aFlashCount}\n");
                }
            }
        }
    }
}
