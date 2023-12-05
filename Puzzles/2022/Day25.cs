using System;
using System.Net.Sockets;
using System.Text;

namespace Puzzles {
    public partial class Year2022 {
        public class Day25 : DayBase {
            protected override string Title { get; } = "Day 25:";

            public override void SetupAll() {
                AddInputFile(@"2022\25_Example.txt");
                //AddInputFile(@"2022\25_rAiner.txt");
                //AddInputFile(@"2022\25_SEGCC.txt");
            }

            public override void Init(string InputFile) => InputAsLines = ReadLines(InputFile, true);

            public override string Solve(bool Part1) {
                return FormatResult(0, "not yet implemented");
            }
        }
    }
}
