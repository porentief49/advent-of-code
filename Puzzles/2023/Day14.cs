
namespace Puzzles {

    public partial class Year2023 {

        public class Day14 : DayBase {

            protected override string Title { get; } = "Day 14: Parabolic Reflector Dish";

            public override void SetupAll() {
                AddInputFile(@"2023\14_Example.txt");
                AddInputFile(@"2023\14_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve() {
                //if (Part2) return "";
                var platform = InputAsLines.Select(i => i.ToCharArray()).ToArray();
                bool stillRolling;
                int startsAt=0;
                int repeatsEvery=0;
                //PrintGrid(platform);

                // tilt north
                List<int> loads = new();
                //for (int i = 0; i < 1000; i++) {
                int count = 0;
                do {
                    //int load = 0;
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
                    if (Part1) return CalcLoad(platform).ToString();

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
                    int load = CalcLoad(platform);
                    count++;
                    Console.WriteLine($"{count} {load}");
                    loads.Add(load);
                } while (!Correlate());

                // calc where we are at 1000000000

                return loads[startsAt + (1000000000-startsAt) % repeatsEvery-1].ToString();

                bool Correlate() {
                    if (loads.Count < 100) return false;
                    int last = loads.Last();
                    int lastIndex = loads.Count - 1;
                    int secondLastIndex = loads.LastIndexOf(last, loads.Count - 2);
                    if (secondLastIndex == -1) return false;
                    bool corr = true; // we might have a match - assume best
                    int distance = lastIndex - secondLastIndex;
                    if (secondLastIndex - distance < 0 || distance<2) return false; // filter out direct repetitions
                    for (int i = 1; i < distance; i++) {
                        if (loads[lastIndex - 1] != loads[secondLastIndex - 1]) {
                            corr = false;
                            break;
                        }
                    }
                    if(corr) {
                        startsAt = secondLastIndex;
                        repeatsEvery = distance;
                        Console.WriteLine($"starts at {secondLastIndex} and repeats evrey {distance}");
                    }
                    return corr;
                }
            }

            private static int CalcLoad(char[][] platform) {
                int load = 0;
                for (int r = 0; r < platform.Length; r++) {
                    load += platform[r].Count(c => c == 'O') * (platform.Length - r);
                }
                return load;
            }
        }
    }
}
