
namespace Puzzles {

    public partial class Year2023 {

        public class Day21 : DayBase {

            protected override string Title { get; } = "Day 21: Step Counter";

            public override void SetupAll() {
                AddInputFile(@"2023\21_Example.txt");
                //AddInputFile(@"2023\21_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            private int[][] _field;
            private int[][] _field2;
            private int[][] _field2new;
            private int _height;
            private int _width;

            public override string Solve() {
                if (Part1) {
                    return "";
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
                    for (int i = 1; i <= 1000; i++) {
                        //Console.WriteLine($"================> ENTERING STEP {i}");
                        //Print(_field2);
                        for (int r = 0; r < _height; r++) {
                            for (int c = 0; c < _width; c++) {
                                if (_field2[r][c] > 0) GoOneStep2(r, c);
                            }
                        }
                        if (i == 6 || i == 10 || i == 50 || i == 100 || i == 500 || i == 1000 || i == 5000) Console.WriteLine($"{i} steps => {_field2new.Select(r => r.Sum()).Sum()} plots");
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
                for (int r = 0; r < _height; r++) {
                    for (int c = 0; c < _width; c++) {
                        if (InputAsLines[r][c] == '#') Console.Write("--- ");
                        else Console.Write((field[r][c].ToString() + "   ").Substring(0, 3) + " ");
                    }
                    Console.WriteLine();
                }
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
                if (InputAsLines[(r + _height - 1) % _height][c] != '#') _field2new[(r + _height - 1) % _height][c] = count + (r == 0 ? 1 : 0);
                if (InputAsLines[(r + 1) % _height][c] != '#') _field2new[(r + 1) % _height][c] = count + (r == _height - 1 ? 1 : 0);
                if (InputAsLines[r][(c + _width - 1) % _width] != '#') _field2new[r][(c + _width - 1) % _width] = count + (c == 0 ? 1 : 0);
                if (InputAsLines[r][(c + 1) % _width] != '#') _field2new[r][(c + 1) % _width] = count + (c == _width - 1 ? 1 : 0);
            }
        }
    }
}
