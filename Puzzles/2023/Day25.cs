namespace Puzzles {

    public partial class Year2023 {

        public class Day25 : DayBase {

            protected override string Title { get; } = "Day 25: ???";

            public override void SetupAll() {
                AddInputFile(@"2023\25_Example.txt");
                //AddInputFile(@"2023\25_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve() {
                return "";
            }
        }
    }
}
