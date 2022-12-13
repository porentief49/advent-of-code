using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.VisualBasic;

namespace Puzzles
{
    public partial class Year2022
    {
        public class Day13 : DayBase
        {
            private List<(Packet left, Packet right)> _pairs;

            protected override string Title { get; } = "Day 13: Distress Signal";

            public override void SetupAll()
            {
                AddInputFile(@"2022\13_Example.txt");
                AddInputFile(@"2022\13_rAiner.txt");
                AddInputFile(@"2022\13_SEGCC.txt");
            }

            public override void Init(string InputFile)
            {
                const int pairSize = 2;
                InputData = ReadFile(InputFile, true);
                _pairs = new();
                for (int i = 0; i < InputData.Length / (pairSize); i++)
                {
                    string[] left = InputData[i * pairSize].Replace("[", ",[,").Replace("]", ",],").Split(',', StringSplitOptions.RemoveEmptyEntries);
                    string[] right = InputData[i * pairSize + 1].Replace("[", ",[,").Replace("]", ",],").Split(',', StringSplitOptions.RemoveEmptyEntries);
                    //Console.WriteLine(string.Join(" ", left));
                    //Console.WriteLine(string.Join(" ", right));
                    //Console.WriteLine("");
                    _pairs.Add((ParseIt(left), ParseIt(right)));
                }
                //Console.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
                //foreach (var packet in _pairs)
                //{
                //    Console.WriteLine(packet.left.Export());
                //    Console.WriteLine(packet.right.Export());
                //    Console.WriteLine("");
                //}
            }

            private Packet ParseIt(string[] input)
            {
                Packet entry = new(null);
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
                if (part1)
                {
                    List<int> rightOders = new();

                    for (int i = 0; i < _pairs.Count; i++)
                    {
                        int result = Compare(_pairs[i].left, _pairs[i].right);
                        if (Verbose) Console.WriteLine($"Pair {i + 1} -> {result}");
                        if (result == 1) rightOders.Add(i + 1);
                    }

                    return FormatResult(rightOders.Aggregate((x, y) => x + y), "not yet implemented");
                }
                Packet[] _packets = new Packet[_pairs.Count * 2 + 2];
                for (int i = 0; i < _pairs.Count; i++)
                {
                    _packets[i * 2] = _pairs[i].left;
                    _packets[i * 2 + 1] = _pairs[i].right;
                }
                _packets[_pairs.Count*2] = ParseIt(new string[] { "[", "[", "2", "]", "]" });
                _packets[_pairs.Count * 2].marker = true;
                _packets[_pairs.Count *2+ 1] = ParseIt(new string[] { "[", "[", "6", "]", "]" });
                _packets[_pairs.Count * 2 + 1].marker = true;
                bool sorted;
                Packet temp;
                int iteration = 0;
                do
                {
                    //Console.WriteLine($"quick sort iteration {iteration++}");
                    sorted = true;// assume best
                    for (int i = 0; i < _packets.Length - 1; i++)
                    {
                        //Console.WriteLine($"packet indes {i}");
                        if (Compare(_packets[i], _packets[i + 1]) < 0)
                        {
                            temp = _packets[i];
                            _packets[i] = _packets[i + 1];
                            _packets[i + 1] = temp;
                            sorted = false;
                        }
                    }
                } while (!sorted);

                int retval = 1;
                for (int i = 0; i < _packets.Length; i++)
                {
                    //Console.WriteLine($"{i+1}: {(_packets[i].marker?"XXX":"   ")} {_packets[i].Export()}");
                    if (_packets[i].marker) retval *= (i + 1);
                }
                return FormatResult(retval, "xxx");
            }

            private int Compare(Packet left, Packet right, int level = 0)
            {
                if (Verbose) Console.WriteLine("  ".Repeat(level) + $"- Compare {left.Export()}vs {right.Export()}");
                if ((left.integer > -1) && (right.integer > -1))
                {
                    if (left.integer < right.integer) return 1;
                    if (left.integer > right.integer) return -1;
                }
                else if ((left.integer == -1) && (right.integer == -1))
                {
                    for (int i = 0; i < Math.Max(left.list.Count, right.list.Count); i++)
                    {
                        if ((i < left.list.Count) && (i < right.list.Count))
                        {
                            int result = Compare(left.list[i], right.list[i], level + 1);
                            if (result != 0) return result;
                        }
                        else
                        {
                            return (left.list.Count < right.list.Count ? 1 : -1);
                        }
                    }
                    return 0;
                }
                else
                {
                    Packet substitute;
                    if (left.integer > -1)
                    {
                        substitute = new(left);
                        substitute.integer = left.integer;
                        if (Verbose) Console.WriteLine("  ".Repeat(level + 1) + $"- Mixed types, convert left {left.Export()}to {substitute.Export()}");
                        left.list.Add(substitute);
                        left.integer = -1;
                        return Compare(left, right, level + 1);
                    }
                    else
                    {
                        substitute = new(right);
                        substitute.integer = right.integer;
                        if (Verbose) Console.WriteLine("  ".Repeat(level + 1) + $"- Mixed types, convert right {right.Export()}to {substitute.Export()}");
                        right.list.Add(substitute);
                        right.integer = -1;
                        return Compare(left, right, level + 1);
                    }
                }
                return 0;
            }

            private class Packet
            {
                public Packet parent;
                public List<Packet> list = new();
                public int integer = -1;
                public bool marker = false;

                public Packet(Packet parent)
                {
                    this.parent = parent;
                }

                public string Export()
                {
                    if (integer > -1) return integer.ToString() + " ";
                    return "[ " + (list.Count == 0 ? "" : list.Select(x => x.Export()).Aggregate((x, y) => x + y)) + "] ";
                }
            }
        }
    }
}
