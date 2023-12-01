using System;
using System.Net.Sockets;
using System.Text;

namespace Puzzles {
    public partial class Year2022 {
        public class Day23 : DayBase {
            protected override string Title { get; } = "Day 23:";

            public override void SetupAll() {
                AddInputFile(@"2022\23_Example.txt");
                //AddInputFile(@"2022\23_rAiner.txt");
                //AddInputFile(@"2022\23_SEGCC.txt");
            }

            public override void Init(string InputFile) => InputData = ReadFile(InputFile, true);

            public override string Solve(bool Part1) {
                return FormatResult(0, "not yet implemented");
            }
        }
    }
}
