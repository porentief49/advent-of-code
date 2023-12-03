using System;
using System.Text.RegularExpressions;

namespace Puzzles {
    public partial class Year2023 {
        public class Day03 : DayBase {

            protected override string Title { get; } = "Day 3: Gear Ratios";

            public override void SetupAll() {
                AddInputFile(@"2023\03_Example.txt");
                AddInputFile(@"2023\03_rAiner.txt");
            }

            public override void Init(string InputFile) => InputData = ReadFile(InputFile, true);

            public override string Solve(bool Part1) {
                List<Number> numbers = new();
                for (int row = 0; row < InputData.Length; row++) {
                    int start = 0;
                    bool inNumber = false;
                    int number = 0;
                    for (int col = 0; col < InputData[0].Length; col++) {
                        var character = InputData[row][col];
                        if (character >= '0' && character <= '9') {
                            if (inNumber) {
                                number = number * 10 + character - '0';
                            } else {
                                number = character - '0';
                                start = col;
                                inNumber = true;
                            }
                        } else {
                            if (inNumber) {
                                inNumber = false;
                                numbers.Add(new Number(number, row, start, col - 1));
                            }
                        }
                    }
                    if (inNumber) numbers.Add(new Number(number, row, start, InputData[0].Length));
                }
                if (Part1) return numbers.Where(n => IsNearSymbol(n)).Sum(n => n.Value).ToString();
                int sum = 0;
                for (int row = 0; row < InputData.Length; row++) {
                    for (int col = 0; col < InputData[0].Length; col++) {
                        if (InputData[row][col] == '*') {
                            var twoFactors = numbers.Where(n => Math.Abs(row - n.Row) <= 1 && col >= n.ColStart - 1 && col <= n.ColEnd + 1);
                            if (twoFactors.Count() == 2) sum+= twoFactors.First().Value * twoFactors.Last().Value;
                        }
                    }
                }
                return sum.ToString();
            }
            private bool IsNearSymbol(Number number) {
                bool isNearSymbol = false;
                for (int col = number.ColStart - 1; col <= number.ColEnd + 1; col++) {
                    if (CheckSymbol(number.Row - 1, col)) isNearSymbol = true;
                    if (CheckSymbol(number.Row + 1, col)) isNearSymbol = true;
                }
                if (CheckSymbol(number.Row, number.ColStart - 1)) isNearSymbol = true;
                if (CheckSymbol(number.Row, number.ColEnd + 1)) isNearSymbol = true;
                return isNearSymbol;

                bool CheckSymbol(int row, int col) {
                    if (row < 0 || row >= InputData.Length - 1 || col < 0 || col >= InputData[0].Length - 1) return false;
                    char c = InputData[row][col];
                    return (c != '.' && (c < '0' || c > '9'));
                }
            }

            private class Number {
                public int Value;
                public int Row;
                public int ColStart;
                public int ColEnd;

                public Number(int value, int row, int colStart, int colEnd) {
                    Value = value;
                    Row = row;
                    ColStart = colStart;
                    ColEnd = colEnd;
                }
            }
        }
    }
}
