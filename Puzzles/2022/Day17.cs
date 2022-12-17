using System;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;

namespace Puzzles
{
    public partial class Year2022
    {
        public class Day17 : DayBase
        {
            private const int cycles = 1;// 2022;
            private int currentTop;
            private List<Rock> rocks;

            protected override string Title { get; } = "Day 17: Pyroclastic Flow";

            public override void SetupAll()
            {
                AddInputFile(@"2022\17_Example.txt");
                //AddInputFile(@"2022\17_rAiner.txt");
                //AddInputFile(@"2022\17_SEGCC.txt");
            }

            public override void Init(string InputFile) => InputData = ReadFile(InputFile, true);

            public override string Solve(bool Part1)
            {
                if (!Part1) return "";
                //char[,] shapes = new char[,] { { 'x', '<' } };
                //int[,] example3 = new int[,] { { 1, 2, 3 }, { 4, 5, 6 } }; if (!Part1) return "";
                //List<string> chamber = new("+-------+");
                rocks = new();
                currentTop = 0; //floor
                for (int i = 1; i <= cycles; i++)
                {
                    Rock rock = new(i);
                    rock.Y = currentTop + 3 + 1;
                    rock.X = 2;
                    rocks.Add(rock);
                    PrintChamber();
                    bool keepGoing = true;
                    do
                    {
                        keepGoing = RockFalls();
                    } while (true);
                    //chamber.Add("|.......|"); // only add as many as needed
                    //chamber.Add("|.......|");
                    //chamber.Add("|.......|");
                }


                return FormatResult(0, "not yet implemented");
            }

            private bool RockFalls()
            {
                return false;
            }

            private void PrintChamber()
            {
                for (int y = currentTop+8; y >= 0; y--)
                {
                    if (y == 0) Console.WriteLine("+-------+");
                    else
                    {
                        StringBuilder sb = new();
                        for (int x = 0; x < 7; x++)
                        {
                            int isRock = 0;
                            foreach (var rock in rocks)
                            {
                                int yRock = y - rock.Y;
                                int xRock = x - rock.X;
                                if (yRock >= 0 && yRock < rock.Height && xRock >= 0 && xRock < rock.Width)
                                {
                                    if (rock.Shape[yRock, xRock])
                                    {
                                        isRock = 1;
                                        break;
                                    }
                                }
                            }
                            sb.Append(isRock > 0 ? '#' : '.');
                        }
                        Console.WriteLine($"|{sb}|");
                    }
                }
            }

            private class Rock
            {
                //static string[] shapess = new string[] { ".... .... .... #.... ....",
                //                                         ".... .#.. ..#. #.... ....",
                //                                         ".... ###. ..#. #.... ##..",
                //                             y = 0 ----> "#### .#.. ###. #.... ##.."};
                //                                          ^
                //                                          |
                //                                        x = 0
                public bool[,] Shape;
                public bool floating = true;
                public int Y;
                public int X;
                public int Height;
                public int Width;


                public Rock(int cycle)
                {

                    Shape = new bool[4, 4]; // all false
                    switch ((cycle - 1) % 5)
                    {
                        case 0:
                            Height = 1;
                            Width = 4;
                            Shape[0, 0] = true;
                            Shape[0, 1] = true;
                            Shape[0, 2] = true;
                            Shape[0, 3] = true;
                            break;
                        case 1:
                            Height = 3;
                            Width = 3;
                            Shape[0, 1] = true;
                            Shape[1, 0] = true;
                            Shape[1, 1] = true;
                            Shape[1, 2] = true;
                            Shape[2, 1] = true;
                            break;
                        case 2:
                            Height = 3;
                            Width = 3;
                            Shape[0, 0] = true;
                            Shape[0, 1] = true;
                            Shape[0, 2] = true;
                            Shape[1, 2] = true;
                            Shape[2, 2] = true;
                            break;
                        case 3:
                            Height = 4;
                            Width = 1;
                            Shape[0, 0] = true;
                            Shape[1, 0] = true;
                            Shape[2, 0] = true;
                            Shape[3, 0] = true;
                            break;
                        case 4:
                            Height = 2;
                            Width = 2;
                            Shape[0, 0] = true;
                            Shape[0, 1] = true;
                            Shape[1, 0] = true;
                            Shape[1, 1] = true;
                            break;
                        default:
                            throw new Exception(); // can't happen
                    }
                }
            }
        }
    }
}
