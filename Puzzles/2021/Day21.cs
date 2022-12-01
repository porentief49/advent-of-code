using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Puzzles
{
    public partial class Year2021
    {
        public class Day21 : DayBase
        {
            int mDeterDice = 1;

            protected override string Title { get; } = "Day 21 - Dirac Dice";

            public override void Init() => Init(Inputs_2021.Rainer_21);

            public override void Init(string aResource) => Input = Tools.SplitLines(aResource, true);

            public override string SolvePuzzle(bool aPart1)
            {
                int lPos1 = int.Parse(Input[0].Split(' ').Last());
                int lPos2 = int.Parse(Input[1].Split(' ').Last());
                if (aPart1) return DeterministicDice(lPos1, lPos2); // not yet implemented
                return QuantumDice(lPos1, lPos2);
            }

            private string DeterministicDice(int aPos1, int aPos2)
            {
                int lScore1 = 0;
                int lScore2 = 0;
                int lDice = 0;
                do
                {
                    if (RollDeterministic(ref aPos1, ref lScore1, ref lDice)) return FormatResult(lScore2 * lDice, $"die hass been rolled {lDice}x, player 2 lost with");
                    if (RollDeterministic(ref aPos2, ref lScore2, ref lDice)) return FormatResult(lScore1 * lDice, $"die hass been rolled {lDice}x, player 1 lost with");
                } while (lScore1 < 1000 && lScore2 < 1000);
                return "didn't complete"; // should not happen
            }

            private bool RollDeterministic(ref int aPos, ref int aScore, ref int aDice)
            {
                int lRoll = 0;
                lRoll += mDeterDice++;
                if (mDeterDice > 100) mDeterDice = 1;
                lRoll += mDeterDice++;
                if (mDeterDice > 100) mDeterDice = 1;
                lRoll += mDeterDice++;
                if (mDeterDice > 100) mDeterDice = 1;
                aPos = (aPos + lRoll) % 10;
                aDice += 3;
                if (aPos == 0) aPos = 10;
                aScore += aPos;
                return aScore >= 1000;
            }

            private string QuantumDice(int aPos1, int aPos2)
            {
                long[,,,] lBoards = new long[11, 31, 11, 31];
                lBoards[aPos1, 0, aPos2, 0] = 1;
                long lScore1 = 0;
                long lScore2 = 0;
                for (int i = 0; i < 20; i++)
                {
                    Play1(ref lBoards);
                    UpdateScore1(lBoards, ref lScore1);
                    Play2(ref lBoards);
                    UpdateScore2(lBoards, ref lScore2);
                }
                return FormatResult(Math.Max(lScore1, lScore2), $"player {(lScore1 > lScore2 ? 1 : 2)} wins with");
            }

            private void Play1(ref long[,,,] aBoards)
            {
                long[,,,] lNewBoards = new long[11, 31, 11, 31];
                int lNewPos;
                for (int lPos1 = 0; lPos1 <= 10; lPos1++)
                {
                    for (int lScore1 = 0; lScore1 <= 20; lScore1++)
                    {
                        for (int lPos2 = 0; lPos2 <= 10; lPos2++)
                        {
                            for (int lScore2 = 0; lScore2 <= 20; lScore2++)
                            {
                                lNewPos = GetNewPos(lPos1, 3);// case 3 (1x)
                                lNewBoards[lNewPos, lScore1 + lNewPos, lPos2, lScore2] += aBoards[lPos1, lScore1, lPos2, lScore2] * 1;
                                lNewPos = GetNewPos(lPos1, 4);// case 4 (3x)
                                lNewBoards[lNewPos, lScore1 + lNewPos, lPos2, lScore2] += aBoards[lPos1, lScore1, lPos2, lScore2] * 3;
                                lNewPos = GetNewPos(lPos1, 5);// case 5 (6x)
                                lNewBoards[lNewPos, lScore1 + lNewPos, lPos2, lScore2] += aBoards[lPos1, lScore1, lPos2, lScore2] * 6;
                                lNewPos = GetNewPos(lPos1, 6);// case 6 (7x)
                                lNewBoards[lNewPos, lScore1 + lNewPos, lPos2, lScore2] += aBoards[lPos1, lScore1, lPos2, lScore2] * 7;
                                lNewPos = GetNewPos(lPos1, 7);// case 7 (6x)
                                lNewBoards[lNewPos, lScore1 + lNewPos, lPos2, lScore2] += aBoards[lPos1, lScore1, lPos2, lScore2] * 6;
                                lNewPos = GetNewPos(lPos1, 8);// case 8 (3x)
                                lNewBoards[lNewPos, lScore1 + lNewPos, lPos2, lScore2] += aBoards[lPos1, lScore1, lPos2, lScore2] * 3;
                                lNewPos = GetNewPos(lPos1, 9);// case 9 (1x)
                                lNewBoards[lNewPos, lScore1 + lNewPos, lPos2, lScore2] += aBoards[lPos1, lScore1, lPos2, lScore2] * 1;
                            }
                        }
                    }
                }
                aBoards = lNewBoards;
            }

            private void Play2(ref long[,,,] aBoards)
            {
                long[,,,] lNewBoards = new long[11, 31, 11, 31];
                int lNewPos;
                for (int lPos2 = 0; lPos2 <= 10; lPos2++)
                {
                    for (int lScore2 = 0; lScore2 <= 20; lScore2++)
                    {
                        for (int lPos1 = 0; lPos1 <= 10; lPos1++)
                        {
                            for (int lScore1 = 0; lScore1 <= 20; lScore1++)
                            {
                                lNewPos = GetNewPos(lPos2, 3);// case 3 (1x)
                                lNewBoards[lPos1, lScore1, lNewPos, lScore2 + lNewPos] += aBoards[lPos1, lScore1, lPos2, lScore2] * 1;
                                lNewPos = GetNewPos(lPos2, 4);// case 4 (3x)
                                lNewBoards[lPos1, lScore1, lNewPos, lScore2 + lNewPos] += aBoards[lPos1, lScore1, lPos2, lScore2] * 3;
                                lNewPos = GetNewPos(lPos2, 5);// case 5 (6x)
                                lNewBoards[lPos1, lScore1, lNewPos, lScore2 + lNewPos] += aBoards[lPos1, lScore1, lPos2, lScore2] * 6;
                                lNewPos = GetNewPos(lPos2, 6);// case 6 (7x)
                                lNewBoards[lPos1, lScore1, lNewPos, lScore2 + lNewPos] += aBoards[lPos1, lScore1, lPos2, lScore2] * 7;
                                lNewPos = GetNewPos(lPos2, 7);// case 7 (6x)
                                lNewBoards[lPos1, lScore1, lNewPos, lScore2 + lNewPos] += aBoards[lPos1, lScore1, lPos2, lScore2] * 6;
                                lNewPos = GetNewPos(lPos2, 8);// case 8 (3x)
                                lNewBoards[lPos1, lScore1, lNewPos, lScore2 + lNewPos] += aBoards[lPos1, lScore1, lPos2, lScore2] * 3;
                                lNewPos = GetNewPos(lPos2, 9);// case 9 (1x)
                                lNewBoards[lPos1, lScore1, lNewPos, lScore2 + lNewPos] += aBoards[lPos1, lScore1, lPos2, lScore2] * 1;
                            }
                        }
                    }
                }
                aBoards = lNewBoards;
            }

            private void UpdateScore1(long[,,,] aBoards, ref long aScore)
            {
                for (int lPos1 = 0; lPos1 <= 10; lPos1++)
                {
                    for (int lScore1 = 21; lScore1 <= 30; lScore1++)
                    {
                        for (int lPos2 = 0; lPos2 <= 10; lPos2++)
                        {
                            for (int lScore2 = 0; lScore2 <= 20; lScore2++)
                            {
                                aScore += aBoards[lPos1, lScore1, lPos2, lScore2];
                                aBoards[lPos1, lScore1, lPos2, lScore2] = 0;
                            }
                        }
                    }
                }
            }

            private void UpdateScore2(long[,,,] aBoards, ref long aScore)
            {
                for (int lPos2 = 0; lPos2 <= 10; lPos2++)
                {
                    for (int lScore2 = 21; lScore2 <= 30; lScore2++)
                    {
                        for (int lPos1 = 0; lPos1 <= 10; lPos1++)
                        {
                            for (int lScore1 = 0; lScore1 <= 20; lScore1++)
                            {
                                aScore += aBoards[lPos1, lScore1, lPos2, lScore2];
                                aBoards[lPos1, lScore1, lPos2, lScore2] = 0;
                            }
                        }
                    }
                }
            }

            private int GetNewPos(int aThis, int aAdd)
            {
                int lNew = (aThis + aAdd) % 10;
                return (lNew == 0) ? 10 : lNew;
            }
        }
    }
}
