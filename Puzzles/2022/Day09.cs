using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Text;

namespace Puzzles
{
    public partial class Year2022
    {
        public class Day09 : DayBase
        {
            protected override string Title { get; } = "Day 9: Rope Bridge";

            public override void SetupAll()
            {
                AddInputFile(@"2022\09_Example1.txt");
                AddInputFile(@"2022\09_Example2.txt");
                AddInputFile(@"2022\09_rAiner.txt");
            }

            public override void Init(string InputFile) => InputData = ReadFile(InputFile, true);

            public override string Solve(bool Part1)
            {
                int _knotCount = Part1 ? 2 : 10;
                var _Trails = new List<List<(int x, int y)>>();
                List<(int x, int y)> _current = new();
                List<(char dir, int steps)> _movements = new();


                for (int i = 0; i < _knotCount; i++)
                {
                    _current.Add((0, 0));
                    _Trails.Add(new List<(int x, int y)> { (0, 0) });
                }


                _movements = InputData.Select(x => (x[0], int.Parse(x.Split(' ')[1]))).ToList();

                int _count = 0;
                foreach (var _move in _movements)
                {
                    _count++;
                    for (int i = 0; i < _move.steps; i++)
                    {
                        // move head
                        switch (_move.dir)
                        {
                            case 'R': // x +
                                _current[0] = (_current[0].x + 1, _current[0].y);
                                break;
                            case 'L': // x -
                                _current[0] = (_current[0].x - 1, _current[0].y);
                                break;
                            case 'U': // y -
                                _current[0] = (_current[0].x, _current[0].y - 1);
                                break;
                            default: // must be 'D' // y +
                                _current[0] = (_current[0].x, _current[0].y + 1);
                                break;
                        }
                        _Trails[0].Add(_current[0]);


                        // move tail(s)
                        for (int ii = 1; ii < _knotCount; ii++)
                        {
                            if (false) //old model
                            {
                                (int x, int y) _distance = (_current[ii - 1].x - _current[ii].x, _current[ii - 1].y - _current[ii].y);
                                if (_distance.x > 1) _current[ii] = (_current[ii].x + 1, _current[ii - 1].y);
                                if (_distance.x < -1) _current[ii] = (_current[ii].x - 1, _current[ii - 1].y);
                                if (_distance.y > 1) _current[ii] = (_current[ii - 1].x, _current[ii].y + 1);
                                if (_distance.y < -1) _current[ii] = (_current[ii - 1].x, _current[ii].y - 1);
                            }
                            else // new model
                            {
                                int _dx = _current[ii - 1].x - _current[ii].x;
                                int _dy = _current[ii - 1].y - _current[ii].y;
                                if (_dx > 1)
                                {
                                    _current[ii] = (_current[ii].x + 1, _dy == 0 ? _current[ii].y : _current[ii].y + Math.Sign(_dy));
                                }
                                else if (_dx < -1)
                                {
                                    _current[ii] = (_current[ii].x - 1, _dy == 0 ? _current[ii].y : _current[ii].y + Math.Sign(_dy));
                                }
                                else if (_dy > 1)
                                {
                                    _current[ii] = (_dx == 0 ? _current[ii].x : _current[ii].x + Math.Sign(_dx), _current[ii].y + 1);
                                }
                                else if (_dy < -1)
                                {
                                    _current[ii] = (_dx == 0 ? _current[ii].x : _current[ii].x + Math.Sign(_dx), _current[ii].y - 1);
                                }
                            }
                            _Trails[ii].Add(_current[ii]);
                            if (Verbose) Console.WriteLine(DumpCurrentState(_Trails, $"{_move.dir} {_move.steps}, in step {i}, after substep {ii}"));
                        }
                    }
                }
                if (Verbose) Console.WriteLine(DumpTrail(_Trails[_knotCount - 1], 'T'));
                return (FormatResult(_Trails[_knotCount - 1].Distinct().Count(), "visited positions"));
            }

            private string DumpTrail(List<(int x, int y)> Trail, char Label)
            {
                int _xMin = Trail.Select(l => l.x).Min();
                int _xMax = Trail.Select(l => l.x).Max();
                int _yMin = Trail.Select(l => l.y).Min();
                int _yMax = Trail.Select(l => l.y).Max();
                char[][] _grid = new char[_yMax - _yMin + 1][];
                for (int i = 0; i < _grid.Length; i++)
                {
                    char[] _line = new char[_xMax - _xMin + 1];
                    for (int ii = 0; ii < _line.Length; ii++) _line[ii] = '.';
                    _grid[i] = _line;
                }
                Trail.ForEach(l => _grid[l.y - _yMin][l.x - _xMin] = Label);
                _grid[0 - _yMin][0 - _xMin] = 's';
                return String.Join(Environment.NewLine, _grid.Select(x => string.Concat(x))) + Environment.NewLine;
            }

            private string DumpCurrentState(List<List<(int x, int y)>> Trails, string Label)
            {
                int _xMin = Trails.Select(l => l.Select(ll => ll.x).Min()).Min();
                int _xMax = Trails.Select(l => l.Select(ll => ll.x).Max()).Max();
                int _yMin = Trails.Select(l => l.Select(ll => ll.y).Min()).Min();
                int _yMax = Trails.Select(l => l.Select(ll => ll.y).Max()).Max();
                char[][] _grid = new char[_yMax - _yMin + 1][];
                StringBuilder _sb = new();
                for (int i = 0; i < _grid.Length; i++)
                {
                    char[] _line = new char[_xMax - _xMin + 1];
                    for (int ii = 0; ii < _line.Length; ii++) _line[ii] = '.';
                    _grid[i] = _line;
                }
                for (int i = 0; i < Trails.Count; i++)
                {
                    _grid[Trails[i].Last().y - _yMin][Trails[i].Last().x - _xMin] = i.ToString()[0];
                    _sb.Append($"{i}-({Trails[i].Last().x}|{Trails[i].Last().y}) ");
                }
                _grid[0 - _yMin][0 - _xMin] = 's';
                return $"== {Label} ==\r\n\r\n" + String.Join(Environment.NewLine, _grid.Select(x => string.Concat(x))) + $"\r\n{_sb}\r\n";
            }
        }
    }
}
