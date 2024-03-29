﻿using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Serialization;

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

            public override string Solve() {
                List<Pos> path = new();
                Pos.Field = InputAsLines;

                // find start pos
                int startRow = InputAsLines.ToList().FindIndex(i => i.Contains('S'));
                int startCol = InputAsLines[startRow].ToList().IndexOf('S');
                path.Add(new Pos(startRow, startCol, null));

                // first step
                if ("-7J".Contains(InputAsLines[startRow][startCol + 1])) path.Add(new Pos(startRow, startCol + 1, path.Last()));
                else if ("|JL".Contains(InputAsLines[startRow + 1][startCol])) path.Add(new Pos(startRow + 1, startCol, path.Last()));
                else path.Add(new Pos(startRow, startCol - 1, path.Last())); // if the other two didn't work, the third must

                // all other steps
                do path.Add(path.Last().Next());
                while (!(path.Last().Row == path[0].Row && path.Last().Col == path[0].Col));
                //Console.WriteLine($"{string.Join("\r\n", path.Select((s, i) => $"Step {i} --- Pos: r {s.Row} | c {s.Col} "))}");
                var farthest = path.Count / 2;
                if (Part1) return farthest.ToString();

                // part2 - area integral
                int area = 0;
                for (int i = 1; i < path.Count; i++) area += (path[i].Col) * (path[i].Row - path[i - 1].Row);
                return (Math.Abs(area) - farthest + 1).ToString();
            }

            private class Pos {
                public static string[] Field;
                public int Row;
                public int Col;
                private Pos? _cameFrom = null;

                public Pos(int row, int col, Pos? cameFrom) {
                    Row = row;
                    Col = col;
                    _cameFrom = cameFrom;
                }

                public Pos Next() {
                    int row = Row;
                    int col = Col;
                    switch (Field[Row][Col]) {
                        case '|':
                            row += _cameFrom?.Row == Row + 1 ? -1 : 1;
                            break;
                        case '-':
                            col += _cameFrom?.Col == Col + 1 ? -1 : 1;
                            break;
                        case 'L':
                            if (_cameFrom?.Row == Row && _cameFrom.Col == Col + 1) row -= 1;
                            else col += 1;
                            break;
                        case 'J':
                            if (_cameFrom?.Row == Row && _cameFrom.Col == Col - 1) row -= 1;
                            else col -= 1;
                            break;
                        case '7':
                            if (_cameFrom?.Row == Row && _cameFrom.Col == Col - 1) row += 1;
                            else col -= 1;
                            break;
                        case 'F':
                            if (_cameFrom?.Row == Row && _cameFrom.Col == Col + 1) row += 1;
                            else col += 1;
                            break;
                        default:
                            break;
                    }
                    return new Pos(row, col, this);
                }
            }
        }
    }
}
