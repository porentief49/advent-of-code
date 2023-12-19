namespace Puzzles {

    public partial class Year2023 {

        public class Day20 : DayBase {

            protected override string Title { get; } = "Day 20: ???";

            public override void SetupAll() {
                AddInputFile(@"2023\20_Example.txt");
                //AddInputFile(@"2023\20_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve() {
                return "";
            }
        }
    }
}
