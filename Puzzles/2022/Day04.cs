using System;

namespace Puzzles {
    public partial class Year2022 {
        public class Day04 : DayBase {
            protected override string Title { get; } = "Day 4: Camp Cleanup";

            public override void SetupAll() {
                AddInputFile(@"2022\04_Example.txt");
                AddInputFile(@"2022\04_rAiner.txt");
                AddInputFile(@"2022\04_SEGCC.txt");
            }

            public override void Init(string InputFile) => InputData = ReadFile(InputFile, true);

            public override string Solve(bool Part1) {
                List<(Range Elf1, Range Elf2)> Assignments = new();
                for (int i = 0; i < InputData?.Length; i++) {
                    string[] _split = InputData[i].Split(',');
                    Assignments.Add((new(_split[0]), new(_split[1])));
                }
                int _overlapCount = 0;
                foreach (var _assignment in Assignments) {
                    if (Part1) _overlapCount += _assignment.Elf1.FullyContaines(_assignment.Elf2);
                    else _overlapCount += _assignment.Elf1.PartiallyOverlap(_assignment.Elf2);

                }
                return FormatResult(_overlapCount, Part1 ? "fully contains" : "partially overlaps");
            }

            private class Range {

                public int From;
                public int To;

                public Range(string Input) {
                    string[] _split = Input.Split('-');
                    From = int.Parse(_split[0]);
                    To = int.Parse(_split[1]);
                }

                private int Length => To - From + 1;

                public int FullyContaines(Range OtherRange) {
                    if (OtherRange.Length < Length) return (From <= OtherRange.From && To >= OtherRange.To) ? 1 : 0;
                    return (OtherRange.From <= From && OtherRange.To >= To) ? 1 : 0;
                }

                public int PartiallyOverlap(Range SmallerRange) => (From <= SmallerRange.To && To >= SmallerRange.From) ? 1 : 0;
            }
        }
    }
}
