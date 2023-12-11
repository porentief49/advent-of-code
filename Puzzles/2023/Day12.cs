namespace Puzzles {

    public partial class Year2023 {

        public class Day12 : DayBase {

            protected override string Title { get; } = "Day 12: ???";

            public override void SetupAll() {
                AddInputFile(@"2023\12_Example.txt");
                //AddInputFile(@"2023\12_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve() {
                return "";
            }
        }
    }
}
