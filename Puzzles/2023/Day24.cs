using System.Transactions;

namespace Puzzles {

    public partial class Year2023 {

        public class Day24 : DayBase {

            protected override string Title { get; } = "Day 24: Never Tell Me The Odds";

            public override void SetupAll() {
                //AddInputFile(@"2023\24_Example.txt");
                AddInputFile(@"2023\24_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve() {
                //const double rangeMin = 7;
                //const double rangeMax = 27;
                const double rangeMin = 200000000000000;
                const double rangeMax = 400000000000000;
                if (Part2) return "";
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
