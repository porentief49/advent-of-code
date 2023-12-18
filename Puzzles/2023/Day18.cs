using System.Net.Mime;
using System.Xml.Xsl;

namespace Puzzles {

    public partial class Year2023 {

        public class Day18 : DayBase {

            protected override string Title { get; } = "Day 18: Lavaduct Lagoon";

            public override void SetupAll() {
                AddInputFile(@"2023\18_Example.txt");
                AddInputFile(@"2023\18_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve() {
                if (Part2) return "";
                var digPlan = InputAsLines.Select(x => new Dig(x)).ToList();
                for (int i =1; i < digPlan.Count; i++) {
                    digPlan[i].ToX = digPlan[i-1].ToX+ digPlan[i].Dir switch { 'R' => 1, 'L' => -1, _ => 0 } * digPlan[i].Length;
                    digPlan[i].ToY = digPlan[i-1].ToY+digPlan[i].Dir switch { 'D' => 1, 'U' => -1, _ => 0 } * digPlan[i].Length;
                }

                // calculate volume (area integral)
                int area = 0;
                for (int i = 1; i < digPlan.Count; i++) area += digPlan[i].ToY * (digPlan[i].ToX - digPlan[i - 1].ToX);
                area += digPlan.Last().ToY * (digPlan[0].ToX - digPlan.Last().ToX);
                int length = digPlan.Select(d => d.Length).Sum();
                return (Math.Abs(area) + length / 2 + 1).ToString();
            }

            private class Dig {
                public char Dir;
                public int Length;
                public string Color;
                public int ToX = 0;
                public int ToY = 0;

                public Dig(string input) {
                    var split = input.Split(' ');
                    Dir = split[0][0];
                    Length = int.Parse(split[1]);
                    Color = split[2];
                }
            }
        }
    }
}
