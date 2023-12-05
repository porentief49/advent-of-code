namespace Puzzles {

    public partial class Year2023 {

        public class Day01 : DayBase {

            protected override string Title { get; } = "Day 1: Trebuchet?!";
            private const string numbers = "123456789";

            public override void SetupAll() {
                AddInputFile(@"2023\01_Example1.txt");
                AddInputFile(@"2023\01_Example2.txt");
                AddInputFile(@"2023\01_rAiner.txt");
            }

            public override void Init(string InputFile) => InputData = ReadLines(InputFile, true);

            public override string Solve(bool Part1) {
                var preprocess = (Part1 ? InputData : InputData.Select(i => Words2Digits(i)));
                var filtered = preprocess.Select(p => p.Where(c => numbers.Contains(c)));
                var calValues = filtered.Select(f => int.Parse($"{f.FirstOrDefault('0')}{f.LastOrDefault('0')}"));
                return calValues?.Aggregate((x, y) => x + y).ToString() ?? string.Empty;
            }

            private string Words2Digits(string input) {
                var words = "one|two|three|four|five|six|seven|eight|nine".Split('|');
                for (var i = 0; i < words.Length; i++) input = input.Replace(words[i], $"{words[i]}{numbers[i]}{words[i]}".ToString());
                return input;
            }
        }
    }
}
