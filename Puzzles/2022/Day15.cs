using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Schema;

namespace Puzzles
{
    public partial class Year2022
    {
        public class Day15 : DayBase
        {
            private List<Sensor> _sensors;
            private bool _isExample = false;
            long _maxDim;

            protected override string Title { get; } = "Day 15: Beacon Exclusion Zone";

            public override void SetupAll()
            {
                AddInputFile(@"2022\15_Example.txt");
                AddInputFile(@"2022\15_rAiner.txt");
                AddInputFile(@"2022\15_SEGCC.txt");
            }

            public override void Init(string InputFile)
            {
                InputData = ReadFile(InputFile, true);
                _isExample = InputFile.Contains("Example");
            }

            public override string Solve(bool Part1)
            {
                _maxDim = _isExample ? 20 : 4000000;
                _sensors = new();
                foreach (var line in InputData)
                {
                    string[] split = line.Replace('=', ' ').Replace(':', ' ').Replace(',', ' ').Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    _sensors.Add(new(long.Parse(split[5]), long.Parse(split[3]), long.Parse(split[13]), long.Parse(split[11])));
                }
                if (Part1)
                {
                    long y = _isExample ? 10 : 2000000;
                    long noBeaconLocations = 0;
                    for (long x = Sensor.xMin; x <= Sensor.xMax; x++) if (!_sensors.Any(c => c.xBeacon == x && c.yBeacon == y)) if (!CheckOutsideRange(y, x, false)) noBeaconLocations++;
                    return FormatResult(noBeaconLocations, $"no beacons in row {y}");
                }
                (long yy, long xx) = FindBeaconSpot();
                return FormatResult($"{xx * 4000000 + yy}", $"tuning frequency");
            }

            private (long y, long x) FindBeaconSpot()
            {
                long y;
                long x;
                for (int i = 0; i < _sensors.Count; i++)
                {
                    for (long ii = 0; ii <= _sensors[i].ManhattanDist; ii++)
                    {
                        (x, y) = (_sensors[i].x + (_sensors[i].ManhattanDist + 1 - ii), _sensors[i].y - ii);//move from rightmost to left up
                        if (CheckOutsideRange(y, x, true)) return (y, x);
                        (x, y) = (_sensors[i].x - ii, _sensors[i].y - (_sensors[i].ManhattanDist + 1 - ii));//move from toptmost to left down
                        if (CheckOutsideRange(y, x, true)) return (y, x);
                        (x, y) = (_sensors[i].x - (_sensors[i].ManhattanDist + 1 - ii), _sensors[i].y + ii);//move from leftmost to right down
                        if (CheckOutsideRange(y, x, true)) return (y, x);
                        (x, y) = (_sensors[i].x + ii, _sensors[i].y + (_sensors[i].ManhattanDist + 1 - ii));//move from bottommost to right up
                        if (CheckOutsideRange(y, x, true)) return (y, x);
                    }
                }
                return (-1, -1); // no beacon spot found
            }

            private bool CheckOutsideRange(long y, long x, bool testRange)
            {
                if (testRange) if (x < 0 || x > _maxDim || y < 0 || y > _maxDim) return false;
                for (int i = 0; i < _sensors.Count; i++) if (_sensors[i].CalcManhattanDist(y, x) <= _sensors[i].ManhattanDist) return false;
                return true;
            }

            private class Sensor
            {
                public static long xMin = long.MaxValue;
                public static long xMax = long.MinValue;
                public long y;
                public long x;
                public long yBeacon;
                public long xBeacon;
                public long ManhattanDist;

                public Sensor(long y, long x, long yBeacon, long xBeacon)
                {
                    this.y = y;
                    this.x = x;
                    this.yBeacon = yBeacon;
                    this.xBeacon = xBeacon;
                    ManhattanDist = CalcManhattanDist(yBeacon, xBeacon);
                    xMin = Math.Min(xMin, x - ManhattanDist);
                    xMax = Math.Max(xMax, x + ManhattanDist);
                }

                public long CalcManhattanDist(long y, long x) => Math.Abs(y - this.y) + Math.Abs(x - this.x);
            }
        }
    }
}
