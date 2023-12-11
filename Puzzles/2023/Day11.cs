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
                //if (!part1) return "";
                List<string> rows = InputAsLines.ToList();


                Galaxy.emptyRows.Clear();
                Galaxy.emptyCols.Clear();
                //List<int> emptyRows = new List<int>();
                //List<int> emptyCols = new List<int>();

                //PrintGrid(rows);

                // expand rows
                int r = 0;
                do {
                    if (rows[r].All(ch => ch == '.')) {
                        //rows.Insert(r, rows[r]);
                        //r++;
                        Galaxy.emptyRows.Add(r);
                    }
                    r++;
                } while (r < rows.Count);

                // expand cols
                int c = 0;
                do {
                    if (rows.Select(r => r[c]).All(ch => ch == '.')) {
                        //for (int i = 0; i < rows.Count; i++) rows[i] = rows[i].Insert(c, ".");
                        //c++;
                        Galaxy.emptyCols.Add(c);
                    }
                    c++;
                } while (c < rows[0].Length);

                //PrintGrid(rows);

                // find galaxies
                var galaxies = new List<Galaxy>();

                for (int row = 0; row < rows.Count; row++) {
                    //string? row = rows[i1];
                    for (int col = 0; col < rows[row].Length; col++) {
                        //char ch = row[i];
                        if (rows[row][col] == '#') galaxies.Add(new Galaxy(row, col));
                    }
                }

                // sum shortest paths
                Galaxy.EmptyReplaceWith = part1 ? 2 : 1000000;

                long totalDist = 0;
                for (int i = 0; i < galaxies.Count; i++) {
                    for (int ii = i + 1; ii < galaxies.Count; ii++) {
                        totalDist += galaxies[i].ManhattanDist(galaxies[ii]);
                    }
                }


                return totalDist.ToString();
            }

            private class Galaxy {
                public int Row;
                public int Col;
                public static List<int> emptyRows = new List<int>();
                public static List<int> emptyCols = new List<int>();
                public static long EmptyReplaceWith = 1;

                public Galaxy(int row, int col) {
                    Row = row;
                    Col = col;
                }

                public long ManhattanDist(Galaxy other) {
                    var fromRow = Math.Min(Row, other.Row);
                    var toRow = Math.Max(Row, other.Row);
                    var rows = (long)(toRow - fromRow) + emptyRows.Where(r => r >= fromRow && r <= toRow).Count() * (EmptyReplaceWith - 1);

                    var fromCol = Math.Min(Col, other.Col);
                    var toCol = Math.Max(Col, other.Col);
                    var cols = (long)(toCol - fromCol) + emptyCols.Where(c => c >= fromCol && c <= toCol).Count() * (EmptyReplaceWith - 1);

                    return rows + cols;
                }
            }
        }
    }
}
