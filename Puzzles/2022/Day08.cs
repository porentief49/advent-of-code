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
                //AddInputFile(@"2022\08_rAiner.txt");
                //AddInputFile(@"2022\08_SEGCC.txt");
            }

            public override void Init(string InputFile) => InputData = ReadFile(InputFile, true);

            public override string Solve(bool Part1)
            {
                int _xSize = InputData[0].Length;
                int _ySize = InputData.Length;
                //int[][] _heights = new int[_xSize][_ySize];
                //int[][] _scenicScores = new int[_xSize][_ySize];
                //bool[][] _visible = new bool[_xSize][_ySize];
                int[][] _heights = new int[_xSize][];
                int[][] _scenicScores = new int[_xSize][];
                bool[][] _visible = new bool[_xSize][];
                for (int i = 0; i < _xSize; i++)
                {
                    _heights[i] = new int[_ySize];
                    _scenicScores[i] = new int[_ySize];
                    _visible[i] = new bool[_ySize];
                }

                //parse all
                for (int y = 0; y < _ySize; y++)
                {
                    for (int x = 0; x < _xSize; x++)
                    {
                        _heights[x][y] = int.Parse(InputData[y][x].ToString());
                        _visible[x][y] = false;
                        _scenicScores[x][y] = 0;
                    }
                }

                //check visibility / free area
                for (int y = 0; y < _ySize; y++)
                {
                    for (int x = 0; x < _xSize; x++)
                    {
                        int _left = 0;
                        int _right = 0;
                        int _up = 0;
                        int _down = 0;

                        if (Part1)
                        {
                            while (x - _left > 0 && _heights[x][y] > _heights[x - _left - 1][y]) _left++;
                            while (x + _right < _xSize - 1 && _heights[x][y] > _heights[x + _right + 1][y]) _right++;
                            while (y - _up > 0 && _heights[x][y] > _heights[x][y - _up - 1]) _up++;
                            while (y + _down < _ySize - 1 && _heights[x][y] > _heights[x][y + _down + 1]) _down++;
                            _visible[x][y] = _left == x || _right == _xSize - x - 1 || _up == y || _down == _ySize - y - 1;

                        }
                        else
                        {
                            if (x > 0) do _left++; while (x - _left > 0 && _heights[x][y] > _heights[x - _left][y]);
                            if (x < _xSize - 1) do _right++; while (x + _right < _xSize - 1 && _heights[x][y] > _heights[x + _right][y]);
                            if (y > 0) do _up++; while (y - _up > 0 && _heights[x][y] > _heights[x][y - _up]);
                            if (y < _ySize - 1) do _down++; while (y + _down < _ySize - 1 && _heights[x][y] > _heights[x][y + _down]);
                            _scenicScores[x][y] = _left * _right * _up * _down;
                        }
                    }
                }

                //count visibility
                int _visibleX = 0;
                int _highest = 0;
                for (int y = 0; y < _ySize; y++)
                {
                    for (int x = 0; x < _xSize; x++)
                    {
                        if (Part1)
                        {
                            if (_visible[x][y]) _visibleX++;
                        }
                        else
                        {
                            if (_scenicScores[x][y] > _highest) _highest = _scenicScores[x][y];
                        }
                    }
                }
                if (Part1) return FormatResult(_visibleX, "visible trees");
                return FormatResult(_highest, "best scenic score");
            }

            void PrintBoolGrid(bool[][] Grid)
            {
                for (int y = 0; y < Grid.GetLength(1); y++)
                {
                    StringBuilder _sb = new();
                    for (int x = 0; x < Grid.GetLength(0); x++) _sb.Append(Grid[x][y] ? "x" : ".");
                    Console.WriteLine(_sb.ToString());
                }
                Console.WriteLine("");
            }
            void PrintIntGrid(int[][] Grid, int digits)
            {
                for (int y = 0; y < Grid.GetLength(1); y++)
                {
                    StringBuilder _sb = new();
                    for (int x = 0; x < Grid.GetLength(0); x++) _sb.Append(Grid[x][y].ToString() + " ".Repeat(digits).Substring(0, digits));
                    Console.WriteLine(_sb.ToString());
                }
                Console.WriteLine("");
            }
        }
    }
}
