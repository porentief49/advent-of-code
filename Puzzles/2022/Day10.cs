using System;
using System.Net.Sockets;
using System.Text;

namespace Puzzles
{
    public partial class Year2022
    {
        public class Day10 : DayBase
        {
            protected override string Title { get; } = "Day 10: Cathode-Ray Tube";

            public override void SetupAll()
            {
                //AddInputFile(@"2022\10_Example1.txt");
                AddInputFile(@"2022\10_Example2.txt");
                AddInputFile(@"2022\10_rAiner.txt");
                AddInputFile(@"2022\10_SEGCC.txt");
            }

            public override void Init(string InputFile) => InputData = ReadFile(InputFile, true);

            public override string Solve(bool Part1)
            {
                int _xReg = 1;
                List<int> _xRegValues = new() { _xReg, _xReg }; // to get the index right
                for (int i = 0; i < InputData.Length; i++)
                {
                    string[] _split = InputData[i].Split(' ');
                    if (_split[0] == "noop") _xRegValues.Add(_xReg);
                    else
                    {
                        _xRegValues.Add(_xReg);
                        _xReg += int.Parse(_split[1]);
                        _xRegValues.Add(_xReg);
                    }
                }

                // Part 1
                if (Part1)
                {
                    int _index = 20;
                    int _signalStrength = 0;
                    if (Verbose) for (int i = 1; i < _xRegValues.Count; i++) Console.WriteLine($"during cycle {i} xreg is {_xRegValues[i]}");
                    while (_index < _xRegValues.Count)
                    {
                        _signalStrength += _xRegValues[_index] * _index;
                        _index += 40;
                    }
                    return FormatResult(_signalStrength, "signal strength");
                }

                //Part 2
                //string[] _out = new string[6];
                //for (int i = 1; i < _xRegValues.Count; i++)
                bool[,] _crt = new bool[40, 6];
                for (int i = 1; i < 241; i++)
                {
                    int _horiz = (i - 1) % 40;
                    int _vert = (int)Math.Floor((i - 1) / 40.0);
                    //_out[_vert] += (_horiz >= _xRegValues[i] - 1 && _horiz <= _xRegValues[i] + 1) ? "#" : ".";
                    _crt[_horiz, _vert] = (_horiz >= _xRegValues[i] - 1 && _horiz <= _xRegValues[i] + 1);
                }
                //foreach (var _line in _out) Console.WriteLine(_line);
                if(Verbose) PrintPaper(_crt);
                string _output = string.Empty;
                for (int i = 0; i < 8; i++)
                {
                    _output += GetLetter(_crt, i * 5);
                }
                return FormatResult(_output, "CRT output");
            }

            void PrintPaper(bool[,] aPaper)
            {
                var lSb = new StringBuilder();
                for (int lY = 0; lY < aPaper.GetLength(1); lY++)
                {
                    for (int lX = 0; lX < aPaper.GetLength(0); lX++)
                    {
                        lSb.Append(aPaper[lX, lY] ? '#' : '.');
                    }
                    lSb.Append(Environment.NewLine);
                }
                Console.WriteLine(lSb.ToString());
            }


            string GetLetter(bool[,] aPaper, int aCol) // from 2021/13
            {
                const int LETTER_WIDTH = 5;
                const int LETTER_HEIGHT = 6;
                const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                //string[] LETTER_PIXELS = {
                //        ".##..###...##.......####.####..##..#..#..###...##.#..#.#...............##..###.......###...###......#..#................#...#.####.",
                //        "#..#.#..#.#..#......#....#....#..#.#..#...#.....#.#.#..#..............#..#.#..#......#..#.#.........#..#................#...#....#.",
                //        "#..#.###..#.........###..###..#....####...#.....#.##...#..............#..#.#..#......#..#.#.........#..#.................#.#....#..",
                //        "####.#..#.#.........#....#....#.##.#..#...#.....#.#.#..#..............#..#.###.......###...##.......#..#..................#....#...",
                //        "#..#.#..#.#..#......#....#....#..#.#..#...#..#..#.#.#..#..............#..#.#.........#.#.....#......#..#..................#...#....",
                //        "#..#.###...##.......####.#.....###.#..#..###..##..#..#.####............##..#.........#..#.###........##...................#...####." };
                string[] LETTER_PIXELS = {
                        ".##..###...##.......####.####..##..#..#..###...##.#..#.#...............##..###.......###...###......#..#.....................####.",
                        "#..#.#..#.#..#......#....#....#..#.#..#...#.....#.#.#..#..............#..#.#..#......#..#.#.........#..#........................#.",
                        "#..#.###..#.........###..###..#....####...#.....#.##...#..............#..#.#..#......#..#.#.........#..#.......................#..",
                        "####.#..#.#.........#....#....#.##.#..#...#.....#.#.#..#..............#..#.###.......###...##.......#..#......................#...",
                        "#..#.#..#.#..#......#....#....#..#.#..#...#..#..#.#.#..#..............#..#.#.........#.#.....#......#..#.....................#....",
                        "#..#.###...##.......####.#.....###.#..#..###..##..#..#.####............##..#.........#..#.###........##......................####." };
                for (int i = 0; i < ALPHABET.Length; i++)
                {
                    int lMatchPixels = 0;
                    for (int lCol = 0; lCol < LETTER_WIDTH; lCol++)
                    {
                        for (int lRow = 0; lRow < LETTER_HEIGHT; lRow++)
                        {
                            bool lThisPixel = LETTER_PIXELS[lRow][lCol + i * LETTER_WIDTH] == '#';
                            if (aPaper[aCol + lCol, lRow] == lThisPixel) lMatchPixels++;
                        }
                    }
                    if (lMatchPixels == LETTER_WIDTH * LETTER_HEIGHT) return ALPHABET[i].ToString();
                }
                return " ";
            }

        }
    }
}
