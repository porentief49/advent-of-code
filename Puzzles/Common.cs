using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Puzzles
{
    public abstract class DayBase
    {
        public bool BareOutput { get; set; } = false;

        public bool Verbose { get; set; } = false;

        protected string[] Input { get; set; }

        protected abstract string Title { get; }

        public abstract void Init();

        public abstract void Init(string aResource);

        public void InitFile(string aFile) => Init(File.ReadAllText(aFile));

        public abstract string SolvePuzzle(bool aPart1);

        public string RunBothAndReport(string aResource = "")
        {
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

    public static class Tools
    {
        public static void FindAllPermutations(string aIn, ref List<string> lResult, string aPrefix = "")
        {
            // credit: https://stackoverflow.com/questions/756055/listing-all-permutations-of-a-string-integer
            if (string.IsNullOrEmpty(aIn)) lResult.Add(aPrefix);
            for (int i = 0; i < aIn.Length; i++) FindAllPermutations(aIn.Remove(i, 1), ref lResult, aPrefix + aIn[i]);
        }

        public static string[] SplitLines(string aResource, bool aRemoveEmptyLines)
        {
            return aResource.Replace("\r", string.Empty).Split(new char[] { '\n' }, StringSplitOptions.TrimEntries & (aRemoveEmptyLines ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None));
        }

        public static void Repeat(int aCount, Action aAction)
        {
            for (int i = 0; i < aCount; i++) aAction();
        }

        // Extensions
        public static string SortCharacters(this string aIn)
        {
            char[] characters = aIn.ToArray();
            Array.Sort(characters);
            return new string(characters);
        }

        public static string Repeat(this string aIn, long aCount)
        {
            StringBuilder lOut = new StringBuilder();
            for (int i = 0; i < aCount; i++) lOut.Append(aIn);
            return lOut.ToString();
        }

        public static void AddIfNew<T>(this List<T> aList, T aAdd)
        {
            if (!aList.Contains(aAdd)) aList.Add(aAdd);
        }

        public static void AddIfNew<T>(this List<T> aList, T[] aAdd)
        {
            foreach (T lAdd in aAdd) if (!aList.Contains(lAdd)) aList.Add(lAdd);
        }
    }
}
