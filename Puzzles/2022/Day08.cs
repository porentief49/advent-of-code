﻿using System;
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
                int[][] _heights = new int[_ySize][];
                int[][] _scenicScores = new int[_ySize][];
                bool[][] _visible = new bool[_ySize][];
                for (int i = 0; i < _ySize; i++)
                {
                    _heights[i] = new int[_xSize];
                    _scenicScores[i] = new int[_xSize];
                    _visible[i] = new bool[_xSize];
                }

                //parse all
                for (int y = 0; y < _ySize; y++)
                {
                    for (int x = 0; x < _xSize; x++)
                    {
                        _heights[y][x] = int.Parse(InputData[y][x].ToString());
                        _visible[y][x] = false;
                        _scenicScores[y][x] = 0;
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
                            while (x - _left > 0 && _heights[y][x] > _heights[y][x - _left - 1]) _left++;
                            while (x + _right < _xSize - 1 && _heights[y][x] > _heights[y][x + _right + 1]) _right++;
                            while (y - _up > 0 && _heights[y][x] > _heights[y - _up - 1][x]) _up++;
                            while (y + _down < _ySize - 1 && _heights[y][x] > _heights[y + _down + 1][x]) _down++;
                            _visible[y][x] = _left == x || _right == _xSize - x - 1 || _up == y || _down == _ySize - y - 1;

                        }
                        else
                        {
                            if (x > 0) do _left++; while (x - _left > 0 && _heights[y][x] > _heights[y][x - _left]);
                            if (x < _xSize - 1) do _right++; while (x + _right < _xSize - 1 && _heights[y][x] > _heights[y][x + _right]);
                            if (y > 0) do _up++; while (y - _up > 0 && _heights[y][x] > _heights[y - _up][x]);
                            if (y < _ySize - 1) do _down++; while (y + _down < _ySize - 1 && _heights[y][x] > _heights[y + _down][x]);
                            _scenicScores[y][x] = _left * _right * _up * _down;
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
                            if (_visible[y][x]) _visibleX++;
                        }
                        else
                        {
                            if (_scenicScores[y][x] > _highest) _highest = _scenicScores[y][x];
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
                    for (int x = 0; x < Grid.GetLength(0); x++) _sb.Append(Grid[y][x] ? "x" : ".");
                    Console.WriteLine(_sb.ToString());
                }
                Console.WriteLine("");
            }
            void PrintIntGrid(int[][] Grid, int digits)
            {
                for (int y = 0; y < Grid.GetLength(1); y++)
                {
                    StringBuilder _sb = new();
                    for (int x = 0; x < Grid.GetLength(0); x++) _sb.Append(Grid[y][x].ToString() + " ".Repeat(digits).Substring(0, digits));
                    Console.WriteLine(_sb.ToString());
                }
                Console.WriteLine("");
            }
        }
    }
}
