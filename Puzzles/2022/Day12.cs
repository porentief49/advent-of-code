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
            private int[][] mapOld;
            private Node startOld;
            private Node endOld;
            private int width;
            private int height;

            protected override string Title { get; } = "Day 12: Hill Climbing Algorithm";

            public override void SetupAll()
            {
                AddInputFile(@"2022\12_Example.txt");
                AddInputFile(@"2022\12_rAiner.txt");
                //AddInputFile(@"2022\12_SEGCC.txt");
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
                        startOld = new Node(y, InputData[y].IndexOf('S'), 0); //ugly hardcoded height
                        InputData[y] = InputData[y].Replace('S', 'a');
                    }
                    if (InputData[y].Contains('E'))
                    {
                        endOld = new Node(y, InputData[y].IndexOf('E'), 25); // ugly hardcoded height
                        InputData[y] = InputData[y].Replace('E', 'z');
                    }
                }
                mapOld = InputData.Select(x => x.Select(x => (int)x - 97).ToArray()).ToArray();
            }

            public override string Solve(bool Part1)
            {

                if (!Part1) return "";

                int bestBestDistance = int.MaxValue;
                Node[][] map = new Node[height][];
                for (int y = 0; y < height; y++)
                {
                    Node[] line = new Node[width];
                    for (int x = 0; x < width; x++)
                    {
                        line[x] = new(y, x, (int)InputData[y][x] - 97);
                    }
                    map[y] = line;
                }

                for (int yyy = (Part1 ? startOld.Y : 0); yyy < (Part1 ? startOld.Y + 1 : height); yyy++)
                {
                    for (int xxx = (Part1 ? startOld.X : 0); xxx < (Part1 ? startOld.X + 1 : width); xxx++)
                    {
                        //if (mapOld[yyy][xxx] == 0)
                        if (map[yyy][xxx].Height == 0)
                        {
                            //if (!Part1) return "";
                            //bool[][] visited = InitJaggedArray(height, width, false);
                            //bool[][] listed = InitJaggedArray(height, width, false);
                            List<Node> backlog = new List<Node>();
                            bool targetReached = false;
                            bool stuck = false;
                            //startOld.Height = mapOld[startOld.Y][startOld.X];
                            //startOld.Distance = 0;
                            //listed[startOld.Y][startOld.X] = true;
                            //backlog.Add(startOld);
                            int bestDistance = 0;
                            int i = 0;

                            startOld.X = xxx;
                            startOld.Y = yyy;
                            int x = xxx;
                            int y = yyy;
                            Node thisNode = map[y][x];
                            thisNode.Distance = 0;
                            thisNode.Listed = true;
                            backlog.Add(thisNode);

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
                                    x = node.X;
                                    y = node.Y;
                                    Console.WriteLine($"x {x} - y {y} - loop {i} - count {backlog.Count}");
                                    //PrintVisited(visited);
                                    //Console.WriteLine("");
                                    //visited[y][x] = true;

                                    node.Visited = true;

                                    targetReached = (x == endOld.X && y == endOld.Y);
                                    if (!targetReached)
                                    {
                                        int newX;
                                        int newY;
                                        newY = y;
                                        newX = x + 1;
                                        Node newNode;
                                        if (newX < width)
                                        {
                                            newNode = map[newY][newX];
                                            if (newNode.Height - node.Height <= 1 && !newNode.Visited)
                                            {
                                                newNode.Distance = node.Distance + 1;
                                                newNode.Predecessor = node;
                                                if (!newNode.Listed)
                                                {
                                                    newNode.Listed = true;
                                                    backlog.Add(newNode);
                                                }
                                            }
                                        }
                                        newY = y;
                                        newX = x - 1;
                                        if (newX >= 0)
                                        {
                                            newNode = map[newY][newX];
                                            if (newNode.Height - node.Height <= 1 && !newNode.Visited)
                                            {
                                                newNode.Distance = node.Distance + 1;
                                                newNode.Predecessor = node;
                                                if (!newNode.Listed)
                                                {
                                                    newNode.Listed = true;
                                                    backlog.Add(newNode);
                                                }
                                            }
                                        }
                                        //if (newX >= 0 && mapOld[newY][newX] - mapOld[y][x] <= 1 && !visited[newY][newX])
                                        //{
                                        //    if (listed[newY][newX])
                                        //    {
                                        //        Node update = backlog.Where(x => (x.X == newX) && (x.Y == newY)).First();
                                        //        if (update.Distance > node.Distance + 1)
                                        //        {
                                        //            update.Distance = node.Distance + 1;
                                        //            update.Predecessor = node;
                                        //        }
                                        //    }
                                        //    else
                                        //    {
                                        //        Node next = new(newY, newX, mapOld[newY][newX]);
                                        //        next.Predecessor = node;
                                        //        next.Distance = node.Distance + 1;
                                        //        next.Listed = true;
                                        //        next.Height = mapOld[newY][newX];
                                        //        backlog.Add(next);
                                        //        listed[newY][newX] = true;
                                        //    }
                                        //}
                                        newY = y + 1;
                                        newX = x;
                                        if (newY < height)
                                        {
                                            newNode = map[newY][newX];
                                            if (newNode.Height - node.Height <= 1 && !newNode.Visited)
                                            {
                                                newNode.Distance = node.Distance + 1;
                                                newNode.Predecessor = node;
                                                if (!newNode.Listed)
                                                {
                                                    newNode.Listed = true;
                                                    backlog.Add(newNode);
                                                }
                                            }
                                        }
                                        //if (newY < height && mapOld[newY][newX] - mapOld[y][x] <= 1 && !visited[newY][newX])
                                        //{
                                        //    if (listed[newY][newX])
                                        //    {
                                        //        Node update = backlog.Where(x => (x.X == newX) && (x.Y == newY)).First();
                                        //        if (update.Distance > node.Distance + 1)
                                        //        {
                                        //            update.Distance = node.Distance + 1;
                                        //            update.Predecessor = node;
                                        //        }
                                        //    }
                                        //    else
                                        //    {
                                        //        Node next = new(newY, newX, mapOld[newY][newX]);
                                        //        next.Predecessor = node;
                                        //        next.Distance = node.Distance + 1;
                                        //        next.Listed = true;
                                        //        next.Height = mapOld[newY][newX];
                                        //        backlog.Add(next);
                                        //        listed[newY][newX] = true;
                                        //    }
                                        //}
                                        newY = y - 1;
                                        newX = x;
                                        if (newY >= 0)
                                        {
                                            newNode = map[newY][newX];
                                            if (newNode.Height - node.Height <= 1 && !newNode.Visited)
                                            {
                                                newNode.Distance = node.Distance + 1;
                                                newNode.Predecessor = node;
                                                if (!newNode.Listed)
                                                {
                                                    newNode.Listed = true;
                                                    backlog.Add(newNode);
                                                }
                                            }
                                        }
                                        //if (newY >= 0 && mapOld[newY][newX] - mapOld[y][x] <= 1 && !visited[newY][newX])
                                        //{
                                        //    if (listed[newY][newX])
                                        //    {
                                        //        Node update = backlog.Where(x => (x.X == newX) && (x.Y == newY)).First();
                                        //        if (update.Distance > node.Distance + 1)
                                        //        {
                                        //            update.Distance = node.Distance + 1;
                                        //            update.Predecessor = node;
                                        //        }
                                        //    }
                                        //    else
                                        //    {
                                        //        Node next = new(newY, newX, mapOld[newY][newX]);
                                        //        next.Predecessor = node;
                                        //        next.Distance = node.Distance + 1;
                                        //        next.Listed = true;
                                        //        next.Height = mapOld[newY][newX];
                                        //        backlog.Add(next);
                                        //        listed[newY][newX] = true;
                                        //    }
                                        //}
                                    }
                                    else bestDistance = node.Distance;
                                }
                                else
                                {
                                    targetReached = true;
                                    stuck = true;
                                }
                            } while (!targetReached);
                            if (!stuck)
                            {
                                if (bestDistance < bestBestDistance) bestBestDistance = bestDistance;
                                //Console.WriteLine($"y {yyy} - x {xxx} ---> best distance {bestDistance}");
                            }
                        }
                    }
                }

                return FormatResult(bestBestDistance, "best distance");
            }
        }
        public class Node
        {
            public int X; // make readonly
            public int Y; // make readonly
            public int Height; // make readonly
            public int Distance = int.MaxValue;
            public bool Visited = false;
            public bool Listed = false;
            public Node? Predecessor;

            public Node(int Y, int X, int Height)
            {
                this.X = X;
                this.Y = Y;
                this.Height = Height;
            }
        }
    }
}
