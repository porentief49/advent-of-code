using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace Puzzles {

    public partial class Year2023 {

        public class Day17 : DayBase {

            protected override string Title { get; } = "Day 17: Clumsy Crucible";

            private enum Dir { None = 0, U = 1, R = 2, D = 4, L = 8 }

            public override void SetupAll() {
                AddInputFile(@"2023\17_Example.txt");
                AddInputFile(@"2023\17_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            private char[][] _grid;
            List<Node> _open;
            List<Node> _done;

            public override string Solve() {
                Verbose = false;
                Node current;

                // initialize
                _grid = InputAsLines.Select(i => i.ToCharArray()).ToArray();
                _open = new() { new Node(0, 0, Dir.None, 0, null, 1) };
                _done = new();

                // dijkstra ... takes LONG!!! But A* didn't improve anything :-(
                int count = 0;
                Node bestSuccessPath = new Node(InputAsLines.Length - 1, InputAsLines[0].Length - 1, Dir.None, 99999, null, 1);
                do {
                    if (Verbose) Console.WriteLine($"Step: {++count} - {_open.Count} nodes");

                    // find node with lowest heat loss so far
                    int bestLoss = _open[0].HeatLossSoFar;
                    int bestindex = 0;
                    for (int i = 0; i < _open.Count; i++) if (_open[i].HeatLossSoFar < bestLoss) (bestLoss, bestindex) = (_open[i].HeatLossSoFar, i);

                    // pick best node
                    current = _open[bestindex];
                    _done.Add(current);
                    _open.RemoveAt(bestindex);
                    if (current.Row == (InputAsLines.Length - 1) && current.Col == (InputAsLines[0].Length - 1)) if (current.HeatLossSoFar < bestSuccessPath.HeatLossSoFar) bestSuccessPath = current;

                    // find all potential next steps
                    CheckDir(current, Dir.R, Part1);
                    CheckDir(current, Dir.D, Part1);
                    CheckDir(current, Dir.L, Part1);
                    CheckDir(current, Dir.U, Part1);
                } while (_open.Any());
                return bestSuccessPath.HeatLossSoFar.ToString();
            }

            private void CheckDir(Node current, Dir newDir, bool part1) {
                int newRow = current.Row + newDir switch { Dir.D => 1, Dir.U => -1, _ => 0 };
                if (newRow >= 0 && newRow < InputAsLines.Length) {
                    int newCol = current.Col + newDir switch { Dir.R => 1, Dir.L => -1, _ => 0 };
                    if (newCol >= 0 && newCol < InputAsLines[0].Length) {
                        if (current.CameThroughDir != OppositeDir(newDir)) {
                            bool criteria;
                            if (part1) criteria = current.CameThroughDir != newDir || current.SameDir < 3;
                            else criteria = ((current.CameThroughDir == newDir || current.CameThroughDir == Dir.None) && current.SameDir < 10) || ((current.CameThroughDir != newDir || current.CameThroughDir == Dir.None) && current.SameDir > 3);
                            if (criteria) {
                                int newHeatLoss = current.HeatLossSoFar + _grid[newRow][newCol] - '0';
                                int newSameDir = (current.Pre != null && newDir == current.CameThroughDir) ? current.SameDir + 1 : 1;
                                int nodeIndex = _open.FindIndex(n => n.Row == newRow && n.Col == newCol && n.CameThroughDir == newDir && n.SameDir == newSameDir);
                                if (nodeIndex < 0) { // does not yet exist
                                    if (_done.FindIndex(n => n.Row == newRow && n.Col == newCol && n.CameThroughDir == newDir && n.SameDir == newSameDir) == -1) _open.Add(new Node(newRow, newCol, newDir, newHeatLoss, current, newSameDir));
                                } else {
                                    if (_open[nodeIndex].HeatLossSoFar > newHeatLoss) _open[nodeIndex] = new Node(newRow, newCol, newDir, newHeatLoss, current, newSameDir);
                                }
                            }
                        }
                    }
                }
            }

            private Dir OppositeDir(Dir dir) => dir switch { Dir.R => Dir.L, Dir.D => Dir.U, Dir.L => Dir.R, Dir.U => Dir.D, _ => Dir.None };

            private class Node {
                public int Row;
                public int Col;
                public Dir CameThroughDir;
                public int HeatLossSoFar;
                public Node Pre;
                public int SameDir;

                public Node(int row, int col, Dir cameThroughDir, int heatLossSoFar, Node pre, int sameDir) {
                    Row = row;
                    Col = col;
                    CameThroughDir = cameThroughDir;
                    HeatLossSoFar = heatLossSoFar;
                    Pre = pre;
                    SameDir = sameDir;
                }
            }
        }
    }
}
