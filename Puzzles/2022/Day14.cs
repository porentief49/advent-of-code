using System;
using System.Net.Sockets;
using System.Text;

namespace Puzzles {
    public partial class Year2022 {
        public class Day14 : DayBase {
            private static (int y, int x) _start = (0, 500);
            private bool _part1 = false;
            private List<List<(int y, int x)>> _rocks;
            private char[][] _cave;
            private static (int y, int x) _min;
            private static (int y, int x) _max;

            protected override string Title { get; } = "Day 14: Regolith Reservoir";

            public override void SetupAll() {
                AddInputFile(@"2022\14_Example.txt");
                AddInputFile(@"2022\14_rAiner.txt");
                AddInputFile(@"2022\14_SEGCC.txt");
            }

            public override void Init(string InputFile) {
                InputAsLines = ReadLines(InputFile, true);
                _min = _start;
                _max = _start;
                _rocks = new();
                foreach (var item in InputAsLines) {
                    List<(int x, int y)> coords = new();
                    foreach (var item2 in item.Split(" -> ")) {
                        string[] split3 = item2.Split(',');
                        int y = int.Parse(split3[1]);
                        int x = int.Parse(split3[0]);
                        if (y < _min.y) _min.y = y;
                        if (y > _max.y) _max.y = y;
                        if (x < _min.x) _min.x = x;
                        if (x > _max.x) _max.x = x;
                        coords.Add((y, x));
                    }
                    _rocks.Add(coords);
                }
            }

            public override string Solve(bool Part1) {
                _part1 = Part1;
                BuildCave();
                if (Verbose) PrintGrid(_cave);
                int count = 0;
                while (DropSand()) {
                    count++;
                    if (Verbose) PrintGrid(_cave);
                }
                return FormatResult(count, "sand units");
            }

            private void SetCave(int y, int x, char what) => _cave[y - _min.y][x - _min.x] = what;

            private char GetCave(int y, int x, bool part1) {
                if (!part1 && y == _max.y) return '#'; // part2 has an infinite base at the bottom
                if (y < _min.y || y > _max.y || x < _min.x || x > _max.x) return '.'; // to avoid error when trying to peek outside the grid
                return _cave[y - _min.y][x - _min.x];
            }

            public void BuildCave() {
                // build field
                if (!_part1) {
                    _max.y += 2;
                    int height = _max.y - _min.y;
                    _min.x = Math.Min(_min.x, _start.x - height - 1); // for part2, extend the shape for the 45deg dumping angle
                    _max.x = Math.Max(_max.x, _start.x + height + 1);
                }
                _cave = InitJaggedArray(_max.y - _min.y + 1, _max.x - _min.x + 1, '.');

                // add rocks
                foreach (var rock in _rocks) {
                    for (int ii = 1; ii < rock.Count; ii++) {
                        int yInc = Math.Sign(rock[ii].y - rock[ii - 1].y);
                        int xInc = Math.Sign(rock[ii].x - rock[ii - 1].x);
                        int yCurrent = rock[ii - 1].y;
                        int xCurrent = rock[ii - 1].x;
                        SetCave(yCurrent, xCurrent, '#');
                        do {
                            yCurrent += yInc;
                            xCurrent += xInc;
                            SetCave(yCurrent, xCurrent, '#');
                        } while (yCurrent != rock[ii].y || xCurrent != rock[ii].x);
                    }
                }
            }

            public bool DropSand() {
                (int y, int x) pos = _start;
                bool done = false;
                if (GetCave(pos.y, pos.x, _part1) != '.') return false;
                do {
                    if (GetCave(pos.y + 1, pos.x, _part1) == '.') pos.y++; // fall straight down
                    else if (GetCave(pos.y + 1, pos.x - 1, _part1) == '.') pos = (pos.y + 1, pos.x - 1); // slide to the left
                    else if (GetCave(pos.y + 1, pos.x + 1, _part1) == '.') pos = (pos.y + 1, pos.x + 1); // slide to the right
                    else // final position
                    {
                        SetCave(pos.y, pos.x, 'o');
                        done = true;
                    }
                    if (pos.y == _max.y) return false;
                } while (!done);
                return true;
            }
        }
    }
}
