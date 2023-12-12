namespace Puzzles {

    public partial class Year2023 {

        public class Day14 : DayBase {

            protected override string Title { get; } = "Day 14: ???";

            public override void SetupAll() {
                AddInputFile(@"2023\14_Example.txt");
                //AddInputFile(@"2023\14_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve() {
                return "";
            }
        }
    }
}
