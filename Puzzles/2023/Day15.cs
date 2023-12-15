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
                //if (Part2) return "";
                var instructions = InputAsLines[0].Split(',').ToList();
                //var hashes = new List<int>();
                var sum = 0;
                foreach (var item in instructions) {
                    int current = CalcHash(item);
                    sum += current;
                    //hashes.Add(current);
                    //Console.WriteLine($"{current}");
                }
                if (Part1) return sum.ToString();

                //part 2
                Box[] boxes = Enumerable.Range(0,256).Select(i=>new Box()).ToArray();
                for (int i = 0; i < instructions.Count; i++) {
                    var label = instructions[i].Replace('=', '-').Split('-')[0];
                    var hash = CalcHash(label);
                    if (instructions[i].Contains('-')) { // '-' ... remove
                        //var label = instructions[i].Replace("-","");
                        if (boxes[hash].Lenses.RemoveAll(l => l.label == label) > 1) throw new Exception($"more than one with label {label}");
                    } else { // '=' ... insert
                        var split = instructions[i].Split('=');
                        //var label = split[0];
                        var focal = int.Parse(split[1]);
                        //var lens = instructions[i].Replace('=', ' ');
                        //foreach (var lens in boxes[hashes[i]].Lenses) {

                        //}
                        var index = boxes[hash].Lenses.FindIndex(l => l.label == label);
                        if (index < 0) boxes[hash].Lenses.Add((label, focal));
                        else boxes[hash].Lenses[index] = (label, focal);
                    }
                    //Console.WriteLine($"After '{instructions[i]}':");
                    //for (int ii = 0; ii < 256; ii++) {
                    //    if(boxes[ii].Lenses.Count>0) Console.WriteLine($"Box {ii}: {string.Join(" | ", boxes[ii].Lenses.Select(l=>l.label + " " + l.focal.ToString()) )}");
                    //}
                    //Console.WriteLine("");
                }

                ulong total = 0;
                for (int i = 0; i < 256; i++) {
                    for (int ii = 0; ii < boxes[i].Lenses.Count; ii++) {
                        total += (ulong)((i + 1) * (ii + 1) * boxes[i].Lenses[ii].focal);
                    }
                }

                return total.ToString();
            }

            private static int CalcHash(string? item) {
                int current = 0;
                foreach (var ch in item) {
                    current += ch;
                    current *= 17;
                    current %= 256;
                }

                return current;
            }

            private class Box {
                public List<(string label, int focal)> Lenses = new();
            }
        }
    }
}
