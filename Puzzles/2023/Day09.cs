namespace Puzzles {

    public partial class Year2023 {

        public class Day09 : DayBase {

            protected override string Title { get; } = "Day 9: ???";

            public override void SetupAll() {
                AddInputFile(@"2023\09_Example.txt");
                //AddInputFile(@"2023\09_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve(bool part1) {
                return "";
            }
        }
    }
}
