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
                List<Row> data;
                data = InputAsLines.Select(i => new Row(i, Part1 ? 1 : 5)).ToList();
                return data.Select(d => d.CountArrangements()).Aggregate((x, y) => x + y).ToString();
            }


            public class Row {
                private bool _verbose = false;
                private string _damaged;
                private List<int> _contiguous;

                public Row(string input, int repeat) {
                    var split = input.Split(' ');
                    _damaged = $"{split[0]}?".Repeat(repeat - 1) + split[0];
                    _contiguous = ($"{split[1]},".Repeat(repeat - 1) + split[1]).Split(',').Select(c => int.Parse(c)).ToList();
                }

                public ulong CountArrangements() {

                    List<string> options = new();
                    List<string> newOptions;
                    List<ulong> counts = new List<ulong> { 1 };
                    List<ulong> newCounts;
                    options.Add(_damaged);

                    if (_verbose) Console.WriteLine($"Running '{_damaged}' for conts {string.Join(",", _contiguous)}");
                    do {

                        // trim (remove leading and trailing '.', and combine any consecutive '..' into single '.')
                        var trim = options.Select(o => Trim(o)).ToList();

                        // filter-out zero-size entries
                        var nonZeroIndices = Enumerable.Range(0, trim.Count).Where(i => trim[i].Length > 0).ToList();
                        options = nonZeroIndices.Select(i => trim[i]).ToList();
                        counts = nonZeroIndices.Select(i => counts[i]).ToList();
                        if (_verbose) Console.WriteLine($"  we have [{string.Join(", ", options)}] for conts {string.Join(",", _contiguous)}");

                        // combine equals into single entries & increase their counts
                        (options, counts) = Combine(options, counts);
                        if (_verbose) Console.WriteLine($"    after combination [{string.Join(", ", options)}] for conts {string.Join(",", _contiguous)}");

                        // try to resolve leftmost by inserting the '.###.' pattern into all possible locations, determine all valid options
                        newOptions = new();
                        newCounts = new();
                        int cont = _contiguous[0];
                        for (int i = 0; i < options.Count; i++) {
                            if (_verbose) Console.WriteLine($"    Testing option '{options[i]}' for conts {string.Join(",", _contiguous)}");
                            for (int ii = 0; ii <= options[i].Length - cont; ii++) {
                                (bool works, string result) = TestPos(options[i], cont, ii);
                                if (_verbose) Console.WriteLine($"      testing index {ii}, works= {works}, result '{result}'");
                                if (works) {
                                    newOptions.Add(result);
                                    newCounts.Add(counts[i]);
                                }
                            }
                        }

                        // trim & combine again
                        (newOptions, newCounts) = Combine(newOptions.Select(o => Trim(o)).ToList(), newCounts);


                        // decimate by taking off left-most (resolved) digits and the first entry in the contiguous list
                        options = newOptions.Select(o => o.Substring(cont)).ToList();
                        counts = newCounts;
                        if (_verbose) Console.WriteLine($"    after decimation: [{string.Join(", ", options)}]");
                        _contiguous.RemoveAt(0);

                    } while (_contiguous.Count > 0);
                    var count = Enumerable.Range(0, options.Count).Where(i => !options[i].Contains('#')).Select(i => counts[i]).Aggregate((x, y) => x + y);
                    if (_verbose) Console.WriteLine($"==> Result: {count}\r\n\r\n");
                    return count;
                }

                private (List<string> decOptions, List<ulong> decCounts) Combine(List<string> options, List<ulong> counts) {
                    List<string> decOptions = options.Distinct().ToList();
                    List<ulong> decCounts = decOptions.Select(o => Enumerable.Range(0, options.Count).Where(i => options[i] == o).Select(i => counts[i]).Aggregate((x, y) => x + y)).ToList();
                    return (decOptions, decCounts);
                }

                private string Trim(string input) {
                    while (input.Contains("..")) input = input.Replace("..", ".");
                    return input.Trim('.');
                }

                private (bool works, string result) TestPos(string option, int cont, int startPos) {
                    char[] opt = option.ToCharArray();
                    for (int i = 0; i < cont; i++) {
                        if (opt[startPos + i] == '.') return (false, "");
                        else opt[startPos + i] = '#';
                    }
                    for (int i = 0; i < startPos; i++) {
                        if (opt[i] == '#') return (false, "");
                        else opt[i] = '.';
                    }
                    if (startPos + cont < option.Length) {
                        if (opt[startPos + cont] == '#') return (false, "");
                        else opt[startPos + cont] = '.';
                    }
                    return (true, string.Join("", opt));
                }
            }
        }
    }
}
