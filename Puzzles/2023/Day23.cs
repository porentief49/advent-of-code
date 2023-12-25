using System.Collections;
using System.ComponentModel.Design;
using pos = (int row, int col);

namespace Puzzles {

    public partial class Year2023 {

        public class Day23 : DayBase {

            protected override string Title { get; } = "Day 23: A Long Walk";

            public override void SetupAll() {
                //AddInputFile(@"2023\23_Example.txt");
                AddInputFile(@"2023\23_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);


            public override string Solve() {
                //if (Part1) return "";
                Verbose = false;
                char[][] maze = InputAsLines.Select(i => i.ToArray()).ToArray();
                char[][] print = InputAsLines.Select(i => i.Replace("#", "-").ToArray()).ToArray();
                List<List<pos>> successPaths = new();
                Queue<List<pos>> paths = new Queue<List<pos>>();
                paths.Enqueue([(0, 1), (1, 1)]);
                //bool done = false;
                int count = 0;
                int path = 0;
                do {
                    var current = paths.Dequeue();
                    if (Verbose) Console.WriteLine($"starting with {paths.Count()} paths, picking the first with {current.Count} elements");
                    path++;
                    bool foundOne = false;
                    do {
                        foundOne = false;
                        var r = current.Last().row;
                        var c = current.Last().col;
                        print[r][c] = path.ToString()[0];
                        if (r == maze.Length - 1) successPaths.Add(current);
                        else {
                            if (Verbose) Console.WriteLine($"  {current.Count} elements in this path");
                            // test left
                            pos nextOnPath = (-1, -1);
                            if (Verbose) Console.Write($"    at pos [{r},{c}] testing left ...");
                            if (maze[r][c - 1] != '#' && (Part2 || (maze[r][c] == '.' || maze[r][c] == '<'))) {
                                if (!current.Contains((r, c - 1))) {
                                    if (!foundOne) {
                                        nextOnPath = (r, c - 1);
                                        //current.Add((r, c - 1));
                                        foundOne = true;
                                        if (Verbose) Console.Write($"keep going");
                                    } else {
                                        var newPath = new List<pos>(current);
                                        newPath.Add((r, c - 1));
                                        paths.Enqueue(newPath);
                                        if (Verbose) Console.Write($"new path added with last element {r}|{c - 1} and {newPath.Count} elements");
                                    }
                                }
                            }
                            if (Verbose) Console.WriteLine($"");

                            // test right
                            if (Verbose) Console.Write($"    at pos [{r},{c}] testing right ...");
                            if (maze[r][c + 1] != '#' && (Part2 || (maze[r][c] == '.' || maze[r][c] == '>'))) {
                                if (!current.Contains((r, c + 1))) {
                                    if (!foundOne) {
                                        nextOnPath = (r, c + 1);
                                        //current.Add((r, c + 1));
                                        foundOne = true;
                                        if (Verbose) Console.Write($"keep going");
                                    } else {
                                        var newPath = new List<pos>(current);
                                        newPath.Add((r, c + 1));
                                        paths.Enqueue(newPath);
                                        if (Verbose) Console.Write($"new path added with last element {r}|{c + 1} and {newPath.Count} elements");
                                    }
                                }
                            }
                            if (Verbose) Console.WriteLine($"");

                            // test up
                            if (Verbose) Console.Write($"    at pos [{r},{c}] testing up ...");
                            if (maze[r - 1][c] != '#' && (Part2 || (maze[r][c] == '.' || maze[r][c] == '^'))) {
                                if (!current.Contains((r - 1, c))) {
                                    if (!foundOne) {
                                        nextOnPath = (r - 1, c);
                                        //current.Add((r - 1, c));
                                        foundOne = true;
                                        if (Verbose) Console.Write($"keep going");
                                    } else {
                                        var newPath = new List<pos>(current);
                                        newPath.Add((r - 1, c));
                                        paths.Enqueue(newPath);
                                        if (Verbose) Console.Write($"new path added with last element {r - 1}|{c} and {newPath.Count} elements");
                                    }
                                }
                            }
                            if (Verbose) Console.WriteLine($"");

                            // test down
                            if (Verbose) Console.Write($"    at pos [{r},{c}] testing down ...");
                            if (maze[r + 1][c] != '#' && (Part2 || (maze[r][c] == '.' || maze[r][c] == 'v'))) {
                                if (!current.Contains((r + 1, c))) {
                                    if (!foundOne) {
                                        nextOnPath = (r + 1, c);
                                        //current.Add((r + 1, c));
                                        foundOne = true;
                                        if (Verbose) Console.Write($"keep going");
                                    } else {
                                        var newPath = new List<pos>(current);
                                        newPath.Add((r + 1, c));
                                        paths.Enqueue(newPath);
                                        if (Verbose) Console.Write($"new path added with last element {r + 1}|{c} and {newPath.Count} elements");
                                    }
                                }
                            }
                            if (foundOne) current.Add(nextOnPath);
                            if (Verbose) Console.WriteLine($"");
                            if (Verbose) Console.WriteLine($"  now we have {paths.Count()} paths");
                        }
                    } while (foundOne);
                    count++;
                    if (count % 100 == 0) Console.WriteLine($"After {count} loops, we have {paths.Count()} candidates and {successPaths.Count} success paths, longest: {successPaths.Max(s => s.Count() - 1)}");
                } while (paths.Any());
                if (Verbose) foreach (var success in successPaths) Console.WriteLine(success.Count() - 1);

                //foreach (var success in successPaths) {
                //    Console.WriteLine($"######### PATH COUNT: {success.Count()}");
                //    for (int i = 0; i < success.Count; i++) {
                //        //(int row, int col) item = success[i];
                //        Console.WriteLine($"   {i}: {success[i].row}|{success[i].col}");
                //    }
                //}


                //PrintGrid(print);

                return successPaths.Max(s => s.Count() - 1).ToString();
            }
            //public string SolveX() {
            //    if (Part1) return "";
            //    char[][] maze = InputAsLines.Select(i => i.ToArray()).ToArray();
            //    //List<pos> current = [(0, 1), (1, 1)];
            //    List<List<pos>> successPaths = new();
            //    Queue<List<pos>> paths = new Queue<List<pos>>();
            //    paths.Enqueue([(0, 1), (1, 1)]);
            //    bool done = false;
            //    int count = 0;
            //    do {
            //        if (Verbose) Console.WriteLine($"starting with {paths.Count()} paths, picking first");
            //        var current = paths.Dequeue();
            //        var r = current.Last().row;
            //        var c = current.Last().col;


            //        // test left
            //        if (Verbose) Console.Write($"  at pos [{r},{c}] testing left ...");
            //        //if (maze[r][c - 1] != '#') {// ok no forest
            //        if (maze[r][c] == '.' || maze[r][c] == '<') {
            //            if (!current.Contains((r, c - 1))) {
            //                var newPath = new List<pos>(current);
            //                newPath.Add((r, c - 1));
            //                paths.Enqueue(newPath);
            //                if (Verbose) Console.Write($"cool!");
            //            }
            //        }
            //        if (Verbose) Console.WriteLine($"");

            //        // test right
            //        if (Verbose) Console.Write($"  at pos [{r},{c}] testing right ...");
            //        //if (maze[r][c + 1] != '#') {// ok no forest
            //        if (maze[r][c] == '.' || maze[r][c] == '>') {
            //            if (!current.Contains((r, c + 1))) {
            //                var newPath = new List<pos>(current);
            //                newPath.Add((r, c + 1));
            //                paths.Enqueue(newPath);
            //                if (Verbose) Console.Write($"cool!");
            //            }
            //        }
            //        if (Verbose) Console.WriteLine($"");

            //        // test up
            //        if (Verbose) Console.Write($"  at pos [{r},{c}] testing up ...");
            //        //if (maze[r - 1][c] != '#') {// ok no forest
            //        if (maze[r][c] == '.' || maze[r][c] == '^') {
            //            if (!current.Contains((r - 1, c))) {
            //                var newPath = new List<pos>(current);
            //                newPath.Add((r - 1, c));
            //                paths.Enqueue(newPath);
            //                if (Verbose) Console.Write($"cool!");
            //            }
            //        }
            //        if (Verbose) Console.WriteLine($"");

            //        // test down
            //        if (Verbose) Console.Write($"  at pos [{r},{c}] testing down ...");
            //        //if (maze[r + 1][c] != '#') {// ok no forest
            //        if (maze[r][c] == '.' || maze[r][c] == 'v') {
            //            if (!current.Contains((r + 1, c))) {
            //                var newPath = new List<pos>(current);
            //                newPath.Add((r + 1, c));
            //                if (r + 1 == maze.Length - 1) successPaths.Add(newPath);
            //                else paths.Enqueue(newPath);
            //                if (Verbose) Console.Write($"cool!");
            //            }
            //        }
            //        if (Verbose) Console.WriteLine($"");
            //        if (Verbose) Console.WriteLine($"  now we have {paths.Count()} paths");

            //        //var loc = current.Last();
            //        //var ch = maze[loc.row][loc.col];
            //        //var newLoc = loc;
            //        //if (ch == '.') {// go any direction

            //        //} else {// must go into one direction
            //        //    newLoc.row += ch switch { '^' => -1, 'v' => +1, _ => 0 };
            //        //    newLoc.col += ch switch { '<' => -1, '>' => +1, _ => 0 };
            //        //}
            //        //current.Add(
            //        //if ()
            //        count++;
            //        if (count % 10000 == 0) Console.WriteLine($"After {count} loops, we have {paths.Count()} paths");
            //    } while (paths.Any());

            //    foreach (var path in successPaths) Console.WriteLine(path.Count() - 1);
            //    return successPaths.Max(s => s.Count() - 1).ToString();
            //}

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
