namespace Puzzles {

    public partial class Year2023 {

        public class Day04 : DayBase {

            protected override string Title { get; } = "Day 4: Scratchcards";

            public override void SetupAll() {
                AddInputFile(@"2023\04_Example.txt");
                AddInputFile(@"2023\04_rAiner.txt");
            }

            public override void Init(string InputFile) => InputData = ReadFile(InputFile, true);

            public override string Solve(bool Part1) {
                var game = InputData.Select(i => new Card(i)).ToList();
                ulong totalScore = 0;
                if (Part1) {
                    for (int i = 0; i < game.Count; i++) if (game[i].MatchCount > 0) totalScore += (ulong)Math.Pow(2, game[i].MatchCount - 1);
                } else {
                    for (int i = 0; i < game.Count; i++) for (int ii = 0; ii < game[i].MatchCount; ii++) game[i + ii + 1].Copies += game[i].Copies;
                    totalScore = game.Select(g => g.Copies).Aggregate((x, y) => x + y);
                }
                return totalScore.ToString();
            }

            private class Card {
                public List<int> Winning;
                public List<int> Mine;
                public readonly int MatchCount;
                public ulong Copies;

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
