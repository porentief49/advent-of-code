namespace Puzzles {

    public partial class Year2023 {

        public class Day19 : DayBase {

            protected override string Title { get; } = "Day 19: ???";

            public override void SetupAll() {
                AddInputFile(@"2023\19_Example.txt");
                //AddInputFile(@"2023\19_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve() {
                return "";
            }
        }
    }
}
