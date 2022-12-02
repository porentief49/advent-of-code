using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Puzzles
{
    public partial class Year2021
    {
        public class Day23 : DayBase_OLD
        {
            protected override string Title { get; } = "Day 23 - Amphipod";

            public override void Init() => Init(Inputs_2021.Rainer_23);

            public override void Init(string aResource) => Input = Tools.SplitLines(aResource, true);

            public override string SolvePuzzle(bool aPart1)
            {
                return ""; // not yet implemented
            }
        }
    }
}
