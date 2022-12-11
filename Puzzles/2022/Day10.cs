using System;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
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
                // work the instructions
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
                    const int _startIndex = 20;
                    const int _IndexInc = 40;
                    int _index = _startIndex;
                    int _signalStrength = 0;
                    if (Verbose) for (int i = 1; i < _xRegValues.Count; i++) Console.WriteLine($"during cycle {i} xreg is {_xRegValues[i]}");
                    while (_index < _xRegValues.Count)
                    {
                        _signalStrength += _xRegValues[_index] * _index;
                        _index += _IndexInc;
                    }
                    return FormatResult(_signalStrength, "signal strength");
                }

                //Part 2
                const int _crtWidth = 40;
                const int _crtHeight = 6;
                const int _charCount = 8;
                bool[][] _crt = InitJaggedArray(_crtHeight, _crtWidth, false);
                for (int i = 1; i < _xRegValues.Count; i++)
                {
                    if (i - 1 < _crtHeight* _crtWidth)
                    {
                        int _horiz = (i - 1) % 40;
                        int _vert = (int)Math.Floor((i - 1) / (double)_crtWidth);
                        _crt[_vert][_horiz] = (_horiz >= _xRegValues[i] - 1 && _horiz <= _xRegValues[i] + 1);
                    }
                }
                if (!Verbose) PrintGrid(_crt);
                string _output = string.Empty;
                for (int i = 0; i < _charCount; i++) _output += GetLetter(_crt, i * 5);
                return FormatResult(_output, "CRT output");
            }

            string GetLetter(bool[][] Grid, int Column) // from 2021/13
            {
                const int _letterHeight = 5;
                const int _letterWidth = 6;
                const string _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                string[] LETTER_PIXELS = {
                        ".##..###...##.......####.####..##..#..#..###...##.#..#.#...............##..###.......###...###......#..#.....................####.",
                        "#..#.#..#.#..#......#....#....#..#.#..#...#.....#.#.#..#..............#..#.#..#......#..#.#.........#..#........................#.",
                        "#..#.###..#.........###..###..#....####...#.....#.##...#..............#..#.#..#......#..#.#.........#..#.......................#..",
                        "####.#..#.#.........#....#....#.##.#..#...#.....#.#.#..#..............#..#.###.......###...##.......#..#......................#...",
                        "#..#.#..#.#..#......#....#....#..#.#..#...#..#..#.#.#..#..............#..#.#.........#.#.....#......#..#.....................#....",
                        "#..#.###...##.......####.#.....###.#..#..###..##..#..#.####............##..#.........#..#.###........##......................####." };
                for (int i = 0; i < _alphabet.Length; i++)
                {
                    int lMatchPixels = 0;
                    for (int lCol = 0; lCol < _letterHeight; lCol++)
                    {
                        for (int lRow = 0; lRow < _letterWidth; lRow++)
                        {
                            bool lThisPixel = LETTER_PIXELS[lRow][lCol + i * _letterHeight] == '#';
                            if (Grid[lRow][Column + lCol] == lThisPixel) lMatchPixels++;
                        }
                    }
                    if (lMatchPixels == _letterHeight * _letterWidth) return _alphabet[i].ToString();
                }
                return " ";
            }
        }
    }
}
