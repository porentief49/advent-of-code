using System;
using System.Net.Sockets;
using System.Text;

namespace Puzzles
{
    public partial class Year2022
    {
        public class Day08 : DayBase
        {
            protected override string Title { get; } = "Day 8: Treetop Tree House";

            public override void SetupAll()
            {
                AddInputFile(@"2022\08_Example.txt");
                AddInputFile(@"2022\08_rAiner.txt");
                //AddInputFile(@"2022\06_Example3.txt");
                //AddInputFile(@"2022\06_Example4.txt");
                //AddInputFile(@"2022\06_Example5.txt");
                //AddInputFile(@"2022\06_rAiner.txt");
                //AddInputFile(@"2022\06_SEGCC.txt");
            }

            public override void Init(string InputFile) => InputData = ReadFile(InputFile, true);

            public override string Solve(bool Part1)
            {
                int _xSize = InputData[0].Length;
                int _ySize = InputData.Length;
                int _highest;
                int[,] _heights = new int[_xSize, _ySize];
                bool[,] _visibleFromLeft = new bool[_xSize, _ySize];
                bool[,] _visibleFromRight = new bool[_xSize, _ySize];
                bool[,] _visibleFromTop = new bool[_xSize, _ySize];
                bool[,] _visibleFromBottom = new bool[_xSize, _ySize];

                //parse all
                for (int y = 0; y < _ySize; y++)
                {
                    _highest = -1;
                    for (int x = 0; x < _xSize; x++)
                    {
                        _heights[x, y] = int.Parse(InputData[y][x].ToString());
                    }
                }


                //from left
                for (int y = 0; y < _ySize; y++)
                {
                    _highest = -1;
                    for (int x = 0; x < _xSize; x++)
                    {
                        if (_heights[x, y] > _highest)
                        {
                            _visibleFromLeft[x, y] = true;
                            _highest = _heights[x, y];
                        }
                    }
                }
                //PrintGrid(_visibleFromLeft);

                //from right
                for (int y = 0; y < _ySize; y++)
                {
                    _highest = -1;
                    for (int x = _xSize - 1; x >= 0; x--)
                    {
                        if (_heights[x, y] > _highest)
                        {
                            _visibleFromRight[x, y] = true;
                            _highest = _heights[x, y];
                        }
                    }
                }
                //PrintGrid(_visibleFromRight);

                //from top
                for (int x = 0; x < _xSize; x++)
                {
                    _highest = -1;
                    for (int y = 0; y < _ySize; y++)
                    {
                        if (_heights[x, y] > _highest)
                        {
                            _visibleFromTop[x, y] = true;
                            _highest = _heights[x, y];
                        }
                    }
                }
                //PrintGrid(_visibleFromTop);

                //from bottom
                for (int x = 0; x < _xSize; x++)
                {
                    _highest = -1;
                    for (int y = _ySize - 1; y >= 0; y--)
                    {
                        if (_heights[x, y] > _highest)
                        {
                            _visibleFromBottom[x, y] = true;
                            _highest = _heights[x, y];
                        }
                    }
                }
                //PrintGrid(_visibleFromBottom);

                // calc all
                int _visibleTrees = 0;
                for (int y = 0; y < _ySize; y++)
                {
                    for (int x = 0; x < _xSize; x++)
                    {
                        if (_visibleFromLeft[x, y] || _visibleFromRight[x, y] || _visibleFromTop[x, y] || _visibleFromBottom[x, y]) _visibleTrees++;
                    }
                }
                return FormatResult(_visibleTrees, "visible trees");
            }

            void PrintGrid(bool[,] Grid)
            {
                for (int y = 0; y < Grid.GetLength(1); y++)
                {
                    StringBuilder _sb = new();
                    for (int x = 0; x < Grid.GetLength(0); x++)
                    {
                        _sb.Append(Grid[x, y] ? "x": ".");

                    }
                    Console.WriteLine(_sb.ToString());
                }
                Console.WriteLine("");
            }
        }
    }
}
