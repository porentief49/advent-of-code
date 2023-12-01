using System;
using System.Collections.Generic;

namespace Puzzles {
    public partial class Year2021 {
        public class Day05 : DayBase_OLD {
            protected override string Title { get; } = "Day 5 - Hydrothermal Vents";

            public override void Init() => Init(Inputs_2021.Rainer_05);

            public override void Init(string aResource) => Input = Tools.SplitLines(aResource, true);

            public override string SolvePuzzle(bool aPart1) {
                List<cLine> lLines = new List<cLine>();
                int lMaxX = 0;
                int lMaxY = 0;
                foreach (string lItem in Input) lLines.Add(new cLine(lItem, ref lMaxX, ref lMaxY));
                int[,] lField = new int[lMaxX + 1, lMaxY + 1];
                foreach (cLine lLine in lLines) {
                    int lXinc = Math.Sign(lLine.X2 - lLine.X1);
                    int lYinc = Math.Sign(lLine.Y2 - lLine.Y1);
                    int lSteps = Math.Abs(lXinc != 0 ? lLine.X1 - lLine.X2 : lLine.Y1 - lLine.Y2);
                    if ((lXinc == 0) || (lYinc == 0) || !aPart1) {
                        for (int i = 0; i <= lSteps; i++) {
                            lField[lLine.X1, lLine.Y1]++;
                            lLine.X1 += lXinc;
                            lLine.Y1 += lYinc;
                        }
                    }
                }
                int lCount = 0;
                for (int lX = 0; lX < lMaxX; lX++) {
                    string lLine = string.Empty;
                    for (int lY = 0; lY < lMaxY; lY++) {
                        if (Verbose) lLine += lField[lY, lX].ToString().Replace("0", ".");
                        if (lField[lX, lY] >= 2) lCount++;
                    }
                    if (Verbose) Console.WriteLine(lLine);
                }
                return FormatResult(lCount, "overlaps");
            }

            public class cLine {
                public int X1;
                public int Y1;
                public int X2;
                public int Y2;

                public cLine(string aDefinition, ref int aMaxX, ref int aMaxY) {
                    string[] lSplit = aDefinition.Replace(" -> ", ",").Split(',');
                    X1 = int.Parse(lSplit[0]);
                    if (X1 > aMaxX) aMaxX = X1;
                    Y1 = int.Parse(lSplit[1]);
                    if (Y1 > aMaxY) aMaxY = Y1;
                    X2 = int.Parse(lSplit[2]);
                    if (X2 > aMaxX) aMaxX = X2;
                    Y2 = int.Parse(lSplit[3]);
                    if (Y2 > aMaxY) aMaxY = Y2;
                }
            }
        }
    }
}
