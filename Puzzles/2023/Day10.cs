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

            private enum Directions { Nowhere, East, South, West, North };

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve(bool part1) {

                List<Pos> path = new();

                //if (!part1) return "";
                Pos.Field = InputAsLines;
                var dimension = new Pos(InputAsLines.Length - 1, InputAsLines[0].Length - 1);

                // find starting point
                int startRow = InputAsLines.ToList().FindIndex(i => i.Contains('S'));
                var start = new Pos(startRow, InputAsLines[startRow].ToList().IndexOf('S'));

                // pick any direction to go - either way is good
                var pos = start.Go(Directions.Nowhere);
                //Console.WriteLine($"Step {0} --- PosA: r {pos.Row} | c {pos.Col} --- PosB: r {pos.Row} | c {pos.Col}");
                path.Add(pos);
                Directions dir = Directions.Nowhere;
                Directions lastDir = Directions.Nowhere;
                if (start.FromStartCanGo(Directions.North)) dir = Directions.North;
                if (start.FromStartCanGo(Directions.East)) dir = Directions.East;
                if (start.FromStartCanGo(Directions.South)) dir = Directions.South;
                if (start.FromStartCanGo(Directions.West)) dir = Directions.West;
                pos = pos.Go(dir);
                path.Add(pos);
                lastDir = dir;
                int steps = 1;
                //Console.WriteLine($"Step {steps} --- PosA: r {pos.Row} | c {pos.Col} --- PosB: r {pos.Row} | c {pos.Col}");
                do {
                    if (pos.CanGo(Directions.North) && lastDir != Directions.South) dir = Directions.North;
                    if (pos.CanGo(Directions.East) && lastDir != Directions.West) dir = Directions.East;
                    if (pos.CanGo(Directions.South) && lastDir != Directions.North) dir = Directions.South;
                    if (pos.CanGo(Directions.West) && lastDir != Directions.East) dir = Directions.West;
                    pos = pos.Go(dir);
                    path.Add(pos);
                    lastDir = dir;
                    steps++;
                    //Console.WriteLine($"Step {steps} --- PosA: r {pos.Row} | c {pos.Col} --- PosB: r {pos.Row} | c {pos.Col}");
                } while (!(pos.Row == start.Row && pos.Col == start.Col));
                if(part1) return (steps/2).ToString();

                // part2 - hull integral
                int area = 0;
                for (int i = 1; i < path.Count; i++) {
                    area += (path[i].Col) * (path[i].Row - path[i - 1].Row);
                }
                return (Math.Abs(area)-steps/2+1).ToString();

            }




            private class Pos {
                public int Row;
                public int Col;
                public char Pipe;
                public static string[] Field;

                public Pos(int row, int col) {
                    Row = row;
                    Col = col;
                    Pipe = Field[Row][Col];
                }

                public Pos Go(Directions direction) {
                    switch (direction) {
                        case Directions.East:
                            return new Pos(Row, Col + 1);
                        case Directions.South:
                            return new Pos(Row + 1, Col);
                        case Directions.West:
                            return new Pos(Row, Col - 1);
                        case Directions.North:
                            return new Pos(Row - 1, Col);
                        default:
                            return new Pos(Row, Col);
                    }
                }

                public bool FromStartCanGo(Directions direction) {
                    char pipe;
                    switch (direction) {
                        case Directions.East:
                            if (Col == Field[0].Length - 1) return false;
                            pipe = Field[Row][Col + 1];
                            return (pipe == 'J' || pipe == '7' || pipe == '-');
                        case Directions.South:
                            if (Row == Field.Length - 1) return false;
                            pipe = Field[Row + 1][Col];
                            return (pipe == 'J' || pipe == 'L' || pipe == '|');
                        case Directions.West:
                            if (Col == 0) return false;
                            pipe = Field[Row][Col - 1];
                            return (pipe == 'L' || pipe == 'F' || pipe == '-');
                        case Directions.North:
                            if (Row == 0) return false;
                            pipe = Field[Row - 1][Col];
                            return (pipe == 'F' || pipe == '7' || pipe == '|');
                        default:
                            throw new Exception("should not happen");
                    }
                }

                public bool CanGo(Directions direction) {
                    char pipe = Field[Row][Col];
                    switch (direction) {
                        case Directions.West:
                            if (Col == 0) return false;
                            //pipe = Field[Row][Col + 1];
                            return (pipe == 'J' || pipe == '7' || pipe == '-');
                        case Directions.North:
                            if (Row == 0) return false;
                            //pipe = Field[Row + 1][Col];
                            return (pipe == 'J' || pipe == 'L' || pipe == '|');
                        case Directions.East:
                            if (Col == Field[0].Length - 1) return false;
                            //pipe = Field[Row][Col - 1];
                            return (pipe == 'L' || pipe == 'F' || pipe == '-');
                        case Directions.South:
                            if (Row == Field.Length - 1) return false;
                            //pipe = Field[Row - 1][Col];
                            return (pipe == 'F' || pipe == '7' || pipe == '|');
                        default:
                            throw new Exception("should not happen");
                    }
                }

                //public bool AcceptsDirection(Directions direction) {
                //    char pipe = InputAsLines[Row][Col];
                //    switch (direction) {
                //        case Directions.East:
                //        case Directions.South:
                //        case Directions.West:
                //        case Directions.North:
                //        default:
                //            break;
                //    }
                //}
            }
        }
    }
}
