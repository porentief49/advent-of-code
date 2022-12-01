using System;

namespace Puzzles
{
    public partial class Year2022
    {
        public class Day01 : DayBase
        {

            protected override string Title { get; } = "Day 1 - Calorie Counting";

            public override void Init() => Init(Inputs_2022.Rainer_01);

            public override void Init(string Resource) => Input = Tools.SplitLines(Resource, false);

            public override string SolvePuzzle(bool Part1)
            {
                List<long> _elfsCalories = new();
                long _calories = 0;
                for (int i = 0; i < Input.Length; i++)
                {
                    if (Input[i].Length == 0)
                    {
                        _elfsCalories.Add(_calories);
                        _calories = 0;
                    }
                    else
                    {
                        _calories += long.Parse(Input[i]);
                    }
                }
                List<long> _elfsCaloriesSortedDesc = _elfsCalories.OrderByDescending(x => x).ToList();
                if (Part1) return FormatResult(_elfsCaloriesSortedDesc[0], "max calories");
                return FormatResult(+_elfsCaloriesSortedDesc[0] + _elfsCaloriesSortedDesc[1] + _elfsCaloriesSortedDesc[2], "first three");
            }
        }
    }
}
