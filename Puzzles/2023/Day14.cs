
using System.Diagnostics.CodeAnalysis;

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

            public override string Solve() {
                List<int> loads = new();
                int startsAt = 0;
                int repeatsEvery = 0;
                if (true) {
                    var platformXX = new Platform(InputAsLines);
                    do {
                        platformXX.Tilt(Dir.N);
                        if (Part1) return platformXX.CalcLoad().ToString();
                        platformXX.Tilt(Dir.W);
                        platformXX.Tilt(Dir.S);
                        platformXX.Tilt(Dir.E);
                        loads.Add(platformXX.CalcLoad());
                    } while (!Correlate(loads, ref startsAt, ref repeatsEvery));
                }
                return loads[startsAt + (1000000000 - startsAt) % repeatsEvery - 1].ToString();
            }

            bool Correlate(List<int> loads, ref int startsAt, ref int repeatsEvery) {
                if (loads.Count < 100) return false;
                int lastIndex = loads.Count - 1;
                startsAt = loads.LastIndexOf(loads.Last(), loads.Count - 2);
                repeatsEvery = lastIndex - startsAt;
                if (startsAt == -1 || startsAt - repeatsEvery < 0 || repeatsEvery < 2 || loads.Count < 100) return false; // filter out direct repetitions (they might occur randomly)
                for (int i = 1; i < repeatsEvery; i++) if (loads[lastIndex - 1] != loads[startsAt - 1]) return false;
                return true;
            }

            private class Platform {

                private char[][] _map;

                public Platform(string[] input) => _map = input.Select(i => i.ToCharArray()).ToArray();

                public int CalcLoad() => _map.Select((r, i) => r.Count(c => c == 'O') * (_map.Length - i)).Sum();

                public void Tilt(Dir dir) {
                    bool stillRolling;
                    do {
                        stillRolling = false;
                        for (int row = (dir == Dir.N || dir == Dir.S) ? 1 : 0; row < _map.Length; row++) {
                            for (int col = (dir == Dir.W || dir == Dir.E) ? 1 : 0; col < _map[0].Length; col++) {
                                int r = dir == Dir.S ? _map.Length - row - 1 : row;
                                int c = dir == Dir.E ? _map[0].Length - col - 1 : col;
                                int rTo = dir switch { Dir.N => r - 1, Dir.S => r + 1, _ => r };
                                int cTo = dir switch { Dir.W => c - 1, Dir.E => c + 1, _ => c };
                                if (_map[r][c] == 'O' && _map[rTo][cTo] == '.') {
                                    (_map[r][c], _map[rTo][cTo]) = (_map[rTo][cTo], _map[r][c]);
                                    stillRolling = true;
                                }
                            }
                        }
                    } while (stillRolling);
                }
            }
        }
    }
}
