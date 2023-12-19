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
                        var wornextWorkflow = workflows.Single(w => w.Name == workflowName);
                        foreach (var instruction in wornextWorkflow.Instructions) {
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
