using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Puzzles
{
    public partial class Year2021
    {
        public class Day24 : DayBase
        {
            protected override string Title { get; } = "Day 24 - Arithmetic Logic Unit";

            public override void Init() => Init(Inputs_2021.Rainer_24);

            public override void Init(string aResource) => Input = Tools.SplitLinesWithoutEmptyOnes(aResource, true);

            public override string SolvePuzzle(bool aPart1)
            {
                return ""; // not yet implemented
            }
        }
    }
}
