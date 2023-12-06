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

                if (Part1) {
                    int[] times = InputAsLines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(t => int.Parse(t)).ToArray();
                    int[] distances = InputAsLines[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(d => int.Parse(d)).ToArray();
                    int result = 1;
                    for (int race = 0; race < times.Count(); race++) {
                        int wincount = 0;
                        for (int chargeTime = 0; chargeTime <= times[race]; chargeTime++) {
                            int dist = chargeTime * (times[race] - chargeTime);
                            if (dist > distances[race]) wincount++;
                        }
                        result *= wincount;
                    }
                    return result.ToString();
                }

                // part 2 - instead of iterating through, now finding the zero point of the second-order function
                // distance = (totaltime - chargetime) * chargettime
                // - chargetime² + totaltime * chargetime - distance = 0
                // zero points of second order function: x12 = -(p/2) +- sqrt((p/2)² - q)
                // chargetime1 = (totaltime/2) - sqrt((totaltime/2)² - distance
                // chargetime2 = (totaltime/2) + sqrt((totaltime/2)² - distance
                // winningseconds is all integer numbers between those ranges
                double time = double.Parse(InputAsLines[0].Replace(" ", string.Empty).Split(':')[1]);
                double distance = double.Parse(InputAsLines[1].Replace(" ", string.Empty).Split(':')[1]);
                double chargetime1 = (time / 2) - Math.Sqrt(Math.Pow(time / 2, 2) - distance);
                double chargetime2 = (time / 2) + Math.Sqrt(Math.Pow(time / 2, 2) - distance);
                return (Math.Floor(chargetime2) - Math.Ceiling(chargetime1) + 1).ToString();
            }
        }
    }
}
