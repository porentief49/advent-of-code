using System.Collections;
using System.ComponentModel.Design;
using pos = (int row, int col);

namespace Puzzles {

    public partial class Year2023 {

        public class Day23 : DayBase {

            protected override string Title { get; } = "Day 23: A Long Walk";

            public override void SetupAll() {
                AddInputFile(@"2023\23_Example.txt");
                //AddInputFile(@"2023\23_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve() {
                //if (Part2) return "";
                char[][] maze = InputAsLines.Select(i => i.ToArray()).ToArray();
                //List<pos> current = [(0, 1), (1, 1)];
                List<List<pos>> successPaths = new();
                Queue<List<pos>> paths = new Queue<List<pos>>();
                paths.Enqueue([(0, 1), (1, 1)]);
                bool done = false;
                int count = 0;
                do {
                    if (Verbose) Console.WriteLine($"starting with {paths.Count()} paths, picking first");
                    var current = paths.Dequeue();
                    var r = current.Last().row;
                    var c = current.Last().col;


                    // test left
                    if (Verbose) Console.Write($"  at pos [{r},{c}] testing left ...");
                    if (maze[r][c - 1] != '#') {// ok no forest
                                                //if (maze[r][c] == '.' || maze[r][c] == '<') {
                        if (!current.Contains((r, c - 1))) {
                            var newPath = new List<pos>(current);
                            newPath.Add((r, c - 1));
                            paths.Enqueue(newPath);
                            if (Verbose) Console.Write($"cool!");
                        }
                    }
                    if (Verbose) Console.WriteLine($"");

                    // test right
                    if (Verbose) Console.Write($"  at pos [{r},{c}] testing right ...");
                    if (maze[r][c + 1] != '#') {// ok no forest
                                                //if (maze[r][c] == '.' || maze[r][c] == '>') {
                        if (!current.Contains((r, c + 1))) {
                            var newPath = new List<pos>(current);
                            newPath.Add((r, c + 1));
                            paths.Enqueue(newPath);
                            if (Verbose) Console.Write($"cool!");
                        }
                    }
                    if (Verbose) Console.WriteLine($"");

                    // test up
                    if (Verbose) Console.Write($"  at pos [{r},{c}] testing up ...");
                    if (maze[r - 1][c] != '#') {// ok no forest
                                                //if (maze[r][c] == '.' || maze[r][c] == '^') {
                        if (!current.Contains((r - 1, c))) {
                            var newPath = new List<pos>(current);
                            newPath.Add((r - 1, c));
                            paths.Enqueue(newPath);
                            if (Verbose) Console.Write($"cool!");
                        }
                    }
                    if (Verbose) Console.WriteLine($"");

                    // test down
                    if (Verbose) Console.Write($"  at pos [{r},{c}] testing down ...");
                    if (maze[r + 1][c] != '#') {// ok no forest
                                                //if (maze[r][c] == '.' || maze[r][c] == 'v') {
                        if (!current.Contains((r + 1, c))) {
                            var newPath = new List<pos>(current);
                            newPath.Add((r + 1, c));
                            if (r + 1 == maze.Length - 1) successPaths.Add(newPath);
                            else paths.Enqueue(newPath);
                            if (Verbose) Console.Write($"cool!");
                        }
                    }
                    if (Verbose) Console.WriteLine($"");
                    if (Verbose) Console.WriteLine($"  now we have {paths.Count()} paths");

                    //var loc = current.Last();
                    //var ch = maze[loc.row][loc.col];
                    //var newLoc = loc;
                    //if (ch == '.') {// go any direction

                    //} else {// must go into one direction
                    //    newLoc.row += ch switch { '^' => -1, 'v' => +1, _ => 0 };
                    //    newLoc.col += ch switch { '<' => -1, '>' => +1, _ => 0 };
                    //}
                    //current.Add(
                    //if ()
                    count++;
                    if (count % 10000 == 0) Console.WriteLine($"After {count} loops, we have {paths.Count()} paths");
                } while (paths.Any());

                foreach (var path in successPaths) Console.WriteLine(path.Count() - 1);
                return successPaths.Max(s => s.Count() - 1).ToString();
            }

            private class Path {
                public int X;
                public int Y;
                public List<Path> Predecessors;

                public Path(int x, int y, Path predecessor) {
                    X = x; Y = y;
                    Predecessors = [predecessor];
                }
            }
        }
    }
}
