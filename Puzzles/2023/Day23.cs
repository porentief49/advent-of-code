namespace Puzzles {

    public partial class Year2023 {

        public class Day23 : DayBase {

            protected override string Title { get; } = "Day 23: ???";

            public override void SetupAll() {
                AddInputFile(@"2023\23_Example.txt");
                //AddInputFile(@"2023\23_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve() {
                return "";
            }
        }
    }
}
