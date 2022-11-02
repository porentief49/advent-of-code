using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Puzzles
{
    public partial class Year2021
    {
        public class Day20 : DayBase
        {
            protected override string Title { get; } = "Day 20 - Trench Map";

            public override void Init() => Init(Inputs.day20);

            public override void Init(string aResource) => Input = Tools.SplitLinesWithoutEmptyOnes(aResource);

            public override string SolvePuzzle(bool aPart1)
            {
                int lRuns = aPart1 ? 2 : 50;
                string lAlgorithm = Input[0];
                int lIndex = 1;
                var lImage = new List<string>();
                do lImage.Add(Input[lIndex++]);
                while (lIndex < Input.Length);
                if (Verbose) PrintImage(lImage, "Base Image:");
                lImage = ExpandImage(lImage, ".");
                for (int i = 0; i < lRuns; i++)
                {
                    var lImageNew = new List<string>();
                    for (int y = 1; y < lImage[0].Length - 1; y++)
                    {
                        var lSb = new StringBuilder();
                        for (int x = 1; x < lImage.Count - 1; x++)
                        {
                            int lLookUp = 0;
                            for (int yy = -1; yy <= 1; yy++) for (int xx = -1; xx <= 1; xx++) lLookUp = (lLookUp << 1) + (lImage[y + yy][x + xx] == '#' ? 1 : 0);
                            lSb.Append(lAlgorithm[lLookUp]);
                        }
                        lImageNew.Add(lSb.ToString());
                    }
                    lImage = ExpandImage(lImageNew, lImageNew[0][0].ToString());
                    if (Verbose) PrintImage(lImage, $"Step {i}");
                }
                return FormatResult(PixelCount(lImage), "pixel count");
            }

            public static List<string> ExpandImage(List<string> aImage, string aExpand)
            {
                var lImage = new List<string>();
                string lEmpty = aExpand.Repeat(aImage[0].Length + 4);
                Tools.Repeat(2, () => lImage.Add(lEmpty));
                foreach (string lLine in aImage) lImage.Add(aExpand.Repeat(2) + lLine + aExpand.Repeat(2));
                Tools.Repeat(2, () => lImage.Add(lEmpty));
                return lImage;
            }

            public static void PrintImage(List<string> aImage, string aTitle)
            {
                Console.WriteLine($"{aTitle}:");
                foreach (string lLine in aImage) Console.WriteLine(lLine);
                Console.WriteLine();
            }

            public static int PixelCount(List<string> aImage)
            {
                int lCount = 0;
                for (int y = 0; y < aImage.Count; y++) for (int x = 0; x < aImage[0].Length; x++) if (aImage[y][x] == '#') lCount++;
                return lCount;
            }
        }
    }
}
