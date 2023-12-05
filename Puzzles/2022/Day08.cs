using System;
using System.Net.Sockets;
using System.Text;

namespace Puzzles {
    public partial class Year2022 {
        public class Day08 : DayBase {
            private int[][] _treeHeights;

            protected override string Title { get; } = "Day 8: Treetop Tree House";

            public override void SetupAll() {
                AddInputFile(@"2022\08_Example.txt");
                AddInputFile(@"2022\08_rAiner.txt");
                AddInputFile(@"2022\08_SEGCC.txt");
            }

            public override void Init(string InputFile) {
                InputAsLines = ReadLines(InputFile, true);
                _treeHeights = InputAsLines.Select(y => y.Select(l => int.Parse(l.ToString())).ToArray()).ToArray();
            }

            public override string Solve(bool Part1) {
                int _xSize = InputAsLines[0].Length;
                int _ySize = InputAsLines.Length;
                bool[][] _visibleTrees = new bool[_ySize][];
                int[][] _scenicScores = new int[_ySize][];
                for (int i = 0; i < _ySize; i++) {
                    _visibleTrees[i] = new bool[_xSize];
                    _scenicScores[i] = new int[_xSize];
                }

                //calc visibilities / free areas
                for (int y = 0; y < _ySize; y++) {
                    for (int x = 0; x < _xSize; x++) {
                        int _left = 0;
                        int _right = 0;
                        int _up = 0;
                        int _down = 0;

                        if (Part1) {
                            while (x - _left > 0 && _treeHeights[y][x] > _treeHeights[y][x - _left - 1]) _left++;
                            while (x + _right < _xSize - 1 && _treeHeights[y][x] > _treeHeights[y][x + _right + 1]) _right++;
                            while (y - _up > 0 && _treeHeights[y][x] > _treeHeights[y - _up - 1][x]) _up++;
                            while (y + _down < _ySize - 1 && _treeHeights[y][x] > _treeHeights[y + _down + 1][x]) _down++;
                            _visibleTrees[y][x] = _left == x || _right == _xSize - x - 1 || _up == y || _down == _ySize - y - 1;
                        } else {
                            if (x > 0) do _left++; while (x - _left > 0 && _treeHeights[y][x] > _treeHeights[y][x - _left]);
                            if (x < _xSize - 1) do _right++; while (x + _right < _xSize - 1 && _treeHeights[y][x] > _treeHeights[y][x + _right]);
                            if (y > 0) do _up++; while (y - _up > 0 && _treeHeights[y][x] > _treeHeights[y - _up][x]);
                            if (y < _ySize - 1) do _down++; while (y + _down < _ySize - 1 && _treeHeights[y][x] > _treeHeights[y + _down][x]);
                            _scenicScores[y][x] = _left * _right * _up * _down;
                        }
                    }
                }

                //determine solution
                if (Part1) {
                    if (Verbose) PrintGrid(_treeHeights, 1);
                    if (Verbose) PrintGrid(_visibleTrees);
                    return FormatResult(_visibleTrees.Select(y => y.Count(x => x == true)).Sum(), "visible trees");
                }
                if (Verbose) PrintGrid(_scenicScores, 3);
                return FormatResult(_scenicScores.Select(y => y.Max()).Max(), "best scenic score");
            }
        }
    }
}
