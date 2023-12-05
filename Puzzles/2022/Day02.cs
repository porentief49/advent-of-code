using System;

namespace Puzzles {
    public partial class Year2022 {
        public class Day02 : DayBase {
            protected override string Title { get; } = "Day 2: Rock Paper Scissors";

            public override void SetupAll() {
                AddInputFile(@"2022\02_Example.txt");
                AddInputFile(@"2022\02_rAiner.txt");
                AddInputFile(@"2022\02_SEGCC.txt");
            }

            public override void Init(string InputFile) => InputAsLines = ReadLines(InputFile, true);

            public override string Solve(bool Part1) {
                List<(char They, char Me)> Rounds = new();
                for (int i = 0; i < InputAsLines?.Length; i++) Rounds.Add((InputAsLines[i][0], InputAsLines[i][2]));
                int _totalScore = 0;
                foreach (var _round in Rounds) _totalScore += Part1 ? Play1(_round) : Play2(_round);
                return FormatResult(_totalScore, "total score");
            }

            private int Play1((char They, char Me) Round) {
                int _forShape;
                int _outcome;
                switch (Round.Me) {
                    case 'X': // Rock
                        _forShape = 1;
                        _outcome = Round.They switch { 'A' => 3, 'B' => 0, 'C' => 6, _ => throw new Exception($"symbol for their draw {Round.They} unknown") };
                        break;
                    case 'Y': // Paper
                        _forShape = 2;
                        _outcome = Round.They switch { 'A' => 6, 'B' => 3, 'C' => 0, _ => throw new Exception($"symbol for their draw {Round.They} unknown") };
                        break;
                    case 'Z': // Scissors
                        _forShape = 3;
                        _outcome = Round.They switch { 'A' => 0, 'B' => 6, 'C' => 3, _ => throw new Exception($"symbol for their draw {Round.They} unknown") };
                        break;
                    default:
                        throw new Exception($"symbol for my draw {Round.Me} unknown");
                }
                return _forShape + _outcome;
            }

            private int Play2((char They, char Me) Round) {
                int _forShape;
                int _outcome;
                switch (Round.Me) {
                    case 'X': // loose
                        _outcome = 0;
                        _forShape = Round.They switch { 'A' => 3, 'B' => 1, 'C' => 2, _ => throw new Exception($"symbol for their draw {Round.They} unknown") };
                        break;
                    case 'Y': // draw
                        _outcome = 3;
                        _forShape = Round.They switch { 'A' => 1, 'B' => 2, 'C' => 3, _ => throw new Exception($"symbol for their draw {Round.They} unknown") };
                        break;
                    case 'Z': // win
                        _outcome = 6;
                        _forShape = Round.They switch { 'A' => 2, 'B' => 3, 'C' => 1, _ => throw new Exception($"symbol for their draw {Round.They} unknown") };
                        break;
                    default:
                        throw new Exception($"symbol for my draw {Round.Me} unknown");
                }
                return _forShape + _outcome;
            }
        }
    }
}
