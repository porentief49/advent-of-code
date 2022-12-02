using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Puzzles
{
    public partial class Year2021
    {
        public class Day22 : DayBase_OLD
        {
            protected override string Title { get; } = "Day 22 - Reactor Reboot";

            public override void Init() => Init(Inputs_2021.Rainer_22);

            public override void Init(string aResource) => Input = Tools.SplitLines(aResource, true);

            public override string SolvePuzzle(bool aPart1)
            {
                var lCubes = new List<cCube>();
                foreach (string lLine in Input)
                {
                    if (Verbose) Console.WriteLine($"\nProcessing: {lLine}");
                    const long PART1_IN_RANGE = 50;
                    string[] lSplit = lLine.Replace('=', ' ').Replace("..", " ").Replace(',', ' ').Split(' ');
                    var lCube = new cCube();
                    lCube.State = lSplit[0] == "on";
                    lCube.Xfrom = long.Parse(lSplit[2]);
                    lCube.Xto = long.Parse(lSplit[3]);
                    lCube.Yfrom = long.Parse(lSplit[5]);
                    lCube.Yto = long.Parse(lSplit[6]);
                    lCube.Zfrom = long.Parse(lSplit[8]);
                    lCube.Zto = long.Parse(lSplit[9]);
                    int lCubeCount = lCubes.Count;
                    if (lCube.State && (!aPart1 || lCube.InRange(PART1_IN_RANGE))) lCubes.Add(lCube);
                    for (int i = 0; i < lCubeCount; i++)
                    {
                        bool lOverlapX = Overlap(lCube.Xfrom, lCube.Xto, lCubes[i].Xfrom, lCubes[i].Xto, out long lOverlapXfrom, out long lOverlapXto);
                        bool lOverlapY = Overlap(lCube.Yfrom, lCube.Yto, lCubes[i].Yfrom, lCubes[i].Yto, out long lOverlapYfrom, out long lOverlapYto);
                        bool lOverlapZ = Overlap(lCube.Zfrom, lCube.Zto, lCubes[i].Zfrom, lCubes[i].Zto, out long lOverlapZfrom, out long lOverlapZto);
                        if (lOverlapX && lOverlapY && lOverlapZ)
                        {
                            var lOverlapCube = new cCube() { Xfrom = lOverlapXfrom, Xto = lOverlapXto, Yfrom = lOverlapYfrom, Yto = lOverlapYto, Zfrom = lOverlapZfrom, Zto = lOverlapZto, State = !lCubes[i].State };
                            lCubes.Add(lOverlapCube);
                        }
                    }
                    if (Verbose) Console.WriteLine($"Now we have {lCubes.Count} cubes");
                }
                long lOnTotal = 0;
                for (int i = 0; i < lCubes.Count; i++)
                {
                    long lAdd = (lCubes[i].Xto - lCubes[i].Xfrom + 1) * (lCubes[i].Yto - lCubes[i].Yfrom + 1) * (lCubes[i].Zto - lCubes[i].Zfrom + 1);
                    lOnTotal += lCubes[i].State ? lAdd : -lAdd;
                }
                return FormatResult(lOnTotal, "cubes on");
            }

            public static bool Overlap(long aFrom1, long aTo1, long aFrom2, long aTo2, out long aOverlapFrom, out long aOverlapTo)
            {
                // credit: https://stackoverflow.com/questions/36035074/how-can-i-find-an-overlap-between-two-given-ranges
                aOverlapFrom = Math.Max(aFrom1, aFrom2);
                aOverlapTo = Math.Min(aTo1, aTo2);
                return aOverlapFrom <= aOverlapTo;
            }

            public class cCube
            {
                public long Xfrom;
                public long Xto;
                public long Yfrom;
                public long Yto;
                public long Zfrom;
                public long Zto;
                public bool State; // on = true

                public bool InRange(long aRange)
                {
                    bool lXin = (Xfrom >= -aRange && Xfrom <= aRange) || (Xto >= -aRange && Xto <= aRange);
                    bool lYin = (Yfrom >= -aRange && Yfrom <= aRange) || (Yto >= -aRange && Yto <= aRange);
                    bool lZin = (Zfrom >= -aRange && Zfrom <= aRange) || (Zto >= -aRange && Zto <= aRange);
                    return lXin && lYin && lZin;
                }
            }
        }
    }
}
