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
                return sorted.Select((s, i) => s.Bid * ((ulong)i + 1)).Aggregate((x, y) => x + y).ToString();
            }

            private enum HandType { FiveOfAKind = 6, FourOfAKind = 5, FullHouse = 4, ThreeOfAKind = 3, TwoPair = 2, OnePair = 1, HighCard = 0 }

            private class Hand : IComparable {
                public string Cards;
                public List<string> CardPermutations = new();
                public int[] CardStrengths;
                public ulong Bid;
                public HandType Type;

                public Hand(string line, string strengths, bool part1) {
                    var split = line.Split(' ');
                    Cards = split[0];
                    Bid = ulong.Parse(split[1]);
                    CardStrengths = Cards.Select(c => strengths.IndexOf(c)).ToArray();
                    CardPermutations.Add(Cards);
                    if (!part1) {
                        for (int i = 0; i < Cards.Length; i++) {
                            if (Cards[i] == 'J') {
                                var cardCount = CardPermutations.Count; // cache as we change the colleciton
                                for (int ii = 0; ii < cardCount; ii++) CardPermutations.AddRange(strengths.Skip(1).Select(s => ReplaceChar(CardPermutations[ii], i, s)));
                            }
                        }
                    }
                    Type = HandType.HighCard; // assume worst
                    foreach (var cards in CardPermutations) {
                        var counts = cards.Distinct().Select(d => cards.Where(c => c == d).Count()).ToList();
                        HandType thisType = HandType.HighCard;
                        if (counts[0] == 5) thisType = HandType.FiveOfAKind;
                        else if (counts.Any(c => c == 4)) thisType = HandType.FourOfAKind;
                        else if (counts.Count == 2) thisType = HandType.FullHouse;
                        else if (counts.Count == 3) thisType = counts.Any(c => c == 3) ? HandType.ThreeOfAKind : HandType.TwoPair;
                        else if (counts.Count == 4) thisType = HandType.OnePair;
                        if (thisType > Type) Type = thisType;
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
