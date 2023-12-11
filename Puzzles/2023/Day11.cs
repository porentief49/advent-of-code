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
                if (!part1) return "";
                List<string> rows = InputAsLines.ToList();
                //PrintGrid(rows);

                // expand rows
                int r = 0;
                do {
                    if (rows[r].All(ch => ch == '.')) {
                        rows.Insert(r, rows[r]);
                        r++;
                    }
                    r++;
                } while (r < rows.Count);

                // expand cols
                int c = 0;
                do {
                    if (rows.Select(r => r[c]).All(ch => ch == '.')) {
                        for (int i = 0; i < rows.Count; i++) rows[i] = rows[i].Insert(c, ".");
                        c++;
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
                int totalDist = 0;
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

                public Galaxy(int row, int col) {
                    Row = row;
                    Col = col;
                }

                public int ManhattanDist(Galaxy other) => Math.Abs(other.Row - Row) + Math.Abs(other.Col - Col);
            }
        }
    }
}
