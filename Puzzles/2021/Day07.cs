using System;
using System.Linq;

namespace Puzzles
{
    public partial class Year2021
    {
        public class Day07 : DayBase
        { //@@@ for part 1, could do median, that would be the best choice
            protected override string Title { get; } = "Day 7 - The Treachery of Whales";

            public override void Init() => Init(Inputs_2021.Rainer_07);

            public override void Init(string aResource) => Input = Tools.SplitLines(aResource, true);

            public override string SolvePuzzle(bool aPart1)
            {
                string[] lIn = Input[0].Split(',');
                int[] lHorizontal = new int[lIn.Length];
                for (int i = 0; i < lHorizontal.Length; i++) lHorizontal[i] = int.Parse(lIn[i]);
                int lMin = lHorizontal.Min();
                int lMax = lHorizontal.Max();
                (int Pos, long Fuel) lBest = (0, 0);
                for (int i = lMin; i <= lMax; i++)
                {
                    long lFuel = 0;
                    for (int ii = 0; ii < lHorizontal.Length; ii++)
                    {
                        if (aPart1) lFuel += Math.Abs(lHorizontal[ii] - i);
                        else lFuel += Math.Abs(lHorizontal[ii] - i) * (Math.Abs(lHorizontal[ii] - i) + 1) / 2;
                    }
                    if (lFuel < lBest.Fuel || i == lMin) lBest = (i, lFuel);
                }
                return FormatResult(lBest.Fuel, $"moving to {lBest.Pos} costs");
            }
        }
    }
}
