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
                var pattern = InputAsText.Split("\n\n", StringSplitOptions.RemoveEmptyEntries).Select(i => new Pattern(i, Part1 ? 0 : 1)).ToList();
                return pattern.Select(p => p.Position).Sum().ToString();
            }

            private class Pattern {
                public int Position = 0;

                public Pattern(string input, int smudges) {
                    char[][] map; // row, column
                    var split = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
                    map = split.Select(s => s.ToCharArray()).ToArray();
                    int smudgeCount = 0;
                    Position = 0;
                    do { // look for vertical
                        smudgeCount = 0;
                        for (int col = 0; col <= Position; col++) {
                            int otherCol = Position * 2 - col + 1;
                            if (otherCol < map[0].Length) for (int row = 0; row < map.Length; row++) if (map[row][col] != map[row][otherCol]) smudgeCount++;
                        }
                    } while (++Position < map[0].Length - 1 && smudgeCount != smudges);
                    if (smudgeCount != smudges) {
                        Position = 0;
                        do { // look for horizontal
                            smudgeCount = 0;
                            for (int row = 0; row <= Position; row++) {
                                int otherRow = Position * 2 - row + 1;
                                if (otherRow < map.Length) for (int col = 0; col < map[0].Length; col++) if (map[row][col] != map[otherRow][col]) smudgeCount++;
                            }
                        } while (++Position < map.Length - 1 && smudgeCount != smudges);
                        Position *= 100; // horizontal * 100 for total sum
                    }
                    if (smudgeCount != smudges) throw new Exception("could not find mirror");
                }
            }
        }
    }
}
