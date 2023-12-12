using System.Linq;

namespace Puzzles {

    public partial class Year2023 {

        public class Day12 : DayBase {

            protected override string Title { get; } = "Day 12: Hot Springs";

            public override void SetupAll() {
                AddInputFile(@"2023\12_Example.txt");
                //AddInputFile(@"2023\12_Try.txt");
                AddInputFile(@"2023\12_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve() {
                Verbose = true;
                List<Row> data;
                data = InputAsLines.Select(i => new Row(i, Part1)).ToList();
                return data.Select(d => d.CountArrangements()).Aggregate((x, y) => x + y).ToString();
            }


            public class Row {
                private bool _verbose = false;
                public string Damaged;
                //public List<char> Repaired;
                public List<int> Contiguous;

                public Row(string input, bool part1) {
                    var split = input.Split(' ');
                    if (part1) {
                        Damaged = split[0];
                        Contiguous = split[1].Split(',').Select(c => int.Parse(c)).ToList();
                    } else {
                        Damaged = split[0] + "?" + split[0] + "?" + split[0] + "?" + split[0] + "?" + split[0];
                        Contiguous = (split[1] + "," + split[1] + "," + split[1] + "," + split[1] + "," + split[1]).Split(',').Select(c => int.Parse(c)).ToList();
                    }
                }

                public ulong CountArrangements() {

                    List<string> options = new();
                    List<string> newOptions;
                    List<ulong> counts = new List<ulong> { 1 };
                    List<ulong> newCounts;
                    options.Add(Damaged);

                    if (_verbose) Console.WriteLine($"Running '{Damaged}' for conts {string.Join(",", Contiguous)}");
                    do {

                        // step 1
                        //options = options.Select(o => Trim(o)).Where(o => o.Length > 0).ToList();
                        var trim = options.Select(o => Trim(o)).ToList();
                        var nonZeroIndices = Enumerable.Range(0, trim.Count).Where(i => trim[i].Length > 0).ToList();
                        options = nonZeroIndices.Select(i => trim[i]).ToList();
                        counts = nonZeroIndices.Select(i => counts[i]).ToList();


                        if (_verbose) Console.WriteLine($"  we have [{string.Join(", ", options)}] for conts {string.Join(",", Contiguous)}");

                        // combine
                        Combine(ref options, ref counts);
                        if (_verbose) Console.WriteLine($"    after combination [{string.Join(", ", options)}] for conts {string.Join(",", Contiguous)}");

                        // steps 2 and 3
                        newOptions = new();
                        newCounts = new();
                        int cont = Contiguous[0];
                        for (int i = 0; i < options.Count; i++) {
                            //string option = options[i];
                            if (_verbose) Console.WriteLine($"    Testing option '{options[i]}' for conts {string.Join(",", Contiguous)}");


                            // insert leftmost, get options
                            for (int ii = 0; ii <= options[i].Length - cont; ii++) {
                                (bool works, string result) = TestPos(options[i], cont, ii);
                                if (_verbose) Console.WriteLine($"      testing index {ii}, worked= {works}, result '{result}'");
                                if (works) {
                                    newOptions.Add(result);
                                    newCounts.Add(counts[i]);
                                }
                            }
                        }

                        // step 4
                        newOptions = newOptions.Select(o => Trim(o)).ToList();
                        if (_verbose) Console.WriteLine($"    new options: [{string.Join(", ", newOptions)}]");
                        //if (Contiguous.Count == 1) break;

                        // combine
                        Combine(ref options, ref counts);
                        if (_verbose) Console.WriteLine($"    after combination [{string.Join(", ", options)}] for conts {string.Join(",", Contiguous)}");

                        // step 5
                        options = newOptions.Select(o => o.Substring(cont)).ToList();
                        counts = newCounts;
                        if (_verbose) Console.WriteLine($"    after decimation: [{string.Join(", ", options)}]");
                        Contiguous.RemoveAt(0);

                    } while (Contiguous.Count > 0);
                    //var count = options.Where(o => !o.Contains('#')).Count();
                    var count = Enumerable.Range(0, options.Count).Where(i => !options[i].Contains('#')).Select(i => counts[i]).Aggregate((x, y) => x + y);
                    if (_verbose) Console.WriteLine($"==> Result: {count}\r\n\r\n");
                    return count;
                }

                private void Combine(ref List<string> options, ref List<ulong> counts) {
                    List<string> decOptions = options.Distinct().ToList();
                    List<ulong> decCounts = new ulong[decOptions.Count].ToList();
                    for (int i = 0; i < decOptions.Count; i++) {
                        for (int ii = 0; ii < options.Count; ii++) {
                            if (decOptions[i] == options[ii]) decCounts[i] += counts[ii];
                        }
                    }
                    options = decOptions;
                    counts = decCounts;
                }

                private string Trim(string input) {
                    //if (input == "....##.") {
                    //    Console.Write("");
                    //}

                    int length;
                    do {
                        length = input.Length;
                        input = input.Replace("..", ".");
                    } while (length > input.Length);
                    if (input.StartsWith(".")) input = input.Substring(1);
                    if (input.EndsWith(".")) input = input.Substring(0, input.Length - 1);
                    return input;
                }

                private (bool works, string result) TestPos(string option, int cont, int startPos) {
                    char[] opt = option.ToCharArray();
                    for (int i = 0; i < cont; i++) {
                        if (opt[startPos + i] == '.') return (false, "");
                        else opt[startPos + i] = '#';
                    }
                    for (int i = 0; i < startPos; i++) {
                        if (opt[i] == '#') return (false, "");
                        //if (opt[i] != '.') return (false, "");
                        else opt[i] = '.';
                    }


                    //if (startPos > 0) {
                    //    if (opt[startPos - 1] == '#') return (false, "");
                    //    else opt[startPos - 1] = '.';
                    //}
                    if (startPos + cont < option.Length) {
                        if (opt[startPos + cont] == '#') return (false, "");
                        else opt[startPos + cont] = '.';
                    }
                    return (true, string.Join("", opt));
                }
            }


            //public class Row {
            //    public List<char> Damaged;
            //    public List<char> Repaired;
            //    public string Contiguous;

            //    public Row(string input) {
            //        var split = input.Split(' ');
            //        Damaged = split[0].ToList();
            //        Repaired = split[0].ToList();
            //        Contiguous = split[1];
            //    }

            //    public int CountArrangements() {
            //        var positions = Enumerable.Range(0, Damaged.Count).Where(i => Damaged[i] == '?').ToList();
            //        int unknown = positions.Count;
            //        int arrangements = 0;
            //        //Console.WriteLine(string.Join("", Damaged) + "   " + Contiguous + "   " + unknown.ToString());
            //        for (ulong i = 0; i < Math.Pow(2, unknown); i++) {
            //            for (int ii = 0; ii < unknown; ii++) {
            //                Repaired[positions[ii]] = ((i >> ii) & 1) == 1 ? '#' : '.';
            //            }
            //            if (DetermineContiguous() == Contiguous) arrangements++;
            //        }
            //        //Console.WriteLine("  " + arrangements.ToString());
            //        return arrangements;
            //    }

            //    private string DetermineContiguous() {
            //        List<int> conts = new();
            //        int soFar = 0;
            //        for (int i = 0; i < Repaired.Count; i++) {
            //            if (Repaired[i] == '#') soFar++;
            //            else {
            //                if (soFar > 0) conts.Add(soFar);
            //                soFar = 0;
            //            }
            //        }
            //        if (soFar > 0) conts.Add(soFar);
            //        //Console.WriteLine("    " + string.Join("", Repaired) + "   " + string.Join(",", conts));
            //        return string.Join(",", conts);
            //    }
            //}
        }
    }

}
