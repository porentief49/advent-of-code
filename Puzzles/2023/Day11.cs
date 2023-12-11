namespace Puzzles {

    public partial class Year2023 {

        public class Day11 : DayBase {

            protected override string Title { get; } = "Day 11: Cosmic Expansion";

            public override void SetupAll() {
                AddInputFile(@"2023\11_Example.txt");
                AddInputFile(@"2023\11_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve(bool part1) {
                List<string> rows = InputAsLines.ToList();
                Galaxy.EmptyReplaceWith = part1 ? 2 : 1000000;
                Galaxy.EmptyRows = Enumerable.Range(0, rows.Count).Where(r => rows[r].All(ch => ch == '.')).ToList();
                Galaxy.EmptyCols = Enumerable.Range(0, rows[0].Length).Where(c => rows.Select(r => r[c]).All(c => c == '.')).ToList();
                var galaxies = new List<Galaxy>();
                for (int row = 0; row < rows.Count; row++) for (int col = 0; col < rows[row].Length; col++) if (rows[row][col] == '#') galaxies.Add(new Galaxy(row, col));
                long totalDist = 0;
                for (int i = 0; i < galaxies.Count; i++) for (int ii = i + 1; ii < galaxies.Count; ii++) totalDist += galaxies[i].ManhattanDist(galaxies[ii]);
                return totalDist.ToString();
            }

            private class Galaxy {
                public int Row;
                public int Col;
                public static List<int> EmptyRows = new();
                public static List<int> EmptyCols = new();
                public static long EmptyReplaceWith;

                public Galaxy(int row, int col) {
                    Row = row;
                    Col = col;
                }

                public long ManhattanDist(Galaxy other) {
                    long fromRow = Math.Min(Row, other.Row);
                    long toRow = Math.Max(Row, other.Row);
                    long rows = (toRow - fromRow) + EmptyRows.Where(r => r >= fromRow && r <= toRow).Count() * (EmptyReplaceWith - 1);
                    long fromCol = Math.Min(Col, other.Col);
                    long toCol = Math.Max(Col, other.Col);
                    long cols = (toCol - fromCol) + EmptyCols.Where(c => c >= fromCol && c <= toCol).Count() * (EmptyReplaceWith - 1);
                    return rows + cols;
                }
            }
        }
    }
}
