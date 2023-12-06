using System.Text;

namespace Puzzles {

    public partial class Year2023 {

        public class Day06 : DayBase {

            protected override string Title { get; } = "Day 6: Wait For It";

            public override void SetupAll() {
                AddInputFile(@"2023\06_Example.txt");
                AddInputFile(@"2023\06_rAiner.txt");
            }

            public override void Init(string InputFile) => InputAsLines = ReadLines(InputFile, true);

            public override string Solve(bool Part1) {
                if (!Part1) return "";
                var times = InputAsLines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(t => int.Parse(t)).ToArray();
                var distances = InputAsLines[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(d => int.Parse(d)).ToArray();
                var result = 1;
                for (int race = 0; race < times.Count(); race++) {
                    var wincount = 0;
                    for (int chargeTime = 0; chargeTime <= times[race]; chargeTime++) {
                        var dist = chargeTime * (times[race] - chargeTime);
                        if (dist > distances[race]) {
                            //Console.WriteLine($"race {race}, chargeTime {chargeTime}, distance {dist}");
                            wincount++;
                        }
                    }
                    //Console.WriteLine(wincount);
                    result *= wincount;
                }
                return result.ToString();
            }
        }
    }
}
