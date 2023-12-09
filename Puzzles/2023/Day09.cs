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

            public override string Solve(bool part1) {
                if (!part1) return "";
                var history = InputAsLines.Select(i => new History(i)).ToList();
                foreach (var row in history) row.Predict();
                foreach (var row in history) row.Print("After");
                return history.Select(h => h.Data.Last().Last()).Sum().ToString();
            }

            private class History {

                public List<List<long>> Data = new();

                public History(string input) {
                    var diff = input.Split(' ').Select(i => long.Parse(i)).ToList();
                    Data.Add(diff);
                    do {
                        diff = diff.Zip(diff.Skip(1), (xx, y) => y - xx).ToList();
                        Data.Add(diff);
                    } while (diff.Max() - diff.Min() > 0);
                    Data.Reverse();
                }

                public void Predict() {
                    long last = 0;
                    foreach (var row in Data) {
                        last += row.Last();
                        row.Add(last);
                    }
                }

                public void Print(string label) {
                    Console.WriteLine(label);
                    foreach (var row in Data) Console.WriteLine(string.Join(" ", row));
                    Console.WriteLine("");
                }
            }
        }
    }
}
