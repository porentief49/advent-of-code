using System;
using System.Net.Sockets;
using System.Text;

namespace Puzzles
{
    public partial class Year2022
    {
        public class Day14 : DayBase
        {
            private const int _xStart = 500;
            private const int _yStart = 0;

            private List<List<(int y, int x)>> _rocks = new();
            private char[][] _cave;
            private int _yMin = _yStart;
            private int _yMax = _yStart;
            private int _xMin = _xStart;
            private int _xMax = _xStart;

            protected override string Title { get; } = "Day 14: Regolith Reservoir";

            public override void SetupAll()
            {
                AddInputFile(@"2022\14_Example.txt");
                //AddInputFile(@"2022\14_rAiner.txt");
                //AddInputFile(@"2022\14_SEGCC.txt");
            }

            public override void Init(string InputFile)
            {
                InputData = ReadFile(InputFile, true);
                //_rocks = InputData.Select(x=>x)
                //List<(int x, int y)> row = InputData[0].Split(" -> ").Select(x => x.Split(',').Select((x, y) => (x, y).ToTuple())).ToList();
                //(int x, int y) row = InputData[0].Split(',').Select((xx, yy) => (int.Parse(xx), int.Parse(yy)));
                //(int x, int y) row = InputData[0].Split(" -> ").Select(x => x.Split(',').Select((string x, string y) => (int.Parse(x), int.Parse(y)).ToTuple()));
                foreach (var item in InputData)
                {
                    List<(int x, int y)> coords = new();
                    foreach (var item2 in item.Split(" -> "))
                    {
                        string[] split3 = item2.Split(',');
                        int y = int.Parse(split3[1]);
                        int x = int.Parse(split3[0]);
                        if (y < _yMin) _yMin = y;
                        if (y > _yMax) _yMax = y;
                        if (x < _xMin) _xMin = x;
                        if (x > _xMax) _xMax = x;
                        coords.Add((y, x));
                    }
                    _rocks.Add(coords);
                }
                //}
                //_yMin = _rocks.Select(c => c.Select(c => c.x).Min()).Min();
                //_yMax = _rocks.Select(c => c.Select(c => c.x).Max()).Max();
            }

            public override string Solve(bool Part1)
            {
                //if (!Part1) return "";
                BuildCave(InputData, Part1);
                if (Verbose) PrintGrid(_cave);
                int count = 0;
                while (DropSand(Part1))
                {
                    count++;
                    if (Verbose) PrintGrid(_cave);
                }

                return FormatResult(count, "sand units");
            }

            private void SetCave(int y, int x, char what) => _cave[y - _yMin][x - _xMin] = what;
            private char GetCave(int y, int x, bool part1)
            {
                if (!part1 && y == _yMax) return '#'; // part2 has an infinite base at the bottom
                if (y < _yMin || y > _yMax || x < _xMin || x > _xMax) return '.'; // to avoid error when trying to peek outside the grid
                return _cave[y - _yMin][x - _xMin];
            }

            public void BuildCave(string[] InputData, bool part1)
            {
                // build field
                //_yMin = Math.Min(InputData.Select(x => x.Split(" -> ").Select(x => int.Parse(x.Split(',')[1])).Min()).Min(), _yStart);
                //_yMax = Math.Max(InputData.Select(x => x.Split(" -> ").Select(x => int.Parse(x.Split(',')[1])).Max()).Max(), _yStart);
                //_xMin = Math.Min(InputData.Select(x => x.Split(" -> ").Select(x => int.Parse(x.Split(',')[0])).Min()).Min(), _xStart);
                //_xMax = Math.Max(InputData.Select(x => x.Split(" -> ").Select(x => int.Parse(x.Split(',')[0])).Max()).Max(), _xStart);
                if (!part1)
                {
                    _yMax += 2;
                    int height = _yMax - _yMin;
                    _xMin = Math.Min(_xMin, _xStart - height - 1); // for part2, extend the shape for the 45deg dumping angle
                    _xMax = Math.Max(_xMax, _xStart + height + 1);
                }
                _cave = InitJaggedArray(_yMax - _yMin + 1, _xMax - _xMin + 1, '.');

                // add rocks
                //for (int i = 0; i < InputData.Count(); i++)
                //for (int i = 0; i < _rocks.Count(); i++)
                foreach (var rock in _rocks)
                {
                    //List<(int y, int x)> rocks = new();
                    //foreach (var coord in InputData[i].Split(" -> "))
                    //{
                    //    string[] coordsplit = coord.Split(',');
                    //    rocks.Add((int.Parse(coordsplit[1]), int.Parse(coordsplit[0])));
                    //}
                    for (int ii = 1; ii < rock.Count; ii++)
                    {
                        int yInc = Math.Sign(rock[ii].y - rock[ii - 1].y);
                        int xInc = Math.Sign(rock[ii].x - rock[ii - 1].x);
                        int yCurrent = rock[ii - 1].y;
                        int xCurrent = rock[ii - 1].x;
                        SetCave(yCurrent, xCurrent, '#');
                        do
                        {
                            yCurrent += yInc;
                            xCurrent += xInc;
                            SetCave(yCurrent, xCurrent, '#');
                        } while (yCurrent != rock[ii].y || xCurrent != rock[ii].x);
                    }
                }
            }

            public bool DropSand(bool part1)
            {
                int x = _xStart;
                int y = _yStart;
                bool done = false;
                if (GetCave(y, x, part1) != '.') return false;
                do
                {
                    if (GetCave(y + 1, x, part1) == '.') y++; // fall straight down
                    else if (GetCave(y + 1, x - 1, part1) == '.') // slide to the left
                    {
                        y++;
                        x--;
                    }
                    else if (GetCave(y + 1, x + 1, part1) == '.') // slide to the right
                    {
                        y++;
                        x++;
                    }
                    else // final position
                    {
                        SetCave(y, x, 'o');
                        done = true;
                    }
                    if (y == _yMax) return false;
                } while (!done);
                return true;
            }
        }
    }
}
