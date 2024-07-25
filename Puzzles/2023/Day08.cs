using static Puzzles.Year2022;

namespace Puzzles {

    public partial class Year2023 {

        public class Day08 : DayBase {

            protected override string Title { get; } = "Day 8: ???";

            public override void SetupAll() {
                AddInputFile(@"2023\08_Example1.txt");
                AddInputFile(@"2023\08_Example2.txt");
                AddInputFile(@"2023\08_rAiner.txt");
                AddInputFile(@"2023\08_rAinerSEGCC.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve() {
                if (InputFile.Contains($"{(Part1 ? "2" : "1")}.")) return string.Empty; // different input files this time
                var instructions = InputAsLines[0];
                var instructionLength = instructions.Length;
                var nodes = InputAsLines.Skip(1).Select(i => new Node(i)).ToList();
                if (Part1) return CalcSteps(nodes[0], 1)[0].ToString();
                var startNodes = nodes.FindAll(n => n.Name.Last() == 'A').ToList();
                var steps = startNodes.Select(n => CalcSteps(n, 2));
                if (steps.All(s => s[0] != s[1] - s[0])) return "not all last elements point to the original starting points - modulo / lcm algorithm does not work";
                return steps.Select(s => (ulong)s[0]).Aggregate((x, y) => LeastCommonMultiplier(x, y)).ToString();

                List<int> CalcSteps(Node node, int howManyLoops) {
                    int steps = 0;
                    int count = 0;
                    var result = new List<int>();
                    do {
                        var next = instructions[steps++ % instructionLength] == 'L' ? node.Left : node.Right;
                        node = nodes.Single(n => n.Name == next);
                        if ((Part1 && node.Name == "ZZZ") || (!Part1 && node.Name.EndsWith('Z'))) {
                            result.Add(steps);
                            count++;
                        }
                    } while (count < howManyLoops);
                    return result;
                }
            }
            
            private ulong GreatestCommonFactor(ulong a, ulong b) { //https://stackoverflow.com/questions/13569810/least-common-multiple
                while (b != 0) (b, a) = (a % b, b);
                return a;
            }
            
            private ulong LeastCommonMultiplier(ulong a, ulong b) => (a / GreatestCommonFactor(a, b)) * b;//https://stackoverflow.com/questions/13569810/least-common-multiple

            private class Node {
                public readonly string Name;
                public readonly string Left;
                public readonly string Right;

                public Node(string instruction) {
                    var split = instruction.Replace("=", string.Empty).Replace(",", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty).Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    (Name, Left, Right) = (split[0], split[1], split[2]);
                }
            }
        }
    }
}
