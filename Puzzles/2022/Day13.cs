using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
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
                //AddInputFile(@"2022\13_rAiner.txt");
                //AddInputFile(@"2022\13_SEGCC.txt");
            }

            public override void Init(string InputFile)
            {
                //Verbose = true;
                InputData = ReadFile(InputFile, true);
                _packets = InputData.Select(x => ParseIt(x)).ToList();
            }

            //private Packet ParseIt(string input, bool marker = false)
            private Packet ParseIt(string iiiii, bool marker = false)
            {
                string[] input = iiiii.Replace("[", ",[,").Replace("]", ",],").Split(',', StringSplitOptions.RemoveEmptyEntries);
                Packet entry = new(null, marker);
                Packet current = entry;
                if (input[0] != "[") throw new Exception("packet value needs to start with '['");
                for (int i = 1; i < input.Length; i++)
                {
                    switch (input[i]) // could be done more concise
                    {
                        case "[":
                            {
                                Packet addOne = new(current);
                                current.list.Add(addOne);
                                current = addOne;
                                break;
                            }

                        case "]":
                            current = current.parent;
                            break;
                        default:
                            {
                                Packet addInt = new(current);
                                addInt.integer = int.Parse(input[i]);
                                current.list.Add(addInt);
                                break;
                            }
                    }
                }
                return entry;
            }

            public override string Solve(bool part1)
            {
                //Verbose = true;
                //if (!part1) return "";
                if (part1)
                {
                    List<int> rightOrders = new();
                    for (int i = 0; i < _packets.Count / 2; i++)
                    {
                        //int result = Compare(_packets[i * 2], _packets[i * 2 + 1]);
                        int result = _packets[i * 2].CompareTo(_packets[i * 2 + 1]);
                        if (Verbose) Console.WriteLine($"Pair {i + 1} -> {result}");
                        if (result == 1) rightOrders.Add(i + 1);
                    }
                    return FormatResult(rightOrders.Aggregate((x, y) => x + y), "right order index product");
                }
                _packets.Add(ParseIt("[[2]]", true));
                _packets.Add(ParseIt("[[6]]", true));
                //bool sorted;
                //Packet temp;
                //int iteration = 0;
                //do
                //{
                //    if (Verbose) Console.WriteLine($"quick sort iteration {iteration++}");
                //    sorted = true;// assume best
                //    for (int i = 0; i < _packets.Count - 1; i++)
                //    {
                //        if (Verbose) Console.WriteLine($"packet index {i}");
                //        //if (Compare(_packets[i], _packets[i + 1]) < 0)
                //        if (_packets[i].CompareTo(_packets[i + 1]) < 0)
                //        {
                //            temp = _packets[i];
                //            _packets[i] = _packets[i + 1];
                //            _packets[i + 1] = temp;
                //            sorted = false;
                //        }
                //    }
                //} while (!sorted);
                _packets.Sort();
                _packets.Reverse();


                int retval = 1;
                for (int i = 0; i < _packets.Count; i++) if (_packets[i].marker) retval *= (i + 1);
                return FormatResult(retval, "marker index product");
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

                public int CompareTo(Packet? other)
                {
                    Packet left = this; //####################################
                    Packet right = other; //####################################
                    //if (Verbose) Console.WriteLine("  ".Repeat(level) + $"- Compare {left.Export()}vs {right.Export()}");
                    if ((left.integer > -1) && (right.integer > -1)) // both integers
                    {
                        if (left.integer < right.integer)
                        {
                            //if (Verbose) Console.WriteLine("  ".Repeat(level + 1) + "- Left side is smaller, so inputs are in the right order\r\n");
                            return 1; // 1 means right sequence
                        }
                        if (left.integer > right.integer)
                        {
                            //if (Verbose) Console.WriteLine("  ".Repeat(level + 1) + "- Right side is smaller, so inputs are not in the right order\r\n");
                            return -1; // -1 means wrong sequence
                        }
                    }
                    else if ((left.integer == -1) && (right.integer == -1)) // both lists
                    {
                        for (int i = 0; i < Math.Max(left.list.Count, right.list.Count); i++)
                        {
                            if ((i < left.list.Count) && (i < right.list.Count))
                            {
                                //int result = Compare(left.list[i], right.list[i], level + 1);
                                int result = left.list[i].CompareTo(right.list[i]); //####################################
                                if (result != 0) return result;
                            }
                            else
                            {
                                if (left.list.Count < right.list.Count)
                                {
                                    //if (Verbose) Console.WriteLine("  ".Repeat(level + 1) + "- Left side ran out of items, so inputs are in the right order\r\n");
                                    return 1;
                                }
                                else
                                {
                                    //if (Verbose) Console.WriteLine("  ".Repeat(level + 1) + "- Right side ran out of items, so inputs are not in the right order\r\n");
                                    return -1;
                                }
                            }
                        }
                        return 0; // 0 means no decision (yet), can't happen at the outermost call
                    }
                    else // mixed types
                    {
                        bool which = left.integer > -1;
                        Packet toReplace = which ? left : right;
                        Packet replacement = new(toReplace);
                        replacement.integer = toReplace.integer;
                        toReplace.list.Add(replacement);
                        toReplace.integer = -1;
                        //if (Verbose) Console.WriteLine("  ".Repeat(level + 1) + $"- Mixed types, convert {(which ? "left" : "right")} {replacement.Export()}to {toReplace.Export()}");
                        //return Compare(left, right, level + 1);
                        return left.CompareTo(right); //####################################
                    }
                    return 0;
                }

                public string Export() => integer > -1 ? $"{integer} " : $"[ {(list.Count == 0 ? "" : list.Select(x => x.Export()).Aggregate((x, y) => x + y))}] ";
            }
        }
    }
}
