using System.Runtime.Intrinsics.Arm;

namespace Puzzles {

    public partial class Year2023 {

        public class Day19 : DayBase {

            protected override string Title { get; } = "Day 19: Aplenty";

            public override void SetupAll() {
                AddInputFile(@"2023\19_Example.txt");
                AddInputFile(@"2023\19_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsText = ReadText(inputFile, true);

            public override string Solve() {
                if (Part1) return "";
                var split = InputAsText.Split("\n\n");
                var workflows = split[0].Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(w => new Workflow(w)).ToList();
                //var parts = split[1].Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(p => new Part(p)).ToList();
                ulong totalCount = 0; ;
                List<PartRange> ranges = new List<PartRange>() { new PartRange() { xFrom = 1, xTo = 4000, mFrom = 1, mTo = 4000, aFrom = 1, aTo = 4000, sFrom = 1, sTo = 4000, Next = "in" } };
                do {
                    List<PartRange> newRranges = new();
                    foreach (var range in ranges) {
                        var nextWorkflow = workflows.Single(w => w.Name == range.Next);
                        foreach (var instruction in nextWorkflow.Instructions) {
                            if (instruction.Condition) {
                                int partFrom = instruction.Category switch { 'x' => range.xFrom, 'm' => range.mFrom, 'a' => range.aFrom, 's' => range.sFrom, _ => throw new Exception("Category does not exist") };
                                int partTo = instruction.Category switch { 'x' => range.xTo, 'm' => range.mTo, 'a' => range.aTo, 's' => range.sTo, _ => throw new Exception("Category does not exist") };
                                if (instruction.Value > partFrom && instruction.Value < partTo) { // split off a part of the range
                                    var splitOff = new PartRange(range);
                                    switch (instruction.Category) {
                                        case 'x':
                                            if (instruction.Comparison == '<') {
                                                splitOff.xTo = instruction.Value - 1;
                                                range.xFrom = instruction.Value;
                                            } else {
                                                splitOff.xFrom = instruction.Value + 1;
                                                range.xTo = instruction.Value;
                                            }
                                            break;
                                        case 'm':
                                            if (instruction.Comparison == '<') {
                                                splitOff.mTo = instruction.Value - 1;
                                                range.mFrom = instruction.Value;
                                            } else {
                                                splitOff.mFrom = instruction.Value + 1;
                                                range.mTo = instruction.Value;
                                            }
                                            break;
                                        case 'a':
                                            if (instruction.Comparison == '<') {
                                                splitOff.aTo = instruction.Value - 1;
                                                range.aFrom = instruction.Value;
                                            } else {
                                                splitOff.aFrom = instruction.Value + 1;
                                                range.aTo = instruction.Value;
                                            }
                                            break;
                                        case 's':
                                            if (instruction.Comparison == '<') {
                                                splitOff.sTo = instruction.Value - 1;
                                                range.sFrom = instruction.Value;
                                            } else {
                                                splitOff.sFrom = instruction.Value + 1;
                                                range.sTo = instruction.Value;
                                            }
                                            break;
                                        default:
                                            throw new Exception("Category does not exist");
                                    }
                                    if (instruction.Next == "A") { // calc sum
                                        totalCount += (ulong)(splitOff.xTo - splitOff.xFrom + 1) * (ulong)(splitOff.mTo - splitOff.mFrom + 1) * (ulong)(splitOff.aTo - splitOff.aFrom + 1) * (ulong)(splitOff.sTo - splitOff.sFrom + 1);
                                    } else if (instruction.Next != "R") {
                                        splitOff.Next = instruction.Next;
                                        newRranges.Add(splitOff);
                                    }
                                }
                            } else {
                                if (instruction.Next == "A") { // calc sum
                                    totalCount += (ulong)(range.xTo - range.xFrom + 1) * (ulong)(range.mTo - range.mFrom + 1) * (ulong)(range.aTo - range.aFrom + 1) * (ulong)(range.sTo - range.sFrom + 1);
                                } else if (instruction.Next != "R") {
                                    range.Next = instruction.Next;
                                    newRranges.Add(range);
                                }
                            }
                        }
                    }
                    ranges = newRranges;
                } while (ranges.Any());
                return totalCount.ToString();
            }

            public string Solve1() {
                if (Part2) return "";
                var split = InputAsText.Split("\n\n");
                var workflows = split[0].Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(w => new Workflow(w)).ToList();
                var parts = split[1].Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(p => new Part(p)).ToList();
                ulong totalRatings = 0;
                foreach (var part in parts) {
                    Console.Write($"Processing part (x={part.x},m={part.m},a={part.a},s={part.s}): in");
                    bool partDone = false;
                    string workflowName = "in";
                    do {
                        //Console.WriteLine($"Processing part (x={part.x},m={part.m},a={part.a},s={part.s})");
                        var nextWorkflow = workflows.Single(w => w.Name == workflowName);
                        foreach (var instruction in nextWorkflow.Instructions) {
                            if (instruction.Condition) {
                                if (CheckCategory(part, instruction)) {
                                    workflowName = instruction.Next;
                                    break;
                                }
                            } else {
                                workflowName = instruction.Next;
                                break;
                            }
                        }
                        Console.Write($" -> {workflowName}");
                        if (workflowName == "A") {
                            totalRatings += (ulong)(part.x + part.m + part.a + part.s);
                            partDone = true;
                        }
                        if (workflowName == "R") {
                            partDone = true;
                        }
                        // check if part is done
                    } while (!partDone);
                    Console.WriteLine("");
                }
                return totalRatings.ToString();
            }

            private bool CheckCategory(Part part, Instruction instruction) {
                int partValue = instruction.Category switch { 'x' => part.x, 'm' => part.m, 'a' => part.a, 's' => part.s, _ => throw new Exception("Category does not exist") };
                if (instruction.Comparison == '<') return partValue < instruction.Value;
                return partValue > instruction.Value;
            }

            private class PartRange {
                public int xFrom;
                public int xTo;
                public int mFrom;
                public int mTo;
                public int aFrom;
                public int aTo;
                public int sFrom;
                public int sTo;
                public string Next;

                public PartRange() { }

                public PartRange(PartRange copyFrom) {
                    xFrom = copyFrom.xFrom;
                    xTo = copyFrom.xTo;
                    mFrom = copyFrom.mFrom;
                    mTo = copyFrom.mTo;
                    aFrom = copyFrom.aFrom;
                    aTo = copyFrom.aTo;
                    sFrom = copyFrom.sFrom;
                    sTo = copyFrom.sTo;
                    Next = copyFrom.Next;
                }
            }

            private class Part {
                public int x;
                public int m;
                public int a;
                public int s;

                public Part(string line) {
                    foreach (var prop in line.Replace("{", "").Replace("}", "").Split(',')) {
                        var split = prop.Split('=');
                        switch (split[0]) {
                            case "x":
                                x = int.Parse(split[1]);
                                break;
                            case "m":
                                m = int.Parse(split[1]);
                                break;
                            case "a":
                                a = int.Parse(split[1]);
                                break;
                            case "s":
                                s = int.Parse(split[1]);
                                break;
                            default:
                                throw new Exception("Category does not exist");
                        }
                    }
                }
            }

            private class Workflow {
                public string Name;
                public List<Instruction> Instructions;


                public Workflow(string line) {
                    var split1 = line.Replace("}", "").Split('{');
                    Name = split1[0];
                    Instructions = split1[1].Split(',').Select(i => new Instruction(i)).ToList();
                }
            }

            private class Instruction {
                public char Category;
                public char Comparison;
                public int Value;
                public string Next;
                public bool Condition;

                public Instruction(string input) {
                    if (input.Contains(':')) {
                        Category = input[0];
                        Comparison = input[1];
                        var split = input.Split(":");
                        Value = int.Parse(split[0].Substring(2));
                        Next = split[1];
                        Condition = true;
                    } else {
                        Next = input;
                        Condition = false;
                    }
                }
            }
        }
    }
}
