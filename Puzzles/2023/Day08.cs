﻿namespace Puzzles {

    public partial class Year2023 {

        public class Day08 : DayBase {

            protected override string Title { get; } = "Day 8: ???";

            public override void SetupAll() {
                AddInputFile(@"2023\08_Example.txt");
                //AddInputFile(@"2023\08_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve(bool part1) {
                return "";
            }
        }
    }
}
