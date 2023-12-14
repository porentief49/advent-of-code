namespace Puzzles {

    public partial class Year2023 {

        public class Day14 : DayBase {

            protected override string Title { get; } = "Day 14: Parabolic Reflector Dish";

            public override void SetupAll() {
                //AddInputFile(@"2023\14_Example.txt");
                AddInputFile(@"2023\14_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve() {
                if (Part2) return "";
                var platform = InputAsLines.Select(i => i.ToCharArray()).ToArray();
                bool stillRolling;
                //PrintGrid(platform);

                // tilt north
                int load = 0;
                for (int i = 0; i < 1000; i++) {

                    do {
                        stillRolling = false;
                        for (int r = 1; r < platform.Length; r++) {
                            for (int c = 0; c < platform[0].Length; c++) {
                                if (platform[r][c] == 'O' && platform[r - 1][c] == '.') {
                                    (platform[r][c], platform[r - 1][c]) = (platform[r - 1][c], platform[r][c]);
                                    stillRolling = true;
                                }
                            }
                        }
                        //PrintGrid(platform);
                    } while (stillRolling);

                    // tilt west
                    do {
                        stillRolling = false;
                        for (int c = 1; c < platform[0].Length; c++) {
                            for (int r = 0; r < platform.Length; r++) {
                                if (platform[r][c] == 'O' && platform[r][c - 1] == '.') {
                                    (platform[r][c], platform[r][c - 1]) = (platform[r][c - 1], platform[r][c]);
                                    stillRolling = true;
                                }
                            }
                        }
                        //PrintGrid(platform);
                    } while (stillRolling);

                    // tilt south
                    do {
                        stillRolling = false;
                        for (int r = platform.Length - 2; r >= 0; r--) {
                            for (int c = 0; c < platform[0].Length; c++) {
                                if (platform[r][c] == 'O' && platform[r + 1][c] == '.') {
                                    (platform[r][c], platform[r + 1][c]) = (platform[r + 1][c], platform[r][c]);
                                    stillRolling = true;
                                }
                            }
                        }
                        //PrintGrid(platform);
                    } while (stillRolling);

                    // tilt east
                    do {
                        stillRolling = false;
                        for (int c = platform[0].Length - 2; c >= 0; c--) {
                            for (int r = 0; r < platform.Length; r++) {
                                if (platform[r][c] == 'O' && platform[r][c + 1] == '.') {
                                    (platform[r][c], platform[r][c + 1]) = (platform[r][c + 1], platform[r][c]);
                                    stillRolling = true;
                                }
                            }
                        }
                    } while (stillRolling);
                    //PrintGrid(platform);

                    // count results
                    load = 0;
                    for (int r = 0; r < platform.Length; r++) {
                        load += platform[r].Count(c => c == 'O') * (platform.Length - r);
                    }
                    Console.WriteLine($"{load}");

                }
                return load.ToString();
            }
        }
    }
}
