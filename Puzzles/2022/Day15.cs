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

            protected override string Title { get; } = "Day 15: Beacon Exclusion Zone:";

            public override void SetupAll()
            {
                AddInputFile(@"2022\15_Example.txt");
                //AddInputFile(@"2022\15_rAiner.txt");
                //AddInputFile(@"2022\15_SEGCC.txt");
            }

            public override void Init(string InputFile) => InputData = ReadFile(InputFile, true);

            public override string Solve(bool Part1)
            {
                //if (!Part1) return "";
                _sensors = new();
                foreach (var line in InputData)
                {
                    string[] split = line.Replace('=', ' ').Replace(':', ' ').Replace(',', ' ').Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    _sensors.Add(new(long.Parse(split[5]), long.Parse(split[3]), long.Parse(split[13]), long.Parse(split[11])));
                }
                if (Part1)
                {
                    //PrintGrid(_map);
                    int row = 10;
                    //int row = 2000000;
                    int noBeaconLocations = 0;
                    for (int i = -10; i < 50; i++)
                    //for (int i = -20000000; i < 20000000; i++)
                    {
                        //check for beacons
                        bool foundBeacon = false;
                        foreach (var sensor in _sensors)
                        {
                            if (sensor.yBeacon == row && sensor.xBeacon == i)
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
                                if (sensor.CalcManhattanDist(sensor.ySensor, sensor.xSensor, row, i) <= sensor.ManhattanDist)
                                {
                                    noBeaconLocations++;
                                    break;
                                }
                            }
                        }
                    }
                    return FormatResult(noBeaconLocations, $"no beacons in row {row}");
                    //1000000: 1529240
                    //3000000: 3529239
                    //10000000: 5403290
                    //20000000: 5403290
                }

                //Part2: sensor must sit on an edge of a diamond shape
                //List<(int y, int x)> _possibleLocations;
                long xx = 0;
                long yy = 0;
                for (int thisSensor = 0; thisSensor < _sensors.Count; thisSensor++)
                {
                    //Console.WriteLine($"this sensor: {thisSensor}");
                    for (long i = 0; i <= _sensors[thisSensor].ManhattanDist; i++)
                    {
                        bool good = true; // be optimistic

                        //move from rightmost to left up
                        xx = _sensors[thisSensor].xSensor + (_sensors[thisSensor].ManhattanDist + 1 - i);
                        yy = _sensors[thisSensor].ySensor - i;
                        for (int otherSensor = 0; otherSensor < _sensors.Count; otherSensor++)
                        {
                            if (thisSensor != otherSensor)
                            {
                                //Console.WriteLine($"  other sensor {thisSensor}");
                                if (xx < 0 || xx > 20 || yy < 0 || yy > 40)
                                {
                                    good = false;
                                    break;
                                }
                                if (_sensors[otherSensor].CalcManhattanDist(_sensors[otherSensor].ySensor, _sensors[otherSensor].xSensor, yy, xx) <= _sensors[otherSensor].ManhattanDist)
                                {
                                    good = false;
                                    break;
                                }
                            }
                        }
                        if (good) Console.WriteLine($"found one at x {xx}, y {yy}");

                        //move from toptmost to left down
                        xx = _sensors[thisSensor].xSensor - i;
                        yy = _sensors[thisSensor].ySensor - (_sensors[thisSensor].ManhattanDist + 1 - i);
                        for (int otherSensor = 0; otherSensor < _sensors.Count; otherSensor++)
                        {
                            if (thisSensor != otherSensor)
                            {
                                //Console.WriteLine($"  other sensor {thisSensor}");
                                if (xx < 0 || xx > 20 || yy < 0 || yy > 40)
                                {
                                    good = false;
                                    break;
                                }
                                if (_sensors[otherSensor].CalcManhattanDist(_sensors[otherSensor].ySensor, _sensors[otherSensor].xSensor, yy, xx) <= _sensors[otherSensor].ManhattanDist)
                                {
                                    good = false;
                                    break;
                                }
                            }
                        }
                        if (good) Console.WriteLine($"found one at x {xx}, y {yy}");

                        //move from leftmost to right down
                        xx = _sensors[thisSensor].xSensor - (_sensors[thisSensor].ManhattanDist + 1 - i);
                        yy = _sensors[thisSensor].ySensor + i;
                        for (int otherSensor = 0; otherSensor < _sensors.Count; otherSensor++)
                        {
                            if (thisSensor != otherSensor)
                            {
                                //Console.WriteLine($"  other sensor {thisSensor}");
                                if (xx < 0 || xx > 20 || yy < 0 || yy > 40)
                                {
                                    good = false;
                                    break;
                                }
                                if (_sensors[otherSensor].CalcManhattanDist(_sensors[otherSensor].ySensor, _sensors[otherSensor].xSensor, yy, xx) <= _sensors[otherSensor].ManhattanDist)
                                {
                                    good = false;
                                    break;
                                }
                            }
                        }
                        if (good) Console.WriteLine($"found one at x {xx}, y {yy}");

                        //move from bottommost to right up
                        xx = _sensors[thisSensor].xSensor + i;
                        yy = _sensors[thisSensor].ySensor + (_sensors[thisSensor].ManhattanDist + 1 - i);
                        for (int otherSensor = 0; otherSensor < _sensors.Count; otherSensor++)
                        {
                            if (thisSensor != otherSensor)
                            {
                                //Console.WriteLine($"  other sensor {thisSensor}");
                                if (xx < 0 || xx > 20 || yy < 0 || yy > 40)
                                {
                                    good = false;
                                    break;
                                }
                                if (_sensors[otherSensor].CalcManhattanDist(_sensors[otherSensor].ySensor, _sensors[otherSensor].xSensor, yy, xx) <= _sensors[otherSensor].ManhattanDist)
                                {
                                    good = false;
                                    break;
                                }
                            }
                        }
                        if (good) Console.WriteLine($"found one at x {xx}, y {yy}");
                    }
                }


                return "";
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
                    this.ManhattanDist = CalcManhattanDist(ySensor, xSensor, yBeacon, xBeacon);
                }

                public long CalcManhattanDist(long ySensor, long xSensor, long yBeacon, long xBeacon) => Math.Abs(yBeacon - ySensor) + Math.Abs(xBeacon - xSensor);
            }
        }
    }
}
