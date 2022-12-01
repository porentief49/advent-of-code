using System;

namespace Puzzles
{
    public partial class Year2021
    {
        public class Day02 : DayBase
        {
            protected override string Title { get; } = "Day 2 - Dive!";

            public override void Init() => Init(Inputs_2021.Rainer_02);

            public override void Init(string aResource) => Input = Tools.SplitLinesWithoutEmptyOnes(aResource, true);

            public override string SolvePuzzle(bool aPart1)
            {
                int lHorizontal = 0;
                int lDepth = 0;
                int lAim = 0;
                for (int i = 0; i < Input.Length; i++)
                {
                    string[] lSplit = Input[i].ToLower().Split(' ');
                    int lSteps = int.Parse(lSplit[1]);
                    switch (lSplit[0])
                    {
                        case "up":
                            if (aPart1) lDepth -= lSteps;
                            else lAim -= lSteps;
                            break;
                        case "down":
                            if (aPart1) lDepth += lSteps;
                            else lAim += lSteps;
                            break;
                        case "forward":
                            if (aPart1) lHorizontal += lSteps;
                            else
                            {
                                lHorizontal += lSteps;
                                lDepth += lAim * lSteps;
                            }
                            break;
                    }
                }
                if (Verbose) Console.WriteLine($"horizontal {lHorizontal} - depth {lDepth}");
                return FormatResult(lHorizontal * lDepth, "pos * depth");
            }
        }
    }
}
