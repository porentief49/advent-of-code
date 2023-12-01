using System;
using System.Linq;

namespace Puzzles {
    public partial class Year2021 {
        public class Day18 : DayBase_OLD {
            protected override string Title { get; } = "Day 18 - Snailfish";

            public override void Init() => Init(Inputs_2021.Rainer_18);

            public override void Init(string aResource) => Input = Tools.SplitLines(aResource, true);

            public override string SolvePuzzle(bool aPart1) => aPart1 ? PartA() : PartB();

            private string PartB() {
                var lPairStructure = new cPair();
                bool lStatus;
                long lMaxMagnitude = 0;
                long lMagnitude;
                for (int i = 0; i < Input.Length; i++) {
                    for (int ii = 0; ii < Input.Length; ii++) {
                        if (i != ii) {

                            // i + ii
                            string lExpression = "[" + Input[i] + "," + Input[ii] + "]";
                            do {
                                lPairStructure = ReadPairs(lExpression);
                                lStatus = TryExploding(lPairStructure);
                                lExpression = Flatten(lPairStructure);
                                if (!lStatus) {
                                    lPairStructure = ReadPairs(lExpression);
                                    lStatus = TrySplitting(lPairStructure);
                                    lExpression = Flatten(lPairStructure);
                                }
                            } while (lStatus);
                            lMagnitude = CalcMagnitude(lPairStructure);
                            if (Verbose) Console.WriteLine($"Mag {lMagnitude} for {lExpression}");
                            if (lMagnitude > lMaxMagnitude) lMaxMagnitude = lMagnitude;

                            // i + ii
                            lExpression = "[" + Input[ii] + "," + Input[i] + "]";
                            do {
                                lPairStructure = ReadPairs(lExpression);
                                lStatus = TryExploding(lPairStructure);
                                lExpression = Flatten(lPairStructure);
                                if (!lStatus) {
                                    lPairStructure = ReadPairs(lExpression);
                                    lStatus = TrySplitting(lPairStructure);
                                    lExpression = Flatten(lPairStructure);
                                }
                            } while (lStatus);
                            lMagnitude = CalcMagnitude(lPairStructure);
                            if (Verbose) Console.WriteLine($"Mag {lMagnitude} for {lExpression}");
                            if (lMagnitude > lMaxMagnitude) lMaxMagnitude = lMagnitude;
                        }
                    }
                }
                return FormatResult(lMaxMagnitude, "max magnitude");
            }

            private string PartA() {
                var lPairStructure = new cPair();
                bool lStatus;
                string lExpression = string.Empty;
                for (int i = 0; i < Input.Length; i++) {
                    if (Verbose) Console.WriteLine($"  {lExpression}");
                    if (Verbose) Console.WriteLine($"+ {Input[i]}");
                    if (i == 0) lExpression = Input[0];
                    else lExpression = "[" + lExpression + "," + Input[i] + "]";
                    do {
                        lPairStructure = ReadPairs(lExpression);
                        lStatus = TryExploding(lPairStructure);
                        lExpression = Flatten(lPairStructure);
                        if (!lStatus) {
                            lPairStructure = ReadPairs(lExpression);
                            lStatus = TrySplitting(lPairStructure);
                            lExpression = Flatten(lPairStructure);
                        }
                    } while (lStatus);
                    if (Verbose) Console.WriteLine($"= {lExpression}\n");
                }
                return FormatResult(CalcMagnitude(lPairStructure), "final sum");
            }

            public long CalcMagnitude(cPair aPair) {
                if (aPair.IsNumeric) return (long)aPair.Number;
                return 3 * CalcMagnitude(aPair.Left) + 2 * CalcMagnitude(aPair.Right);
            }

            public string Flatten(cPair aPair) {
                if (aPair is null) return "#";
                if (aPair.IsNumeric) return (aPair.Number ?? 0).ToString();
                return "[" + Flatten(aPair.Left) + "," + Flatten(aPair.Right) + "]";
            }

            public bool TryExploding(cPair aPair) {
                bool lStatus = false;
                const int EXPLOSION_LEVEL_MIN = 4;
                if (Verbose) Console.WriteLine($"Exploding: {aPair.Left.Number}, {aPair.Right.Number}, {aPair.Level}");
                if (aPair != null) {
                    if (!aPair.IsNumeric) {
                        if (aPair.Left.IsNumeric && aPair.Right.IsNumeric && aPair.Level >= EXPLOSION_LEVEL_MIN) {
                            if (aPair.Left.PrevNumber != null) aPair.Left.PrevNumber.Number += (int)aPair.Left.Number;
                            if (aPair.Right.NextNumber != null) aPair.Right.NextNumber.Number += (int)aPair.Right.Number;
                            aPair.Left = null;
                            aPair.Right = null;
                            aPair.Number = 0;
                            lStatus = true;
                        }
                    }
                    if (!lStatus) lStatus = TryExploding(aPair.Left);
                    if (!lStatus) lStatus = TryExploding(aPair.Right);
                }
                return lStatus;
            }

            public bool TrySplitting(cPair aPair) {
                bool lStatus = false;
                if (Verbose) Console.WriteLine($"Splitting: {aPair.Left.Number}, {aPair.Right.Number}, {aPair.Level}");
                if (aPair != null) {
                    if (aPair.IsNumeric) {
                        if (aPair.Number >= 10) {
                            var lNewPair = new cPair();
                            lNewPair.Number = aPair.Number / 2;
                            aPair.Left = lNewPair;
                            lNewPair = new cPair();
                            lNewPair.Number = aPair.Number - aPair.Left.Number;
                            aPair.Right = lNewPair;
                            aPair.Number = null;
                            lStatus = true;
                        }
                    }
                    if (!lStatus) lStatus = TrySplitting(aPair.Left);
                    if (!lStatus) lStatus = TrySplitting(aPair.Right);
                }
                return lStatus;
            }

            public cPair ReadPairs(string aExpression) {
                int lPos = 0;
                cPair lPreviousNumber = null;
                return Sub(aExpression, ref lPreviousNumber, ref lPos, 0);
            }

            public cPair Sub(string aExpression, ref cPair aPreviousNumber, ref int aPos, int aLevel) {
                var lPair = new cPair() { Level = aLevel };

                // check if a number
                if ("0123456789".Contains(aExpression[aPos])) {
                    int lNumSize = 1;
                    while ((aPos + lNumSize < aExpression.Length) && "0123456789".Contains(aExpression[aPos + lNumSize])) lNumSize++;
                    lPair.Number = int.Parse(aExpression.Substring(aPos, lNumSize));
                    if (Verbose) Console.WriteLine(" ".Repeat(aLevel * 4) + $"Found Number '{lPair.Number}' - Pos: {aPos}, Level: {aLevel}");
                    aPos += lNumSize;
                    if (aPreviousNumber != null) aPreviousNumber.NextNumber = lPair;
                    lPair.PrevNumber = aPreviousNumber;
                    aPreviousNumber = lPair;
                    return lPair;
                }

                // ok, not a number
                if (aExpression[aPos] == '[') {
                    if (Verbose) Console.WriteLine(" ".Repeat(aLevel * 4) + $"Starting Pair with '{aExpression[aPos]}' - Pos: {aPos}, Level: {aLevel}");
                    aPos++;
                } else Console.WriteLine(" ".Repeat(aLevel * 4) + $"Problem, found '{aExpression[aPos]}' but expected '[' - Pos: {aPos}, Level: {aLevel}");
                lPair.Left = Sub(aExpression, ref aPreviousNumber, ref aPos, aLevel + 1);
                if (aExpression[aPos] == ',') {
                    if (Verbose) Console.WriteLine(" ".Repeat(aLevel * 4) + $"Continuing Pair with '{aExpression[aPos]}' - Pos: {aPos}, Level: {aLevel}");
                    aPos++;
                } else Console.WriteLine(" ".Repeat(aLevel * 4) + $"Problem, found '{aExpression[aPos]}'  but expected ',' - Pos: {aPos}, Level: {aLevel}");
                lPair.Right = Sub(aExpression, ref aPreviousNumber, ref aPos, aLevel + 1);
                if (aExpression[aPos] == ']') {
                    if (Verbose) Console.WriteLine(" ".Repeat(aLevel * 4) + $"Closing Pair with '{aExpression[aPos]}' - Pos: {aPos}, Level: {aLevel}");
                    aPos++;
                } else Console.WriteLine(" ".Repeat(aLevel * 4) + $"Problem, found '{aExpression[aPos]}' but expected ']' - Pos: {aPos}, Level: {aLevel}");
                return lPair;
            }

            public class cPair {
                public cPair Left;
                public cPair Right;
                public cPair PrevNumber;
                public cPair NextNumber;
                public int Level = 0;
                public int? Number = null;

                public bool IsNumeric => Number != null;
            }
        }
    }
}
