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
                if (Part1) {
                    var patterns = InputAsText.Split("\n\n", StringSplitOptions.RemoveEmptyEntries).Select(i => new Pattern(i)).ToList();
                    return patterns.Select(p => p.result).Sum().ToString();
                }
                var pat2 = InputAsText.Split("\n\n", StringSplitOptions.RemoveEmptyEntries).Select(i => new Pattern2(i)).ToList();
                return pat2.Select(p => p.result).Sum().ToString();
            }

            private class Pattern2 {
                public char[][] map; // row, column
                public bool isVertical;
                public int leftOrAbove;
                public int result;

                public Pattern2(string input) {
                    var split = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
                    map = split.Select(s => s.ToCharArray()).ToArray();

                    //try vertical
                    //bool foundIt;
                    int smudgeCount = 0;
                    int pos = 0;
                    do {
                        //foundIt = true; // assume best
                        smudgeCount = 0;
                        for (int col = 0; col <= pos; col++) {
                            // pos = 0, c = 0, otherCol = 1;
                            // pos = 3, c = 3, otherCol = 4;
                            // pos = 3, c = 2, otherCol = 5;
                            // pos = 3, c = 0, otherCol = 7;
                            int otherCol = pos + (pos - col) + 1;
                            if (otherCol < map[0].Length) {
                                for (int row = 0; row < map.Length; row++) {
                                    //if (map[row][col] != map[row][otherCol]) foundIt = false;
                                    if (map[row][col] != map[row][otherCol]) smudgeCount++;
                                }
                            }
                        }
                        pos++;
                    //} while (!foundIt && pos < map[0].Length - 1);
                    } while (smudgeCount!=1 && pos < map[0].Length - 1);
                    isVertical = true;
                    leftOrAbove = pos;

                    //try horizontal
                    //if (!foundIt) {
                    if (smudgeCount != 1) {
                        pos = 0;
                        do {
                            //foundIt = true; // assume best
                            smudgeCount = 0;
                            for (int row = 0; row <= pos; row++) {
                                int otherRow = pos + (pos - row) + 1;
                                if (otherRow < map.Length) {
                                    for (int col = 0; col < map[0].Length; col++) {
                                        //if (map[row][col] != map[otherRow][col]) foundIt = false;
                                        if (map[row][col] != map[otherRow][col]) smudgeCount++;
                                    }
                                }
                            }
                            pos++;
                        //} while (!foundIt && pos < map.Length - 1);
                        } while (smudgeCount != 1 && pos < map.Length - 1);
                        isVertical = false;
                        leftOrAbove = pos;
                    }
                    result = leftOrAbove * (isVertical ? 1 : 100);
                    Console.WriteLine($"vertical: {isVertical}, left/above: {leftOrAbove}");
                }
            }
            private class Pattern {
                public char[][] map; // row, column
                public bool isVertical;
                public int leftOrAbove;
                public int result;

                public Pattern(string input) {
                    var split = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
                    map = split.Select(s => s.ToCharArray()).ToArray();

                    //try vertical
                    bool foundIt;
                    int pos = 0;
                    do {
                        foundIt = true; // assume best
                        for (int col = 0; col <= pos; col++) {
                            // pos = 0, c = 0, otherCol = 1;
                            // pos = 3, c = 3, otherCol = 4;
                            // pos = 3, c = 2, otherCol = 5;
                            // pos = 3, c = 0, otherCol = 7;
                            int otherCol = pos + (pos - col) + 1;
                            if (otherCol < map[0].Length) {
                                for (int row = 0; row < map.Length; row++) {
                                    if (map[row][col] != map[row][otherCol]) foundIt = false;
                                }
                            }
                        }
                        pos++;
                    } while (!foundIt && pos < map[0].Length - 1);
                    isVertical = true;
                    leftOrAbove = pos;

                    //try horizontal
                    if (!foundIt) {
                        pos = 0;
                        do {
                            foundIt = true; // assume best
                            for (int row = 0; row <= pos; row++) {
                                int otherRow = pos + (pos - row) + 1;
                                if (otherRow < map.Length) {
                                    for (int col = 0; col < map[0].Length; col++) {
                                        if (map[row][col] != map[otherRow][col]) foundIt = false;
                                    }
                                }
                            }
                            pos++;
                        } while (!foundIt && pos < map.Length - 1);
                        isVertical = false;
                        leftOrAbove = pos;
                    }
                    result = leftOrAbove * (isVertical ? 1 : 100);
                    Console.WriteLine($"vertical: {isVertical}, left/above: {leftOrAbove}");
                }
            }
        }
    }
}
