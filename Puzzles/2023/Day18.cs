namespace Puzzles {

    public partial class Year2023 {

        public class Day18 : DayBase {

            protected override string Title { get; } = "Day 18: ???";

            public override void SetupAll() {
                AddInputFile(@"2023\18_Example.txt");
                //AddInputFile(@"2023\18_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve() {
                return "";
            }
        }
    }
}
