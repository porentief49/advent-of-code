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
                if (Part1) {
                    var hands1 = InputAsLines.Select(i => new Hand1(i, "23456789TJQKA"));
                    var sorted = hands1.OrderBy(h => h).ToList();
                    return sorted.Select((s, i) => s.Bid * ((ulong)i + 1)).Aggregate((x, y) => x + y).ToString();
                } else {
                    var hands2 = InputAsLines.Select(i => new Hand2(i, "J23456789TQKA"));
                    var sorted = hands2.OrderBy(h => h).ToList();
                    return sorted.Select((s, i) => s.Bid * ((ulong)i + 1)).Aggregate((x, y) => x + y).ToString();
                }
            }

            private enum HandType { FiveOfAKind = 6, FourOfAKind = 5, FullHouse = 4, ThreeOfAKind = 3, TwoPair = 2, OnePair = 1, HighCard = 0 }

            private class Hand2 : IComparable {
                public string Cards;
                public List<string> CardPermutaions = new();
                public int[] CardStrengths;
                public ulong Bid;
                public HandType Type;
                //public HandType TypeJoker;

                public Hand2(string line, string strengths) {
                    var split = line.Split(' ');
                    Cards = split[0];
                    Bid = ulong.Parse(split[1]);
                    CardPermutaions.Add(Cards);
                    for (int i = 0; i < Cards.Length; i++) {
                        if (Cards[i] == 'J') {
                            var cardCount = CardPermutaions.Count; // cache as we change the colleciton
                            for (int ii = 0; ii < cardCount; ii++) {
                                var sb = new StringBuilder(CardPermutaions[ii]);
                                for (int iii = 2; iii <= 9; iii++) {
                                    sb[i] = iii.ToString()[0];
                                    CardPermutaions.Add(sb.ToString());
                                }
                                sb[i] = 'A';
                                CardPermutaions.Add(sb.ToString());
                                sb[i] = 'K';
                                CardPermutaions.Add(sb.ToString());
                                sb[i] = 'Q';
                                CardPermutaions.Add(sb.ToString());
                                sb[i] = 'T';
                                CardPermutaions.Add(sb.ToString());
                            }
                        }
                    }


                    // card strengths
                    CardStrengths = Cards.Select(c => strengths.IndexOf(c)).ToArray();


                    // hand type
                    Type = HandType.HighCard; // start with worst
                    foreach (var cards in CardPermutaions) {
                        //var distinct = cards.Distinct().ToList();
                        var counts = cards.Distinct().Select(d => cards.Where(c => c == d).Count()).ToList();
                        HandType thisType = HandType.HighCard;
                        if (counts[0] == 5) thisType = HandType.FiveOfAKind;
                        else if (counts.Any(c => c == 4)) thisType = HandType.FourOfAKind;
                        else if (counts.Count == 2) thisType = HandType.FullHouse;
                        //else if (counts.Count == 3 && counts.Any(c => c == 3)) thisType = HandType.ThreeOfAKind;
                        //else if (counts.Count == 3) thisType = HandType.TwoPair;
                        else if (counts.Count == 3) thisType = counts.Any(c => c == 3) ? HandType.ThreeOfAKind : HandType.TwoPair;
                        else if (counts.Count == 4) thisType = HandType.OnePair;
                        //else thisType = HandType.HighCard;
                        if (thisType > Type) Type = thisType;
                    }
                }

                public int CompareTo(object? incomingHand) {
                    Hand2 incoming = incomingHand as Hand2;
                    if (Type > incoming.Type) return 1;
                    if (Type < incoming.Type) return -1;
                    for (int i = 0; i < CardStrengths.Length; i++) {
                        if (CardStrengths[i] > incoming.CardStrengths[i]) return 1;
                        if (CardStrengths[i] < incoming.CardStrengths[i]) return -11;
                    }
                    return 0;
                }
            }

            private class Hand1 : IComparable {
                public string Cards;
                public int[] CardStrengths;
                public ulong Bid;
                public HandType Type;

                public Hand1(string line, string strengths) {
                    var split = line.Split(' ');
                    Cards = split[0];
                    Bid = ulong.Parse(split[1]);
                    CardStrengths = Cards.Select(c => strengths.IndexOf(c)).ToArray();

                    // hand type
                    var counts = Cards.Distinct().Select(d => Cards.Where(c => c == d).Count()).ToList();
                    if (counts[0] == 5) Type = HandType.FiveOfAKind;
                    else if (counts.Any(c => c == 4)) Type = HandType.FourOfAKind;
                    else if (counts.Count == 2) Type = HandType.FullHouse;
                    else if (counts.Count == 3) Type = counts.Any(c => c == 3) ? HandType.ThreeOfAKind : HandType.TwoPair;
                    else if (counts.Count == 4) Type = HandType.OnePair;
                    else Type = HandType.HighCard;
                }

                public int CompareTo(object? incomingHand) {
                    Hand1 incoming = incomingHand as Hand1;
                    if (Type > incoming.Type) return 1;
                    if (Type < incoming.Type) return -1;
                    for (int i = 0; i < CardStrengths.Length; i++) {
                        if (CardStrengths[i] > incoming.CardStrengths[i]) return 1;
                        if (CardStrengths[i] < incoming.CardStrengths[i]) return -11;
                    }
                    return 0;
                }
            }

        }
    }
}
