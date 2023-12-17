using System.Collections;
using System.Data;

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
            private enum Dir { None = 0, U = 1, R = 2, D = 4, L = 8 }

            public override string Solve() {
                if (Part1) return CalcEnergyLevel(0, -1, Dir.R).ToString();
                var rowMax = Enumerable.Range(0, InputAsLines.Length).Select(i => Math.Max(CalcEnergyLevel(i, -1, Dir.R), CalcEnergyLevel(i, InputAsLines[0].Length, Dir.L))).Max();
                var colMax = Enumerable.Range(0, InputAsLines[0].Length).Select(i => Math.Max(CalcEnergyLevel(-1, i, Dir.D), CalcEnergyLevel(InputAsLines.Length, i, Dir.U))).Max();
                return Math.Max(rowMax, colMax).ToString();
            }

            private int CalcEnergyLevel(int startRow, int startCol, Dir startDir) {
                Dir[][] energized = new Dir[InputAsLines.Length][];
                for (int i = 0; i < InputAsLines.Length; i++) energized[i] = new Dir[InputAsLines[0].Length];
                Queue<(int, int, Dir)> beams = new();
                beams.Enqueue((startRow, startCol, startDir));
                do {
                    if (Verbose) Console.Write($"Queue has {beams.Count} entries, ");
                    (int row, int col, Dir dir) = beams.Dequeue();
                    if (Verbose) Console.WriteLine($"dequeuing [{row}|{col}] going {dir}");
                    switch (dir) {
                        case Dir.U:
                            row--;
                            if (row < 0) break; // leave grid
                            if (Verbose) Console.WriteLine($"   to [{row}|{col}], finding character '{InputAsLines[row][col]}', energized with: {energized[row][col]}");
                            if ((energized[row][col] & dir) != Dir.None) break;
                            energized[row][col] |= dir;
                            switch (InputAsLines[row][col]) {
                                case '/':
                                    beams.Enqueue((row, col, Dir.R));
                                    break;
                                case '\\':
                                    beams.Enqueue((row, col, Dir.L));
                                    break;
                                case '-':
                                    beams.Enqueue((row, col, Dir.R));
                                    beams.Enqueue((row, col, Dir.L));
                                    break;
                                default:
                                    beams.Enqueue((row, col, dir));
                                    break;
                            }
                            break;
                        case Dir.R:
                            col++;
                            if (col == InputAsLines[0].Length) break; // leave grid
                            if (Verbose) Console.WriteLine($"   to [{row}|{col}], finding character '{InputAsLines[row][col]}', energized with: {energized[row][col]}");
                            if ((energized[row][col] & dir) != Dir.None) break;
                            energized[row][col] |= dir;
                            switch (InputAsLines[row][col]) {
                                case '/':
                                    beams.Enqueue((row, col, Dir.U));
                                    break;
                                case '\\':
                                    beams.Enqueue((row, col, Dir.D));
                                    break;
                                case '|':
                                    beams.Enqueue((row, col, Dir.U));
                                    beams.Enqueue((row, col, Dir.D));
                                    break;
                                default:
                                    beams.Enqueue((row, col, dir));
                                    break;
                            }
                            break;
                        case Dir.D:
                            row++;
                            if (row == InputAsLines.Length) break; // leave grid
                            if (Verbose) Console.WriteLine($"   to [{row}|{col}], finding character '{InputAsLines[row][col]}', energized with: {energized[row][col]}");
                            if ((energized[row][col] & dir) != Dir.None) break;
                            energized[row][col] |= dir;
                            switch (InputAsLines[row][col]) {
                                case '/':
                                    beams.Enqueue((row, col, Dir.L));
                                    break;
                                case '\\':
                                    beams.Enqueue((row, col, Dir.R));
                                    break;
                                case '-':
                                    beams.Enqueue((row, col, Dir.L));
                                    beams.Enqueue((row, col, Dir.R));
                                    break;
                                default:
                                    beams.Enqueue((row, col, dir));
                                    break;
                            }
                            break;
                        case Dir.L:
                            col--;
                            if (col < 0) break; // leave grid
                            if (Verbose) Console.WriteLine($"   to [{row}|{col}], finding character '{InputAsLines[row][col]}', energized with: {energized[row][col]}");
                            if ((energized[row][col] & dir) != Dir.None) break;
                            energized[row][col] |= dir;
                            switch (InputAsLines[row][col]) {
                                case '/':
                                    beams.Enqueue((row, col, Dir.D));
                                    break;
                                case '\\':
                                    beams.Enqueue((row, col, Dir.U));
                                    break;
                                case '|':
                                    beams.Enqueue((row, col, Dir.D));
                                    beams.Enqueue((row, col, Dir.U));
                                    break;
                                default:
                                    beams.Enqueue((row, col, dir));
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
