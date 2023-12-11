namespace Puzzles {

    public partial class Year2023 {

        public class Day04 : DayBase {

            protected override string Title { get; } = "Day 4: Scratchcards";

            public override void SetupAll() {
                AddInputFile(@"2023\04_Example.txt");
                AddInputFile(@"2023\04_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve() {
                var game = InputAsLines.Select(i => new Card(i)).ToList();
                if (Part1) return game.Where(g => g.MatchCount > 0).Select(g => Math.Pow(2, g.MatchCount - 1)).Sum().ToString();
                for (int i = 0; i < game.Count; i++) for (int ii = 0; ii < game[i].MatchCount; ii++) game[i + ii + 1].Copies += game[i].Copies;
                return game.Select(g => g.Copies).Aggregate((x, y) => x + y).ToString();
            }

            private class Card {
                public List<int> Winning;
                public List<int> Mine;
                public readonly int MatchCount;
                public ulong Copies; // smells like this is getting LARGE

                public Card(string line) {
                    var split = line.Replace(':', '|').Split('|', StringSplitOptions.RemoveEmptyEntries);
                    Winning = split[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToList();
                    Mine = split[2].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToList();
                    MatchCount = Winning.Intersect(Mine).Count();
                    Copies = 1;
                }
            }
        }
    }
}
