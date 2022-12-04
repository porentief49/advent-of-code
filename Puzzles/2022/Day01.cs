using System;

namespace Puzzles
{
    public partial class Year2022
    {
        public class Day01 : DayBase
        {

            protected override string Title { get; } = "Day 1: Calorie Counting";

            public override void SetupAll()
            {
                InputFiles.Add(@"2022\01_Example.txt");
                InputFiles.Add(@"2022\01_rAiner.txt");
            }

            public override void Init(string InputFile) => InputData = ReadFile(InputFile, false);

            public override string Solve(bool Part1)
            {
                List<long> _elfsCalories = new();
                long _calories = 0;
                for (int i = 0; i < InputData?.Length; i++)
                {
                    if (InputData[i].Length > 0) _calories += long.Parse(InputData[i]);
                    else
                    {
                        _elfsCalories.Add(_calories);
                        _calories = 0;
                    }
                }
                List<long> _elfsCaloriesSortedDesc = _elfsCalories.OrderByDescending(x => x).ToList();
                if (Part1) return FormatResult(_elfsCaloriesSortedDesc[0], "max calories");
                return FormatResult(+_elfsCaloriesSortedDesc[0] + _elfsCaloriesSortedDesc[1] + _elfsCaloriesSortedDesc[2], "first three");
            }
        }
    }
}
