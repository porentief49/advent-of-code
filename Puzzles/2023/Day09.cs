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

            public override string Solve(bool part1) => InputAsLines.Select(i => new History(i)).Select(h => h.Predict(part1)).Sum().ToString();

            private class History {

                public List<List<long>> Data = new();

                public History(string input) {
                    Data.Add(input.Split(' ').Select(i => long.Parse(i)).ToList());
                    do Data.Insert(0, Data[0].Zip(Data[0].Skip(1), (xx, y) => y - xx).ToList());
                    while (Data[0].Max() != 0 || Data[0].Min() != 0);
                }

                public long Predict(bool right) {
                    long last = 0;
                    foreach (var row in Data) last = right ? row.Last() + last : row.First() - last;
                    return last;
                }

                public void Print(string label) => Console.WriteLine($"{label}\r\n{string.Join("\r\n", Data.Select(d => string.Join(" ", d)))}\r\n");
            }
        }
    }
}
