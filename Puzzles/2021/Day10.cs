using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puzzles
{
    public partial class Year2021
    {
        public class Day10 : DayBase
        {
            protected override string Title { get; } = "Day 10 - Syntax Scoring";

            public override void Init() => Init(Inputs_2021.Rainer_10);

            public override void Init(string aResource) => Input = Tools.SplitLines(aResource, true);

            public override string SolvePuzzle(bool aPart1)
            {
                var ALL_OPENING = new char[] { '(', '[', '{', '<' };
                var ALL_CLOSING = new char[] { ')', ']', '}', '>' };
                var MATCH_OPENING_DICT = new Dictionary<char, char>();
                var MATCH_CLOSING_DICT = new Dictionary<char, char>();
                var SCORE1_DICT = new Dictionary<char, int>() { { ')', 3 }, { ']', 57 }, { '}', 1197 }, { '>', 25137 } };
                var SCORE2_DICT = new Dictionary<char, int>();
                for (int i = 0; i < ALL_OPENING.Length; i++)
                {
                    MATCH_OPENING_DICT.Add(ALL_CLOSING[i], ALL_OPENING[i]);
                    MATCH_CLOSING_DICT.Add(ALL_OPENING[i], ALL_CLOSING[i]);
                    SCORE2_DICT.Add(ALL_CLOSING[i], i + 1);
                }
                int lScore1 = 0;
                var lScores2 = new List<long>();
                for (int i = 0; i < Input.Length; i++)
                {
                    var lStack = new Stack<char>();
                    int lIndex = 0;
                    bool lGood = true;
                    char lPop;
                    do
                    {
                        if (ALL_OPENING.Contains(Input[i][lIndex])) lStack.Push(Input[i][lIndex]);
                        else if (ALL_CLOSING.Contains(Input[i][lIndex]))
                        {
                            lPop = lStack.Pop();
                            if (lPop != MATCH_OPENING_DICT[Input[i][lIndex]]) lGood = false;
                        }
                        lIndex++;
                    } while (lGood && lIndex < Input[i].Length);
                    if (aPart1)
                    {
                        if (Verbose) Console.WriteLine($"Line {i}: '{Input[i]}' -> {(lGood ? "ok" : Input[i][lIndex - 1].ToString())}");
                        if (!lGood) lScore1 += SCORE1_DICT[Input[i][lIndex - 1]];
                    }
                    else
                    {
                        long lThisScore2 = 0;
                        StringBuilder lRemaining = new StringBuilder();
                        if (lGood)
                        {
                            while (lStack.Any()) lRemaining.Append(MATCH_CLOSING_DICT[lStack.Pop()]);
                            for (int ii = 0; ii < lRemaining.Length; ii++) lThisScore2 = lThisScore2 * 5 + SCORE2_DICT[lRemaining[ii]];
                            lScores2.Add(lThisScore2);
                        }
                        if (Verbose) Console.WriteLine($"Line {i}: '{Input[i]}' -> {(lGood ? lRemaining + " - " + lThisScore2.ToString() : "corrupted")}");
                    }
                }
                if (aPart1) return FormatResult(lScore1, "score");
                else
                {
                    lScores2.Sort();
                    return FormatResult(lScores2[(lScores2.Count - 1) / 2], "score");
                }
            }
        }
    }
}
