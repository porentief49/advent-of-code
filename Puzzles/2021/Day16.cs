using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puzzles
{
    public partial class Year2021
    {
        public class Day16 : DayBase
        {
            protected override string Title { get; } = "Day 16 - Packet Decoder";

            public override void Init() => Init(Inputs.day16);

            public override void Init(string aResource) => Input = Tools.SplitLinesWithoutEmptyOnes(aResource);

            public override string SolvePuzzle(bool aPart1)
            {
                string lBin = string.Join(string.Empty, Input[0].ToLower().Select(x => HEX_BIN_LOOKUP[x]));
                int lIndex = 0;
                long lVersionSum = 0;
                long lValue = ReadSubPacket(lBin, ref lIndex, ref lVersionSum);
                if (aPart1) return FormatResult(lVersionSum, "version sum"); ;
                return FormatResult(lValue, "full evaluation");
            }

            private readonly Dictionary<char, string> HEX_BIN_LOOKUP = new Dictionary<char, string> { { '0', "0000" }, { '1', "0001" }, { '2', "0010" }, { '3', "0011" },
                { '4', "0100" }, { '5', "0101" }, { '6', "0110" }, { '7', "0111" }, { '8', "1000" }, { '9', "1001" }, { 'a', "1010" }, { 'b', "1011" }, { 'c', "1100" },
                { 'd', "1101" }, { 'e', "1110" }, { 'f', "1111" } };

            private long ReadSubPacket(string aBin, ref int aIndex, ref long aVersionSum)
            {
                long lExpressionValue = 0;
                long lVersion = Convert.ToInt64(ReadNext(aBin, ref aIndex, 3), 2); // version
                aVersionSum += lVersion;
                long lType = Convert.ToInt64(ReadNext(aBin, ref aIndex, 3), 2); // type
                if (lType == 4)
                { //literal package
                    var lLiteralValue = new StringBuilder();
                    string lRead;
                    do
                    { // 5 bit chunks until [0] is 0
                        lRead = ReadNext(aBin, ref aIndex, 5);
                        lLiteralValue.Append(lRead.Substring(1, 4));
                    } while (lRead[0] == '1');
                    lExpressionValue = Convert.ToInt64(lLiteralValue.ToString(), 2);
                    if (Verbose) Console.WriteLine($"  Literal (Version {lVersion}, Type {lType}):  {lLiteralValue} -> {lExpressionValue}");
                }
                else
                {
                    string lLengthTypeId = ReadNext(aBin, ref aIndex, 1); // length if
                    long lLength = Convert.ToInt64(ReadNext(aBin, ref aIndex, (lLengthTypeId == "0" ? 15 : 11)), 2); // length in bits or size in packages
                    int lStartIndex = aIndex;
                    switch (lType)
                    {
                        case 0: //sum
                            lExpressionValue = ReadSubPacket(aBin, ref aIndex, ref aVersionSum);
                            if (lLengthTypeId == "0") while (aIndex < lStartIndex + lLength) lExpressionValue += ReadSubPacket(aBin, ref aIndex, ref aVersionSum);
                            else for (int i = 1; i < lLength; i++) lExpressionValue += ReadSubPacket(aBin, ref aIndex, ref aVersionSum);
                            break;
                        case 1: //product
                            lExpressionValue = ReadSubPacket(aBin, ref aIndex, ref aVersionSum);
                            if (lLengthTypeId == "0") while (aIndex < lStartIndex + lLength) lExpressionValue *= ReadSubPacket(aBin, ref aIndex, ref aVersionSum);
                            else for (int i = 1; i < lLength; i++) lExpressionValue *= ReadSubPacket(aBin, ref aIndex, ref aVersionSum);
                            break;
                        case 2: //minimum
                            lExpressionValue = ReadSubPacket(aBin, ref aIndex, ref aVersionSum);
                            if (lLengthTypeId == "0") while (aIndex < lStartIndex + lLength) lExpressionValue = Math.Min(lExpressionValue, ReadSubPacket(aBin, ref aIndex, ref aVersionSum));
                            else for (int i = 1; i < lLength; i++) lExpressionValue = Math.Min(lExpressionValue, ReadSubPacket(aBin, ref aIndex, ref aVersionSum));
                            break;
                        case 3: //maximum
                            lExpressionValue = ReadSubPacket(aBin, ref aIndex, ref aVersionSum);
                            if (lLengthTypeId == "0") while (aIndex < lStartIndex + lLength) lExpressionValue = Math.Max(lExpressionValue, ReadSubPacket(aBin, ref aIndex, ref aVersionSum));
                            else for (int i = 1; i < lLength; i++) lExpressionValue = Math.Max(lExpressionValue, ReadSubPacket(aBin, ref aIndex, ref aVersionSum));
                            break;
                        case 5: //greater than
                            lExpressionValue = ReadSubPacket(aBin, ref aIndex, ref aVersionSum) > ReadSubPacket(aBin, ref aIndex, ref aVersionSum) ? 1 : 0;
                            break;
                        case 6: //less than
                            lExpressionValue = ReadSubPacket(aBin, ref aIndex, ref aVersionSum) < ReadSubPacket(aBin, ref aIndex, ref aVersionSum) ? 1 : 0;
                            break;
                        case 7: //equal to
                            lExpressionValue = ReadSubPacket(aBin, ref aIndex, ref aVersionSum) == ReadSubPacket(aBin, ref aIndex, ref aVersionSum) ? 1 : 0;
                            break;
                    }
                    if (Verbose) Console.WriteLine($"  Operator (Version {lVersion}, Type {lType}):  {(lLengthTypeId == "0" ? 15 : 11)} bit type -> {lLength} {(lLengthTypeId == "0" ? "bits" : "sub packages")}");
                }
                if (Verbose) Console.WriteLine($"    Value {lExpressionValue}");
                return lExpressionValue;
            }

            private string ReadNext(string aBin, ref int aIndex, int aSize)
            {
                string lOut = aBin.Substring(aIndex, aSize);
                aIndex += aSize;
                return lOut;
            }
        }
    }
}
