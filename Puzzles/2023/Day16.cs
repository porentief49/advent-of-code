namespace Puzzles {

    public partial class Year2023 {

        public class Day16 : DayBase {

            protected override string Title { get; } = "Day 16: ???";

            public override void SetupAll() {
                AddInputFile(@"2023\16_Example.txt");
                //AddInputFile(@"2023\16_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve() {
                return "";
            }
        }
    }
}
