﻿using System;
using System.Text;

namespace Puzzles {
    public partial class Year2022 {
        public class Day05 : DayBase {
            protected override string Title { get; } = "Day 5: Supply Stacks";

            public override void SetupAll() {
                AddInputFile(@"2022\05_Example.txt");
                AddInputFile(@"2022\05_rAiner.txt");
                AddInputFile(@"2022\05_SEGCC.txt");
            }

            public override void Init(string InputFile) => InputAsLines = ReadLines(InputFile, false);

            public override string Solve() {
                int _splitterRow = 0;
                int _stackCount = 0;

                //prepare stuff
                _splitterRow = Array.IndexOf(InputAsLines, string.Empty);
                _stackCount = InputAsLines[_splitterRow - 1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).Max();

                // build stacks
                Stack<char>[] _stacks = new Stack<char>[_stackCount];
                for (int i = 0; i < _stackCount; i++) _stacks[i] = new Stack<char>();
                for (int _row = _splitterRow - 2; _row >= 0; _row--) {
                    for (int ii = 0; ii < _stackCount; ii++) {
                        int _col = ii * 4 + 1;
                        if (InputAsLines[_row].Length > _col && InputAsLines[_row][_col] != ' ') _stacks[ii].Push(InputAsLines[_row][_col]);
                    }
                }

                // move crates
                for (int _row = _splitterRow + 1; _row < InputAsLines?.Length; _row++) {
                    if (InputAsLines[_row].Length > 0) {
                        string[] _split = InputAsLines[_row].Split(' ');
                        int _count = int.Parse(_split[1]);
                        int _from = int.Parse(_split[3]);
                        int _to = int.Parse(_split[5]);
                        if (Part1) for (int ii = 0; ii < _count; ii++) _stacks[_to - 1].Push(_stacks[_from - 1].Pop());
                        else {
                            Stack<char> _tempStack = new();
                            for (int ii = 0; ii < _count; ii++) _tempStack.Push(_stacks[_from - 1].Pop());
                            for (int ii = 0; ii < _count; ii++) _stacks[_to - 1].Push(_tempStack.Pop());
                        }
                    }
                }

                // read top stack item
                StringBuilder _sb = new();
                for (int i = 0; i < _stackCount; i++) _sb.Append(_stacks[i].Peek());
                return FormatResult(_sb.ToString(), "stack tops");
            }
        }
    }
}
