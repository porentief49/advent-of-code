using System.Collections.Generic;

namespace Puzzles {

    public partial class Year2023 {

        public class Day19 : DayBase {

            protected override string Title { get; } = "Day 19: Aplenty";

            public override void SetupAll() {
                AddInputFile(@"2023\19_Example.txt");
                AddInputFile(@"2023\19_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsText = ReadText(inputFile, true);

            private List<Workflow> _workflows;

            public override string Solve() {
                var split = InputAsText.Split("\n\n");
                _workflows = split[0].Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(w => new Workflow(w)).ToList();
                return (Part1 ? Solve1(split[1]) : Solve2()).ToString();
            }

            private ulong Solve1(string partList) {
                ulong totalRatings = 0;
                List<Dictionary<char, int>> parts = partList.Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(p => p.ReplaceAnyChar("{}", "").Split(',').ToDictionary(x => x[0], y => int.Parse(y.Substring(2)))).ToList();
                foreach (var part in parts) {
                    if (Verbose) Console.Write($"Processing part (x={part['x']},m={part['m']},a={part['a']},s={part['s']}): in");
                    string workflowName = "in";
                    do {
                        var nextWorkflow = _workflows.Single(w => w.Name == workflowName);
                        foreach (var instruction in nextWorkflow.Instructions) {
                            if (instruction.Condition) {
                                if ((instruction.Comparison == '<') ? part[instruction.Category] < instruction.Value : part[instruction.Category] > instruction.Value) {
                                    workflowName = instruction.Next;
                                    break;
                                }
                            } else {
                                workflowName = instruction.Next;
                                break;
                            }
                        }
                        if (Verbose) Console.Write($" -> {workflowName}");
                        if (workflowName == "A") totalRatings += (ulong)part.Values.Sum();
                    } while (workflowName != "A" && workflowName != "R");
                    if (Verbose) Console.WriteLine("");
                }
                return totalRatings;
            }

            private ulong Solve2() {
                ulong totalCount = 0; ;
                List<Range> ranges = new List<Range>() { new Range() { From = new Dictionary<char, int> { { 'x', 1 }, { 'm', 1 }, { 'a', 1 }, { 's', 1 } }, To = new Dictionary<char, int> { { 'x', 4000 }, { 'm', 4000 }, { 'a', 4000 }, { 's', 4000 } }, Next = "in" } };
                do {
                    List<Range> newRanges = new();
                    foreach (var range in ranges) {
                        var nextWorkflow = _workflows.Single(w => w.Name == range.Next);
                        foreach (var instruction in nextWorkflow.Instructions) {
                            if (instruction.Condition) {
                                if (instruction.Value > range.From[instruction.Category] && instruction.Value < range.To[instruction.Category]) {
                                    var splitOff = new Range(range);
                                    if (instruction.Comparison == '<') {
                                        splitOff.To[instruction.Category] = instruction.Value - 1;
                                        range.From[instruction.Category] = instruction.Value;
                                    } else {
                                        splitOff.From[instruction.Category] = instruction.Value + 1;
                                        range.To[instruction.Category] = instruction.Value;
                                    }
                                    if (instruction.Next == "A") totalCount += splitOff.To.Values.Zip(splitOff.From.Values, (x, y) => x - y + 1).Select(s=>(ulong)s).Aggregate((x, y) => x * y);
                                    else if (instruction.Next != "R") {
                                        splitOff.Next = instruction.Next;
                                        newRanges.Add(splitOff);
                                    }
                                }
                            } else if (instruction.Next == "A") totalCount += range.To.Values.Zip(range.From.Values, (x, y) => x - y + 1).Select(s => (ulong)s).Aggregate((x, y) => x * y);
                            else if (instruction.Next != "R") {
                                range.Next = instruction.Next;
                                newRanges.Add(range);
                            }
                        }
                    }
                    ranges = newRanges;
                } while (ranges.Any());
                return totalCount;
            }

            private class Range {
                public Dictionary<char, int> From;
                public Dictionary<char, int> To;
                public string Next;

                public Range() { }

                public Range(Range copyFrom) {
                    Next = copyFrom.Next;
                    From = new();
                    To = new();
                    foreach (var value in copyFrom.From.Keys) {
                        From.Add(value, copyFrom.From[value]);
                        To.Add(value, copyFrom.To[value]);
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

                public class Instruction {
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
}
