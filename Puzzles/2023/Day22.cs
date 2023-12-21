namespace Puzzles {

    public partial class Year2023 {

        public class Day22 : DayBase {

            protected override string Title { get; } = "Day 22: ???";

            public override void SetupAll() {
                AddInputFile(@"2023\22_Example.txt");
                //AddInputFile(@"2023\22_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve() {
                return "";
            }
        }
    }
}
