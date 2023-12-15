using System.Linq;
using System.Reflection.Emit;

namespace Puzzles {

    public partial class Year2023 {

        public class Day15 : DayBase {

            protected override string Title { get; } = "Day 15: Lens Library";

            public override void SetupAll() {
                AddInputFile(@"2023\15_Example.txt");
                AddInputFile(@"2023\15_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve() {
                var instructions = InputAsLines[0].Split(',').ToList();
                if (Part1) return instructions.Select(i => CalcHash(i)).Sum().ToString();

                //part 2
                Box[] boxes = Enumerable.Range(0, 256).Select(i => new Box()).ToArray();
                for (int i = 0; i < instructions.Count; i++) {
                    var instruction = instructions[i].Replace('=', '-').Split('-', StringSplitOptions.RemoveEmptyEntries);
                    boxes[CalcHash(instruction[0])].Process(instruction);
                    if (Verbose) Console.WriteLine($"After '{instructions[i]}':\r\n{string.Join("\r\n", boxes.Select((b, i) => new { box = b, idx = i }).Where(x => x.box.Lenses.Count() > 0).Select(x => $"Box {x.idx}: {string.Join(" ", x.box.Lenses.Select(l => $"[{l.label} {l.focal}]"))}"))}\r\n");
                }
                int total = 0;
                for (int b = 0; b < 256; b++) for (int l = 0; l < boxes[b].Lenses.Count; l++) total += ((b + 1) * (l + 1) * int.Parse(boxes[b].Lenses[l].focal));
                return total.ToString();
            }

            private static int CalcHash(string value) => value.Select(v => (int)v).Aggregate(0, (x, y) => ((x + y) * 17) % 256);

            private class Box {
                public List<(string label, string focal)> Lenses = new();

                public void Process(string[] instruction) {
                    if (instruction.Length == 1) Lenses.RemoveAll(l => l.label == instruction[0]); // '-' ... remove
                    else { // '=' ... insert
                        var index = Lenses.FindIndex(l => l.label == instruction[0]);
                        if (index < 0) Lenses.Add((instruction[0], instruction[1]));
                        else Lenses[index] = (instruction[0], instruction[1]);
                    }
                }
            }
        }
    }
}
