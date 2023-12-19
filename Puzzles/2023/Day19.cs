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
                //List<PartRange> ranges = new List<PartRange>() { new PartRange() { xFrom = 1, xTo = 4000, mFrom = 1, mTo = 4000, aFrom = 1, aTo = 4000, sFrom = 1, sTo = 4000, Next = "in" } };
                //List<Dictionary<char, (int from, int to)>> ranges2 = new() { new Dictionary<char, (int, int)> { { 'x', (1, 4000) }, { 'm', (1, 4000) }, { 'a', (1, 4000) }, { 's', (1, 4000) } } };
                List<PartRange> ranges2 = new List<PartRange>() { new PartRange() { Ranges = new Dictionary<char, (int from, int to)> { { 'x', (1, 4000) }, { 'm', (1, 4000) }, { 'a', (1, 4000) }, { 's', (1, 4000) } }, Next = "in" } };
                do {
                    List<PartRange> newRranges = new();
                    foreach (var range in ranges2) {
                        var nextWorkflow = _workflows.Single(w => w.Name == range.Next);
                        foreach (var instruction in nextWorkflow.Instructions) {
                            if (instruction.Condition) {
                                //int partFrom = instruction.Category switch { 'x' => range.Ranges['x'].From, 'm' => range.mFrom, 'a' => range.aFrom, 's' => range.sFrom, _ => throw new Exception("Category does not exist") };
                                int partFrom = range.Ranges[instruction.Category].From;// instruction.Category switch { 'x' => range.Ranges['x'].From, 'm' => range.mFrom, 'a' => range.aFrom, 's' => range.sFrom, _ => throw new Exception("Category does not exist") };
                                //int partTo = instruction.Category switch { 'x' => range.xTo, 'm' => range.mTo, 'a' => range.aTo, 's' => range.sTo, _ => throw new Exception("Category does not exist") };
                                int partTo = range.Ranges[instruction.Category].To;// switch { 'x' => range.xTo, 'm' => range.mTo, 'a' => range.aTo, 's' => range.sTo, _ => throw new Exception("Category does not exist") }; ; ; ;  ; ; ; ;// ;// ;
                                if (instruction.Value > partFrom && instruction.Value < partTo) { // split off a part of the range
                                    var splitOff = new PartRange(range);
                                    if (instruction.Comparison == '<') {
                                        //switch (instruction.Category) {
                                        //    case 'x':
                                        //        splitOff.xTo = instruction.Value - 1;
                                        //        range.xFrom = instruction.Value;
                                        //        break;
                                        //    case 'm':
                                        //        splitOff.mTo = instruction.Value - 1;
                                        //        range.mFrom = instruction.Value;
                                        //        break;
                                        //    case 'a':
                                        //        splitOff.aTo = instruction.Value - 1;
                                        //        range.aFrom = instruction.Value;
                                        //        break;
                                        //    case 's':
                                        //        splitOff.sTo = instruction.Value - 1;
                                        //        range.sFrom = instruction.Value;
                                        //        break;
                                        //    default:
                                        //        throw new Exception("Category does not exist");
                                        //}

                                        splitOff.Ranges[instruction.Category] = (splitOff.Ranges[instruction.Category].From, instruction.Value - 1);
                                        range.Ranges[instruction.Category] = (instruction.Value, range.Ranges[instruction.Category].To);


                                    } else {
                                        //switch (instruction.Category) {
                                        //    case 'x':
                                        //        splitOff.xFrom = instruction.Value + 1;
                                        //        range.xTo = instruction.Value;
                                        //        break;
                                        //    case 'm':
                                        //        splitOff.mFrom = instruction.Value + 1;
                                        //        range.mTo = instruction.Value;
                                        //        break;
                                        //    case 'a':
                                        //        splitOff.aFrom = instruction.Value + 1;
                                        //        range.aTo = instruction.Value;
                                        //        break;
                                        //    case 's':
                                        //        splitOff.sFrom = instruction.Value + 1;
                                        //        range.sTo = instruction.Value;
                                        //        break;
                                        //    default:
                                        //        throw new Exception("Category does not exist");
                                        //}

                                        splitOff.Ranges[instruction.Category] = (instruction.Value + 1, splitOff.Ranges[instruction.Category].To);
                                        range.Ranges[instruction.Category] = (range.Ranges[instruction.Category].From, instruction.Value);

                                    }
                                    if (instruction.Next == "A") totalCount += (ulong)(splitOff.Ranges['x'].To - splitOff.Ranges['x'].From + 1) * (ulong)(splitOff.Ranges['m'].To - splitOff.Ranges['m'].From + 1) * (ulong)(splitOff.Ranges['a'].To - splitOff.Ranges['a'].From + 1) * (ulong)(splitOff.Ranges['s'].To - splitOff.Ranges['s'].From + 1);
                                    else if (instruction.Next != "R") {
                                        splitOff.Next = instruction.Next;
                                        newRranges.Add(splitOff);
                                    }
                                }
                                //} else if (instruction.Next == "A") totalCount += (ulong)(range.xTo - range.xFrom + 1) * (ulong)(range.mTo - range.mFrom + 1) * (ulong)(range.aTo - range.aFrom + 1) * (ulong)(range.sTo - range.sFrom + 1);
                            } else if (instruction.Next == "A") totalCount += (ulong)(range.Ranges['x'].To - range.Ranges['x'].From + 1) * (ulong)(range.Ranges['m'].To - range.Ranges['m'].From + 1) * (ulong)(range.Ranges['a'].To - range.Ranges['a'].From + 1) * (ulong)(range.Ranges['s'].To - range.Ranges['s'].From + 1);
                            else if (instruction.Next != "R") {
                                range.Next = instruction.Next;
                                newRranges.Add(range);
                            }
                        }
                    }
                    ranges2 = newRranges;
                } while (ranges2.Any());
                return totalCount;
            }

            private class PartRange {
                //public int xFrom;
                //public int xTo;
                //public int mFrom;
                //public int mTo;
                //public int aFrom;
                //public int aTo;
                //public int sFrom;
                //public int sTo;
                public Dictionary<char, (int From, int To)> Ranges;
                public string Next;


                public PartRange() { }

                public PartRange(PartRange copyFrom) {
                    //xFrom = copyFrom.xFrom;
                    //xTo = copyFrom.xTo;
                    //mFrom = copyFrom.mFrom;
                    //mTo = copyFrom.mTo;
                    //aFrom = copyFrom.aFrom;
                    //aTo = copyFrom.aTo;
                    //sFrom = copyFrom.sFrom;
                    //sTo = copyFrom.sTo;
                    Next = copyFrom.Next;
                    Ranges = new();
                    foreach (var value in copyFrom.Ranges.Keys) Ranges.Add(value, copyFrom.Ranges[value]);
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
