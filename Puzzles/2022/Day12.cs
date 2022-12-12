using System;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Puzzles
{
    public partial class Year2022
    {
        public class Day12 : DayBase
        {
            private int[][] map;
            private Node start;
            private Node end;
            private int width;
            private int height;

            protected override string Title { get; } = "Day 12: Hill Climbing Algorithm";

            public override void SetupAll()
            {
                AddInputFile(@"2022\12_Example.txt");
                AddInputFile(@"2022\12_rAiner.txt");
                AddInputFile(@"2022\12_SEGCC.txt");
            }

            public override void Init(string InputFile)
            {
                InputData = ReadFile(InputFile, true);
                height = InputData.Length;
                width = InputData[0].Length;
                for (int y = 0; y < height; y++)
                {
                    if (InputData[y].Contains('S'))
                    {
                        start = new Node(y, InputData[y].IndexOf('S'));
                        InputData[y] = InputData[y].Replace('S', 'a');
                    }
                    if (InputData[y].Contains('E'))
                    {
                        end = new Node(y, InputData[y].IndexOf('E'));
                        InputData[y] = InputData[y].Replace('E', 'z');
                    }

                }
                map = InputData.Select(x => x.Select(x => (int)x - 97).ToArray()).ToArray();
            }

            public override string Solve(bool Part1)
            {
                int bestBestDistance = int.MaxValue;
                for (int yyy = (Part1 ? start.Y : 0); yyy < (Part1 ? start.Y + 1 : height); yyy++)
                {
                    for (int xxx = (Part1 ? start.X : 0); xxx < (Part1 ? start.X+1 : width); xxx++)
                    {
                        if (map[yyy][xxx] == 0)
                        {
                            start.X = xxx;
                            start.Y = yyy;
                            //if (!Part1) return "";
                            bool[][] visited = InitJaggedArray(height, width, false);
                            bool[][] listed = InitJaggedArray(height, width, false);
                            List<Node> backlog = new List<Node>();
                            bool targetReached = false;
                            bool nirvana = false;
                            start.Height = map[start.Y][start.X];
                            start.Distance = 0;
                            listed[start.Y][start.X] = true;
                            backlog.Add(start);
                            int bestDistance = 0;
                            int i = 0;
                            do
                            {
                                i++;
                                if (backlog.Count > 0)
                                {


                                    int minDist = backlog.Select(x => x.Distance).Min();
                                    var minDistNodes = backlog.Where(x => x.Distance == minDist);
                                    int bestHeight = minDistNodes.Select(x => x.Height).Max();
                                    Node node = minDistNodes.Where(x => x.Height == bestHeight).First();
                                    backlog.Remove(node);
                                    int x = node.X;
                                    int y = node.Y;
                                    //Console.WriteLine($"x {x} - y {y} - loop {i} - count {backlog.Count}");
                                    //PrintVisited(visited);
                                    //Console.WriteLine("");
                                    visited[y][x] = true;
                                    targetReached = (x == end.X && y == end.Y);
                                    if (!targetReached)
                                    {
                                        int newX;
                                        int newY;
                                        if (x < width - 1 && map[y][x + 1] - map[y][x] <= 1 && !visited[y][x + 1])
                                        {
                                            newY = y;
                                            newX = x + 1;
                                            if (listed[newY][newX])
                                            {
                                                Node update = backlog.Where(x => (x.X == newX) && (x.Y == newY)).First();
                                                if (update.Distance > node.Distance + 1)
                                                {
                                                    update.Distance = node.Distance + 1;
                                                    update.Predecessor = node;
                                                }
                                            }
                                            else
                                            {
                                                Node next = new(newY, newX);
                                                next.Predecessor = node;
                                                next.Distance = node.Distance + 1;
                                                next.Height = map[newY][newX];
                                                backlog.Add(next);
                                                listed[newY][newX] = true;
                                            }
                                        }
                                        if (x > 0 && map[y][x - 1] - map[y][x] <= 1 && !visited[y][x - 1])
                                        {
                                            newY = y;
                                            newX = x - 1;
                                            if (listed[newY][newX])
                                            {
                                                Node update = backlog.Where(x => (x.X == newX) && (x.Y == newY)).First();
                                                if (update.Distance > node.Distance + 1)
                                                {
                                                    update.Distance = node.Distance + 1;
                                                    update.Predecessor = node;
                                                }
                                            }
                                            else
                                            {
                                                Node next = new(newY, newX);
                                                next.Predecessor = node;
                                                next.Distance = node.Distance + 1;
                                                next.Height = map[newY][newX];
                                                backlog.Add(next);
                                                listed[newY][newX] = true;
                                            }
                                        }
                                        if (y < height - 1 && map[y + 1][x] - map[y][x] <= 1 && !visited[y + 1][x])
                                        {
                                            newY = y + 1;
                                            newX = x;
                                            if (listed[newY][newX])
                                            {
                                                Node update = backlog.Where(x => (x.X == newX) && (x.Y == newY)).First();
                                                if (update.Distance > node.Distance + 1)
                                                {
                                                    update.Distance = node.Distance + 1;
                                                    update.Predecessor = node;
                                                }
                                            }
                                            else
                                            {
                                                Node next = new(newY, newX);
                                                next.Predecessor = node;
                                                next.Distance = node.Distance + 1;
                                                next.Height = map[newY][newX];
                                                backlog.Add(next);
                                                listed[newY][newX] = true;
                                            }
                                        }
                                        if (y > 0 && map[y - 1][x] - map[y][x] <= 1 && !visited[y - 1][x])
                                        {
                                            newY = y - 1;
                                            newX = x;
                                            if (listed[newY][newX])
                                            {
                                                Node update = backlog.Where(x => (x.X == newX) && (x.Y == newY)).First();
                                                if (update.Distance > node.Distance + 1)
                                                {
                                                    update.Distance = node.Distance + 1;
                                                    update.Predecessor = node;
                                                }
                                            }
                                            else
                                            {
                                                Node next = new(newY, newX);
                                                next.Predecessor = node;
                                                next.Distance = node.Distance + 1;
                                                next.Height = map[newY][newX];
                                                backlog.Add(next);
                                                listed[newY][newX] = true;
                                            }
                                        }
                                    }
                                    else bestDistance = node.Distance;
                                }
                                else
                                {
                                    targetReached = true;
                                    nirvana = true;
                                }
                            } while (!targetReached);
                            if (!nirvana)
                            {
                                if (bestDistance < bestBestDistance) bestBestDistance = bestDistance;
                                //Console.WriteLine($"y {yyy} - x {xxx} ---> best distance {bestDistance}");
                            }
                        }
                    }
                }

                return FormatResult(bestBestDistance, "best distance");
            }

            public void PrintVisited(bool[][] Visited)
            {
                foreach (var line in Visited) Console.WriteLine(line.Select(x => x ? 'x' : ' ').ToArray());
            }
        }
        public class Node
        {
            public int X;
            public int Y;
            public int Distance = int.MaxValue;
            //public bool Visited = false;
            public Node? Predecessor;
            public int Height;

            public Node(int Y, int X)
            {
                this.X = X;
                this.Y = Y;
            }
        }
    }
}
