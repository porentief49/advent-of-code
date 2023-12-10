using System.Reflection.Metadata.Ecma335;

namespace Puzzles {

    public partial class Year2023 {

        public class Day10 : DayBase {

            protected override string Title { get; } = "Day 10: Pipe Maze";

            public override void SetupAll() {
                AddInputFile(@"2023\10_Example1.txt");
                AddInputFile(@"2023\10_Example2.txt");
                AddInputFile(@"2023\10_rAiner.txt");
            }

            private enum Directions { Nowhere, East, South, West, North };

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve(bool part1) {

                List<Pos> path = new();

                if (!part1) return "";
                Pos.Field = InputAsLines;
                var dimension = new Pos(InputAsLines.Length - 1, InputAsLines[0].Length - 1);

                // find starting point
                int startRow = InputAsLines.ToList().FindIndex(i => i.Contains('S'));
                var start = new Pos(startRow, InputAsLines[startRow].ToList().IndexOf('S'));

                // find two directions to go to

                var posA = start.Go(Directions.Nowhere);
                Directions dirA = Directions.Nowhere;
                Directions dirAlast = Directions.Nowhere;
                if (start.FromStartCanGo(Directions.North)) dirA = Directions.North;
                if (start.FromStartCanGo(Directions.East)) dirA = Directions.East;
                if (start.FromStartCanGo(Directions.South)) dirA = Directions.South;
                if (start.FromStartCanGo(Directions.West)) dirA = Directions.West;
                posA = posA.Go(dirA);
                dirAlast = dirA;
                int steps = 1;
                do {
                    if (posA.CanGo(Directions.North) && dirAlast != Directions.South) dirA = Directions.North;
                    if (posA.CanGo(Directions.East) && dirAlast != Directions.West) dirA = Directions.East;
                    if (posA.CanGo(Directions.South) && dirAlast != Directions.North) dirA = Directions.South;
                    if (posA.CanGo(Directions.West) && dirAlast != Directions.East) dirA = Directions.West;
                    posA = posA.Go(dirA);
                    dirAlast = dirA;
                    steps++;
                    //Console.WriteLine($"Step {steps} --- PosA: r {posA.Row} | c {posA.Col} --- PosB: r {posB.Row} | c {posB.Col}");
                } while (!(posA.Row == start.Row && posA.Col == start.Col));
                return (steps/2).ToString();
             
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
