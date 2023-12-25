namespace Puzzles {

    public partial class Year2023 {

        public class Day25 : DayBase {

            protected override string Title { get; } = "Day 25: Snowverload";

            public override void SetupAll() {
                //AddInputFile(@"2023\25_Example.txt");
                AddInputFile(@"2023\25_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            List<(string, string)> _connections = new();
            List<string> _points = new();

            public override string Solve() {
                if (Part2) return "";
                foreach (var line in InputAsLines) {
                    var split = line.Split(':', StringSplitOptions.TrimEntries);
                    _points.AddIfNew(split[0]);
                    foreach (var wire in split[1].Split(' ')) {
                        _connections.Add((split[0], wire));
                        _points.AddIfNew(wire);
                    }
                }

                //foreach (var item in _connections) {
                //    Console.WriteLine($"{item.Item1}, {item.Item2}, 1");
                //}

                //foreach (var item in _points) {
                //    Console.WriteLine($"{item}");
                //}
                //return "";

                //// the connections we're looking for are:
                //// Example: hfx/pzl, bvb/cmg, nvd/jqt
                //Console.WriteLine($"{_connections.Count} connections before the cut, 1");
                //_connections.RemoveAll(c => c == ("hfx", "pzl"));
                //_connections.RemoveAll(c => c == ("pzl", "hfx"));
                //_connections.RemoveAll(c => c == ("bvb", "cmg"));
                //_connections.RemoveAll(c => c == ("cmg", "bvb"));
                //_connections.RemoveAll(c => c == ("nvd", "jqt"));
                //_connections.RemoveAll(c => c == ("jqt", "nvd"));
                //Console.WriteLine($"{_connections.Count} connections after the cut, 1");

                // using https://app.flourish.studio/visualisation/16256681/edit
                // the connections we're looking for are:
                // rAiner: xnn/txf, nhg/jjn, tmc/lms
                Console.WriteLine($"{_connections.Count} connections before the cut, 1");
                _connections.RemoveAll(c => c == ("xnn", "txf"));
                _connections.RemoveAll(c => c == ("txf", "xnn"));
                _connections.RemoveAll(c => c == ("nhg", "jjn"));
                _connections.RemoveAll(c => c == ("jjn", "nhg"));
                _connections.RemoveAll(c => c == ("tmc", "lms"));
                _connections.RemoveAll(c => c == ("lms", "tmc"));
                Console.WriteLine($"{_connections.Count} connections after the cut, 1");
                //return "";

                // pick one and see how many it connects to
                List<string> found = new();
                Queue<string> toGo = new();
                toGo.Enqueue(_points[0]);
                do {
                    string current = toGo.Dequeue();
                    found.Add(current);
                    Console.WriteLine($"Node: {current}");
                    foreach (var conn in _connections) {
                        if (conn.Item1 == current) if (!toGo.Contains(conn.Item2) && !found.Contains(conn.Item2)) toGo.Enqueue(conn.Item2);
                        if (conn.Item2 == current) if (!toGo.Contains(conn.Item1) && !found.Contains(conn.Item1)) toGo.Enqueue(conn.Item1);
                    }
                } while (toGo.Any());
                return (found.Count * (_points.Count-found.Count)).ToString();
            }
        }
    }
}
