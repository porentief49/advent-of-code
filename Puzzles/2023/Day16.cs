using System.Collections;
using System.Data;
using Beam = (int Row, int Col, Puzzles.Year2023.Day16.Dir Dir);

namespace Puzzles {

    public partial class Year2023 {

        public class Day16 : DayBase {

            protected override string Title { get; } = "Day 16: The Floor Will Be Lava";

            public override void SetupAll() {
                AddInputFile(@"2023\16_Example.txt");
                AddInputFile(@"2023\16_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            [Flags]
            public enum Dir { None = 0, U = 1, R = 2, D = 4, L = 8 }

            public override string Solve() {
                if (Part1) return CalcEnergyLevel(0, -1, Dir.R).ToString();
                var rowMax = Enumerable.Range(0, InputAsLines.Length).Select(i => Math.Max(CalcEnergyLevel(i, -1, Dir.R), CalcEnergyLevel(i, InputAsLines[0].Length, Dir.L))).Max();
                var colMax = Enumerable.Range(0, InputAsLines[0].Length).Select(i => Math.Max(CalcEnergyLevel(-1, i, Dir.D), CalcEnergyLevel(InputAsLines.Length, i, Dir.U))).Max();
                return Math.Max(rowMax, colMax).ToString();
            }

            private int CalcEnergyLevel(int startRow, int startCol, Dir startDir) {
                Dir[][] energized = new Dir[InputAsLines.Length][];
                for (int i = 0; i < InputAsLines.Length; i++) energized[i] = new Dir[InputAsLines[0].Length];
                Queue<Beam> beams = new();
                beams.Enqueue((startRow, startCol, startDir));
                do {
                    if (Verbose) Console.Write($"Queue has {beams.Count} entries, ");
                    Beam beam = beams.Dequeue();
                    if (Verbose) Console.WriteLine($"dequeuing [{beam.Row}|{beam.Col}] going {beam.Dir}");
                    switch (beam.Dir) {
                        case Dir.U:
                            beam.Row--;
                            if (beam.Row < 0) break; // leave grid
                            if (Verbose) Console.WriteLine($"   to [{beam.Row}|{beam.Col}], finding character '{InputAsLines[beam.Row][beam.Col]}', energized with: {energized[beam.Row][beam.Col]}");
                            if ((energized[beam.Row][beam.Col] & beam.Dir) != Dir.None) break;
                            energized[beam.Row][beam.Col] |= beam.Dir;
                            switch (InputAsLines[beam.Row][beam.Col]) {
                                case '/':
                                    beams.Enqueue((beam.Row, beam.Col, Dir.R));
                                    break;
                                case '\\':
                                    beams.Enqueue((beam.Row, beam.Col, Dir.L));
                                    break;
                                case '-':
                                    beams.Enqueue((beam.Row, beam.Col, Dir.R));
                                    beams.Enqueue((beam.Row, beam.Col, Dir.L));
                                    break;
                                default:
                                    beams.Enqueue((beam.Row, beam.Col, beam.Dir));
                                    break;
                            }
                            break;
                        case Dir.R:
                            beam.Col++;
                            if (beam.Col == InputAsLines[0].Length) break; // leave grid
                            if (Verbose) Console.WriteLine($"   to [{beam.Row}|{beam.Col}], finding character '{InputAsLines[beam.Row][beam.Col]}', energized with: {energized[beam.Row][beam.Col]}");
                            if ((energized[beam.Row][beam.Col] & beam.Dir) != Dir.None) break;
                            energized[beam.Row][beam.Col] |= beam.Dir;
                            switch (InputAsLines[beam.Row][beam.Col]) {
                                case '/':
                                    beams.Enqueue((beam.Row, beam.Col, Dir.U));
                                    break;
                                case '\\':
                                    beams.Enqueue((beam.Row, beam.Col, Dir.D));
                                    break;
                                case '|':
                                    beams.Enqueue((beam.Row, beam.Col, Dir.U));
                                    beams.Enqueue((beam.Row, beam.Col, Dir.D));
                                    break;
                                default:
                                    beams.Enqueue((beam.Row, beam.Col, beam.Dir));
                                    break;
                            }
                            break;
                        case Dir.D:
                            beam.Row++;
                            if (beam.Row == InputAsLines.Length) break; // leave grid
                            if (Verbose) Console.WriteLine($"   to [{beam.Row}|{beam.Col}], finding character '{InputAsLines[beam.Row][beam.Col]}', energized with: {energized[beam.Row][beam.Col]}");
                            if ((energized[beam.Row][beam.Col] & beam.Dir) != Dir.None) break;
                            energized[beam.Row][beam.Col] |= beam.Dir;
                            switch (InputAsLines[beam.Row][beam.Col]) {
                                case '/':
                                    beams.Enqueue((beam.Row, beam.Col, Dir.L));
                                    break;
                                case '\\':
                                    beams.Enqueue((beam.Row, beam.Col, Dir.R));
                                    break;
                                case '-':
                                    beams.Enqueue((beam.Row, beam.Col, Dir.L));
                                    beams.Enqueue((beam.Row, beam.Col, Dir.R));
                                    break;
                                default:
                                    beams.Enqueue((beam.Row, beam.Col, beam.Dir));
                                    break;
                            }
                            break;
                        case Dir.L:
                            beam.Col--;
                            if (beam.Col < 0) break; // leave grid
                            if (Verbose) Console.WriteLine($"   to [{beam.Row}|{beam.Col}], finding character '{InputAsLines[beam.Row][beam.Col]}', energized with: {energized[beam.Row][beam.Col]}");
                            if ((energized[beam.Row][beam.Col] & beam.Dir) != Dir.None) break;
                            energized[beam.Row][beam.Col] |= beam.Dir;
                            switch (InputAsLines[beam.Row][beam.Col]) {
                                case '/':
                                    beams.Enqueue((beam.Row, beam.Col, Dir.D));
                                    break;
                                case '\\':
                                    beams.Enqueue((beam.Row, beam.Col, Dir.U));
                                    break;
                                case '|':
                                    beams.Enqueue((beam.Row, beam.Col, Dir.D));
                                    beams.Enqueue((beam.Row, beam.Col, Dir.U));
                                    break;
                                default:
                                    beams.Enqueue((beam.Row, beam.Col, beam.Dir));
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                } while (beams.Count > 0);
                int count = energized.Select(r => r.Where(c => c != Dir.None).Count()).Sum();
                return count;
            }
        }
    }
}
