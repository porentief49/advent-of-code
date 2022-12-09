using System;
using System.Net.Sockets;
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
                AddInputFile(@"2022\09_Example.txt");
                AddInputFile(@"2022\09_rAiner.txt");
            }

            public override void Init(string InputFile) => InputData = ReadFile(InputFile, true);

            public override string Solve(bool Part1)
            {
                var _headTrail = new List<(int x, int y)> { (0, 0) };
                var _tailTrail = new List<(int x, int y)> { (0, 0) };
                (int x, int y) _head = (0, 0);
                (int x, int y) _tail = (0, 0);

                List<(char dir, int steps)> _movements = new();
                _movements = InputData.Select(x => (x[0], int.Parse(x.Split(' ')[1]))).ToList();

                foreach (var _move in _movements)
                {
                    for (int i = 0; i < _move.steps; i++)
                    {
                        // move head

                        switch (_move.dir)
                        {
                            case 'R': // x +
                                _head = (_head.x + 1, _head.y);
                                break;
                            case 'L': // x -
                                _head = (_head.x - 1, _head.y);
                                break;
                            case 'U': // y -
                                _head = (_head.x, _head.y - 1);
                                break;
                            default: // must be 'D' // y +
                                _head = (_head.x, _head.y + 1);
                                break;
                        }
                        _headTrail.Add(_head);

                        // move tail
                        (int x, int y) _distance = (_head.x - _tail.x, _head.y - _tail.y);
                        if (_distance.x > 1) _tail = (_tail.x + 1, _head.y);
                        if (_distance.x < -1) _tail = (_tail.x -1, _head.y);
                        if (_distance.y > 1) _tail = (_head.x, _tail.y + 1);
                        if (_distance.y < -1) _tail = (_head.x, _tail.y - 1);
                        _tailTrail.Add(_tail);
                    }
                }
                if (Part1)
                {
                    Console.WriteLine(DumpTrail(_headTrail, 'H'));
                    Console.WriteLine(DumpTrail(_tailTrail, 'T'));
                }
                return (FormatResult(_tailTrail.Distinct().Count(),"visited positions"));
            }
            private (int x, int y) GetDistance((int x, int y) Head, (int x, int y) Tail)
            {
                return (Head.x - Tail.x, Head.y - Tail.y);
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
        }
    }
}
