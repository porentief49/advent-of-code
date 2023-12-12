namespace Puzzles {

    public partial class Year2023 {

        public class Day15 : DayBase {

            protected override string Title { get; } = "Day 15: ???";

            public override void SetupAll() {
                AddInputFile(@"2023\15_Example.txt");
                //AddInputFile(@"2023\15_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve() {
                return "";
            }
        }
    }
}
