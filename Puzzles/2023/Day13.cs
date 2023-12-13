namespace Puzzles {

    public partial class Year2023 {

        public class Day13 : DayBase {

            protected override string Title { get; } = "Day 13: Point of Incidence";

            public override void SetupAll() {
                AddInputFile(@"2023\13_Example.txt");
                AddInputFile(@"2023\13_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsText = ReadText(inputFile, true);

            public override string Solve() {
                var pat2 = InputAsText.Split("\n\n", StringSplitOptions.RemoveEmptyEntries).Select(i => new Pattern(i, Part1 ? 0 : 1)).ToList();
                return pat2.Select(p => p.Result).Sum().ToString();
            }

            private class Pattern {
                public char[][] map; // row, column
                public int Result = 0;

                public Pattern(string input, int smudges) {
                    var split = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
                    map = split.Select(s => s.ToCharArray()).ToArray();
                    int smudgeCount = 0;
                    int pos = 0;
                    do { // look for vertical
                        smudgeCount = 0;
                        for (int col = 0; col <= pos; col++) {
                            int otherCol = pos + (pos - col) + 1;
                            if (otherCol < map[0].Length) for (int row = 0; row < map.Length; row++) if (map[row][col] != map[row][otherCol]) smudgeCount++;
                        }
                        pos++;
                    } while (smudgeCount != smudges && pos < map[0].Length - 1);
                    if (smudgeCount == smudges) Result = pos;
                    else {
                        pos = 0;
                        do { // look for horizontal
                            smudgeCount = 0;
                            for (int row = 0; row <= pos; row++) {
                                int otherRow = pos + (pos - row) + 1;
                                if (otherRow < map.Length) for (int col = 0; col < map[0].Length; col++) if (map[row][col] != map[otherRow][col]) smudgeCount++;
                            }
                            pos++;
                        } while (smudgeCount != smudges && pos < map.Length - 1);
                        Result = pos * 100;
                    }
                    if (smudgeCount != smudges) throw new Exception("could not find  mirror");
                }
            }
        }
    }
}
