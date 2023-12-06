namespace Puzzles {

    public partial class Year2023 {

        public class Day11 : DayBase {

            protected override string Title { get; } = "Day 11: ???";

            public override void SetupAll() {
                AddInputFile(@"2023\11_Example.txt");
                //AddInputFile(@"2023\11_rAiner.txt");
            }

            public override void Init(string InputFile) => InputAsLines = ReadLines(InputFile, true);

            public override string Solve(bool Part1) {
                return "";
            }
        }
    }
}
