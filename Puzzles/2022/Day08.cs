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
                AddInputFile(@"2022\08_SEGCC.txt");
            }

            public override void Init(string InputFile) => InputData = ReadFile(InputFile, true);

            public override string Solve(bool Part1)
            {
                int _xSize = InputData[0].Length;
                int _ySize = InputData.Length;
                //int _highest = 0;
                int[,] _heights = new int[_xSize, _ySize];
                bool[,] _visibleFromLeft = new bool[_xSize, _ySize];
                bool[,] _visibleFromRight = new bool[_xSize, _ySize];
                bool[,] _visibleFromTop = new bool[_xSize, _ySize];
                bool[,] _visibleFromBottom = new bool[_xSize, _ySize];
                int[,] _scenicScores = new int[_xSize, _ySize];
                bool[,] _visible = new bool[_xSize, _ySize];

                //parse all
                for (int y = 0; y < _ySize; y++)
                {
                    //_highest = -1;
                    for (int x = 0; x < _xSize; x++)
                    {
                        _heights[x, y] = int.Parse(InputData[y][x].ToString());
                    }
                }

                if (Part1)
                {

                    //check visibility
                    for (int y = 0; y < _ySize; y++)
                    {
                        for (int x = 0; x < _xSize; x++)
                        {
                            // go left
                            int _left = 0;
                            while (x - _left > 0 && _heights[x, y] > _heights[x - _left - 1, y]) _left++;

                            // go right
                            int _right = 0;
                            while (x + _right < _xSize - 1 && _heights[x, y] > _heights[x + _right + 1, y]) _right++;

                            // go up
                            int _up = 0;
                            while (y - _up > 0 && _heights[x, y] > _heights[x, y - _up - 1]) _up++;

                            // go down
                            int _down = 0;
                            while (y + _down < _ySize - 1 && _heights[x, y] > _heights[x, y + _down + 1]) _down++;

                            _visible[x, y] = (_left == x || _right == _xSize - x - 1 || _up == y || _down == _ySize - y - 1);
                        }
                    }

                    //PrintIntGrid(_heights, 3);
                    //PrintBoolGrid(_visible);

                    //count visibility
                    int _visibleX = 0;
                    for (int y = 0; y < _ySize; y++)
                    {
                        for (int x = 0; x < _xSize; x++)
                        {
                            if (_visible[x, y]) _visibleX++;
                        }
                    }
                    return FormatResult(_visibleX, "visible trees");



                    ////from left
                    //for (int y = 0; y < _ySize; y++)
                    //{
                    //    _highest = -1;
                    //    for (int x = 0; x < _xSize; x++)
                    //    {
                    //        if (_heights[x, y] > _highest)
                    //        {
                    //            _visibleFromLeft[x, y] = true;
                    //            _highest = _heights[x, y];
                    //        }
                    //    }
                    //}
                    ////PrintGrid(_visibleFromLeft);

                    ////from right
                    //for (int y = 0; y < _ySize; y++)
                    //{
                    //    _highest = -1;
                    //    for (int x = _xSize - 1; x >= 0; x--)
                    //    {
                    //        if (_heights[x, y] > _highest)
                    //        {
                    //            _visibleFromRight[x, y] = true;
                    //            _highest = _heights[x, y];
                    //        }
                    //    }
                    //}
                    ////PrintGrid(_visibleFromRight);

                    ////from top
                    //for (int x = 0; x < _xSize; x++)
                    //{
                    //    _highest = -1;
                    //    for (int y = 0; y < _ySize; y++)
                    //    {
                    //        if (_heights[x, y] > _highest)
                    //        {
                    //            _visibleFromTop[x, y] = true;
                    //            _highest = _heights[x, y];
                    //        }
                    //    }
                    //}
                    ////PrintGrid(_visibleFromTop);

                    ////from bottom
                    //for (int x = 0; x < _xSize; x++)
                    //{
                    //    _highest = -1;
                    //    for (int y = _ySize - 1; y >= 0; y--)
                    //    {
                    //        if (_heights[x, y] > _highest)
                    //        {
                    //            _visibleFromBottom[x, y] = true;
                    //            _highest = _heights[x, y];
                    //        }
                    //    }
                    //}
                    ////PrintGrid(_visibleFromBottom);

                    //// calc all
                    //int _visibleTrees = 0;
                    //for (int y = 0; y < _ySize; y++)
                    //{
                    //    for (int x = 0; x < _xSize; x++)
                    //    {
                    //        if (_visibleFromLeft[x, y] || _visibleFromRight[x, y] || _visibleFromTop[x, y] || _visibleFromBottom[x, y]) _visibleTrees++;
                    //    }
                    //}
                    //return FormatResult(_visibleTrees, "visible trees");
                }
                else
                {
                    //calc scenic scores
                    for (int y = 0; y < _ySize; y++)
                    {
                        for (int x = 0; x < _xSize; x++)
                        {
                            if (x == 2 && y == 2)
                            {
                                Console.Write("x");
                            }

                            int _toRight = _xSize - x - 1;
                            for (int i = x + 1; i < _xSize; i++)
                            {
                                if (_heights[i, y] >= _heights[x, y])
                                {
                                    _toRight = i - x;
                                    break;
                                }
                            }

                            int _toLeft = x;
                            for (int i = x - 1; i >= 0; i--)
                            {
                                if (_heights[i, y] >= _heights[x, y])
                                {
                                    _toLeft = x - i;
                                    break;
                                }
                            }

                            int _toBottom = _ySize - y - 1;
                            for (int i = y + 1; i < _ySize; i++)
                            {
                                if (_heights[x, i] >= _heights[x, y])
                                {
                                    _toBottom = i - y;
                                    break;
                                }
                            }

                            int _toTop = y;
                            for (int i = y - 1; i >= 0; i--)
                            {
                                if (_heights[x, i] >= _heights[x, y])
                                {
                                    _toTop = y - i;
                                    break;
                                }
                            }

                            //while (x - _toLeft > 0 && _heights[x - _toLeft - 1, y] < _heights[x, y]) _toLeft++;
                            //int _toRight = 0;
                            //while (x + _toRight < _xSize - 1 && _heights[x + _toRight + 1, y] < _heights[x, y]) _toRight++;
                            //int _toTop = 0;
                            //while (y - _toTop > 0 && _heights[x, y - _toTop - 1] < _heights[x, y]) _toTop++;
                            //int _toBottom = 0;
                            //while (y + _toBottom < _ySize - 1 && _heights[x , y+ _toBottom + 1] < _heights[x, y]) _toBottom++;
                            _scenicScores[x, y] = _toLeft * _toRight * _toTop * _toBottom;
                        }
                    }

                    //PrintIntGrid(_heights, 3);
                    //PrintIntGrid(_scenicScores, 3);

                    //find max
                    int _highest = 0;
                    for (int y = 0; y < _ySize; y++)
                    {
                        for (int x = 0; x < _xSize; x++)
                        {
                            if (_scenicScores[x, y] > _highest)
                            {
                                //Console.WriteLine($"found scenic score {_scenicScores[x, y]} at x={x}|y={y}");
                                _highest = _scenicScores[x, y];
                            }
                        }
                    }
                    return FormatResult(_highest, "best scenic score");
                }
            }

            void PrintBoolGrid(bool[,] Grid)
            {
                for (int y = 0; y < Grid.GetLength(1); y++)
                {
                    StringBuilder _sb = new();
                    for (int x = 0; x < Grid.GetLength(0); x++) _sb.Append(Grid[x, y] ? "x" : ".");
                    Console.WriteLine(_sb.ToString());
                }
                Console.WriteLine("");
            }
            void PrintIntGrid(int[,] Grid, int digits)
            {
                for (int y = 0; y < Grid.GetLength(1); y++)
                {
                    StringBuilder _sb = new();
                    for (int x = 0; x < Grid.GetLength(0); x++) _sb.Append(Grid[x, y].ToString() + " ".Repeat(digits).Substring(0, digits));
                    Console.WriteLine(_sb.ToString());
                }
                Console.WriteLine("");
            }
        }
    }
}
