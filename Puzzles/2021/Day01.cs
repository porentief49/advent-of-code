using System;

namespace Puzzles
{
    public partial class Year2021
    {
        public class Day01 : DayBase
        {
            int[] mDepths;

            protected override string Title { get; } = "Day 1 - Sonar Sweep";

            public override void Init() => Init(Inputs.day01); 

            public override void Init(string aResource)
            {
                Input = aResource.Replace("\r", string.Empty).Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                mDepths = new int[Input.Length];
                for (int i = 0; i < Input.Length; i++) mDepths[i] = int.Parse(Input[i]);
            }

            public override string SolvePuzzle(bool aPart1)
            {
                int lPrev = mDepths[0] + (aPart1 ? 0 : (mDepths[1] + mDepths[2]));
                int lCountIncreases = 0;
                for (int i = 1; i < Input.Length - (aPart1 ? 0 : 2); i++)
                {
                    int lThis = mDepths[i] + (aPart1 ? 0 : (mDepths[i + 1] + mDepths[i + 2]));
                    if (lThis > lPrev) lCountIncreases++;
                    lPrev = lThis;
                }
                return FormatResult(lCountIncreases, "increases");
            }
        }
    }
}
