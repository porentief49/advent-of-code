using System;

namespace Puzzles
{
    public partial class Year2021
    {
        public class Day02 : DayBase
        {
            protected override string Title { get; } = "Day 2 - Dive!";

            public override void SetupAll()
            {
                InputFiles.Add(@"2021\02_Example.txt");
                InputFiles.Add(@"2021\02_rAiner.txt");
            }

            public override void Init(string InputFile) => InputData = ReadFile(InputFile, true);

            public override string Solve(bool aPart1)
            {
                int lHorizontal = 0;
                int lDepth = 0;
                int lAim = 0;
                for (int i = 0; i < InputData?.Length; i++)
                {
                    string[] lSplit = InputData[i].ToLower().Split(' ');
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
