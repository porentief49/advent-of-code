namespace Puzzles {

    public partial class Year2023 {

        public class Day13 : DayBase {

            protected override string Title { get; } = "Day 13: ???";

            public override void SetupAll() {
                AddInputFile(@"2023\13_Example.txt");
                //AddInputFile(@"2023\13_rAiner.txt");
            }

            public override void Init(string InputFile) => InputAsLines = ReadLines(InputFile, true);

            public override string Solve(bool Part1) {
                return "";
            }
        }
    }
}
