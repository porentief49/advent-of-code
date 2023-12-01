using System.Linq;

namespace Puzzles {
    public partial class Year2021 {
        public class Day06 : DayBase_OLD {
            protected override string Title { get; } = "Day 6 - Lanternfish";

            public override void Init() => Init(Inputs_2021.Rainer_06);

            public override void Init(string aResource) => Input = Tools.SplitLines(aResource, true);

            public override string SolvePuzzle(bool aPart1) {
                int lDays = aPart1 ? 80 : 256;
                long[] lFish = new long[9];
                string[] lIn = Input[0].Split(',');
                for (int i = 0; i < lIn.Length; i++) lFish[int.Parse(lIn[i])]++;
                for (int i = 0; i < lDays; i++) {
                    long lTemp0 = lFish[0];
                    for (int ii = 0; ii < 8; ii++) lFish[ii] = lFish[ii + 1];
                    lFish[6] += lTemp0;
                    lFish[8] = lTemp0;
                }
                return FormatResult(lFish.Sum(), $"fish after {lDays} days");
            }
        }
    }
}
