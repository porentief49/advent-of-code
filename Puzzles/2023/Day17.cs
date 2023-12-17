using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace Puzzles {

    public partial class Year2023 {

        public class Day17 : DayBase {

            protected override string Title { get; } = "Day 17: Clumsy Crucible";

            private enum Dir { None = 0, U = 1, R = 2, D = 4, L = 8 }

            public override void SetupAll() {
                AddInputFile(@"2023\17_Example.txt");
                //AddInputFile(@"2023\17_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve() {
                if (Part2) return "";
                Verbose = true;

                // initialize
                char[][] grid = InputAsLines.Select(i => i.ToCharArray()).ToArray();
                List<Node> open = new() { new Node(0, 0, Dir.None, 0, null) };
                List<Node> done = new();
                int row;
                int col;
                Node current;
                int newRow;
                int newCol;
                Dir newDir;

                // dijkstra
                int count = 0;
                Node bestSuccessPath = new Node(InputAsLines.Length - 1, InputAsLines[0].Length - 1, Dir.None, 99999, null);
                do {

                    Console.WriteLine($"Step: {++count} - {open.Count} nodes");

                    // find node with lowest heat loss so far
                    int bestLoss = open[0].HeatLossSoFar;
                    int bestindex = 0;
                    for (int i = 0; i < open.Count; i++) {
                        if (open[i].HeatLossSoFar < bestLoss) {
                            bestLoss = open[i].HeatLossSoFar;
                            bestindex = i;
                        }
                    }
                    if (Verbose) Console.WriteLine($"  best node is #{bestindex} [{open[bestindex].Row}|{open[bestindex].Col}] with HeatLossSoFar {bestLoss}");

                    // pick best node
                    current = open[bestindex];
                    done.Add(current);
                    open.RemoveAt(bestindex);
                    if (current.Row == (InputAsLines.Length - 1) && current.Col == (InputAsLines[0].Length - 1)) {
                        if (current.HeatLossSoFar < bestSuccessPath.HeatLossSoFar) bestSuccessPath = current;
                    }
                    if (Verbose) Console.WriteLine($"  now left {open.Count} nodes");

                    // find all potential next steps
                    row = current.Row;
                    col = current.Col;

                    // check right
                    newRow = row;
                    newCol = col + 1;
                    newDir = Dir.R;
                    if (Verbose) Console.Write($"  from [{row}|{col}] checking {newDir} to [{newRow}|{newCol}] ... ");
                    if (newCol < InputAsLines[0].Length) PerformStep(grid, open, done, current, newRow, newCol, newDir);

                    if (Verbose) Console.WriteLine($"done!");

                    // check down
                    newRow = row + 1;
                    newCol = col;
                    newDir = Dir.D;
                    if (Verbose) Console.Write($"  from [{row}|{col}] checking {newDir} to [{newRow}|{newCol}] ... ");
                    if (newRow < InputAsLines.Length) PerformStep(grid, open, done, current, newRow, newCol, newDir);
                    if (Verbose) Console.WriteLine($"done!");


                    // check left
                    newRow = row;
                    newCol = col - 1;
                    newDir = Dir.L;
                    if (Verbose) Console.Write($"  from [{row}|{col}] checking {newDir} to [{newRow}|{newCol}] ... ");
                    if (newCol >= 0) PerformStep(grid, open, done, current, newRow, newCol, newDir);
                    if (Verbose) Console.WriteLine($"done!");

                    // check up
                    newRow = row - 1;
                    newCol = col;
                    newDir = Dir.U;
                    if (Verbose) Console.Write($"  from [{row}|{col}] checking {newDir} to [{newRow}|{newCol}] ... ");
                    if (newRow >= 0) PerformStep(grid, open, done, current, newRow, newCol, newDir);
                    if (Verbose) Console.WriteLine($"done!");


                } while (open.Any());

                current = bestSuccessPath;
                do {
                    grid[current.Row][current.Col] = '.';
                    current = current.Pre;
                } while (current.Row != 0 || current.Col != 0);

                PrintGrid(grid);

                return bestSuccessPath.HeatLossSoFar.ToString();
            }

            private void PerformStep(char[][] grid, List<Node> open, List<Node> done, Node current, int newRow, int newCol, Dir newDir) {
                if (Verbose) Console.Write($"within grid good ...");
                if ((current.CameThroughDir != newDir || current.SameDir < 3) && current.CameThroughDir != OppositeDir(newDir)) {
                    if (Verbose) Console.Write($"only {current.SameDir}x{current.CameThroughDir} so far ...");
                    int newHeatLoss = current.HeatLossSoFar + grid[newRow][newCol] - '0';
                    if (Verbose) Console.Write($"loss {newHeatLoss} ...");
                    int nodeIndex = open.FindIndex(n => n.Row == newRow && n.Col == newCol && n.CameThroughDir == newDir);
                    if (nodeIndex < 0) { // does not yet exist
                        if (done.FindIndex(n => n.Row == newRow && n.Col == newCol && n.CameThroughDir == newDir) == -1) {
                            open.Add(new Node(newRow, newCol, newDir, newHeatLoss, current));
                            if (Verbose) Console.Write($"found a new path ... ");
                        } else {
                            if (Verbose) Console.Write($"done already ... ");
                        }
                    } else {
                        if (open[nodeIndex].HeatLossSoFar > newHeatLoss) {
                            open[nodeIndex].HeatLossSoFar = newHeatLoss;
                            open[nodeIndex].Pre = current;
                            if (Verbose) Console.Write($"found a better path ... ");
                        }
                    }
                }
            }

            private Dir OppositeDir(Dir dir) => dir switch { Dir.R => Dir.L, Dir.D => Dir.U, Dir.L => Dir.R, Dir.U => Dir.D, _ => Dir.None };

            private class Node {
                public int Row;
                public int Col;
                public Dir CameThroughDir;
                //public int HeatLoss;
                public int HeatLossSoFar;
                public Node Pre;
                public int SameDir;

                public Node(int row, int col, Dir cameThroughDir, int heatLossSoFar, Node pre) {
                    Row = row;
                    Col = col;
                    CameThroughDir = cameThroughDir;
                    //HeatLoss = heatLoss;
                    HeatLossSoFar = heatLossSoFar;
                    Pre = pre;
                    if (pre != null && cameThroughDir == pre.CameThroughDir) SameDir = pre.SameDir + 1;
                    else SameDir = 1;
                }
            }
        }
    }
}
