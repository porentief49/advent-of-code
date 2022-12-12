using System;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static Puzzles.Year2022;

namespace Puzzles
{
    public partial class Year2022
    {
        public class Day12 : DayBase
        {
            private Node[][] map;
            private List<Node> backlog = new List<Node>();
            private (int y, int x) start1;
            private (int y, int x) end;
            private int dimX;
            private int dimY;

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
                dimY = InputData.Length;
                dimX = InputData[0].Length;
                map = new Node[dimY][];
                for (int y = 0; y < dimY; y++)
                {
                    Node[] line = new Node[dimX];
                    for (int x = 0; x < dimX; x++)
                    {
                        int height;
                        switch (InputData[y][x])
                        {
                            case 'S':
                                start1 = (y, x);
                                height = 0;
                                break;
                            case 'E':
                                end = (y, x);
                                height = 25;
                                break;
                            default:
                                height = InputData[y][x] - 97;
                                break;
                        }
                        line[x] = new(y, x, height);
                    }
                    map[y] = line;
                }
            }

            public override string Solve(bool Part1)
            {
                int bestBestDistance = int.MaxValue;
                List<Node> startLocations = new();
                if (Part1) startLocations.Add(map[start1.y][start1.x]);
                else for (int y = 0; y < dimY; y++) for (int x = 0; x < dimX; x++) if (map[y][x].Height == 0) startLocations.Add(map[y][x]);
                foreach (Node startNode in startLocations)
                {
                    bool targetReached = false;
                    bool stuck = false;
                    int bestDistance = 0;
                    int thisY = startNode.Y;
                    int thisX = startNode.X;
                    for (int yy = 0; yy < dimY; yy++) for (int xx = 0; xx < dimX; xx++) map[yy][xx].Clear();
                    backlog.Clear();
                    Node node = startNode;
                    node.Distance = 0;
                    node.Listed = true;
                    backlog.Add(node);
                    do
                    {
                        if (backlog.Count > 0)
                        {
                            int minDistance = backlog.Select(x => x.Distance).Min(); // much faster if done separately
                            var possibleNodes = backlog.Where(x => x.Distance == minDistance);
                            int bestHeight = possibleNodes.Select(x => x.Height).Max();
                            node = possibleNodes.Where(x => x.Height == bestHeight).First(); // any is good
                            backlog.Remove(node);
                            thisX = node.X;
                            thisY = node.Y;
                            if (Verbose) Console.WriteLine($"x {thisX} - y {thisY} - count {backlog.Count}");
                            node.Visited = true;
                            targetReached = (thisX == end.x && thisY == end.y);
                            if (targetReached) bestDistance = node.Distance;
                            else
                            {
                                ProcessNeighbors(node, backlog, thisY, thisX + 1);
                                ProcessNeighbors(node, backlog, thisY, thisX - 1);
                                ProcessNeighbors(node, backlog, thisY + 1, thisX);
                                ProcessNeighbors(node, backlog, thisY - 1, thisX);
                            }
                        }
                        else stuck = true;
                    } while (!targetReached && !stuck);
                    if (!stuck && bestDistance < bestBestDistance) bestBestDistance = bestDistance;
                    if (Verbose) Console.WriteLine($"x {startNode.X} - y {startNode.Y} - {bestDistance}");
                }
                return FormatResult(bestBestDistance, "best distance");
            }

            private void ProcessNeighbors(Node Current, List<Node> Backlog, int NewY, int NewX)
            {
                Node newNode;
                if (NewX >= 0 && NewX < dimX && NewY >= 0 && NewY < dimY)
                {
                    newNode = map[NewY][NewX];
                    if (newNode.Height - Current.Height <= 1 && !newNode.Visited)
                    {
                        newNode.Distance = Current.Distance + 1;
                        newNode.Predecessor = Current;
                        if (!newNode.Listed)
                        {
                            newNode.Listed = true;
                            Backlog.Add(newNode);
                        }
                    }
                }

            }
        }

        public class Node
        {
            public readonly int X;
            public readonly int Y; 
            public readonly int Height; 
            public int Distance;
            public bool Visited;
            public bool Listed;
            public Node? Predecessor;

            public Node(int Y, int X, int Height)
            {
                this.X = X;
                this.Y = Y;
                this.Height = Height;
                Clear();
            }

            public void Clear()
            {
                Distance = int.MaxValue;
                Visited = false;
                Listed = false;
                Predecessor = null;
            }
        }
    }
}
