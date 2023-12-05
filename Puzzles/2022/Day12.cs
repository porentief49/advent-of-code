using System;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using static Puzzles.Year2022;

namespace Puzzles {
    public partial class Year2022 {
        public class Day12 : DayBase {
            private Node[][] _map;
            private List<Node> _backlog = new List<Node>();
            private (int y, int x) _start1;
            private (int y, int x) _end;
            private (int y, int x) _dim;

            protected override string Title { get; } = "Day 12: Hill Climbing Algorithm";

            public override void SetupAll() {
                AddInputFile(@"2022\12_Example.txt");
                AddInputFile(@"2022\12_rAiner.txt");
                AddInputFile(@"2022\12_SEGCC.txt");
            }

            public override void Init(string file) {
                InputData = ReadLines(file, true);
                _dim = (InputData.Length, InputData[0].Length);
                _map = new Node[_dim.y][];
                for (int y = 0; y < _dim.y; y++) {
                    Node[] line = new Node[_dim.x];
                    for (int x = 0; x < _dim.x; x++) {
                        int height;
                        switch (InputData[y][x]) {
                            case 'S':
                                _start1 = (y, x);
                                height = 0;
                                break;
                            case 'E':
                                _end = (y, x);
                                height = 25;
                                break;
                            default:
                                height = InputData[y][x] - 97;
                                break;
                        }
                        line[x] = new(y, x, height);
                    }
                    _map[y] = line;
                }
            }

            public override string Solve(bool part1) {
                int bestBestDistance = int.MaxValue;
                List<Node> startLocations = new();
                if (part1) startLocations.Add(_map[_start1.y][_start1.x]);
                else for (int y = 0; y < _dim.y; y++) for (int x = 0; x < _dim.x; x++) if (_map[y][x].Height == 0) startLocations.Add(_map[y][x]);
                foreach (Node startNode in startLocations) {
                    bool targetReached = false;
                    bool stuck = false;
                    int bestDistance = 0;
                    for (int yy = 0; yy < _dim.y; yy++) for (int xx = 0; xx < _dim.x; xx++) _map[yy][xx].Clear();
                    _backlog.Clear();
                    Node thisNode = startNode;
                    thisNode.Distance = 0;
                    thisNode.Listed = true;
                    _backlog.Add(thisNode);
                    do {
                        if (_backlog.Count > 0) {
                            int minDistance = _backlog.Select(x => x.Distance).Min(); // much faster if done separately
                            var possibleNodes = _backlog.Where(x => x.Distance == minDistance);
                            int bestHeight = possibleNodes.Select(x => x.Height).Max();
                            thisNode = possibleNodes.Where(x => x.Height == bestHeight).First(); // any is good
                            _backlog.Remove(thisNode);
                            if (Verbose) Console.WriteLine($"x {thisNode.X} - y {thisNode.Y} - backlog count {_backlog.Count}");
                            thisNode.Visited = true;
                            targetReached = (thisNode.X == _end.x && thisNode.Y == _end.y);
                            if (targetReached) bestDistance = thisNode.Distance;
                            else {
                                ProcessNeighbors(thisNode, _backlog, thisNode.Y, thisNode.X + 1);
                                ProcessNeighbors(thisNode, _backlog, thisNode.Y, thisNode.X - 1);
                                ProcessNeighbors(thisNode, _backlog, thisNode.Y + 1, thisNode.X);
                                ProcessNeighbors(thisNode, _backlog, thisNode.Y - 1, thisNode.X);
                            }
                        } else stuck = true;
                    } while (!targetReached && !stuck);
                    if (!stuck && bestDistance < bestBestDistance) bestBestDistance = bestDistance;
                    if (Verbose) Console.WriteLine($"x {startNode.X} - y {startNode.Y} - {bestDistance}");
                }
                return FormatResult(bestBestDistance, "best distance");
            }

            private void ProcessNeighbors(Node current, List<Node> backlog, int newY, int newX) {
                if (newX >= 0 && newX < _dim.x && newY >= 0 && newY < _dim.y) {
                    Node newNode = _map[newY][newX];
                    if (newNode.Height - current.Height <= 1 && !newNode.Visited) {
                        newNode.Distance = current.Distance + 1;
                        newNode.Predecessor = current;
                        if (!newNode.Listed) {
                            newNode.Listed = true;
                            backlog.Add(newNode);
                        }
                    }
                }
            }
        }

        public class Node {
            public readonly int X;
            public readonly int Y;
            public readonly int Height;
            public int Distance;
            public bool Visited;
            public bool Listed;
            public Node? Predecessor;

            public Node(int Y, int X, int Height) {
                this.X = X;
                this.Y = Y;
                this.Height = Height;
                Clear();
            }

            public void Clear() {
                Distance = int.MaxValue;
                Visited = false;
                Listed = false;
                Predecessor = null;
            }
        }
    }
}
