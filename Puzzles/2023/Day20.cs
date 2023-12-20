namespace Puzzles {

    public partial class Year2023 {

        public class Day20 : DayBase {

            protected override string Title { get; } = "Day 20: Pulse Propagation";

            public override void SetupAll() {
                //AddInputFile(@"2023\20_Example1.txt");
                //AddInputFile(@"2023\20_Example2.txt");
                AddInputFile(@"2023\20_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);


            public override string Solve() {
                //if (Part2) return "";
                var modules = InputAsLines.Select(i => new Module(i)).ToList();
                for (int i = 0; i < modules.Count; i++) modules[i].UpdateRefs(modules); // will not cover the unknown ones added within this loop, but that's ok
                modules.Insert(0, new Module("button -> broadcaster"));
                Dictionary<string, ulong> xmInputs = new();
                if (Part2) {
                    //Module rx = modules.Single(m => m.Name == "rx");
                    Module xm = modules.Single(m => m.OutputNames.Contains("rx"));
                    xmInputs = modules.FindAll(m => m.Outputs.Contains(xm)).ToDictionary(x => x.Name, x => 0UL);
                    //foreach (var mod in modules.FindAll(m => m.Outputs.Contains(xm))) {
                    //    var thisModule = mod;
                    //    do {
                    //        thisModule
                    //    } while (thisModule.Name!="button");
                    //    Console.WriteLine(mod.Name);
                    //}
                    //return "";
                }
                Queue<(Module from, Module to, bool pulse)> pulses = new();
                ulong lows = 0;
                ulong highs = 0;
                ulong count = 0;
                bool done = false;
                do {
                    count++;
                    //if (count % 1000000 == 0) Console.WriteLine(count);
                    pulses.Enqueue((modules.Single(m => m.Name == "button"), modules.Single(m => m.Name == "broadcaster"), false)); // push button
                    lows++;
                    do {
                        (Module from, Module to, bool inPulse) = pulses.Dequeue();
                        bool? outPulse = to.ProcessPulse(inPulse, from);
                        if (outPulse != null) {
                            foreach (var output in to.Outputs) {
                                pulses.Enqueue((to, output, (bool)outPulse));
                                if ((bool)outPulse) highs++;
                                else lows++;
                                if (Verbose) Console.WriteLine($"{to.Name} -{((bool)outPulse ? "high" : "low")}-> {output.Name}");
                                if (Part2 && (bool)outPulse == false && output.Name == "rx") done = true;
                                if (Part2 && (bool)outPulse == false && xmInputs.Keys.Contains(output.Name)) {
                                    Console.WriteLine($"Gate {output.Name} after {count} pushes");
                                    xmInputs[output.Name] = count;
                                }
                            }
                        }
                    } while (pulses.Any());
                    if (Verbose) Console.WriteLine();
                    if (Part1) done = count == 1000;
                    else done = !xmInputs.Values.Any(x => x == 0);
                } while (!done);
                if (Verbose) Console.WriteLine($"========> {lows} lows, {highs} highs");
                //if (Part2) Console.WriteLine(xmInputs.Values.Aggregate((x, y) => x * y));
                return (Part1 ? (lows * highs) : xmInputs.Values.Aggregate((x, y) => x * y)).ToString();
            }

            private enum ModType { FlipFlop, Conjunction, Other }

            private class Module {

                public string Name;
                public ModType Type;
                public bool State = false;
                public List<string> OutputNames = new();
                public List<Module> Outputs = new();
                public Dictionary<string, bool> InputStates;

                public Module(string definition) {
                    var split = definition.Split("->", StringSplitOptions.TrimEntries);
                    Name = split[0].ReplaceAnyChar("&%", string.Empty);
                    Type = split[0][0] switch { '%' => ModType.FlipFlop, '&' => ModType.Conjunction, _ => ModType.Other };
                    if (split.Length > 1) OutputNames = split[1].Split(',', StringSplitOptions.TrimEntries).ToList();
                    //else {
                    //    OutputNames = new();
                    //    Outputs = new();
                    //}
                }

                public void UpdateRefs(List<Module> modules) {
                    foreach (var unknown in OutputNames.Where(o => !modules.Any(m => m.Name == o))) modules.Add(new Module(unknown)); // add the ones we only find in the outputs
                    Outputs = OutputNames.Select(o => modules.Single(m => m.Name == o)).ToList();
                    InputStates = modules.Where(m => m.OutputNames.Contains(Name)).ToDictionary(m => m.Name, m => false);
                }

                public bool? ProcessPulse(bool pulse, Module from) {
                    switch (Type) {
                        case ModType.FlipFlop:
                            if (pulse) return null;
                            State = !State;
                            return State;
                        case ModType.Conjunction:
                            InputStates[from.Name] = pulse;
                            return !InputStates.Values.All(x => x);
                        case ModType.Other:
                            return State;
                        default:
                            throw new Exception("should not happen");
                    }
                }
            }
        }
    }
}
