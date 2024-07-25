using System.Xml.Schema;
using System.Xml.Serialization;

namespace Puzzles {

    public partial class Year2023 {

        public class Day09 : DayBase {

            protected override string Title { get; } = "Day 9: Mirage Maintenance";

            public override void SetupAll() {
                AddInputFile(@"2023\09_Example.txt");
                AddInputFile(@"2023\09_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve() => InputAsLines.Select(i => new History(i)).Select(h => h.Predict(Part1)).Sum().ToString();

            private class History {

                private List<List<long>> _data = new();

                public History(string input) {
                    _data.Add(input.Split(' ').Select(i => long.Parse(i)).ToList());
                    do _data.Insert(0, _data[0].Zip(_data[0].Skip(1), (x, y) => y - x).ToList());
                    while (_data[0].Any(i => i != 0));
                }

                public long Predict(bool right) {
                    long last = 0;
                    foreach (var row in _data) last = right ? row.Last() + last : row.First() - last;
                    return last;
                }

                public void Print(string label) => Console.WriteLine($"{label}\r\n{string.Join("\r\n", _data.Select(d => string.Join(" ", d)))}\r\n");
            }
        }
    }
}
