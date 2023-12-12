namespace Puzzles {

    public partial class Year2023 {

        public class Day17 : DayBase {

            protected override string Title { get; } = "Day 17: ???";

            public override void SetupAll() {
                AddInputFile(@"2023\17_Example.txt");
                //AddInputFile(@"2023\17_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve() {
                return "";
            }
        }
    }
}
