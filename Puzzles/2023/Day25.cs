namespace Puzzles {

    public partial class Year2023 {

        public class Day25 : DayBase {

            protected override string Title { get; } = "Day 25: Snowverload";

            public override void SetupAll() {
                AddInputFile(@"2023\25_Example.txt");
                AddInputFile(@"2023\25_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve() {
                if (Part2) return "";

                // add documented connections
                Dictionary<string, HashSet<string>> modules = new();
                foreach (string line in InputAsLines.Select(i=>i.Replace(":", ""))) {
                    string[] names = line.Split(' ');
                    modules.Add(names[0], names.Skip(1).ToHashSet());
                }

                // add all reverse connections
                foreach (var key in modules.Keys.ToList()) {
                    foreach (var connection in modules[key]) {
                        if (!modules.ContainsKey(connection)) modules[connection] = new();
                        modules[connection].Add(key);
                    }
                }

                List<int> counts = new();
                List<string> wireGroup = modules.Keys.ToList();
                while (true) {
                    counts.Clear();
                    HashSet<string> wireGroupSet = wireGroup.ToHashSet();
                    foreach (string wire in wireGroup) {
                        int count = modules[wire].Except(wireGroupSet).Count();
                        counts.Add(count);
                    }
                    if (counts.Sum() == 3) break;
                    string removeWire = wireGroup[counts.FindIndex(x => x == counts.Max())];
                    wireGroup.Remove(removeWire);
                }
                return (wireGroup.Count * modules.Keys.Except(wireGroup).Count()).ToString();
            }
        }
    }
}
