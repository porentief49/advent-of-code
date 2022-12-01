using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puzzles
{
    public partial class Year2021
    {
        public class Day15 : DayBase
        {
            protected override string Title { get; } = "Day 15 - Chiton";

            public override void Init() => Init(Inputs_2021.Rainer_15);

            public override void Init(string aResource) => Input = Tools.SplitLinesWithoutEmptyOnes(aResource, true);

            public override string SolvePuzzle(bool aPart1)
            {
                // @@@ use of SortedSet should result in even better performance compared to Dictionary
                //      - but neither SortedSet, SortedList or SortedDictionary work out of the box for our purpose here. Would need to construct a fitting class myself
                // @@@ adding a distance to the accumulated risk levels so far ... not perfectly clean but it works ...
                //      - I guess that's "correct enough" and fulfilling the requirement to "always underestimate"

                //initialize
                int lXcount;
                int lYcount;
                int[,] lRisks;
                int[,] lPath;
                int[,] lRiskSums;
                (int X, int Y)[,] lPredecessors;
                byte[,] lState; // 0 = unknown, 1 = known, 2 = final
                double[,] lEstimatedRisks;
                double[,] lBestCaseRemainingRisks;
                if (aPart1)
                {
                    lXcount = Input.Length;
                    lYcount = Input[0].Length;
                    lRisks = new int[lXcount, lYcount];
                    for (int y = 0; y < lXcount; y++) for (int x = 0; x < lYcount; x++) lRisks[y, x] = int.Parse(Input[y][x].ToString());
                }
                else
                {
                    int lXsmall = Input.Length;
                    int lYsmall = Input[0].Length;
                    lXcount = lXsmall * 5;
                    lYcount = lYsmall * 5;
                    lRisks = new int[lXcount, lYcount];
                    for (int y = 0; y < lXsmall; y++) for (int x = 0; x < lYsmall; x++) lRisks[y, x] = int.Parse(Input[y][x].ToString());
                    for (int xx = 1; xx < 5; xx++) for (int y = 0; y < lXsmall; y++) for (int x = 0; x < lYsmall; x++) lRisks[y, x + xx * lXsmall] = lRisks[y, x + (xx - 1) * lXsmall] < 9 ? lRisks[y, x + (xx - 1) * lXsmall] + 1 : 1;
                    for (int yy = 1; yy < 5; yy++) for (int y = 0; y < lYsmall; y++) for (int x = 0; x < lXcount; x++) lRisks[y + yy * lYsmall, x] = lRisks[y + (yy - 1) * lYsmall, x] < 9 ? lRisks[y + (yy - 1) * lYsmall, x] + 1 : 1;
                }
                lPredecessors = new (int X, int Y)[lXcount, lYcount];
                lPath = new int[lXcount, lYcount];
                lRiskSums = new int[lXcount, lYcount];
                lState = new byte[lXcount, lYcount];
                lEstimatedRisks = new double[lXcount, lYcount];
                lBestCaseRemainingRisks = new double[lXcount, lYcount];
                for (int y = 0; y < lXcount; y++)
                {
                    for (int x = 0; x < lYcount; x++)
                    {
                        lRiskSums[x, y] = int.MaxValue;
                        lPredecessors[x, y] = (-1, -1);
                        lPath[x, y] = -1;
                        lState[x, y] = 0;
                        // crazy: Math.Pow, moving the y-part out of the second loop, pushing this thing into a sub function or calculating only when needed (instead of pre-calculating) all made things slower than this line. 
                        lBestCaseRemainingRisks[x, y] = Math.Sqrt((lXcount - 1 - x) * (lXcount - 1 - x) + (lYcount - 1 - y) * (lYcount - 1 - y));
                    }
                }
                if (Verbose) PrintMap(lRisks, "Risk Levels");

                // A* algorithm
                Dictionary<(int X, int Y), double> lStateOnes = new Dictionary<(int X, int Y), double>();
                lRiskSums[0, 0] = 0;
                lState[0, 0] = 1;
                lEstimatedRisks[0, 0] = 0 + Math.Sqrt((lXcount - 1) * (lXcount - 1) + (lYcount - 1) * (lYcount - 1));
                lStateOnes.Add((0, 0), lEstimatedRisks[0, 0]);

                do
                {
                    double[] lEstimates = lStateOnes.Values.ToArray();
                    double llBestEstimate = lEstimates[0];
                    int lBestIndex = 0;
                    for (int i = 1; i < lEstimates.Length; i++)
                    {
                        if (lEstimates[i] < llBestEstimate)
                        {
                            lBestIndex = i;
                            llBestEstimate = lEstimates[i];
                        }
                    }
                    (int lBestX, int lBestY) = lStateOnes.Keys.ElementAt(lBestIndex);
                    lState[lBestX, lBestY] = 2;
                    lStateOnes.Remove((lBestX, lBestY));
                    if (lBestX > 0) CheckNode(lBestX, lBestY, -1, 0);
                    if (lBestX < lXcount - 1) CheckNode(lBestX, lBestY, 1, 0);
                    if (lBestY > 0) CheckNode(lBestX, lBestY, 0, -1);
                    if (lBestY < lYcount - 1) CheckNode(lBestX, lBestY, 0, 1);
                } while (lStateOnes.Count > 0);

                // walk through path
                int lBestPathRisk = lRiskSums[lXcount - 1, lYcount - 1];
                (int X, int Y) lNode = (lXcount - 1, lYcount - 1);
                do
                {
                    lPath[lNode.X, lNode.Y] = lRisks[lNode.X, lNode.Y];
                    lNode = lPredecessors[lNode.X, lNode.Y];
                } while (lNode.X > -1);
                if (Verbose) PrintMap(lPath, "Best Path");
                return FormatResult(lBestPathRisk, "best path risk sum"); ;

                void CheckNode(int aX, int aY, int aDx, int aDy)
                {
                    int lXnew = aX + aDx;
                    int lYnew = aY + aDy;
                    if (lState[lXnew, lYnew] < 2)
                    {
                        int lNewRiskSum = lRiskSums[aX, aY] + lRisks[lXnew, lYnew];
                        if (lRiskSums[lXnew, lYnew] > lNewRiskSum)
                        {
                            lRiskSums[lXnew, lYnew] = lNewRiskSum;
                            lPredecessors[lXnew, lYnew] = (aX, aY);
                            lEstimatedRisks[lXnew, lYnew] = lNewRiskSum + lBestCaseRemainingRisks[lXnew, lYnew];
                        }
                        if (lState[lXnew, lYnew] == 0)
                        {
                            lState[lXnew, lYnew] = 1;
                            lStateOnes.Add((lXnew, lYnew), lEstimatedRisks[lXnew, lYnew]);
                        }
                        else
                        {
                            lStateOnes[(lXnew, lYnew)] = lEstimatedRisks[lXnew, lYnew];
                        }
                    }
                }

                void PrintMap(int[,] aMap, string aTitle)
                {
                    var lMap = new StringBuilder();
                    for (int y = 0; y < lYcount; y++) for (int x = 0; x < lXcount; x++) lMap.Append((aMap[y, x] >= 0 ? aMap[y, x].ToString() : ".") + (x == lXcount - 1 ? Environment.NewLine : string.Empty));
                    Console.WriteLine($"{aTitle}:\n{lMap}");
                }
            }

            public string SolvePuzzle_Dijkstra(bool aPart1)
            {
                //initialize
                int lXcount;
                int lYcount;
                int[,] lRisks;
                int[,] lPath;
                int[,] lRiskSums;
                (int X, int Y)[,] lPredecessors;
                bool[,] lExhausted;
                int lRemainingNodesCount;
                if (aPart1)
                {
                    lXcount = Input.Length;
                    lYcount = Input[0].Length;
                    lRisks = new int[lXcount, lYcount];
                    for (int y = 0; y < lXcount; y++) for (int x = 0; x < lYcount; x++) lRisks[y, x] = int.Parse(Input[y][x].ToString());
                }
                else
                {
                    int lXsmall = Input.Length;
                    int lYsmall = Input[0].Length;
                    lXcount = lXsmall * 5;
                    lYcount = lYsmall * 5;
                    lRisks = new int[lXcount, lYcount];
                    for (int y = 0; y < lXsmall; y++) for (int x = 0; x < lYsmall; x++) lRisks[y, x] = int.Parse(Input[y][x].ToString());
                    for (int xx = 1; xx < 5; xx++) for (int y = 0; y < lXsmall; y++) for (int x = 0; x < lYsmall; x++) lRisks[y, x + xx * lXsmall] = lRisks[y, x + (xx - 1) * lXsmall] < 9 ? lRisks[y, x + (xx - 1) * lXsmall] + 1 : 1;
                    for (int yy = 1; yy < 5; yy++) for (int y = 0; y < lYsmall; y++) for (int x = 0; x < lXcount; x++) lRisks[y + yy * lYsmall, x] = lRisks[y + (yy - 1) * lYsmall, x] < 9 ? lRisks[y + (yy - 1) * lYsmall, x] + 1 : 1;
                }
                lPredecessors = new (int X, int Y)[lXcount, lYcount];
                lPath = new int[lXcount, lYcount];
                lRiskSums = new int[lXcount, lYcount];
                lExhausted = new bool[lXcount, lYcount];
                lRemainingNodesCount = lXcount * lYcount - 1;
                for (int y = 0; y < lXcount; y++)
                {
                    for (int x = 0; x < lYcount; x++)
                    {
                        lRiskSums[y, x] = (y == 0 && x == 0) ? 0 : int.MaxValue;
                        lPredecessors[y, x] = (-1, -1);
                        lPath[y, x] = -1;
                    }
                }
                if (Verbose) PrintMap(lRisks, "Risk Levels");

                //Dijkstra algorithm
                (int X, int Y) lNode = (0, 0);
                while (lRemainingNodesCount > 0)
                {

                    //find node with lowest path sum so far
                    bool lFoundLowest = false;
                    int lBestRiskSum = 0;
                    for (int lRow = 0; lRow < lXcount; lRow++)
                    {
                        for (int lCol = 0; lCol < lYcount; lCol++)
                        {
                            if (!lExhausted[lRow, lCol])
                            {
                                if (!lFoundLowest)
                                {
                                    lNode = (lRow, lCol);
                                    lFoundLowest = true;
                                    lBestRiskSum = lRiskSums[lRow, lCol];
                                }
                                else if (lRiskSums[lRow, lCol] < lBestRiskSum)
                                {
                                    lNode = (lRow, lCol);
                                    lBestRiskSum = lRiskSums[lRow, lCol];
                                }
                            }
                        }
                    }

                    //pick lowest distance neighbor
                    int lBestRisk = int.MaxValue;
                    (int X, int Y) lBestNeighbor = lNode;
                    if (lNode.X > 0) CheckBestNeighbor(lNode.X - 1, lNode.Y, ref lBestRisk, ref lBestNeighbor);
                    if (lNode.Y > 0) CheckBestNeighbor(lNode.X, lNode.Y - 1, ref lBestRisk, ref lBestNeighbor);
                    if (lNode.Y < lYcount - 1) CheckBestNeighbor(lNode.X, lNode.Y + 1, ref lBestRisk, ref lBestNeighbor);
                    if (lNode.X < lXcount - 1) CheckBestNeighbor(lNode.X + 1, lNode.Y, ref lBestRisk, ref lBestNeighbor);
                    if (lBestRisk < int.MaxValue)
                    { // ok we found one
                        lRiskSums[lBestNeighbor.X, lBestNeighbor.Y] = lRiskSums[lNode.X, lNode.Y] + lBestRisk;
                        lPredecessors[lBestNeighbor.X, lBestNeighbor.Y] = lNode;
                        lRemainingNodesCount--;
                        if (Verbose) Console.WriteLine(lRemainingNodesCount.ToString());
                        if (lNode.X == lXcount - 1 && lNode.Y == lYcount - 1) break;
                    }
                    else lExhausted[lNode.X, lNode.Y] = true;
                }

                // walk through path
                int lBestPathRisk = lRiskSums[lXcount - 1, lYcount - 1];
                lNode = (lXcount - 1, lYcount - 1);
                do
                {
                    lPath[lNode.X, lNode.Y] = lRisks[lNode.X, lNode.Y];
                    lNode = lPredecessors[lNode.X, lNode.Y];
                } while (lNode.X > -1);
                if (Verbose) PrintMap(lPath, "Best Path");
                return FormatResult(lBestPathRisk, "best path risk sum"); ;

                void CheckBestNeighbor(int aX, int aY, ref int aBestRisk, ref (int X, int Y) aBestNeighbor)
                {
                    if (lRiskSums[aX, aY] == int.MaxValue)
                    {
                        if (lRisks[aX, aY] < aBestRisk)
                        {
                            aBestRisk = lRisks[aX, aY];
                            aBestNeighbor = (aX, aY);
                        }
                    }
                }

                void PrintMap(int[,] aMap, string aTitle)
                {
                    var lMap = new StringBuilder();
                    for (int y = 0; y < lYcount; y++) for (int x = 0; x < lXcount; x++) lMap.Append((aMap[y, x] >= 0 ? aMap[y, x].ToString() : ".") + (x == lXcount - 1 ? Environment.NewLine : string.Empty));
                    Console.WriteLine($"{aTitle}:\n{lMap}");
                }
            }
        }
    }
}
