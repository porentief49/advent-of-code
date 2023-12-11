using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Text;

namespace Puzzles {
    public partial class Year2022 {
        public class Day09 : DayBase {
            private List<(char dir, int steps)> _movements = new();

            protected override string Title { get; } = "Day 9: Rope Bridge";

            public override void SetupAll() {
                AddInputFile(@"2022\09_Example1.txt");
                AddInputFile(@"2022\09_Example2.txt");
                AddInputFile(@"2022\09_rAiner.txt");
                AddInputFile(@"2022\09_SEGCC.txt");
            }

            public override void Init(string InputFile) {
                InputAsLines = ReadLines(InputFile, true);
                _movements = InputAsLines.Select(x => (x[0], int.Parse(x.Split(' ')[1]))).ToList();
            }

            public override string Solve() {
                int _knotCount = Part1 ? 2 : 10;
                var _Trails = new List<List<(int x, int y)>>();
                List<(int x, int y)> _current = new();
                for (int i = 0; i < _knotCount; i++) {
                    _current.Add((0, 0));
                    _Trails.Add(new List<(int x, int y)> { (0, 0) });
                }

                // move rope
                foreach (var _move in _movements) {
                    (int x, int y) _delta;
                    for (int i = 0; i < _move.steps; i++) {
                        // move head
                        _delta = _move.dir switch { 'R' => (1, 0), 'L' => (-1, 0), 'U' => (0, -1), _ => (0, 1) }; // _ must be 'D'
                        _current[0] = (_current[0].x + _delta.x, _current[0].y + _delta.y);
                        _Trails[0].Add(_current[0]);

                        // move tail(s)
                        for (int ii = 1; ii < _knotCount; ii++) {
                            _delta = (_current[ii - 1].x - _current[ii].x, _current[ii - 1].y - _current[ii].y);
                            if (_delta.x > 1) _current[ii] = (_current[ii].x + 1, _current[ii].y + Math.Sign(_delta.y));
                            else if (_delta.x < -1) _current[ii] = (_current[ii].x - 1, _current[ii].y + Math.Sign(_delta.y));
                            else if (_delta.y > 1) _current[ii] = (_current[ii].x + Math.Sign(_delta.x), _current[ii].y + 1);
                            else if (_delta.y < -1) _current[ii] = (_current[ii].x + Math.Sign(_delta.x), _current[ii].y - 1);
                            _Trails[ii].Add(_current[ii]);
                            if (Verbose) PrintCurrentState(_Trails, $"{_move.dir} {_move.steps}, in step {i}, after substep {ii}");
                        }
                    }
                }

                //determine solutions
                if (Verbose) PrintTrail(_Trails[_knotCount - 1], 'T');
                return (FormatResult(_Trails[_knotCount - 1].Distinct().Count(), "visited positions"));
            }

            private void PrintTrail(List<(int x, int y)> Trail, char Label) {
                int _xMin = Trail.Select(l => l.x).Min();
                int _xMax = Trail.Select(l => l.x).Max();
                int _yMin = Trail.Select(l => l.y).Min();
                int _yMax = Trail.Select(l => l.y).Max();
                char[][] _grid = InitJaggedArray(_yMax - _yMin + 1, _xMax - _xMin + 1, '.');
                Trail.ForEach(l => _grid[l.y - _yMin][l.x - _xMin] = Label);
                _grid[0 - _yMin][0 - _xMin] = 's';
                Console.WriteLine(string.Join(Environment.NewLine, _grid.Select(x => string.Concat(x))) + Environment.NewLine);
            }

            private void PrintCurrentState(List<List<(int x, int y)>> Trails, string Label) {
                int _xMin = Trails.Select(l => l.Select(ll => ll.x).Min()).Min();
                int _xMax = Trails.Select(l => l.Select(ll => ll.x).Max()).Max();
                int _yMin = Trails.Select(l => l.Select(ll => ll.y).Min()).Min();
                int _yMax = Trails.Select(l => l.Select(ll => ll.y).Max()).Max();
                char[][] _grid = InitJaggedArray(_yMax - _yMin + 1, _xMax - _xMin + 1, '.');
                StringBuilder _sb = new();
                for (int i = 0; i < Trails.Count; i++) {
                    _grid[Trails[i].Last().y - _yMin][Trails[i].Last().x - _xMin] = i.ToString()[0];
                    _sb.Append($"{i}-({Trails[i].Last().x}|{Trails[i].Last().y}) ");
                }
                _grid[0 - _yMin][0 - _xMin] = 's';
                Console.WriteLine($"== {Label} ==\r\n\r\n{string.Join(Environment.NewLine, _grid.Select(x => string.Concat(x)))}\r\n{_sb}\r\n");
            }
        }
    }
}
