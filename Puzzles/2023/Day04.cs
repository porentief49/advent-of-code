using System;
using System.Text.RegularExpressions;

namespace Puzzles {
    public partial class Year2023 {
        public class Day04 : DayBase {

            protected override string Title { get; } = "Day 3: Gear Ratios";

            public override void SetupAll() {
                AddInputFile(@"2023\04_Example.txt");
                AddInputFile(@"2023\04_rAiner.txt");
            }

            public override void Init(string InputFile) => InputData = ReadFile(InputFile, true);

            public override string Solve(bool Part1) {
                if (!Part1) return "";
                var game = InputData.Select(i => new Card(i)).ToList();
                int totalScore = 0;
                for (int i = 0; i < game.Count; i++) if (game[i].MatchCount > 0) totalScore += (int)Math.Pow(2, game[i].MatchCount - 1);
                return totalScore.ToString();
            }

            private class Card {
                public List<int> Winning;
                public List<int> Mine;
                public readonly int MatchCount;

                public Card(string line) {
                    var split = line.Replace(':', '|').Split('|', StringSplitOptions.RemoveEmptyEntries);
                    Winning = split[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToList();
                    Mine = split[2].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToList();
                    MatchCount = Winning.Intersect(Mine).Count();
                }

            }
        }
    }
}
