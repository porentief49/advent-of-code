using System;
using System.Collections.Generic;
using System.Linq;

namespace Puzzles {
    public partial class Year2021 {
        public class Day08 : DayBase_OLD { //@@@ speed up - part 2 takes 2.5 seconds!
            protected override string Title { get; } = "Day 8 - Seven Segment Search";

            public override void Init() => Init(Inputs_2021.Rainer_08);

            public override void Init(string aResource) => Input = Tools.SplitLines(aResource, true);

            public override string SolvePuzzle(bool aPart1) {
                if (aPart1) {
                    int[] lLengths = new int[8];
                    foreach (string lLine in Input) foreach (string lValue in lLine.Split('|')[1].Trim().Split(' ')) lLengths[lValue.Trim().Length]++;
                    return FormatResult(lLengths[2] + lLengths[3] + lLengths[4] + lLengths[7], $"digits 1, 4, 7 and 8 appear");
                } else {
                    const string MAPPING_FROM = "abcdefg";
                    string[] SEVEN_SEGMENTS = new string[] { "ABCEFG", "CF", "ACDEG", "ACDFG", "BCDF", "ABDFG", "ABDEFG", "ACF", "ABCDEFG", "ABCDFG" };
                    int lTotal = 0;
                    List<string> lPermutations = new List<string>();
                    Tools.FindAllPermutations(MAPPING_FROM.ToUpper(), ref lPermutations);
                    foreach (string lLinw in Input) {
                        foreach (string lMappingTo in lPermutations) {
                            string lMapped = lLinw;
                            for (int ii = 0; ii < MAPPING_FROM.Length; ii++) lMapped = lMapped.Replace(MAPPING_FROM[ii], lMappingTo[ii]);
                            string[] lLeftRight = lMapped.Split('|');
                            string[] lAllDigits = lLeftRight[0].Trim().Split(' ');
                            string[] lAllDigitsSorted = new string[lAllDigits.Length];
                            for (int ii = 0; ii < lAllDigits.Length; ii++) lAllDigitsSorted[ii] = lAllDigits[ii].SortCharacters();
                            bool lMatch = true; // assume best
                            for (int ii = 0; ii < lAllDigits.Length; ii++) if (!SEVEN_SEGMENTS.Contains(lAllDigitsSorted[ii])) lMatch = false;
                            if (lMatch) {
                                string[] lData = lLeftRight[1].Trim().Split(' ');
                                int lNumber = 0;
                                for (int ii = 0; ii < lData.Length; ii++) {
                                    int lDigit = Array.IndexOf(SEVEN_SEGMENTS, lData[ii].SortCharacters());
                                    if (lDigit > -1) lNumber = lNumber * 10 + lDigit;
                                    else throw new Exception("data could not be decoded - source data must be wrong");
                                }
                                if (Verbose) Console.WriteLine($"Number: {lNumber} using mapping {MAPPING_FROM} --> {lMappingTo}");
                                lTotal += lNumber;
                                break;
                            }
                        }
                    }
                    return FormatResult(lTotal, $"output value sum up to");
                }
            }
        }
    }
}
