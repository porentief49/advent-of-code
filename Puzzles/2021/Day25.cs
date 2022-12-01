using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Puzzles
{
    public partial class Year2021
    {
        public class Day25 : DayBase
        {
            protected override string Title { get; } = "Day 25 - Sea Cucumber";

            public override void Init() => Init(Inputs_2021.Rainer_25);

            public override void Init(string aResource) => Input = Tools.SplitLines(aResource, true);

            public override string SolvePuzzle(bool aPart1)
            {
                return ""; // not yet implemented
            }
        }
    }
}
