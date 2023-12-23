namespace Puzzles {

    public partial class Year2023 {

        public class Day24 : DayBase {

            protected override string Title { get; } = "Day 24: ???";

            public override void SetupAll() {
                AddInputFile(@"2023\24_Example.txt");
                //AddInputFile(@"2023\24_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve() {
                return "";
            }
        }
    }
}
