namespace Puzzles {

    public partial class Year2023 {

        public class Day12 : DayBase {

            protected override string Title { get; } = "Day 12: Hot Springs";

            public override void SetupAll() {
                AddInputFile(@"2023\12_Example.txt");
                AddInputFile(@"2023\12_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve() {
                if (Part2) return "";
                var data = InputAsLines.Select(i => new Row(i)).ToList();
                return data.Select(d => d.CountArrangements()).Sum().ToString();
            }

            public class Row {
                public List<char> Damaged;
                public List<char> Repaired;
                public string Contiguous;

                public Row(string input) {
                    var split = input.Split(' ');
                    Damaged = split[0].ToList();
                    Repaired = split[0].ToList();
                    Contiguous = split[1];
                }

                public int CountArrangements() {
                    var positions = Enumerable.Range(0, Damaged.Count).Where(i => Damaged[i] == '?').ToList();
                    int unknown = positions.Count;
                    int arrangements = 0;
                    //Console.WriteLine(string.Join("", Damaged) + "   " + Contiguous + "   " + unknown.ToString());
                    for (ulong i = 0; i < Math.Pow(2, unknown); i++) {
                        for (int ii = 0; ii < unknown; ii++) {
                            Repaired[positions[ii]] = ((i >> ii) & 1) == 1 ? '#' : '.';
                        }
                        if (DetermineContiguous() == Contiguous) arrangements++;
                    }
                    //Console.WriteLine("  " + arrangements.ToString());
                    return arrangements;
                }

                private string DetermineContiguous() {
                    List<int> conts = new();
                    int soFar = 0;
                    for (int i = 0; i < Repaired.Count; i++) {
                        if (Repaired[i] == '#') soFar++;
                        else {
                            if (soFar > 0) conts.Add(soFar);
                            soFar = 0;
                        }
                    }
                    if (soFar > 0) conts.Add(soFar);
                    //Console.WriteLine("    " + string.Join("", Repaired) + "   " + string.Join(",", conts));
                    return string.Join(",", conts);
                }
            }
        }
    }
}
