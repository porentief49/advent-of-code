using System.Collections.Generic;
using System.Linq;
using System.Numerics;


namespace Puzzles {

    public partial class Year2023 {

        public class Day21 : DayBase {

            protected override string Title { get; } = "Day 21: Step Counter";

            public override void SetupAll() {
                //AddInputFile(@"2023\21_Example.txt");
                AddInputFile(@"2023\21_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            private int[][] _field;
            private int[][] _field2;
            private int[][] _field2new;
            private int _height;
            private int _width;
            private int[] _rowUp;
            private int[] _rowDown;
            private int[] _colRight;
            private int[] _colLeft;

            public override string Solve() {
                if (Part1) {
                    //return "";
                    int stepCount = InputFile.Contains("rAiner") ? 64 : 6;
                    _field = InputAsLines.Select(r => r.Select(c => c switch { '#' => 999, '.' => -1, _ => 0 }).ToArray()).ToArray();
                    for (int i = 0; i < stepCount; i++) {
                        for (int r = 0; r < _field.Length; r++) {
                            for (int c = 0; c < _field[0].Length; c++) {
                                if (_field[r][c] == i) GoOneStep1(r, c);
                            }
                        }
                    }
                    return _field.Select(r => r.Count(c => c == stepCount)).Sum().ToString();
                } else {

                    // oh my! No way without help from the internet. https://github.com/villuna/aoc23/wiki/A-Geometric-solution-to-advent-of-code-2023,-day-21 has a good explanation, but I'm not (yet) entirely sure how the code works ...

                    //var x = new Other();
                    //return x.PartTwo(string.Join("\n", InputAsLines)).ToString();


                    _height = InputAsLines.Length;
                    _width = InputAsLines[0].Length;
                    _field2 = InputAsLines.Select(r => r.Select(c => 0).ToArray()).ToArray();
                    _field2new = _field2.Select(r => r.Select(c => 0).ToArray()).ToArray();
                    for (int i = 0; i < _height; i++) _field2[i] = new int[_width];
                    int row = InputAsLines.ToList().FindIndex(r => r.Contains('S'));
                    int col = InputAsLines[row].IndexOf('S');
                    //GoOneStep2(row, col);
                    _field2new[row][col] = 1;
                    Console.WriteLine(_field2new.Select(r => r.Sum()).Sum());
                    //Print(_field2new);
                    _field2 = _field2new.Select(r => r.Select(c => c).ToArray()).ToArray();
                    //Print(_field2);
                    _field2new = _field2.Select(r => r.Select(c => 0).ToArray()).ToArray();
                    //Print(_field2new);
                    //return "";
                    for (int i = 1; i <= 20; i++) {
                        //Console.WriteLine($"================> ENTERING STEP {i}");
                        //Print(_field2);

                        _rowUp = new int[_width];
                        _rowDown = new int[_width];
                        _colRight = new int[_height];
                        _colLeft = new int[_height];
                        Print(_field2);


                        for (int r = 0; r < _height; r++) {
                            for (int c = 0; c < _width; c++) {
                                if (_field2[r][c] > 0) GoOneStep2(r, c);
                            }
                        }

                        Print(_field2new);

                        //// update left & right col
                        //for (int r = 0; r < _height; r++) {
                        //    _field2new[r][_width - 1] |= _colLeft[r];
                        //    _field2new[r][0] |= _colRight[r];
                        //}

                        //// update top & bottom row
                        //for (int c = 0; c < _width; c++) {
                        //    _field2new[_height - 1][c] |= _rowUp[c];
                        //    _field2new[0][c] |= _rowDown[c];
                        //}

                        // update left & right col
                        for (int r = 1; r < _height - 1; r++) {
                            _field2new[r][_width - 1] |= _colLeft[r];
                            _field2new[r][0] |= _colRight[r];
                        }

                        // update top & bottom row
                        for (int c = 1; c < _width - 1; c++) {
                            _field2new[_height - 1][c] |= _rowUp[c];
                            _field2new[0][c] |= _rowDown[c];
                        }

                        Print(_field2new);

                        //if (i == 6 || i == 10 || i == 50 || i == 100 || i == 500 || i == 1000 || i == 5000) Console.WriteLine($"{i} steps => {_field2new.Select(r => r.Sum()).Sum()} plots");
                        Console.WriteLine($"Step {i} => {_field2new.Select(r => r.Sum()).Sum()} plots");
                        //if (i <= 10) Console.WriteLine($"{i} steps => {_field2new.Select(r => r.Count(c => c > 0)).Sum()} plots");
                        //Print(_field2new);
                        _field2 = _field2new.Select(r => r.Select(c => c).ToArray()).ToArray();
                        //Print(_field2);
                        _field2new = _field2.Select(r => r.Select(c => 0).ToArray()).ToArray();
                        //Print(_field2);
                    }
                    return "";
                }
            }

            private void Print(int[][] field) {
                Console.WriteLine($"    {string.Join(" ", _rowUp.Select(x => x.ToString()))}");
                Console.WriteLine("  -" + "--".Repeat(InputAsLines[0].Length + 1));
                for (int r = 0; r < _height; r++) {
                    Console.Write($"{_colLeft[r]} | ");
                    for (int c = 0; c < _width; c++) {
                        if (InputAsLines[r][c] == '#') Console.Write(". ");
                        else if (field[r][c] == 0) Console.Write("  ");
                        else Console.Write((field[r][c].ToString()) + " ");
                    }
                    Console.WriteLine($"| {_colRight[r]}");
                }
                Console.WriteLine("  -" + "--".Repeat(InputAsLines[0].Length + 1));
                Console.WriteLine($"    {string.Join(" ", _rowDown.Select(x => x.ToString()))}");
                Console.WriteLine();
            }

            private void GoOneStep1(int r, int c) {
                int current = _field[r][c];
                if (r > 0) if (_field[r - 1][c] < current) _field[r - 1][c] = current + 1;
                if (r < _field.Length - 1) if (_field[r + 1][c] < current) _field[r + 1][c] = current + 1;
                if (c > 0) if (_field[r][c - 1] < current) _field[r][c - 1] = current + 1;
                if (c < _field[0].Length - 1) if (_field[r][c + 1] < current) _field[r][c + 1] = current + 1;
            }

            private void GoOneStep2(int r, int c) {

                int count = _field2[r][c];
                int rNew = r;
                int cNew = c;

                // check up
                rNew = (r - 1 + _height) % _height;
                if (InputAsLines[rNew][c] != '#') {
                    if (r > 0) _field2new[rNew][c] = count;
                    else _rowUp[c] = count;
                }

                // check down
                rNew = (r + 1) % _height;
                if (InputAsLines[rNew][c] != '#') {
                    if (r < _height - 1) _field2new[rNew][c] = count;
                    else _rowDown[c] = count;
                }

                // check left
                cNew = (c - 1 + _width) % _width;
                if (InputAsLines[r][cNew] != '#') {
                    if (c > 0) _field2new[r][cNew] = count;
                    else _colLeft[r] = count;
                }

                // check right
                cNew = (c + 1) % _width;
                if (InputAsLines[r][cNew] != '#') {
                    if (c < _width - 1) _field2new[r][cNew] = count;
                    else _colRight[r] = count;
                }



                //_field2new[(r + _height - 1) % _height][c] = (r == _field2[(r + _height - 1) % _height][c] + 1 ? 1 : count);



                //int count = _field2[r][c];
                //if (InputAsLines[(r + _height - 1) % _height][c] != '#') _field2new[(r + _height - 1) % _height][c] = (r == _field2[(r + _height - 1) % _height][c] + 1 ? 1 : count);
                //if (InputAsLines[(r + 1) % _height][c] != '#') _field2new[(r + 1) % _height][c] = (r == _height - 1 ? _field2[(r + 1) % _height][c] + 1 : count);
                //if (InputAsLines[r][(c + _width - 1) % _width] != '#') _field2new[r][(c + _width - 1) % _width] = (c == 0 ? _field2[r][(c + _width - 1) % _width] + 1 : count);
                //if (InputAsLines[r][(c + 1) % _width] != '#') _field2new[r][(c + 1) % _width] = (c == _width - 1 ? _field2[r][(c + 1) % _width] + 1 : count);
            }
        }


        private class Other {

            // wip
            public object PartOne(string input) {
                var map = ParseMap(input);
                var s = map.Keys.Where(k => map[k] == 'S').Single();
                var pos = new HashSet<Complex> { s };
                for (var i = 0; i < 64; i++) {
                    pos = Step(map, pos);
                }
                return pos.Count;
            }

            public object PartTwo(string input) {
                var steps = 26501365;
                var map = ParseMap(input);
                var s = map.Keys.Where(k => map[k] == 'S').Single();
                var br = map.Keys.MaxBy(pos => pos.Real + pos.Imaginary);
                var loop = 260;

                Complex center = new Complex(65, 65);

                Complex[] corners = [
                    new Complex(0, 0),
                    new Complex(0, 130),
                    new Complex(130, 130),
                    new Complex(130, 0),
                ];

                Complex[] middles = [
                    new Complex(65, 0),
                    new Complex(65, 130),
                    new Complex(0, 65),
                    new Complex(130, 65),
                ];
                var cohorts = new Dictionary<Complex, long[]>();

                cohorts[center] = new long[loop + 1];
                foreach (var corner in corners) {
                    cohorts[corner] = new long[loop + 1];
                }
                foreach (var middle in middles) {
                    cohorts[middle] = new long[loop + 1];
                }

                var m = 0;
                cohorts[center][m] = 1;
                var phaseLength = loop + 1;
                for (var i = 1; i <= steps; i++) {

                    var nextM = (m + phaseLength - 1) % phaseLength;
                    foreach (var item in cohorts.Keys) {
                        var phase = cohorts[item];
                        var a = phase[(m + phase.Length - 1) % phase.Length];
                        var b = phase[(m + phase.Length - 2) % phase.Length];
                        var c = phase[(m + phase.Length - 3) % phase.Length];

                        phase[nextM] = 0;
                        phase[(nextM + phase.Length - 1) % phase.Length] = b;
                        phase[(nextM + phase.Length - 2) % phase.Length] = a + c;
                    }
                    m = nextM;

                    if (i >= 132 && (i - 132) % 131 == 0) {
                        var newItems = i / 131;
                        foreach (var corner in corners) {
                            cohorts[corner][m] += newItems;
                        }
                    } else if (i >= 66 && (i - 66) % 131 == 0) {
                        foreach (var middle in middles) {
                            cohorts[middle][m]++;
                        }
                    }
                }

                var res = 0L;

                // var counts = 0;
                foreach (var item in cohorts.Keys) {
                    var phase = cohorts[item];
                    var pos = new HashSet<Complex> { item };
                    for (var i = 0; i < phase.Length; i++) {
                        var count = phase[(m + i) % phase.Length];
                        res += pos.Count * count;
                        pos = Step(map, pos);
                    }
                }
                return res;
            }

            HashSet<Complex> Step(Dictionary<Complex, char> map, HashSet<Complex> pos) {
                var res = new HashSet<Complex>();
                foreach (var p in pos) {
                    foreach (var dir in new Complex[] { 1, -1, Complex.ImaginaryOne, -Complex.ImaginaryOne }) {
                        var pT = p + dir;
                        if (map.ContainsKey(pT) && map[pT] != '#') {
                            res.Add(pT);
                        }
                    }
                }
                return res;
            }

            Dictionary<Complex, char> ParseMap(string input) {
                var lines = input.Split("\n");
                return (
                    from irow in Enumerable.Range(0, lines.Length)
                    from icol in Enumerable.Range(0, lines[0].Length)
                    select new KeyValuePair<Complex, char>(
                        new Complex(icol, irow), lines[irow][icol]
                    )
                ).ToDictionary();
            }
        }
    }

}
