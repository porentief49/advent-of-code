using System;
using System.Net.Sockets;
using System.Text;

namespace Puzzles
{
    public partial class Year2022
    {
        public class Day10 : DayBase
        {
            protected override string Title { get; } = "Day 10: Cathode-Ray Tube";

            public override void SetupAll()
            {
                //AddInputFile(@"2022\10_Example1.txt");
                AddInputFile(@"2022\10_Example2.txt");
                AddInputFile(@"2022\10_rAiner.txt");
                //AddInputFile(@"2022\10_SEGCC.txt");
            }

            public override void Init(string InputFile) => InputData = ReadFile(InputFile, true);

            public override string Solve(bool Part1)
            {
                int _xReg = 1;
                List<int> _xRegValues = new() { _xReg , _xReg }; // to get the index right
                for (int i = 0; i < InputData.Length; i++)
                {
                    string[] _split = InputData[i].Split(' ');
                    if (_split[0] == "noop") _xRegValues.Add(_xReg);
                    else
                    {
                        _xRegValues.Add(_xReg);
                        _xReg += int.Parse(_split[1]);
                        _xRegValues.Add(_xReg);
                    }
                }

                if(Part1 && Verbose) for (int i =1; i < _xRegValues.Count; i++) Console.WriteLine($"during cycle {i} xreg is {_xRegValues[i]}");

                // determine solution
                int _index = 20;
                int _signalStrength = 0;
                while (_index < _xRegValues.Count)
                {
                    _signalStrength += _xRegValues[_index] * _index;
                    _index += 40;
                }

                return FormatResult(_signalStrength, "signal strength");
            }
        }
    }
}
