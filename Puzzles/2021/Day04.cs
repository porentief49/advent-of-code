using System;
using System.Collections.Generic;

namespace Puzzles
{
    public partial class Year2021
    {
        public class Day04 : DayBase
        {
            const int BOARD_SIZE = 5;

            int[] mDrawNumbers;
            List<cBingoBoard> mBingoBoards = new List<cBingoBoard>();

            protected override string Title { get; } = "Day 4 - Giant Squid";

            public override void Init() => Init(Inputs_2021.Rainer_04);

            public override void Init(string aResource)
            {
                Input = Tools.SplitLines(aResource, true); //removing empty lines
                string[] lDraws = Input[0].Split(','); // first line
                mDrawNumbers = new int[lDraws.Length];
                for (int i = 0; i < lDraws.Length; i++) mDrawNumbers[i] = int.Parse(lDraws[i]);
                int lNextLine = 1; // bingo boards start at line 2
                do
                {
                    cBingoBoard lThisBingo = new cBingoBoard();
                    for (int i = 0; i < BOARD_SIZE; i++)
                    {
                        string[] lSplit = Input[lNextLine++].Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        for (int ii = 0; ii < BOARD_SIZE; ii++) lThisBingo.Fields[i, ii] = int.Parse(lSplit[ii]);
                    }
                    mBingoBoards.Add(lThisBingo);
                } while (lNextLine < Input.Length - 1);
            }

            public override string SolvePuzzle(bool aPart1)
            {
                int lScore = 0;
                int lProduct = 0;
                for (int i = 0; i < mDrawNumbers.Length; i++)
                {
                    foreach (cBingoBoard lBingoBoard in mBingoBoards)
                    {
                        if (lBingoBoard.Check(mDrawNumbers[i]))
                        {
                            if (Verbose) Console.WriteLine($"Bingo! number {mDrawNumbers[i]} at index {i}");
                            lScore = lBingoBoard.Score();
                            lProduct = lScore * mDrawNumbers[i];
                            if (aPart1)
                            {
                                if (Verbose) Console.WriteLine($"board score {lScore}");
                                return FormatResult(lProduct, "final score");
                            }
                        }
                    }
                }
                if (Verbose) Console.WriteLine($"board score {lScore}");
                return FormatResult(lProduct, "final score");
            }

            private class cBingoBoard
            {
                private bool mAlreadyDone = false;
                private int[,] mDrawns;
                public int[,] Fields;

                public cBingoBoard()
                {
                    Fields = new int[BOARD_SIZE, BOARD_SIZE];
                    mDrawns = new int[BOARD_SIZE, BOARD_SIZE];
                }

                public bool Check(int aNumber)
                {
                    if (!mAlreadyDone)
                    {
                        for (int i = 0; i < BOARD_SIZE; i++) for (int ii = 0; ii < BOARD_SIZE; ii++) if (Fields[i, ii] == aNumber) mDrawns[i, ii] = 1;
                        bool lBingo = false;
                        for (int i = 0; i < BOARD_SIZE; i++)
                        {
                            int lHorizontal = 0;
                            int lVertical = 0;
                            for (int ii = 0; ii < BOARD_SIZE; ii++)
                            {
                                lHorizontal += mDrawns[i, ii];
                                lVertical += mDrawns[ii, i];
                            }
                            if (lHorizontal == BOARD_SIZE || lVertical == BOARD_SIZE)
                            {
                                lBingo = true;
                                mAlreadyDone = true;
                                break;
                            }
                        }
                        return lBingo;
                    }
                    else return false;
                }

                public int Score()
                {
                    int lScore = 0;
                    for (int i = 0; i < BOARD_SIZE; i++) for (int ii = 0; ii < BOARD_SIZE; ii++) if (mDrawns[i, ii] == 0) lScore += Fields[i, ii];
                    return lScore;
                }
            }
        }
    }
}
