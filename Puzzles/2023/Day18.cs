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
                var digPlan = InputAsLines.Select(x => new Dig(x, Part1)).ToList();
                for (int i = 1; i < digPlan.Count; i++) {
                    digPlan[i].ToX = digPlan[i - 1].ToX + digPlan[i].Dir switch { 'R' => 1, 'L' => -1, _ => 0 } * digPlan[i].Length;
                    digPlan[i].ToY = digPlan[i - 1].ToY + digPlan[i].Dir switch { 'D' => 1, 'U' => -1, _ => 0 } * digPlan[i].Length;
                }
                long area = 0;
                for (int i = 1; i < digPlan.Count; i++) area += digPlan[i].ToY * (digPlan[i].ToX - digPlan[i - 1].ToX);
                area += digPlan.Last().ToY * (digPlan[0].ToX - digPlan.Last().ToX);// area integratl -- again!
                long length = digPlan.Select(d => d.Length).Aggregate((x, y) => x + y);
                return (Math.Abs(area) + length / 2 + 1).ToString();
            }

            private class Dig {
                public char Dir;
                public long Length;
                public long ToX = 0;
                public long ToY = 0;

                public Dig(string input, bool part1) {
                    var split = input.Split(' ');
                    Dir = part1? split[0][0]: split[2][7] switch { '0' => 'R', '1' => 'D', '2' => 'L', '3' => 'U', _ => throw new Exception($"invalid dir") };
                    Length = part1? long.Parse(split[1]): Convert.ToInt64(split[2].Substring(2, 5), 16);
                }
            }
        }
    }
}
