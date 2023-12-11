using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Puzzles {
    public abstract class DayBase {

        private const string _relativePath = @"..\..\..\..\InputData\";

        private List<string> _inputFiles = new();

        public bool BareOutput { get; set; } = false;

        public bool Verbose { get; set; } = false;

        public bool Part1 { get; set; }

        public bool Part2 => !Part1;

        protected abstract string Title { get; }

        protected string[] InputAsLines { get; set; } = new string[0];

        protected string InputAsText { get; set; } = string.Empty;

        protected string InputFile { get; set; } = string.Empty;

        protected void AddInputFile(string inputFile) => _inputFiles.Add(inputFile);

        public abstract void Init(string inputFile);

        public abstract string Solve();

        public abstract void SetupAll();

        public string RunAll() {
            SetupAll();
            string report = string.Empty;
            string result;
            report += Title + $" - RunAll on {_inputFiles.Count} input files\r\n";
            foreach (var inputFile in _inputFiles) {
                InputFile = inputFile;
                report += $"\r\n  File: {inputFile}\r\n";
                if (File.Exists(_relativePath + inputFile)) {
                    Stopwatch lWatch = Stopwatch.StartNew();
                    Init(inputFile);
                    report += $"    {lWatch.Elapsed.TotalSeconds.ToString("0.0000000", CultureInfo.InvariantCulture)}s Initialization\r\n";
                    Part1 = true;
                    lWatch.Restart();
                    result = Solve();
                    report += $"    {lWatch.Elapsed.TotalSeconds.ToString("0.0000000", CultureInfo.InvariantCulture)}s Part 1 ==> {result}\r\n";
                    Part1 = false;
                    lWatch.Restart();
                    result = Solve();
                    report += $"    {lWatch.Elapsed.TotalSeconds.ToString("0.0000000", CultureInfo.InvariantCulture)}s Part 2 ==> {result}\r\n";
                } else report += $"    not found!\r\n";
            }
            return report;
        }

        protected string FormatResult(object result, string label) => (BareOutput ? string.Empty : label + ": ") + result.ToString();

        protected string ReadText(string filePath, bool replaceCrLfWithLfOnly) {
            string raw = File.ReadAllText(_relativePath + filePath);
            return replaceCrLfWithLfOnly ? raw.Replace("\r", string.Empty) : raw;
        }

        protected string[] ReadLines(string filePath, bool removeEmptyLines) => ReadText(filePath, true).Split('\n', removeEmptyLines ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);

        protected static T[][] InitJaggedArray<T>(int dim1, int dim2, T initValue) {
            T[][] _grid = new T[dim1][];
            for (int i = 0; i < dim1; i++) {
                T[] _line = new T[dim2];
                for (int ii = 0; ii < dim2; ii++) _line[ii] = initValue;
                _grid[i] = _line;
            }
            return _grid;
        }

        protected static void PrintGrid(bool[][] grid) => Console.WriteLine(string.Join("\r\n", grid.Select(y => string.Concat(y.Select(x => x ? '#' : '.')))) + "\r\n");

        protected static void PrintGrid(char[][] grid) => Console.WriteLine(string.Join("\r\n", grid.Select(y => new string(y))) + "\r\n");

        protected static void PrintGrid(List<string> grid) => Console.WriteLine(string.Join("\r\n", grid) + "\r\n");

        protected static void PrintGrid(int[][] grid, int digits) => Console.WriteLine(string.Join("\r\n", grid.Select(y => string.Join(' ', y.Select(x => x.ToString() + " ".Repeat(digits).Substring(0, digits)).ToArray()))) + "\r\n");
    }

    public abstract class DayBase_OLD {
        public bool BareOutput { get; set; } = false;

        public bool Verbose { get; set; } = false;

        protected string[] Input { get; set; }

        protected abstract string Title { get; }

        public abstract void Init();

        public abstract void Init(string resource);

        public void InitFile(string file) => Init(File.ReadAllText(file));

        public abstract string SolvePuzzle(bool part1);

        public string RunBothAndReport(string aResource = "") {
            Stopwatch lWatch = Stopwatch.StartNew();
            if (aResource.Length == 0) Init();
            else Init(aResource);
            string lReport = Title + $"\r\nInitialization";
            lReport += $"\r\nin {lWatch.Elapsed.TotalMilliseconds.ToString("0.00", CultureInfo.InvariantCulture)}ms\r\n\r\nPart One\r\n==> ";
            lWatch.Restart();
            lReport += SolvePuzzle(true);
            lReport += $"\r\nin {lWatch.Elapsed.TotalMilliseconds.ToString("0.00", CultureInfo.InvariantCulture):0.000}ms\r\n\r\nPart Two\r\n==> ";
            lWatch.Restart();
            lReport += SolvePuzzle(false);
            lReport += $"\r\nin {lWatch.Elapsed.TotalMilliseconds.ToString("0.00", CultureInfo.InvariantCulture):0.000}ms";
            return lReport;
        }

        protected string FormatResult(object aResult, string aLabel) => (BareOutput ? string.Empty : aLabel + ": ") + aResult.ToString();
    }

    public static class Tools {
        public static void FindAllPermutations(string input, ref List<string> result, string prefix = "") {
            // credit: https://stackoverflow.com/questions/756055/listing-all-permutations-of-a-string-integer
            if (string.IsNullOrEmpty(input)) result.Add(prefix);
            for (int i = 0; i < input.Length; i++) FindAllPermutations(input.Remove(i, 1), ref result, prefix + input[i]);
        }

        public static string[] SplitLines(string resource, bool removeEmptyLines) {
            return resource.Replace("\r", string.Empty).Split(new char[] { '\n' }, StringSplitOptions.TrimEntries | (removeEmptyLines ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None));
        }

        public static void Repeat(int count, Action action) {
            for (int i = 0; i < count; i++) action();
        }

        // Extensions
        public static string SortCharacters(this string input) {
            char[] characters = input.ToArray();
            Array.Sort(characters);
            return new string(characters);
        }

        public static string Repeat(this string input, long count) {
            StringBuilder lOut = new StringBuilder();
            for (int i = 0; i < count; i++) lOut.Append(input);
            return lOut.ToString();
        }

        public static void AddIfNew<T>(this List<T> list, T add) {
            if (!list.Contains(add)) list.Add(add);
        }

        public static void AddIfNew<T>(this List<T> list, T[] add) {
            foreach (T lAdd in add) if (!list.Contains(lAdd)) list.Add(lAdd);
        }
    }
}
