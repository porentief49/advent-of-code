using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace Puzzles {

    public partial class Year2023 {

        public class Day10 : DayBase {

            protected override string Title { get; } = "Day 10: Pipe Maze";

            public override void SetupAll() {
                AddInputFile(@"2023\10_Example1.txt");
                AddInputFile(@"2023\10_Example2.txt");
                AddInputFile(@"2023\10_Example3.txt");
                AddInputFile(@"2023\10_Example4.txt");
                AddInputFile(@"2023\10_Example5.txt");
                AddInputFile(@"2023\10_Example6.txt");
                AddInputFile(@"2023\10_rAiner.txt");
            }

            private enum Dir { Nowhere, East, South, West, North };

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve(bool part1) {
                List<Pos> path = new();
                Pos.Field = InputAsLines;
                int startRow = InputAsLines.ToList().FindIndex(i => i.Contains('S'));
                var startPos = new Pos(startRow, InputAsLines[startRow].ToList().IndexOf('S'));
                var pos = startPos.Go(Dir.Nowhere);
                path.Add(pos);
                Dir dir = Dir.Nowhere;
                Dir prevDir = Dir.Nowhere;
                if (startPos.Go(Dir.East).CanGo(Dir.West)) dir = Dir.East;
                else if (startPos.Go(Dir.South).CanGo(Dir.North)) dir = Dir.South;
                else dir = Dir.West; // third has to work if other two (random) failed
                pos = pos.Go(dir);
                path.Add(pos);
                prevDir = dir;
                do {
                    if (pos.CanGo(Dir.North) && prevDir != Dir.South) dir = Dir.North;
                    if (pos.CanGo(Dir.East) && prevDir != Dir.West) dir = Dir.East;
                    if (pos.CanGo(Dir.South) && prevDir != Dir.North) dir = Dir.South;
                    if (pos.CanGo(Dir.West) && prevDir != Dir.East) dir = Dir.West;
                    pos = pos.Go(dir);
                    path.Add(pos);
                    prevDir = dir;
                } while (!(pos.Row == startPos.Row && pos.Col == startPos.Col));
                //Console.WriteLine($"{string.Join("\r\n", path.Select((s, i) => $"Step {i} --- PosA: r {s.Row} | c {s.Col} "))}");
                var farthest = path.Count / 2;
                if (part1) return farthest.ToString();

                // part2 - area integral
                int area = 0;
                for (int i = 1; i < path.Count; i++) area += (path[i].Col) * (path[i].Row - path[i - 1].Row);
                return (Math.Abs(area) - farthest + 1).ToString();
            }




            private class Pos {
                public int Row;
                public int Col;
                public char Pipe;
                public static string[] Field;

                public Pos(int row, int col) {
                    Row = row;
                    Col = col;
                }

                public Pos Go(Dir direction) {
                    if (direction == Dir.East) return new Pos(Row, Col + 1);
                    if (direction == Dir.South) return new Pos(Row + 1, Col);
                    if (direction == Dir.West) return new Pos(Row, Col - 1);
                    if (direction == Dir.North) return new Pos(Row - 1, Col);
                    return new Pos(Row, Col);
                }

                public bool CanGo(Dir direction) {
                    if (direction == Dir.West) return Col > 0 && "J7-".Contains(Field[Row][Col]);
                    if (direction == Dir.North) return Row > 0 && "JL|".Contains(Field[Row][Col]);
                    if (direction == Dir.East) return Col < (Field[0].Length - 1) && "LF-".Contains(Field[Row][Col]);
                    if (direction == Dir.South) return Row < (Field.Length - 1) && "F7|".Contains(Field[Row][Col]);
                    throw new Exception("should not happen");
                }
            }
        }
    }
}
