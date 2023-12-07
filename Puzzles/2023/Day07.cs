using System.ComponentModel.Design;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Puzzles {

    public partial class Year2023 {

        public class Day07 : DayBase {

            protected override string Title { get; } = "Day 7: ???";

            public override void SetupAll() {
                AddInputFile(@"2023\07_Example.txt");
                AddInputFile(@"2023\07_rAiner.txt");
            }

            public override void Init(string InputFile) => InputAsLines = ReadLines(InputFile, true);

            public override string Solve(bool Part1) {
                var hands = InputAsLines.Select(i => new Hand(i, Part1 ? "23456789TJQKA" : "J23456789TQKA", Part1));
                var sorted = hands.OrderBy(h => h).ToList();
                if (!Part1) foreach (var item in sorted) Console.WriteLine($"{item.Cards} - {item.Type}");


                return sorted.Select((s, i) => s.Bid * ((ulong)i + 1)).Aggregate((x, y) => x + y).ToString();
            }

            private enum HandType { FiveOfAKind = 6, FourOfAKind = 5, FullHouse = 4, ThreeOfAKind = 3, TwoPairs = 2, OnePair = 1, HighCard = 0 }

            private class Hand : IComparable {
                public string Cards;
                public int[] CardStrengths;
                public ulong Bid;
                public HandType Type;

                public Hand(string line, string strengths, bool part1) {
                    var split = line.Split(' ');
                    Cards = split[0];
                    Bid = ulong.Parse(split[1]);
                    CardStrengths = Cards.Select(c => strengths.IndexOf(c)).ToArray();
                    var diffCardCount = Cards.Distinct().Select(d => Cards.Where(c => c == d).Count()).ToList();
                    if (diffCardCount[0] == 5) Type = HandType.FiveOfAKind;
                    else if (diffCardCount.Any(c => c == 4)) Type = HandType.FourOfAKind;
                    else if (diffCardCount.Count == 2) Type = HandType.FullHouse;
                    else if (diffCardCount.Count == 3) Type = diffCardCount.Any(c => c == 3) ? HandType.ThreeOfAKind : HandType.TwoPairs;
                    else if (diffCardCount.Count == 4) Type = HandType.OnePair;
                    else Type = HandType.HighCard;
                    if (!part1) {
                        var jokers = Cards.Count(c => c == 'J');
                        if (jokers > 0) {
                            Type = Type switch {
                                HandType.HighCard => HandType.OnePair,
                                HandType.OnePair => HandType.ThreeOfAKind,
                                HandType.TwoPairs => jokers == 1 ? HandType.FullHouse : HandType.FourOfAKind,
                                HandType.ThreeOfAKind => HandType.FourOfAKind,
                                HandType.FullHouse => jokers == 1 ? HandType.FourOfAKind : HandType.FiveOfAKind,
                                HandType.FourOfAKind => HandType.FiveOfAKind,
                                HandType.FiveOfAKind => HandType.FiveOfAKind
                            };
                        }
                    }
                }

                public int CompareTo(object? incomingHand) {
                    Hand? incoming = incomingHand as Hand;
                    if (Type > incoming?.Type) return 1;
                    if (Type < incoming?.Type) return -1;
                    for (int i = 0; i < CardStrengths.Length; i++) {
                        if (CardStrengths[i] > incoming?.CardStrengths[i]) return 1;
                        if (CardStrengths[i] < incoming?.CardStrengths[i]) return -11;
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
