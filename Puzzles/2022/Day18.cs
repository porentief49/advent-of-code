using System;
using System.Net.Sockets;
using System.Text;

namespace Puzzles {
    public partial class Year2022 {
        public class Day18 : DayBase {
            protected override string Title { get; } = "Day 18:";

            public override void SetupAll() {
                AddInputFile(@"2022\18_Example.txt");
                //AddInputFile(@"2022\18_rAiner.txt");
                //AddInputFile(@"2022\18_SEGCC.txt");
            }

            public override void Init(string InputFile) => InputAsLines = ReadLines(InputFile, true);

            public override string Solve() {
                return FormatResult(0, "not yet implemented");
            }
        }
    }
}
