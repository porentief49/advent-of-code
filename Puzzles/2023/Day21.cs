
namespace Puzzles {

    public partial class Year2023 {

        public class Day21 : DayBase {

            protected override string Title { get; } = "Day 21: Step Counter";

            public override void SetupAll() {
                AddInputFile(@"2023\21_Example.txt");
                AddInputFile(@"2023\21_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            private int[][] _field;

            public override string Solve() {
                int stepCount = InputFile.Contains("rAiner")? 64:6;
                if (Part2) return "";
                _field = InputAsLines.Select(r => r.Select(c => c switch { '#' => 999, '.' => -1, _ => 0 }).ToArray()).ToArray();
                for (int i = 0; i < stepCount; i++) {
                    for (int r = 0; r < _field.Length; r++) {
                        for (int c = 0; c < _field[0].Length; c++) {
                            if (_field[r][c] == i) GoOneStep(r, c);
                        }
                    }
                }
                return _field.Select(r=>r.Count(c=>c== stepCount)).Sum().ToString();
            }

            private void GoOneStep(int r, int c) {
                int current = _field[r][c];
                if (r > 0) if (_field[r - 1][c] <current) _field[r - 1][c] = current + 1;
                if (r < _field.Length - 1) if (_field[r + 1][c] < current) _field[r + 1][c] = current + 1;
                if (c > 0) if (_field[r][c - 1] < current) _field[r][c - 1] = current + 1;
                if (c < _field[0].Length - 1) if (_field[r][c + 1] < current) _field[r][c + 1] = current + 1;
            }
        }
    }
}
