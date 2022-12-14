using System;
using System.Net.Sockets;
using System.Text;

namespace Puzzles
{
    public partial class Year2022
    {
        public class Day14 : DayBase
        {

            protected override string Title { get; } = "Day 14: Regolith Reservoir";

            public override void SetupAll()
            {
                AddInputFile(@"2022\14_Example.txt");
                AddInputFile(@"2022\14_rAiner.txt");
                //AddInputFile(@"2022\14_SEGCC.txt");
            }

            public override void Init(string InputFile) => InputData = ReadFile(InputFile, true);

            public override string Solve(bool Part1)
            {
                if (Part1) return "";
                Cave.Build(InputData, Part1);
                Cave.Print();
                int count = 0;
                while (Cave.DropSand(Part1))
                {
                    count++;
                    //Cave.Print();
                }

                return FormatResult(count, "sand units");
            }

            private static class Cave
            {
                public const int _xStart = 500;
                public const int _yStart = 0;

                public static char[][] _cave;
                public static int _yMin;
                public static int _yMax;
                public static int _xMin;
                public static int _xMax;
                //public static int _yFloor;

                private static void SetCave(int y, int x, char what) => _cave[y - _yMin][x - _xMin] = what;
                private static char GetCave(int y, int x, bool part1)
                {
                    if (!part1 && y == _yMax) return '#';
                    if (y < _yMin || y > _yMax || x < _xMin || x > _xMax) return '.'; // to avoid error when trying to peek outside the grid
                    return _cave[y - _yMin][x - _xMin];
                }

                public static void Build(string[] InputData, bool part1)
                {
                    // build field
                    _yMin = Math.Min(InputData.Select(x => x.Split(" -> ").Select(x => int.Parse(x.Split(',')[1])).Min()).Min(), _yStart);
                    _yMax = Math.Max(InputData.Select(x => x.Split(" -> ").Select(x => int.Parse(x.Split(',')[1])).Max()).Max(), _yStart);
                    _xMin = Math.Min(InputData.Select(x => x.Split(" -> ").Select(x => int.Parse(x.Split(',')[0])).Min()).Min(), _xStart);
                    _xMax = Math.Max(InputData.Select(x => x.Split(" -> ").Select(x => int.Parse(x.Split(',')[0])).Max()).Max(), _xStart);
                    //_yFloor = _yMax + 2;
                    if (!part1)
                    {
                        _yMax += 2;
                        _xMin -= 500;
                        _xMax += 500;
                    }
                    //_cave = InitJaggedArray(part1 ? _yMax : _yFloor - _yMin + 1, _xMax - _xMin + 1, '.');
                    _cave = InitJaggedArray(_yMax - _yMin + 1, _xMax - _xMin + 1, '.');
                    //if (!part1) _yMax = _yFloor;

                    // add rocks
                    for (int i = 0; i < InputData.Count(); i++)
                    {
                        List<(int y, int x)> rocks = new();
                        foreach (var coord in InputData[i].Split(" -> "))
                        {
                            string[] coordsplit = coord.Split(',');
                            rocks.Add((int.Parse(coordsplit[1]), int.Parse(coordsplit[0])));
                        }
                        for (int ii = 1; ii < rocks.Count; ii++)
                        {
                            int yInc = Math.Sign(rocks[ii].y - rocks[ii - 1].y);
                            int xInc = Math.Sign(rocks[ii].x - rocks[ii - 1].x);
                            int yCurrent = rocks[ii - 1].y;
                            int xCurrent = rocks[ii - 1].x;
                            SetCave(yCurrent, xCurrent, '#');
                            do
                            {
                                yCurrent += yInc;
                                xCurrent += xInc;
                                SetCave(yCurrent, xCurrent, '#');
                            } while (yCurrent != rocks[ii].y || xCurrent != rocks[ii].x);
                        }
                    }
                }

                public static bool DropSand(bool part1)
                {
                    int x = _xStart;
                    int y = _yStart;
                    bool done = false;
                    if (GetCave(y, x, part1) != '.') return false;
                    do
                    {
                        //if (!part1 && GetCave(y, x - 1) != '.') return false;
                        if (GetCave(y + 1, x, part1) == '.') y++;
                        else if (GetCave(y + 1, x - 1, part1) == '.')
                        {
                            y++;
                            x--;
                        }
                        else if (GetCave(y + 1, x + 1, part1) == '.')
                        {
                            y++;
                            x++;
                        }
                        else
                        {
                            SetCave(y, x, 'o');
                            done = true;
                        }
                        if (y == _yMax) return false;
                    } while (!done);
                    return true;
                }


                public static void Print() => PrintGrid(_cave);

            }
        }
    }
}
