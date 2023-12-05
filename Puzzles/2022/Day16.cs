using System;
using System.Collections;
using System.Net.Sockets;
using System.Text;

namespace Puzzles {
    public partial class Year2022 {
        public class Day16 : DayBase {
            private Dictionary<string, Valve> _valves;
            //private long _maxReleased = 0;

            protected override string Title { get; } = "Day 16: Proboscidea Volcanium";

            public override void SetupAll() {
                AddInputFile(@"2022\16_Example.txt");
                //AddInputFile(@"2022\16_rAiner.txt");
                //AddInputFile(@"2022\16_SEGCC.txt");
            }

            public override void Init(string InputFile) {
                _valves = new();
                InputData = ReadLines(InputFile, true);
                foreach (var line in InputData) {
                    string[] split = line.Replace(";", string.Empty).Replace('=', ' ').Replace(", ", "|").Split(' ');
                    _valves.Add(split[1], new Valve(split[1], long.Parse(split[5]), split[10].Split('|')));
                    //Id = split[1];
                    //FlowRate = long.Parse(split[5]);
                    //TunnelsTo = split[10].Split('|');

                }
            }

            public override string Solve(bool Part1) {
                if (!Part1) return "";

                //Queue<State> currentStates = new();
                List<State> currentStates = new();
                currentStates.Add(new State("AA", "", 0, 0));


                long maxReleasedSoFar;


                for (int i = 0; i < 15; i++) {
                    List<State> nextStates = new();
                    maxReleasedSoFar = long.MinValue;
                    foreach (var current in currentStates)

                    //while (currentStates.Count > 0)
                    {
                        //State current = currentStates.Dequeue();


                        // option a - open this valve, only if that makes sense
                        if (_valves[current.Valve].FlowRate > 0) {
                            if (!current.OpenedValves.Contains(current.Valve)) {
                                //ReleasePressure(valve, openedValves + " " + valve, timeLeft - 1, flowRate + _valves[valve].FlowRate, pressureReleased + flowRate);
                                nextStates.Add(new State(current.Valve, current.OpenedValves + " " + current.Valve, current.FlowRate + _valves[current.Valve].FlowRate, current.PressureReleased + current.FlowRate));
                            }
                        }

                        foreach (var nextValve in _valves[current.Valve].TunnelsTo) {
                            //if (!current.VisitedValves.Contains(nextValve))
                            //{
                            //ReleasePressure(nextValve, openedValves, timeLeft - 1, flowRate, pressureReleased + flowRate);
                            nextStates.Add(new State(nextValve, current.OpenedValves, current.FlowRate, current.PressureReleased + current.FlowRate));
                            //}
                        }
                        if (maxReleasedSoFar < current.PressureReleased + current.FlowRate) maxReleasedSoFar = current.PressureReleased + current.FlowRate;
                    }
                    Console.WriteLine($"After minute {i + 1} we have {nextStates.Count()} states");
                    //if (nextStates.Count() > 100000)
                    //{
                    //    currentStates = new();
                    //    foreach (var next in nextStates)
                    //    {
                    //        if (next.PressureReleased > maxReleasedSoFar / 10) currentStates.Add(next);
                    //    }
                    //    Console.WriteLine($"  screened down to {currentStates.Count()} states");
                    //}
                    //else currentStates = nextStates;
                    currentStates = nextStates;
                }

                //Console.WriteLine("");
                long maxReleased = long.MinValue;
                string openedValves = string.Empty;

                foreach (var current in currentStates)
                    //    while (currentStates.Count > 0)
                    //{
                    //    State current = currentStates.Dequeue();
                    if (current.PressureReleased > maxReleased) {
                        maxReleased = current.PressureReleased;
                        openedValves = current.OpenedValves;
                    }

                Console.WriteLine($"\r\nreleased: {maxReleased} with valves {openedValves}");

                //ReleasePressure(_valves.First().Key, "", 30, 0, 0);



                return FormatResult(0, "not yet implemented");
            }

            //private void ReleasePressure(string valve, string openedValves, int timeLeft, long flowRate, long pressureReleased)
            //{
            //    if (timeLeft > 0)
            //    {
            //        // option a - open this valve, only if that makes sense
            //        if (_valves[valve].FlowRate > 0)
            //        {
            //            if (!openedValves.Contains(valve))
            //            {
            //                ReleasePressure(valve, openedValves + " " + valve, timeLeft - 1, flowRate + _valves[valve].FlowRate, pressureReleased + flowRate);
            //            }
            //        }

            //        // option b - do not open, go right to tunnels
            //        foreach (var nextValve in _valves[valve].TunnelsTo)
            //        {
            //            if (!openedValves.Contains(nextValve))
            //            {
            //                ReleasePressure(nextValve, openedValves, timeLeft - 1, flowRate, pressureReleased + flowRate);
            //            }
            //        }
            //    }
            //    else if (pressureReleased > _maxReleased)
            //    {
            //        Console.WriteLine($"released: {pressureReleased} with valves {openedValves}");
            //        _maxReleased = pressureReleased;
            //    }
            //}

            private struct State {
                public string Valve;
                public string OpenedValves;
                //public string VisitedValves;
                public long FlowRate;
                public long PressureReleased;

                public State(string valve, string openedValves, long flowRate, long pressureReleased) {
                    Valve = valve;
                    OpenedValves = openedValves;
                    //VisitedValves = visitedValves;
                    FlowRate = flowRate;
                    PressureReleased = pressureReleased;
                }
            }

            private class Valve {
                public string Id;
                public long FlowRate;
                public string[] TunnelsTo;
                public bool Open;

                public Valve(string id, long flowRate, string[] tunnelTo) {
                    Id = id;
                    FlowRate = flowRate;
                    TunnelsTo = tunnelTo;
                }
            }

        }
    }
}
