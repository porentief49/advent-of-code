using System;
using System.Net.Sockets;
using System.Text;

namespace Puzzles
{
    public partial class Year2022
    {
        public class Day12 : DayBase
        {
            protected override string Title { get; } = "Day 12:";

            public override void SetupAll()
            {
                AddInputFile(@"2022\12_Example.txt");
                //AddInputFile(@"2022\12_rAiner.txt");
                //AddInputFile(@"2022\12_SEGCC.txt");
            }

            public override void Init(string InputFile) => InputData = ReadFile(InputFile, true);

            public override string Solve(bool Part1)
            {
                return FormatResult(0, "not yet implemented");
            }
        }
    }
}
