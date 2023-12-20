namespace Puzzles {

    public partial class Year2023 {

        public class Day21 : DayBase {

            protected override string Title { get; } = "Day 21: ???";

            public override void SetupAll() {
                AddInputFile(@"2023\21_Example.txt");
                //AddInputFile(@"2023\21_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve() {
                return "";
            }
        }
    }
}
