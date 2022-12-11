using System;
using System.Data.Common;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
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
                long modulo = 2 * 3 * 5 * 7 * 11 * 13 * 17 * 19 * 23;
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
                            if (item < 0)
                            {
                                Console.WriteLine(item);
                            }
                            monkey.InspectCount++;
                            if (Part1) item = (long)Math.Floor(item / 3.0); // relief
                            else item %= modulo; 
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
        public class Day11old : DayBase
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
                //List<long> x72 = Monkey.Factorize(72);
                //List<long> x76 = Monkey.Add(x72, 4);

                //return "";
                //if (!Part1) return "";

                // parsing
                List<Monkey> monkeys = new();
                for (int i = 0; i < InputData.Length / 6; i++) monkeys.Add(new Monkey(InputData.Skip(i * 6).Take(6).ToArray()));
                //if (Part1) PrintRound(monkeys, 0);

                // throw rounds
                int rounds = Part1 ? 20 : 1000;
                for (int i = 0; i < rounds; i++)
                {
                    //if (Part1) Console.WriteLine($"Round {i}");
                    foreach (Monkey monkey in monkeys)
                    {
                        //if (Part1) Console.WriteLine($"Monkey {monkey.Id}");
                        while (monkey.Items.Count > 0)
                        {
                            List<long> item = monkey.Items.Dequeue();
                            //if (Part1) Console.WriteLine($"Item {Monkey.DeFactorize(item)}");

                            //item = monkey.Operator switch { '+' => item + monkey.Operand, '-' => item - monkey.Operand, '*' => item * monkey.Operand, '/' => item / monkey.Operand, _ => item * item }; //default = '^'
                            switch (monkey.Operator)
                            {
                                case '+':
                                    //item = item + monkey.Operand;
                                    item = Monkey.Add(item, monkey.Operand);
                                    break;
                                case '*':
                                    //item = item * monkey.Operand;
                                    item.Add(monkey.Operand);
                                    break;
                                case '^':
                                    //item = item * item;
                                    item.AddRange(item);
                                    break;
                                case '/':
                                //item = item / monkey.Operand; // not implemented for PArt2
                                //break;
                                case '-':
                                //item = item - monkey.Operand; // not implemented for PArt2
                                //break;
                                default:
                                    throw new Exception($"operator {monkey.Operator} not implemented!");
                            }
                            monkey.InspectCount++;
                            if (Part1) item = Monkey.Factorize((long)Math.Floor(Monkey.DeFactorize(item) / 3.0)); // relief
                            int throwTo = item.Contains(monkey.DivisibleBy) ? monkey.ThrowTrue : monkey.ThrowFalse;
                            monkeys[throwTo].Items.Enqueue(item);
                        }
                    }
                    //if (Part1) PrintRound(monkeys, i + 1);
                    if (!Part1 && i % 100 == 0) PrintInspectionCounts(monkeys, i + 1);
                }

                // evaluate
                long[] inspectCounts = monkeys.Select(x => x.InspectCount).OrderByDescending(x => x).ToArray();

                return FormatResult(inspectCounts[0] * inspectCounts[1], "monkey business level");
            }

            public void PrintLevels(List<Monkey> monkeys, int Round)
            {
                Console.WriteLine($"Afer round {Round}, the monkeys are holding items with these worry levels:");
                foreach (var monkey in monkeys) monkey.Print();
            }

            public void PrintInspectionCounts(List<Monkey> monkeys, int Round)
            {
                Console.WriteLine($"== Afer round {Round} ==");
                foreach (var monkey in monkeys) Console.WriteLine($"Monkey {monkey.Id} inspected items {monkey.InspectCount} times.");

            }


            public class Monkey
            {
                public int Id;
                public Queue<List<long>> Items;
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
                    foreach (var item in Input[1].Replace(",", string.Empty).Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(2)) Items.Enqueue(Factorize(long.Parse(item)));
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

                //public static List<long> Factorize(long Value)
                //{
                //    return new List<long>();
                //}

                //public static List<long> Factorize(long number) //taken from: https://stackoverflow.com/questions/5872962/prime-factors-in-c-sharp
                //{
                //    var primes = new List<long>();
                //    for (long div = 2; div <= number; div++)
                //        while (number % div == 0)
                //        {
                //            primes.Add(div);
                //            number = number / div;
                //        }
                //    if (primes.Count == 0) primes.Add(number);
                //    return primes;
                //}


                public static List<long> Factorize(long n)
                {
                    var primes = new List<long>();
                    while (n % 2 == 0)
                    {
                        //Console.Write(2 + " ");
                        primes.Add(2);
                        n /= 2;
                    }

                    for (long i = 3; i <= Math.Sqrt(n); i += 2)
                    {
                        while (n % i == 0)
                        {
                            //Console.Write(i + " ");
                            primes.Add(i);
                            n /= i;
                        }
                    }
                    //if (n > 2) Console.Write(n);
                    if (n > 2) primes.Add(n);
                    return primes;
                }




                public static long DeFactorize(List<long> Values)
                {
                    long result = 1;
                    foreach (var value in Values) result *= value;
                    return result;
                }

                public static List<long> Add(List<long> Base, long Value)
                {
                    // 72 + 4
                    // (2 * 2 * 2 * 3 * 3) + (2 * 2)
                    // (2 * 2) * ((2 * 3 * 3) + 1)
                    // 4 * (18 + 1)
                    // 76

                    List<long> adder = Factorize(Value);
                    List<long> smaller;
                    List<long> smallerCopy;
                    List<long> larger;
                    List<long> remain1 = new();
                    List<long> remain2 = new();
                    List<long> common = new();
                    if (adder.Count < Base.Count)
                    {
                        smaller = adder.Select(x => x).ToList(); // copy
                        larger = Base.Select(x => x).ToList(); // copy
                    }
                    else
                    {
                        smaller = Base.Select(x => x).ToList(); // copy
                        larger = adder.Select(x => x).ToList(); // copy
                    }
                    smallerCopy = smaller.Select(x => x).ToList(); // copy
                    for (int i = 0; i < smallerCopy.Count; i++)
                    {
                        long current = smallerCopy[i];
                        if (larger.Contains(current))
                        {
                            common.Add(current);
                            smaller.Remove(current);
                            larger.Remove(current);
                        }
                    }
                    long _product = DeFactorize(smaller) + DeFactorize(larger);
                    common.AddRange(Factorize(_product)); // that's not common anymore, get new name
                    return common.ToList();

                }

                public void Print() => Console.WriteLine($"Monkey {Id}: {string.Join(", ", Items.Select(x => DeFactorize(x).ToString()))}");
            }
        }
    }
}
