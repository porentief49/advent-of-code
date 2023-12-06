namespace Puzzles {

    public partial class Year2023 {

        public class Day07 : DayBase {

            protected override string Title { get; } = "Day 7: ???";

            public override void SetupAll() {
                AddInputFile(@"2023\07_Example.txt");
                //AddInputFile(@"2023\07_rAiner.txt");
            }

            public override void Init(string InputFile) => InputAsLines = ReadLines(InputFile, true);

            public override string Solve(bool Part1) {
                return "";
            }
        }
    }
}
