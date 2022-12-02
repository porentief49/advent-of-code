using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Puzzles
{
    public partial class Year2021
    {
        public class Day13 : DayBase_OLD
        {
            protected override string Title { get; } = "Day 13 - Transparent Origami";

            public override void Init() => Init(Inputs_2021.Rainer_13);

            public override void Init(string aResource) => Input = Tools.SplitLines(aResource, true);

            public override string SolvePuzzle(bool aPart1)
            {
                const int LETTER_WIDTH = 5;
                const int LETTER_HEIGHT = 6;

                //create paper
                int lLine = 0;
                string[] lSplit;
                var lCoords = new List<(int X, int Y)>();
                while (Input[lLine][0] != 'f')
                {
                    lSplit = Input[lLine++].Split(',');
                    lCoords.Add((int.Parse(lSplit[0]), int.Parse(lSplit[1])));
                }
                bool[,] lPaper = new bool[lCoords.Max(i => i.X) + 1, lCoords.Max(i => i.Y) + 1];
                foreach ((int X, int Y) lCoord in lCoords) lPaper[lCoord.X, lCoord.Y] = true;
                if (Verbose) PrintPaper(lPaper);

                //now fold
                int lFoldCount = 0;
                while (aPart1 ? (lFoldCount == 0) : (lLine < Input.Length))
                {
                    if (Verbose) Console.WriteLine($"\n{Input[lLine]}");
                    lSplit = Input[lLine].Replace("=", " ").Split(' ');
                    int lFoldMark = int.Parse(lSplit[3]);
                    bool[,] lPaperNew;
                    if (lSplit[2] == "y")
                    {
                        lPaperNew = new bool[lPaper.GetLength(0), lFoldMark];
                        for (int lX = 0; lX < lPaperNew.GetLength(0); lX++)
                        {
                            for (int lY = 0; lY < lFoldMark; lY++)
                            {
                                int lFromY = lFoldMark * 2 - lY;
                                if (lFromY < lPaper.GetLength(1)) lPaperNew[lX, lY] = lPaper[lX, lY] || lPaper[lX, lFromY];
                                else lPaperNew[lX, lY] = lPaper[lX, lY];
                            }
                        }
                    }
                    else
                    {
                        lPaperNew = new bool[lFoldMark, lPaper.GetLength(1)];
                        for (int lX = 0; lX < lFoldMark; lX++)
                        {
                            for (int lY = 0; lY < lPaperNew.GetLength(1); lY++)
                            {
                                int lFromX = lFoldMark * 2 - lX;
                                if (lFromX < lPaper.GetLength(0)) lPaperNew[lX, lY] = lPaper[lX, lY] || lPaper[lFromX, lY];
                                else lPaperNew[lX, lY] = lPaper[lX, lY];
                            }
                        }
                    }
                    lPaper = lPaperNew;
                    if (Verbose) PrintPaper(lPaper);
                    lLine++;
                    lFoldCount++;
                }
                if (!aPart1) PrintPaper(lPaper);

                //count dots
                int lCount = 0;
                for (int lX = 0; lX < lPaper.GetLength(0); lX++) for (int lY = 0; lY < lPaper.GetLength(1); lY++) if (lPaper[lX, lY]) lCount++;
                if (aPart1) return FormatResult(lCount, "total dots");
                string lActivationCode = "";
                for (int i = 0; i < lPaper.GetLength(0) - LETTER_WIDTH + 1; i++) lActivationCode += GetLetter(lPaper, i);
                return FormatResult(lActivationCode, "activation code");

                void PrintPaper(bool[,] aPaper, string aFile = "")
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
                    if (aFile.Length == 0) Console.WriteLine(lSb.ToString());
                    else File.WriteAllText("c:\\temp\\" + aFile, lSb.ToString());
                }

                string GetLetter(bool[,] aPaper, int aCol)
                {
                    const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    string[] LETTER_PIXELS = {
                        ".##..###...##.......####.####..##..#..#..###...##.#..#.#...............##..###.......###...###......#..#................#...#.####.",
                        "#..#.#..#.#..#......#....#....#..#.#..#...#.....#.#.#..#..............#..#.#..#......#..#.#.........#..#................#...#....#.",
                        "#..#.###..#.........###..###..#....####...#.....#.##...#..............#..#.#..#......#..#.#.........#..#.................#.#....#..",
                        "####.#..#.#.........#....#....#.##.#..#...#.....#.#.#..#..............#..#.###.......###...##.......#..#..................#....#...",
                        "#..#.#..#.#..#......#....#....#..#.#..#...#..#..#.#.#..#..............#..#.#.........#.#.....#......#..#..................#...#....",
                        "#..#.###...##.......####.#.....###.#..#..###..##..#..#.####............##..#.........#..#.###........##...................#...####." };
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
                    return string.Empty;
                }
            }
        }
    }
}
