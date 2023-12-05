namespace Puzzles {

    public partial class Year2023 {

        public class Day05 : DayBase {

            protected override string Title { get; } = "Day 5: If You Give A Seed A Fertilizer";

            public override void SetupAll() {
                AddInputFile(@"2023\05_Example.txt");
                AddInputFile(@"2023\05_rAiner.txt");
            }

            public override void Init(string InputFile) => InputData = ReadFile(InputFile, false);

            public override string Solve(bool Part1) {
                if (!Part1) return "";
                var mapDefinitions = string.Join('\n', InputData).Split("\n\n");
                var maps = mapDefinitions.Skip(1).Select(m => new Map(m));
                var seeds = mapDefinitions.First().Split(' ').Skip(1).Select(m => ulong.Parse(m)).ToArray();
                List<ulong> locations = new();
                foreach (var seed in seeds) {
                    ulong location = seed;
                    //Console.Write($"{seed}");
                    foreach (var map in maps) {
                        foreach (var range in map.Ranges) {
                            if (location >= range.SourceStart && location < range.SourceStart + range.Length) {
                                location = location - range.SourceStart + range.DestinationStart;
                                break;
                            }
                        }
                        //Console.Write($" --> {location}");
                    }
                    //Console.WriteLine();
                    locations.Add(location);
                }
                return locations.Min().ToString();
                //var game = InputData.Select(i => new Card(i)).ToList();
                //if (Part1) return game.Where(g => g.MatchCount > 0).Select(g => Math.Pow(2, g.MatchCount - 1)).Sum().ToString();
                //for (int i = 0; i < game.Count; i++) for (int ii = 0; ii < game[i].MatchCount; ii++) game[i + ii + 1].Copies += game[i].Copies;
                //return game.Select(g => g.Copies).Aggregate((x, y) => x + y).ToString();
            }

            private class Map {

                public List<(ulong DestinationStart, ulong SourceStart, ulong Length)> Ranges = new();

                public Map(string mapDefinition) {
                    var lines = mapDefinition.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 1; i < lines.Length; i++) {
                        var split = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        Ranges.Add((ulong.Parse(split[0]), ulong.Parse(split[1]), ulong.Parse(split[2])));
                    }
                }
            }
        }
    }
}
