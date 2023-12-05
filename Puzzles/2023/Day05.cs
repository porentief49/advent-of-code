using System.Text;

namespace Puzzles {

    public partial class Year2023 {

        public class Day05 : DayBase {

            protected override string Title { get; } = "Day 5: If You Give A Seed A Fertilizer";

            public override void SetupAll() {
                AddInputFile(@"2023\05_Example.txt");
                AddInputFile(@"2023\05_rAiner.txt");
            }

            public override void Init(string InputFile) => InputData = ReadLines(InputFile, false);

            public override string Solve(bool Part1) {
                var mapDefinitions = string.Join('\n', InputData).Split("\n\n");
                var maps = mapDefinitions.Skip(1).Select(m => new Map(m));
                var seeds = mapDefinitions.First().Split(' ').Skip(1).Select(m => ulong.Parse(m)).ToArray();
                if (Part1) {
                    List<ulong> locations = new();
                    foreach (var seed in seeds) locations.Add(CalcLocation(maps, seed));
                    return locations.Min().ToString();
                }

                //Part 2
                List<(ulong start, ulong length)> seedRanges = new();
                for (int i = 0; i < seeds.Length / 2; i++) seedRanges.Add((seeds[i * 2], seeds[i * 2 + 1]));
                //Console.WriteLine($"starting with seedranges: {string.Join(" | ", seedRanges.Select((s, i) => $"{i}: {s.Start}-{s.Length}"))}");
                foreach (var map in maps) {
                    List<(ulong start, ulong length)> newSeedRanges = new();
                    foreach (var seedRange in seedRanges) {
                        ulong seed = seedRange.start;
                        ulong length = seedRange.length;
                        do {
                            ulong howMany = 0;
                            foreach (var range in map.Ranges) {
                                if (seed >= range.sourceStart && seed < range.sourceStart + range.length) { // found a range
                                    howMany = Math.Min(length, range.length - (seed - range.sourceStart));
                                    newSeedRanges.Add((seed + range.destinationStart - range.sourceStart, howMany));
                                    break;
                                }
                            }
                            if (howMany == 0) { // found no range
                                howMany = length;
                                newSeedRanges.Add((seed, length));
                            }
                            seed += howMany;
                            length -= howMany;
                        } while (seed < seedRange.start + seedRange.length);
                    }
                    seedRanges = newSeedRanges;
                    //Console.WriteLine($"finished round with {NewSeedRanges.Count()} seedranges: {string.Join(" | ", seedRanges.Select((s, i) => $"{i}: {s.Start}-{s.Length}"))}");
                }
                return seedRanges.Min(s => s.start).ToString();
            }

            private static ulong CalcLocation(IEnumerable<Map> maps, ulong seed) {
                ulong location = seed;
                foreach (var map in maps) {
                    foreach (var range in map.Ranges) {
                        if (location >= range.sourceStart && location < range.sourceStart + range.length) {
                            location = location - range.sourceStart + range.destinationStart;
                            break;
                        }
                    }
                }
                return location;
            }

            private class Map {

                public List<(ulong destinationStart, ulong sourceStart, ulong length)> Ranges = new();

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
