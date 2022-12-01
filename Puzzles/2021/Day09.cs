using System;
using System.Collections.Generic;
using System.Linq;

namespace Puzzles
{
    public partial class Year2021
    {
        public class Day09 : DayBase
        {
            protected override string Title { get; } = "Day 9 - Smoke Basin";

            public override void Init() => Init(Inputs_2021.Rainer_09);

            public override void Init(string aResource) => Input = Tools.SplitLines(aResource, true);

            public override string SolvePuzzle(bool aPart1)
            {
                int lRowCount = Input.Length;
                int lColCount = Input[0].Length;
                int[,] lMap = new int[lRowCount, lColCount];
                int[,] lBasin = new int[lRowCount, lColCount];
                bool[,] lCovered = new bool[lRowCount, lColCount];
                Queue<(int, int)> lQueue = new Queue<(int, int)>();
                if (Verbose) PrintMap(lMap);
                for (int lRow = 0; lRow < lRowCount; lRow++) for (int lCol = 0; lCol < lColCount; lCol++) lMap[lRow, lCol] = int.Parse(Input[lRow][lCol].ToString());
                if (aPart1)
                {
                    List<int> lLowPoints = new List<int>();
                    for (int lRow = 0; lRow < lRowCount; lRow++)
                    {
                        for (int lCol = 0; lCol < lColCount; lCol++)
                        {
                            bool lLow = true; //assume best
                            if (lRow < (lRowCount - 1)) if (lMap[lRow + 1, lCol] <= lMap[lRow, lCol]) lLow = false;
                            if (lRow > 0) if (lMap[lRow - 1, lCol] <= lMap[lRow, lCol]) lLow = false;
                            if (lCol < (lColCount - 1)) if (lMap[lRow, lCol + 1] <= lMap[lRow, lCol]) lLow = false;
                            if (lCol > 0) if (lMap[lRow, lCol - 1] <= lMap[lRow, lCol]) lLow = false;
                            if (lLow) lLowPoints.Add(lMap[lRow, lCol]);
                        }
                    }
                    if (Verbose) Console.WriteLine($"Low Points: {string.Join(",", lLowPoints)}");
                    return FormatResult(lLowPoints.Sum() + lLowPoints.Count, "risk level sum");
                }
                else
                {
                    int lThisBasinIndex = 1;
                    for (int lRow = 0; lRow < lRowCount; lRow++)
                    {
                        for (int lCol = 0; lCol < lColCount; lCol++)
                        {
                            QueueIfNotAlreadCovered(lRow, lCol);
                            bool lFoundBasin = false; // assume worst
                            while (lQueue.Any())
                            {
                                (int Row, int Col) = lQueue.Dequeue();
                                if (lMap[Row, Col] < 9)
                                {
                                    lBasin[Row, Col] = lThisBasinIndex;
                                    lFoundBasin = true;
                                    if (Row < (lRowCount - 1)) QueueIfNotAlreadCovered(Row + 1, Col);
                                    if (Row > 0) QueueIfNotAlreadCovered(Row - 1, Col);
                                    if (Col < (lColCount - 1)) QueueIfNotAlreadCovered(Row, Col + 1);
                                    if (Col > 0) QueueIfNotAlreadCovered(Row, Col - 1);
                                }
                                else lBasin[Row, Col] = -1;
                            }
                            if (lFoundBasin) lThisBasinIndex++;
                        }
                    }
                    if (Verbose) PrintMap(lBasin);
                    int[] lSizes = new int[lThisBasinIndex];
                    for (int lRow = 0; lRow < lRowCount; lRow++) for (int lCol = 0; lCol < lColCount; lCol++) if (lBasin[lRow, lCol] > 0) lSizes[lBasin[lRow, lCol]]++;
                    if (Verbose) Console.WriteLine($"Basin Sizes: {string.Join(",", lSizes)}");
                    lSizes = lSizes.OrderByDescending(x => x).ToArray();
                    if (Verbose) Console.WriteLine($"Basin Sizes sorted: {string.Join(",", lSizes)}");
                    return FormatResult(lSizes[0] * lSizes[1] * lSizes[2], "product of 3 largest basin sizes");
                }

                void QueueIfNotAlreadCovered(int llRow, int llCol)
                {
                    if (!lCovered[llRow, llCol])
                    {
                        if (lBasin[llRow, llCol] == 0) lQueue.Enqueue((llRow, llCol));
                        lCovered[llRow, llCol] = true;
                    }
                }

                void PrintMap(int[,] aMap)
                {
                    for (int lRow = 0; lRow < lRowCount; lRow++) for (int lCol = 0; lCol < lColCount; lCol++) Console.Write(aMap[lRow, lCol] >= 0 ? string.Format("{0,3}", aMap[lRow, lCol]) : "   ");
                }
            }
        }
    }
}
