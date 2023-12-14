
namespace Puzzles {

    public partial class Year2023 {

        public class Day14 : DayBase {

            protected override string Title { get; } = "Day 14: Parabolic Reflector Dish";

            public override void SetupAll() {
                AddInputFile(@"2023\14_Example.txt");
                AddInputFile(@"2023\14_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            private enum Dir { N, W, S, E }

            private class Platform {
                private char[][] _map;

                public Platform(string[] input) => _map = input.Select(i => i.ToCharArray()).ToArray();

                public int CalcLoad() {
                    int load = 0;
                    for (int r = 0; r < _map.Length; r++) {
                        load += _map[r].Count(c => c == 'O') * (_map.Length - r);
                    }
                    return load;
                }

                public void Tilt(Dir dir) {
                    bool stillRolling;
                    do {
                        stillRolling = false;
                        int rStart = dir switch { Dir.N => 1, Dir.W => 0, Dir.S => 1, _ => 0 };
                        int cStart = dir switch { Dir.N => 0, Dir.W => 1, Dir.S => 0, _ => 1 };

                        for (int row = rStart; row < _map.Length; row++) {
                            for (int col = cStart; col < _map[0].Length; col++) {
                                int r = dir switch { Dir.N => row, Dir.W => row, Dir.S => _map.Length - row - 1, _ => row };
                                int c = dir switch { Dir.N => col, Dir.W => col, Dir.S => col, _ => _map[0].Length - col - 1 };
                                int rTo = dir switch { Dir.N => r - 1, Dir.W => r, Dir.S => r + 1, _ => r };
                                int cTo = dir switch { Dir.N => c, Dir.W => c - 1, Dir.S => c, _ => c + 1 };
                                if (_map[r][c] == 'O' && _map[rTo][cTo] == '.') {
                                    (_map[r][c], _map[rTo][cTo]) = (_map[rTo][cTo], _map[r][c]);
                                    stillRolling = true;
                                }
                            }
                        }
                    } while (stillRolling);
                    //PrintGrid(_map);
                }

                public void TiltNorth() {
                    bool stillRolling;
                    do {
                        int rStart = 1;
                        int cStart = 0;
                        stillRolling = false;
                        for (int row = rStart; row < _map.Length; row++) {
                            for (int col = cStart; col < _map[0].Length; col++) {
                                int r = row;
                                int rTo = r - 1;
                                int c = col;
                                int cTo = c;
                                if (_map[r][c] == 'O' && _map[rTo][cTo] == '.') {
                                    (_map[r][c], _map[rTo][cTo]) = (_map[rTo][cTo], _map[r][c]);
                                    stillRolling = true;
                                }
                            }
                        }
                    } while (stillRolling);
                    //PrintGrid(_map);
                }

                public void TiltWest() {
                    bool stillRolling;
                    do {
                        stillRolling = false;
                        int rStart = 0;
                        int cStart = 1;
                        for (int row = rStart; row < _map.Length; row++) {
                            for (int col = cStart; col < _map[0].Length; col++) {
                                int r = row;
                                int rTo = r;
                                int c = col;
                                int cTo = c - 1;
                                if (_map[r][c] == 'O' && _map[rTo][cTo] == '.') {
                                    (_map[r][c], _map[rTo][cTo]) = (_map[rTo][cTo], _map[r][c]);
                                    stillRolling = true;
                                }
                            }
                        }
                    } while (stillRolling);
                    //PrintGrid(_map);
                }

                public void TiltSouth() {
                    bool stillRolling;
                    do {
                        stillRolling = false;
                        int rStart = 1;
                        int cStart = 0;
                        for (int row = rStart; row < _map.Length; row++) {
                            for (int col = cStart; col < _map[0].Length; col++) {
                                int r = _map.Length - row - 1;
                                int rTo = r + 1;
                                int c = col;
                                int cTo = c;
                                if (_map[r][c] == 'O' && _map[rTo][cTo] == '.') {
                                    (_map[r][c], _map[rTo][cTo]) = (_map[rTo][cTo], _map[r][c]);
                                    stillRolling = true;
                                }
                            }
                        }
                    } while (stillRolling);
                    //PrintGrid(_map);
                }

                public void TiltEast() {
                    bool stillRolling;
                    do {
                        stillRolling = false;
                        int rStart = 0;
                        int cStart = 1;
                        for (int row = rStart; row < _map.Length; row++) {
                            for (int col = cStart; col < _map[0].Length; col++) {
                                int r = row;
                                int rTo = r;
                                int c = _map[0].Length - col - 1;
                                int cTo = c + 1;
                                if (_map[r][c] == 'O' && _map[rTo][cTo] == '.') {
                                    (_map[r][c], _map[rTo][cTo]) = (_map[rTo][cTo], _map[r][c]);
                                    stillRolling = true;
                                }
                            }
                        }
                    } while (stillRolling);
                    //PrintGrid(_map);
                }
            }

            public override string Solve() {
                List<int> loads = new();
                int startsAt = 0;
                int repeatsEvery = 0;
                if (true) {
                    var platformXX = new Platform(InputAsLines);
                    do {
                        platformXX.TiltNorth();
                        if (Part1) return platformXX.CalcLoad().ToString();
                        platformXX.TiltWest();
                        platformXX.TiltSouth();
                        platformXX.TiltEast();
                        int load = platformXX.CalcLoad();
                        Console.WriteLine($"{loads.Count} {load}");
                        loads.Add(load);
                    } while (!Correlate(loads, ref startsAt, ref repeatsEvery));
                }
                return loads[startsAt + (1000000000 - startsAt) % repeatsEvery - 1].ToString();
            }

            bool Correlate(List<int> loads, ref int startsAt, ref int repeatsEvery) {
                if (loads.Count < 100) return false;
                //if (loads.Count > 1000) return true;
                int last = loads.Last();
                int lastIndex = loads.Count - 1;
                int secondLastIndex = loads.LastIndexOf(last, loads.Count - 2);
                if (secondLastIndex == -1) return false;
                bool corr = true; // we might have a match - assume best
                int distance = lastIndex - secondLastIndex;
                if (secondLastIndex - distance < 0 || distance < 2) return false; // filter out direct repetitions
                for (int i = 1; i < distance; i++) {
                    if (loads[lastIndex - 1] != loads[secondLastIndex - 1]) {
                        corr = false;
                        break;
                    }
                }
                if (corr) {
                    startsAt = secondLastIndex;
                    repeatsEvery = distance;
                    Console.WriteLine($"starts at {secondLastIndex} and repeats evrey {distance}");
                }
                return corr;
            }

            //private static int CalcLoad(char[][] platform) {
            //    int load = 0;
            //    for (int r = 0; r < platform.Length; r++) {
            //        load += platform[r].Count(c => c == 'O') * (platform.Length - r);
            //    }
            //    return load;
            //}
        }
    }
}
