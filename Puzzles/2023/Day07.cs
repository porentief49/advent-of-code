using System.ComponentModel.Design;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System;

namespace Puzzles {

    public partial class Year2023 {

        public class Day07 : DayBase {

            protected override string Title { get; } = "Day 7: ???";

            public override void SetupAll() {
                AddInputFile(@"2023\07_Example.txt");
                AddInputFile(@"2023\07_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve(bool part1) {
                var hands = InputAsLines.Select(i => new Hand(i, part1 ? "23456789TJQKA" : "J23456789TQKA", part1));
                var sorted = hands.OrderBy(h => h).ToList();
                return sorted.Select((s, i) => s.Bid * ((ulong)i + 1)).Aggregate((x, y) => x + y).ToString();
            }

            private class Hand : IComparable {
                public string Cards;
                public int[] CardStrengths;
                public ulong Bid;
                public int Type = 0; //FiveOfAKind = 6, FourOfAKind = 5, FullHouse = 4, ThreeOfAKind = 3, TwoPairs = 2, OnePair = 1, HighCard = 0

                public Hand(string line, string strengths, bool part1) {
                    var split = line.Split(' ');
                    Cards = split[0];
                    Bid = ulong.Parse(split[1]);
                    CardStrengths = Cards.Select(c => strengths.IndexOf(c)).ToArray();
                    var diffCardCount = Cards.Distinct().Select(d => Cards.Where(c => c == d).Count()).ToList();
                    if (diffCardCount[0] == 5) Type = 6;
                    else if (diffCardCount.Any(c => c == 4)) Type = 5;
                    else if (diffCardCount.Count == 2) Type = 4;
                    else if (diffCardCount.Count == 3) Type = diffCardCount.Any(c => c == 3) ? 3 : 2;
                    else if (diffCardCount.Count == 4) Type = 1;
                    if (!part1) {
                        var jokerCount = Cards.Count(c => c == 'J');
                        if (jokerCount > 0) Type = Type switch { 0 => 1, 1 => 3, 2 => jokerCount == 1 ? 4 : 5, 3 => 5, 4 => jokerCount == 1 ? 5 : 6, _ => 6 };
                    }
                }

                public int CompareTo(object? incomingHand) {
                    Hand? incoming = incomingHand as Hand;
                    int x = Type.CompareTo(incoming?.Type);
                    if (x != 0) return x;
                    for (int i = 0; i < CardStrengths.Length; i++) {
                        x = CardStrengths[i].CompareTo(incoming?.CardStrengths[i]);
                        if (x != 0) return x;
                    }
                    return 0;
                }

                private string ReplaceChar(string input, int index, char ch) {
                    var x = new StringBuilder(input);
                    x[index] = ch;
                    return x.ToString();
                }
            }
        }
    }
}
