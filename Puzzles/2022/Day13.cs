using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.VisualBasic;

namespace Puzzles
{
    public partial class Year2022
    {
        public class Day13 : DayBase
        {
            List<Packet> _packets;
            protected override string Title { get; } = "Day 13: Distress Signal";

            public override void SetupAll()
            {
                AddInputFile(@"2022\13_Example.txt");
                AddInputFile(@"2022\13_rAiner.txt");
                AddInputFile(@"2022\13_SEGCC.txt");
            }

            public override void Init(string InputFile) => _packets = ReadFile(InputFile, true).Select(x => Packet.Parse(x)).ToList();

            public override string Solve(bool part1)
            {
                if (part1)
                {
                    int rightOrders = 0;
                    for (int i = 0; i < _packets.Count / 2; i++)
                    {
                        int result = _packets[i * 2].CompareTo(_packets[i * 2 + 1]);
                        if (Verbose) Console.WriteLine($"Pair {i + 1} -> {result}");
                        if (result == -1) rightOrders+=(i + 1);
                    }
                    return FormatResult(rightOrders, "right order index sum");
                }
                _packets.AddRange((new string[] { "[[2]]", "[[6]]" }).Select(x => Packet.Parse(x, true)));
                _packets.Sort();
                int product = Enumerable.Range(0, _packets.Count).Where(x => _packets[x].marker).Aggregate((x, y) => (x+1) * (y+1));
                return FormatResult(product, "marker index product");
            }

            private class Packet : IComparable<Packet>
            {
                public Packet parent;
                public List<Packet> list = new();
                public int integer = -1;
                public bool marker;

                public Packet(Packet parent, bool marker = false)
                {
                    this.parent = parent;
                    this.marker = marker;
                }

                public static Packet Parse(string input, bool marker = false)
                {
                    string[] split = input.Replace("[", ",[,").Replace("]", ",],").Split(',', StringSplitOptions.RemoveEmptyEntries);
                    Packet entry = new(null, marker);
                    Packet current = entry;
                    if (split[0] != "[") throw new Exception("packet value needs to start with '['");
                    for (int i = 1; i < split.Length; i++)
                    {
                        if (split[i]=="]") current = current.parent;
                        else
                        {
                            Packet addOne = new(current);
                            if (split[i] == "[")
                            {
                                current.list.Add(addOne);
                                current = addOne;
                            }
                            else
                            {
                                addOne.integer = int.Parse(split[i]);
                                current.list.Add(addOne);
                            }
                        }
                    }
                    return entry;
                }

                public int CompareTo(Packet? other)
                {
                    if ((this.integer > -1) && (other.integer > -1)) // both integers
                    {
                        if (this.integer < other.integer) return -1; // -1 means right sequence
                        if (this.integer > other.integer)return 1; // 1 means wrong sequence
                    }
                    else if ((this.integer == -1) && (other.integer == -1)) // both lists
                    {
                        for (int i = 0; i < Math.Max(this.list.Count, other.list.Count); i++)
                        {
                            if ((i < this.list.Count) && (i < other.list.Count))
                            {
                                int result = this.list[i].CompareTo(other.list[i]); 
                                if (result != 0) return result;
                            }
                            else
                            {
                                if (this.list.Count < other.list.Count) return -1;
                                else return 1;
                            }
                        }
                        return 0; // 0 means no decision (yet), can't happen at the outermost call
                    }
                    else // mixed types
                    {
                        bool which = this.integer > -1;
                        Packet toReplace = which ? this : other;
                        Packet replacement = new(toReplace);
                        replacement.integer = toReplace.integer;
                        toReplace.list.Add(replacement);
                        toReplace.integer = -1;
                        return this.CompareTo(other); 
                    }
                    return 0;
                }

                public string Export() => integer > -1 ? $"{integer} " : $"[ {(list.Count == 0 ? "" : list.Select(x => x.Export()).Aggregate((x, y) => x + y))}] ";
            }
        }
    }
}



//private int Compare(Packet left, Packet right, int level = 0)
//{
//    if (Verbose) Console.WriteLine("  ".Repeat(level) + $"- Compare {left.Export()}vs {right.Export()}");
//    if ((left.integer > -1) && (right.integer > -1)) // both integers
//    {
//        if (left.integer < right.integer)
//        {
//            if (Verbose) Console.WriteLine("  ".Repeat(level + 1) + "- Left side is smaller, so inputs are in the right order\r\n");
//            return 1; // 1 means right sequence
//        }
//        if (left.integer > right.integer)
//        {
//            if (Verbose) Console.WriteLine("  ".Repeat(level + 1) + "- Right side is smaller, so inputs are not in the right order\r\n");
//            return -1; // -1 means wrong sequence
//        }
//    }
//    else if ((left.integer == -1) && (right.integer == -1)) // both lists
//    {
//        for (int i = 0; i < Math.Max(left.list.Count, right.list.Count); i++)
//        {
//            if ((i < left.list.Count) && (i < right.list.Count))
//            {
//                int result = Compare(left.list[i], right.list[i], level + 1);
//                if (result != 0) return result;
//            }
//            else
//            {
//                if (left.list.Count < right.list.Count)
//                {
//                    if (Verbose) Console.WriteLine("  ".Repeat(level + 1) + "- Left side ran out of items, so inputs are in the right order\r\n");
//                    return 1;
//                }
//                else
//                {
//                    if (Verbose) Console.WriteLine("  ".Repeat(level + 1) + "- Right side ran out of items, so inputs are not in the right order\r\n");
//                    return -1;
//                }
//            }
//        }
//        return 0; // 0 means no decision (yet), can't happen at the outermost call
//    }
//    else // mixed types
//    {
//        bool which = left.integer > -1;
//        Packet toReplace = which ? left : right;
//        Packet replacement = new(toReplace);
//        replacement.integer = toReplace.integer;
//        toReplace.list.Add(replacement);
//        toReplace.integer = -1;
//        if (Verbose) Console.WriteLine("  ".Repeat(level + 1) + $"- Mixed types, convert {(which ? "left" : "right")} {replacement.Export()}to {toReplace.Export()}");
//        return Compare(left, right, level + 1);
//    }
//    return 0;
//}

