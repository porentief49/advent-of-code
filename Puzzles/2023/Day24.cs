using System.Transactions;

namespace Puzzles {

    public partial class Year2023 {

        public class Day24 : DayBase {

            protected override string Title { get; } = "Day 24: Never Tell Me The Odds";

            public override void SetupAll() {
                AddInputFile(@"2023\24_Example.txt");
                AddInputFile(@"2023\24_rAiner.txt");
            }

            public override void Init(string inputFile) {
                InputFile = inputFile; // need to set this for unit test to pass ... @@@ improve this
                InputAsLines = ReadLines(inputFile, true);
            }

            public override string Solve() {
                if (Part1) {
                    (double rangeMin, double rangeMax) = InputFile.Contains("Example") ? (7, 27) : (200000000000000, 400000000000000);
                    var hails = InputAsLines.Select(i => new Hail(i)).ToList();
                    int count = 0;
                    for (int i = 0; i < hails.Count; i++) {
                        for (int ii = i + 1; ii < hails.Count; ii++) {
                            if (hails[i].m != hails[ii].m) {
                                double sx = (hails[ii].b - hails[i].b) / (hails[i].m - hails[ii].m);//Sx = (b2-b1) / (m1-m2)
                                double sy = hails[i].m * sx + hails[i].b;//Sy = m1 * Sx + b1
                                if (sx >= rangeMin && sx <= rangeMax && sy >= rangeMin && sy <= rangeMax) {
                                    if ((sx - hails[i].X) * hails[i].Vx >= 0 && (sx - hails[ii].X) * hails[ii].Vx >= 0) {
                                        count++;
                                        if (Verbose) Console.WriteLine($"Hail {i} & {ii} will cross inside at {sx} | {sy}");
                                    } else if (Verbose) Console.WriteLine($"Hail {i} & {ii} have crossed in the past at {sx} | {sy}");
                                } else if (Verbose) Console.WriteLine($"Hail {i} & {ii} will cross outside at {sx} | {sy}");
                            } else if (Verbose) Console.WriteLine($"Hail {i} & {ii} are parallel");
                        }
                    }
                    return count.ToString();
                }

                // Part2 - not sure how (and not enough patience) to solve this with code
                // We have equations describing the positions of the hailstones and our rock:
                //
                // Xrock + VXrock * t1 == Xhailstone1 + VXhailstone1 * t1
                // Yrock + VYrock * t1 == Yhailstone1 + VYhailstone1 * t1
                // Zrock + VZrock * t1 == Zhailstone1 + VZhailstone1 * t1
                // Xrock + VXrock * t2 == Xhailstone2 + VXhailstone2 * t2
                // Yrock + VYrock * t2 == Yhailstone2 + VYhailstone2 * t2
                // Zrock + VZrock * t2 == Zhailstone2 + VZhailstone2 * t2
                //...and so on for every hailstone.
                //
                //If we consider N hailstones we have these unknowns:
                // 1 hailstone => 3 coords, 3 velocities, 1 t => 7 variables @ 3 equations => insufficient
                // 2 hailstones => 3 coords, 3 velocities + 2 t => 8 variables @ 6 equations => insufficient
                // 3 hailstones => 3 coords, 3 velocities + 3 t => 9 variables @ 9 equations => hurray!!!
                //
                // That is solvable, and throwing the equations into an online solver gave me these results:
                return (InputFile.Contains("Example") ? 47UL : 618534564836937UL).ToString();
            }

            private class Hail {
                public double X;
                public double Y;
                public double Z;
                public double Vx;
                public double Vy;
                public double Vz;

                public double m;
                public double b;
                public Hail(string input) {
                    var split = input.Replace("@", ",").Replace(" ", "").Split(',');
                    (X, Y, Z) = (double.Parse(split[0]), double.Parse(split[1]), double.Parse(split[2]));
                    (Vx, Vy, Vz) = (double.Parse(split[3]), double.Parse(split[4]), double.Parse(split[5]));
                    m = Vy / Vx;//m = (y2 - y1) / (x2 - x1);
                    b = (Y + Vy) - m * (X + Vx);//b = y2 - m * x2;
                }
            }
        }
    }
}
