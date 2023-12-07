namespace Puzzles {

    public partial class Year2023 {

        public class Day02 : DayBase {

            protected override string Title { get; } = "Day 2: Cube Conundrum";

            public override void SetupAll() {
                AddInputFile(@"2023\02_Example.txt");
                AddInputFile(@"2023\02_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve(bool part1) {
                var games = InputAsLines.Select(i => new Game(i)).ToList();
                int sum = 0;
                foreach (var game in games) {
                    if (part1) {
                        bool valid = true; // assume best
                        foreach (var draw in game.Draws) if (draw.red > 12 || draw.green > 13 || draw.blue > 14) valid = false;
                        if (valid) sum += game.Id;
                    } else {
                        int red = game.Draws.Max(d => d.red);
                        int green = game.Draws.Max(d => d.green);
                        int blue = game.Draws.Max(d => d.blue);
                        sum += red * green * blue;
                    }
                }
                return sum.ToString();
            }

            private class Game {

                public int Id;
                public List<(int red, int green, int blue)> Draws = new();

                public Game(string game) {
                    var split = game.Trim().Split(':');
                    Id = int.Parse(split[0].Trim().Split(" ")[1]);
                    Draws = split[1].Trim().Split(";").Select(d => Draw(d)).ToList();
                }

                private (int red, int green, int blue) Draw(string draw) {
                    int red = 0;
                    int green = 0;
                    int blue = 0;
                    foreach (var item in draw.Trim().Split(',')) {
                        var split = item.Trim().Split(" ");
                        if (split[1] == "red") red = int.Parse(split[0]);
                        if (split[1] == "green") green = int.Parse(split[0]);
                        if (split[1] == "blue") blue = int.Parse(split[0]);
                    }
                    return (red, green, blue);
                }
            }
        }
    }
}
