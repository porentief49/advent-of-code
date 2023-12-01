using System;
using System.Net.Sockets;
using System.Text;

namespace Puzzles {
    public partial class Year2022 {
        public class Day06 : DayBase {
            protected override string Title { get; } = "Day 6: Tuning Trouble";

            public override void SetupAll() {
                AddInputFile(@"2022\06_Example1.txt");
                AddInputFile(@"2022\06_Example2.txt");
                AddInputFile(@"2022\06_Example3.txt");
                AddInputFile(@"2022\06_Example4.txt");
                AddInputFile(@"2022\06_Example5.txt");
                AddInputFile(@"2022\06_rAiner.txt");
                AddInputFile(@"2022\06_SEGCC.txt");
            }

            public override void Init(string InputFile) => InputData = ReadFile(InputFile, true);

            public override string Solve(bool Part1) {
                int _markerLength = Part1 ? 4 : 14;
                for (int i = 0; i < InputData[0].Length - _markerLength; i++) if (InputData[0].Skip(i).Take(_markerLength).Distinct().Count() == _markerLength) return FormatResult(i + _markerLength, "packet start");
                return FormatResult(-1, "no packet marker found");
            }
        }
    }
}
