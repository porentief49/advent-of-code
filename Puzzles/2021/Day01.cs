using System;

namespace Puzzles
{
    public partial class Year2021
    {
        public class Day01 : DayBase
        {
            int[] mDepths = new int[] { };

            protected override string Title { get; } = "Day 1: Sonar Sweep";

            public override void SetupAll()
            {
                AddInputFile(@"2021\01_Example.txt");
                AddInputFile(@"2021\01_rAiner.txt");
            }

            public override void Init(string InputFile) => mDepths = ReadFile(InputFile, true).Select(x => int.Parse(x)).ToArray();

            public override string Solve(bool Part1)
            {
                int lPrev = mDepths[0] + (Part1 ? 0 : (mDepths[1] + mDepths[2]));
                int lCountIncreases = 0;
                for (int i = 1; i < mDepths.Length - (Part1 ? 0 : 2); i++)
                {
                    int lThis = mDepths[i] + (Part1 ? 0 : (mDepths[i + 1] + mDepths[i + 2]));
                    if (lThis > lPrev) lCountIncreases++;
                    lPrev = lThis;
                }
                return FormatResult(lCountIncreases, "increases");
            }
        }
    }
}
