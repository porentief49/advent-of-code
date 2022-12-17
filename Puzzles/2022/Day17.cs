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
            private const int cycles = 2022;
            private int currentTop;
            private List<Rock> rocks;

            protected override string Title { get; } = "Day 17: Pyroclastic Flow";

            public override void SetupAll()
            {
                AddInputFile(@"2022\17_Example.txt");
                AddInputFile(@"2022\17_rAiner.txt");
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
                int streamIndex = 0;
                for (int i = 1; i <= cycles; i++)
                {
                    Rock rock = new(i);
                    rock.Y = currentTop + 3 + 1;
                    rock.X = 2;

                    rocks.Add(rock);
                    //PrintChamber();

                    //bool keepGoing = true;
                    //do
                    //{
                    //    keepGoing = RockFalls();
                    //} while (keepGoing);
                    bool push = true;
                    while (rock.Floating)
                    {
                        RockFalling(rock, ref push, ref streamIndex);
                        //PrintChamber();
                    }
                    //chamber.Add("|.......|"); // only add as many as needed
                    //chamber.Add("|.......|");
                    //chamber.Add("|.......|");
                    currentTop = Math.Max(currentTop, rock.Y + rock.Height - 1);
                }

                return FormatResult(currentTop, "rock tower height");
            }

            private void RockFalling(Rock rock, ref bool push, ref int streamIndex)
            {
                int yCache = rock.Y;
                int xCache = rock.X;

                // try step
                if (push) // streams push sideways
                {
                    if (InputData[0][streamIndex++ % InputData[0].Length] == '>') rock.X++;
                    else rock.X--;
                }
                else // fall down
                {
                    rock.Y--;
                }
                bool collision = false;
                for (int y = 0; y < rock.Height; y++)
                {
                    for (int x = 0; x < rock.Width; x++)
                    {
                        if (rock.Shape[y, x])
                        {
                            if (CheckIntersect(rock.Y + y, rock.X + x, false) > 0) collision = true;
                        }
                    }
                }

                if (collision)
                {  // didn't work
                    rock.Y = yCache;
                    rock.X = xCache;
                    if (!push) rock.Floating = false; // can only rest if falling
                }
                push = !push;
            }

            private void PrintChamber()
            {
                //for (int y = currentTop + 8; y >= 0; y--)
                for (int y = rocks.Last().Y + rocks.Last().Height - 1; y >= 0; y--)
                {
                    StringBuilder sb = new();
                    for (int x = -1; x <= 7; x++)
                    {
                        //int isRock = 0;
                        //foreach (var rock in rocks)
                        //{
                        //    int yRock = y - rock.Y;
                        //    int xRock = x - rock.X;
                        //    if (yRock >= 0 && yRock < rock.Height && xRock >= 0 && xRock < rock.Width)
                        //    {
                        //        if (rock.Shape[yRock, xRock])
                        //        {
                        //            isRock = 1;
                        //            break;
                        //        }
                        //    }
                        //}
                        sb.Append(CheckIntersect(y, x, true) switch { 1 => '#', 2 => '@', 3 => '|', 4 => '=', _ => '.' });
                    }
                    Console.WriteLine($"{sb}");
                }
                Console.WriteLine(string.Empty);
            }

            private int CheckIntersect(int y, int x, bool includeFloating)
            {
                // 0 -> good
                // 1 -> other rock
                // 2 -> floating rock
                // 3 -> wall
                // 4 -> floor
                if (x < 0 || x > 6) return 3;
                if (y <= 0) return 4;

                foreach (var rock in rocks)

                    //check other rocks
                    if (includeFloating || !rock.Floating)
                    {
                        {
                            int yRock = y - rock.Y;
                            int xRock = x - rock.X;
                            if (yRock >= 0 && yRock < rock.Height && xRock >= 0 && xRock < rock.Width) if (rock.Shape[yRock, xRock]) return rock.Floating ? 2 : 1;
                        }
                    }
                return 0;
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
                public bool Floating = true;
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
