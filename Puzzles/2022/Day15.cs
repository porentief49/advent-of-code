using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

namespace Puzzles
{
    public partial class Year2022
    {
        public class Day15 : DayBase
        {
            private List<Sensor> _sensors;
            private bool _isExample = false;

            protected override string Title { get; } = "Day 15: Beacon Exclusion Zone:";

            public override void SetupAll()
            {
                AddInputFile(@"2022\15_Example.txt");
                AddInputFile(@"2022\15_rAiner.txt");
                //AddInputFile(@"2022\15_SEGCC.txt");
            }

            public override void Init(string InputFile)
            {
                InputData = ReadFile(InputFile, true);
                _isExample = InputFile.Contains("Example");
            }

            public override string Solve(bool Part1)
            {
                long part1searchRow = _isExample ? 10 : 2000000;
                long part1searchRange = _isExample ? 50 : 20000000;
                long maxDim = _isExample ? 20 : 4000000;

                _sensors = new();
                foreach (var line in InputData)
                {
                    string[] split = line.Replace('=', ' ').Replace(':', ' ').Replace(',', ' ').Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    _sensors.Add(new(long.Parse(split[5]), long.Parse(split[3]), long.Parse(split[13]), long.Parse(split[11])));
                }
                if (Part1)
                {
                    long noBeaconLocations = 0;
                    for (long i = -part1searchRange; i < part1searchRange; i++) //EXAMPLE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    {
                        //check for beacons
                        bool foundBeacon = false;
                        foreach (var sensor in _sensors)
                        {
                            if (sensor.yBeacon == part1searchRow && sensor.xBeacon == i)
                            {
                                foundBeacon = true;
                                break;
                            }
                        }

                        //now check if within manhattan dist of any
                        if (!foundBeacon)
                        {
                            foreach (var sensor in _sensors)
                            {
                                //if (sensor.CalcManhattanDist_OLD(sensor.ySensor, sensor.xSensor, part1searchRow, i) <= sensor.ManhattanDist)
                                    if (sensor.CalcManhattanDist(part1searchRow, i) <= sensor.ManhattanDist)
                                    {
                                        noBeaconLocations++;
                                    break;
                                }
                            }
                        }
                    }
                    return FormatResult(noBeaconLocations, $"no beacons in row {part1searchRow}");
                }

                //Part2: sensor must sit on an edge of a diamond shape
                long xx = 0;
                long yy = 0;
                for (int thisSensor = 0; thisSensor < _sensors.Count; thisSensor++)
                {
                    bool good = true;
                    Console.WriteLine($"this sensor: {thisSensor}");
                    for (long i = 0; i <= _sensors[thisSensor].ManhattanDist; i++)
                    {
                        good = true; // be optimistic

                        //move from rightmost to left up
                        xx = _sensors[thisSensor].xSensor + (_sensors[thisSensor].ManhattanDist + 1 - i);
                        yy = _sensors[thisSensor].ySensor - i;
                        for (int otherSensor = 0; otherSensor < _sensors.Count; otherSensor++)
                        {
                            if (thisSensor != otherSensor)
                            {
                                if (xx < 0 || xx > maxDim || yy < 0 || yy > maxDim)
                                {
                                    good = false;
                                    break;
                                }
                                if (_sensors[otherSensor].CalcManhattanDist(yy, xx) <= _sensors[otherSensor].ManhattanDist)
                                {
                                    good = false;
                                    break;
                                }
                            }
                        }
                        if (good)
                        {
                            Console.WriteLine($"found one at x {xx}, y {yy}");
                            break;
                        }

                        //move from toptmost to left down
                        xx = _sensors[thisSensor].xSensor - i;
                        yy = _sensors[thisSensor].ySensor - (_sensors[thisSensor].ManhattanDist + 1 - i);
                        for (int otherSensor = 0; otherSensor < _sensors.Count; otherSensor++)
                        {
                            if (thisSensor != otherSensor)
                            {
                                if (xx < 0 || xx > maxDim || yy < 0 || yy > maxDim)
                                {
                                    good = false;
                                    break;
                                }
                                if (_sensors[otherSensor].CalcManhattanDist(yy, xx) <= _sensors[otherSensor].ManhattanDist)
                                {
                                    good = false;
                                    break;
                                }
                            }
                        }
                        if (good)
                        {
                            Console.WriteLine($"found one at x {xx}, y {yy}");
                            break;
                        }

                        //move from leftmost to right down
                        xx = _sensors[thisSensor].xSensor - (_sensors[thisSensor].ManhattanDist + 1 - i);
                        yy = _sensors[thisSensor].ySensor + i;
                        for (int otherSensor = 0; otherSensor < _sensors.Count; otherSensor++)
                        {
                            if (thisSensor != otherSensor)
                            {
                                if (xx < 0 || xx > maxDim || yy < 0 || yy > maxDim)
                                {
                                    good = false;
                                    break;
                                }
                                if (_sensors[otherSensor].CalcManhattanDist(yy, xx) <= _sensors[otherSensor].ManhattanDist)
                                {
                                    good = false;
                                    break;
                                }
                            }
                        }
                        if (good)
                        {
                            Console.WriteLine($"found one at x {xx}, y {yy}");
                            break;
                        }

                        //move from bottommost to right up
                        xx = _sensors[thisSensor].xSensor + i;
                        yy = _sensors[thisSensor].ySensor + (_sensors[thisSensor].ManhattanDist + 1 - i);
                        for (int otherSensor = 0; otherSensor < _sensors.Count; otherSensor++)
                        {
                            if (thisSensor != otherSensor)
                            {
                                if (xx < 0 || xx > maxDim || yy < 0 || yy > maxDim)
                                {
                                    good = false;
                                    break;
                                }
                                if (_sensors[otherSensor].CalcManhattanDist(yy, xx) <= _sensors[otherSensor].ManhattanDist)
                                {
                                    good = false;
                                    break;
                                }
                            }
                        }
                        if (good)
                        {
                            Console.WriteLine($"found one at x {xx}, y {yy}");
                            break;
                        }
                    }
                    if (good) break;
                }
                return FormatResult($"{xx * 4000000 + yy}", $"tuning frequency");
            }

            private class Sensor
            {
                public long ySensor;
                public long xSensor;
                public long yBeacon;
                public long xBeacon;
                public long ManhattanDist;

                public Sensor(long ySensor, long xSensor, long yBeacon, long xBeacon)
                {
                    this.ySensor = ySensor;
                    this.xSensor = xSensor;
                    this.yBeacon = yBeacon;
                    this.xBeacon = xBeacon;
                    //this.ManhattanDist = CalcManhattanDist_OLD(ySensor, xSensor, yBeacon, xBeacon);
                    ManhattanDist = CalcManhattanDist(yBeacon, xBeacon);
                }

                //public long CalcManhattanDist_OLD(long ySensor, long xSensor, long yBeacon, long xBeacon) => Math.Abs(yBeacon - ySensor) + Math.Abs(xBeacon - xSensor);
                public long CalcManhattanDist(long y, long x) => Math.Abs(y - ySensor) + Math.Abs(x - xSensor);
            }
        }
    }
}
