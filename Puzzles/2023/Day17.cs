namespace Puzzles {

    public partial class Year2023 {

        public class Day17 : DayBase {

            protected override string Title { get; } = "Day 17: Clumsy Crucible";

            public override void SetupAll() {
                AddInputFile(@"2023\17_Example.txt");
                //AddInputFile(@"2023\17_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve() {
                // a* algorithm
                List<Node> open = new();
                //List<Node> closed = new();
                open.Add(new Node() { Row = 0, Col = 0 });

                int count = 0;
                bool done = false;
                do {
                    Node current = open.Min();
                    open.Remove(current);
                    if (current.Row == InputAsLines.Length - 1 && current.Col == InputAsLines[0].Length - 1) return "path found";

                    // expand node


                } while (count++ < 1000 && done == false); ;

                return "";
            }

            private class Node : IComparable {
                public int Row;
                public int Col;
                //public int HeatLoss;
                public int SoFar;
                public Node Pre;

                public int CompareTo(object? obj) => SoFar.CompareTo((obj as Node)?.SoFar);
            }
        }
    }
}
