namespace Puzzles {

    public partial class Year2023 {

        public class Day14 : DayBase {

            protected override string Title { get; } = "Day 14: Parabolic Reflector Dish";

            public override void SetupAll() {
                AddInputFile(@"2023\14_Example.txt");
                AddInputFile(@"2023\14_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            //private enum Direction { N, E, S, W}

            public override string Solve() {
                if (Part2) return "";
                var platform = InputAsLines.Select(i => i.ToCharArray()).ToArray();
                //PrintGrid(platform);
                // tilt north
                bool stillRolling;
                do {
                    stillRolling = false;
                    for (int r = 1; r < platform.Length; r++) {
                        for (int c = 0; c < platform[r].Length; c++) {
                            if (platform[r][c] == 'O' && platform[r - 1][c] == '.') {
                                (platform[r][c], platform[r - 1][c]) = (platform[r - 1][c], platform[r][c]);
                                stillRolling = true;
                            }
                        }
                    }
                    //PrintGrid(platform);
                } while (stillRolling);

                // count results
                int load = 0;
                for (int r = 0; r < platform.Length; r++) {
                    load += platform[r].Count(c => c == 'O') * (platform.Length - r);
                }
                return load.ToString();
            }
        }
    }
}
