using System;
using System.Collections.Generic;
using System.Linq;

namespace Puzzles {
    public partial class Year2021 {
        public class Day03 : DayBase_OLD {
            protected override string Title { get; } = "Day 3 - Binary Diagnostic";

            public override void Init() => Init(Inputs_2021.Rainer_03);

            public override void Init(string aResource) => Input = Tools.SplitLines(aResource, true);

            public override string SolvePuzzle(bool aPart1) {
                if (aPart1) {
                    int[] lDiagnosticReport = new int[Input.Length];
                    for (int i = 0; i < Input.Length; i++) lDiagnosticReport[i] = Convert.ToInt32(Input[i], 2);
                    long lGammaRate = 0;
                    long lEpsilonRate = 0;
                    for (int i = 0; i < Input[i].Length; i++) {
                        int lCountOnes = 0;
                        long lMask = 1 << i;
                        for (int ii = 0; ii < lDiagnosticReport.Length; ii++) if ((lDiagnosticReport[ii] & lMask) != 0) lCountOnes++;
                        if (lCountOnes > (lDiagnosticReport.Length - lCountOnes)) lGammaRate |= lMask;
                        else lEpsilonRate |= lMask;
                    }
                    if (Verbose) Console.WriteLine($"gamma {lGammaRate} - epsilon {lEpsilonRate}");
                    return FormatResult(lGammaRate * lEpsilonRate, "power consumtion");
                } else {
                    List<string> lCurrentSelection;
                    List<string> lNumbersToKeep;
                    int lBitWidth = Input[0].Length;
                    int lPos = 0;

                    //oxy
                    lCurrentSelection = Input.ToList();
                    do {
                        lNumbersToKeep = new List<string>();
                        int lCountOnes = 0;
                        for (int i = 0; i < lCurrentSelection.Count; i++) if (lCurrentSelection[i][lPos] == '1') lCountOnes++;
                        char lSelect = (lCountOnes >= (lCurrentSelection.Count - lCountOnes) ? '1' : '0');
                        for (int i = 0; i < lCurrentSelection.Count; i++) if (lCurrentSelection[i][lPos] == lSelect) lNumbersToKeep.Add(lCurrentSelection[i]);
                        lPos++;
                        lCurrentSelection = lNumbersToKeep;
                    } while (lPos <= lBitWidth && lCurrentSelection.Count > 1);
                    long lOxygenGeneratorRating = Convert.ToInt64(lCurrentSelection[0], 2);

                    //co2
                    lPos = 0;
                    lCurrentSelection = Input.ToList();
                    do {
                        lNumbersToKeep = new List<string>();
                        int lCountOnes = 0;
                        for (int i = 0; i < lCurrentSelection.Count; i++) if (lCurrentSelection[i][lPos] == '1') lCountOnes++;
                        char lSelect = (lCountOnes >= (lCurrentSelection.Count - lCountOnes) ? '0' : '1');
                        for (int i = 0; i < lCurrentSelection.Count; i++) if (lCurrentSelection[i][lPos] == lSelect) lNumbersToKeep.Add(lCurrentSelection[i]);
                        lPos++;
                        lCurrentSelection = lNumbersToKeep;
                    } while (lPos <= lBitWidth && lCurrentSelection.Count > 1);
                    long lCo2ScrubberRating = Convert.ToInt64(lCurrentSelection[0], 2);
                    if (Verbose) Console.WriteLine($"oxy gen {lOxygenGeneratorRating} - co2 scrub {lCo2ScrubberRating}");
                    return FormatResult(lOxygenGeneratorRating * lCo2ScrubberRating, "life support");
                }
            }
        }
    }
}
