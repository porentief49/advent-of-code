using System;

namespace Puzzles
{
    public partial class Year2022
    {
        public class Day03 : DayBase
        {
            protected override string Title { get; } = "Day 3: Rucksack Reorganization";

            public override void SetupAll()
            {
                AddInputFile(@"2022\03_Example.txt");
                AddInputFile(@"2022\03_rAiner.txt");
                AddInputFile(@"2022\03_SEGCC.txt");
            }

            public override void Init(string InputFile) => InputData = ReadFile(InputFile, true);

            public override string Solve(bool Part1)
            {
                List<PackItems> Rucksacks = new();
                for (int i = 0; i < InputData?.Length; i++) Rucksacks.Add(new(InputData[i]));
                int _prioritySum = 0;
                if (Part1)
                {
                    foreach (var _rucksack in Rucksacks) _prioritySum += _rucksack.RearrangementPriority();
                    return FormatResult(_prioritySum, "priority sum");
                }
                for (int i = 0; i < Rucksacks.Count; i += 3)
                {
                    var _intersect = Rucksacks[i].AllItems.Intersect(Rucksacks[i+1].AllItems.Intersect(Rucksacks[i+2].AllItems));
                    foreach(var _item in _intersect) _prioritySum += Day03.CalcPriority(_item);
                }
                return FormatResult(_prioritySum, "priority sum");
            }

            public static int CalcPriority(char Letter) => (int)Letter - ((int)Letter >= 97 ? 96 : 38);

            private class PackItems
            {
                public string AllItems;
                public string Compartment1;
                public string Compartment2;

                public PackItems(string PackList)
                {
                    AllItems = PackList;
                    Compartment1 = PackList.Substring(0, PackList.Length / 2);
                    Compartment2 = PackList.Substring(PackList.Length / 2, PackList.Length / 2);
                }

                public int RearrangementPriority()
                {
                    var _intersect = Compartment1.Intersect(Compartment2).ToList();
                    int _priority = 0;
                    foreach (var _item in _intersect) _priority += Day03.CalcPriority(_item);
                    return _priority;
                }
            }
        }
    }
}
