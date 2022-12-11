using System;
using System.Data.Common;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using static Puzzles.Year2022.Day11;

namespace Puzzles
{
    public partial class Year2022
    {

        public class Day11 : DayBase
        {
            protected override string Title { get; } = "Day 11: Monkey in the Middle";

            public override void SetupAll()
            {
                AddInputFile(@"2022\11_Example.txt");
                //AddInputFile(@"2022\11_rAiner.txt");
                //AddInputFile(@"2022\11_SEGCC.txt");
            }

            public override void Init(string InputFile) => InputData = ReadFile(InputFile, true);

            public override string Solve(bool Part1)
            {
                // parsing
                List<Monkey> monkeys = new();
                for (int i = 0; i < InputData.Length / 6; i++) monkeys.Add(new Monkey(InputData.Skip(i * 6).Take(6).ToArray()));
                if (Part1) PrintRound(monkeys, 0);

                // throw rounds
                int rounds = Part1 ? 20 : 10000;
                for (int i = 0; i < rounds; i++)
                {
                    foreach (Monkey monkey in monkeys)
                    {
                        while (monkey.Items.Count > 0)
                        {
                            long item = monkey.Items.Dequeue();
                            item = monkey.Operator switch { '+' => item + monkey.Operand, '-' => item - monkey.Operand, '*' => item * monkey.Operand, '/' => item / monkey.Operand, _ => item * item }; //default = '^'
                            if (item<0)
                            {
                                Console.WriteLine(item);
                            }
                            monkey.InspectCount++;
                            if(Part1) item = (long)Math.Floor(item / 3.0); // relief
                            int throwTo = (item % monkey.DivisibleBy == 0) ? monkey.ThrowTrue : monkey.ThrowFalse;
                            monkeys[throwTo].Items.Enqueue(item);
                        }
                    }
                    if (Part1) PrintRound(monkeys, i + 1);
                }

                // evaluate
                long[] inspectCounts = monkeys.Select(x => x.InspectCount).OrderByDescending(x => x).ToArray();

                return FormatResult(inspectCounts[0] * inspectCounts[1], "monkey business level");
            }

            public void PrintRound(List<Monkey> monkeys, int Round)
            {
                Console.WriteLine($"Afer round {Round}, the monkeys are holding items with these worry levels:");
                foreach (var monkey in monkeys) monkey.Print();
            }

            public class Monkey
            {
                public int Id;
                public Queue<long> Items;
                public char Operator;
                public int Operand;
                public int DivisibleBy;
                public int ThrowTrue;
                public int ThrowFalse;
                public long InspectCount = 0;

                public Monkey(string[] Input)
                {
                    Id = int.Parse(Input[0].Replace(":", string.Empty).Split(' ')[1]);
                    Items = new();
                    foreach (var item in Input[1].Replace(",", string.Empty).Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(2)) Items.Enqueue(long.Parse(item));
                    string[] _split = Input[2].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (_split[5] == "old")
                    {
                        Operator = '^';
                        Operand = 2;
                    }
                    else
                    {
                        Operator = _split[4][0];
                        Operand = int.Parse(_split[5]);
                    }
                    DivisibleBy = int.Parse(Input[3].Split(' ', StringSplitOptions.RemoveEmptyEntries).Last());
                    ThrowTrue = int.Parse(Input[4].Split(' ', StringSplitOptions.RemoveEmptyEntries).Last());
                    ThrowFalse = int.Parse(Input[5].Split(' ', StringSplitOptions.RemoveEmptyEntries).Last());
                }

                public void Print() => Console.WriteLine($"Monkey {Id}: {string.Join(", ", Items.Select(x => x.ToString()))}");
            }
        }
    }
}
