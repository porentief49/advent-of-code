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
                var modules = InputAsLines.Select(i => new Module(i)).ToList();
                for (int i = 0; i < modules.Count; i++) modules[i].UpdateRefs(modules); // will not cover the unknown ones added within this loop, but that's ok
                modules.Insert(0, new Module("button -> broadcaster"));
                Dictionary<string, ulong> part2inputs = new();
                if (Part2) {
                    // ok I hate this. Fiddling some patterns out of the input data is lame!
                    // Turns out four gates feed into the Conjunction before rx. Since all of
                    // these must be high at the same time, we need to look for repetition
                    // patterns. Turns out, that's the case every ~3-4000 pushes. And of course
                    // they are primes, so we loop until each has fired once and then multiply
                    // the counts. And I had to look online to get a tip into the right direction ...
                    Module theOneFeedingIntoRx = modules.Single(m => m.OutputNames.Contains("rx"));
                    part2inputs = modules.FindAll(m => m.Outputs.Contains(theOneFeedingIntoRx)).ToDictionary(x => x.Name, x => 0UL);
                }
                Queue<(Module from, Module to, bool pulse)> pulses = new();
                ulong lows = 0;
                ulong highs = 0;
                ulong count = 0;
                bool done = false;
                do {
                    count++;
                    pulses.Enqueue((modules.Single(m => m.Name == "button"), modules.Single(m => m.Name == "broadcaster"), false)); // push button
                    lows++;
                    do {
                        (Module from, Module to, bool inPulse) = pulses.Dequeue();
                        bool? outPulse = to.ProcessPulse(inPulse, from);
                        if (outPulse != null) {
                            bool pulse = (bool)outPulse;
                            foreach (var output in to.Outputs) {
                                pulses.Enqueue((to, output, pulse));
                                if (pulse) highs++;
                                else lows++;
                                if (Verbose) Console.WriteLine($"{to.Name} -{(pulse ? "high" : "low")}-> {output.Name}");
                                if (Part2 && !pulse && part2inputs.Keys.Contains(output.Name)) {
                                    if (Verbose) Console.WriteLine($"Gate {output.Name} after {count} pushes");
                                    part2inputs[output.Name] = count;
                                }
                            }
                        }
                    } while (pulses.Any());
                    if (Verbose) Console.WriteLine();
                    if (Part1) done = count == 1000;
                    else done = !part2inputs.Values.Any(x => x == 0);
                } while (!done);
                if (Verbose) Console.WriteLine($"========> {lows} lows, {highs} highs");
                return (Part1 ? (lows * highs) : part2inputs.Values.Aggregate((x, y) => x * y)).ToString();
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
