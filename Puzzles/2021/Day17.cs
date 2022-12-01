using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace Puzzles
{
    public partial class Year2021
    {
        public class Day17 : DayBase
        {
            protected override string Title { get; } = "Day 17 - Trick Shot";

            public override void Init() => Init(Inputs_2021.Rainer_17);

            public override void Init(string aResource) => Input = Tools.SplitLinesWithoutEmptyOnes(aResource, true);

            public override string SolvePuzzle(bool aPart1) => aPart1 ? Part1(Input[0]) : Part2(Input[0]);

            private string Part1(string aTargetInput)
            {
                string[] lSplit = aTargetInput.Replace("..", "=").Replace(", ", "=").Split('=');
                (int X, int Y) lTargetBottomLeft = (int.Parse(lSplit[1]), int.Parse(lSplit[4]));
                (int X, int Y) lTargetTopRight = (int.Parse(lSplit[2]), int.Parse(lSplit[5]));
                (int X, int Y) lVelocity;

                // find starting x velocity that'll get us to the target
                lVelocity = (0, 0);
                bool lReach;
                do
                {
                    lVelocity.X++;
                    lReach = DoesIntLandInTarget(lVelocity, lTargetBottomLeft, lTargetTopRight);
                    if (Verbose) Console.WriteLine($"Trying to reach: ({lVelocity.X},{lVelocity.Y}) -> {lReach})");
                } while (!lReach);

                // now aim upwards;
                int lMaxHeight = 0;
                bool lHit;
                for (int i = 0; i < 100; i++)
                { // UGLY!!!
                    lVelocity.Y++;
                    lHit = CalcTrajectory(lVelocity, lTargetBottomLeft, lTargetTopRight, out int lThisHeight);
                    if (lHit) lMaxHeight = lThisHeight;
                    if (Verbose) Console.WriteLine($"Now aiming up: ({lVelocity.X},{lVelocity.Y}) -> {lThisHeight} ({lHit})");
                }
                if (Verbose) Console.WriteLine($"=====> Best Height: {lMaxHeight}\n");
                lVelocity.X++;
                lVelocity.Y = 0;
                return FormatResult(lMaxHeight, "highest y");
            }

            private string Part2(string aTargetInput)
            {
                string[] lSplit = aTargetInput.Replace("..", "=").Replace(", ", "=").Split('=');
                (int X, int Y) lTargetBottomLeft = (int.Parse(lSplit[1]), int.Parse(lSplit[4]));
                (int X, int Y) lTargetTopRight = (int.Parse(lSplit[2]), int.Parse(lSplit[5]));
                (int X, int Y) lVelocity;
                var lHits = new List<(int X, int Y)>();
                for (lVelocity.X = -3000; lVelocity.X < 3000; lVelocity.X++)
                {
                    for (lVelocity.Y = -3000; lVelocity.Y < 3000; lVelocity.Y++)
                    {
                        if (CalcTrajectory(lVelocity, lTargetBottomLeft, lTargetTopRight, out int lThisHeight))
                        {
                            lHits.Add(lVelocity);
                            if (Verbose) Console.WriteLine($"Hitting: {lVelocity.X},{lVelocity.Y}");
                        }
                    }
                }
                return FormatResult(lHits.Count, "disctinct velocities");
            }

            private bool DoesIntLandInTarget((int X, int Y) aVelocity, (int X, int Y) aTargetBottomLeft, (int X, int Y) aTargetTopRight)
            {
                (int X, int Y) lPos = (0, 0);
                while (aVelocity.X > 0) lPos.X += aVelocity.X--;
                return lPos.X >= aTargetBottomLeft.X && lPos.X <= aTargetTopRight.X;
            }

            private bool CalcTrajectory((int X, int Y) lVelocity, (int X, int Y) lTargetBottomLeft, (int X, int Y) lTargetTopRight, out int aMaxHeight)
            {
                (int X, int Y) lPos = (0, 0);
                var lPositions = new List<(int X, int Y)>();
                lPositions.Add(lPos);
                aMaxHeight = 0;
                bool aHit = false;
                do
                {
                    lPos.X += lVelocity.X;
                    lPos.Y += lVelocity.Y;
                    aMaxHeight = Math.Max(aMaxHeight, lPos.Y);
                    if (lVelocity.X > 0) lVelocity.X--;
                    if (lVelocity.X < 0) lVelocity.X++;
                    lVelocity.Y--;
                    lPositions.Add(lPos);
                    if (lPos.X >= lTargetBottomLeft.X && lPos.X <= lTargetTopRight.X && lPos.Y >= lTargetBottomLeft.Y && lPos.Y <= lTargetTopRight.Y) aHit = true;
                } while (lPos.Y >= lTargetBottomLeft.Y);
                //Plot(aMaxHeight);
                return aHit;

                void Plot(int aMaxH)
                {
                    var lSb = new StringBuilder();
                    for (int y = aMaxH; y >= lTargetBottomLeft.Y; y--)
                    {
                        lSb.Append($"{y,8:d}   ");
                        for (int x = 0; x <= lTargetTopRight.X; x++)
                        {
                            bool lTrace = false;
                            foreach ((int X, int Y) llPos in lPositions)
                            {
                                if (x == llPos.X && y == llPos.Y)
                                {
                                    lSb.Append((x == 0 && y == 0) ? 'S' : '#');
                                    lTrace = true;
                                    break;
                                }
                            }
                            if (!lTrace)
                            {
                                if (x >= lTargetBottomLeft.X && x <= lTargetTopRight.X && y >= lTargetBottomLeft.Y && y <= lTargetTopRight.Y) lSb.Append('T');
                                else lSb.Append('.');
                            }
                        }
                        lSb.Append(Environment.NewLine);
                    }
                    if (Verbose) Console.WriteLine(lSb.ToString());
                }
            }
        }
    }
}
