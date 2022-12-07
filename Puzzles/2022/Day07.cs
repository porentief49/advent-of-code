using System;
using System.Net.Sockets;
using System.Text;

namespace Puzzles
{
    public partial class Year2022
    {
        public class Day07 : DayBase
        {
            protected override string Title { get; } = "Day 7: No Space Left On Device";

            public override void SetupAll()
            {
                AddInputFile(@"2022\07_Example.txt");
                AddInputFile(@"2022\07_rAiner.txt");
                AddInputFile(@"2022\07_SEGCC.txt");
            }

            public override void Init(string InputFile) => InputData = ReadFile(InputFile, true);

            public override string Solve(bool Part1)
            {
                const long _totalFileSystemSize = 70000000;
                const long _updateSize = 30000000;
                FileSystemElement _fileSystem = new("/", null, 0);
                FileSystemElement _root = _fileSystem;
                FileSystemElement _curDir = _root;
                List<long> _dirSizes = new();
                for (int i = 0; i < InputData?.Length; i++)
                {
                    string[] _split = InputData[i].Split(' ');
                    if (_split[0] == "$") // command
                    {
                        if (_split[1] == "cd") _curDir = _split[2] switch { "/" => _root, ".." => _curDir.ParentDir ?? _root, _ => _curDir.SubElements.First(x => x.Name == _split[2]) };
                    }
                    else _curDir?.SubElements.Add(new(_split[1], _curDir, _split[0] == "dir" ? 0 : long.Parse(_split[0]))); // listing
                }
                _root.CalcAllSizes(_dirSizes, Verbose);
                if (Part1) return FormatResult(_dirSizes.Where(x => x <= 100000).Sum(), "total folder sizes");
                long _spaceLeft = _totalFileSystemSize - _root.CalcSize();
                long _freeUp = _updateSize - _spaceLeft;
                if (Verbose) Console.WriteLine($"current space left: {_spaceLeft}, need to free up {_freeUp}");
                long _justEnough = _dirSizes.OrderBy(x => x).First(x => x >= _freeUp);
                return FormatResult(_justEnough, "folder size");
            }

            public class FileSystemElement
            {
                public List<FileSystemElement> SubElements = new();
                public string Name;
                public long Size;
                public FileSystemElement? ParentDir;

                public FileSystemElement(string Name, FileSystemElement? ParentDir, long Size)
                {
                    this.Name = Name;
                    this.ParentDir = ParentDir;
                    this.Size = Size;
                }

                public void CalcAllSizes(List<long> DirSizes, bool Verbose, int Level = 0)
                {
                    StringBuilder _sb = new("  ".Repeat(Level) + " - " + Name + (Size == 0 ? " (dir)" : $" (file, size={Size})"));
                    if (Size == 0)
                    {
                        long _size = CalcSize();
                        DirSizes.Add(_size);
                        _sb.Append($" -------------> {_size} {(_size <= 100000 ? " (qualifies for Part 1)" : string.Empty)}");
                    }
                    if (Verbose) Console.WriteLine(_sb.ToString());
                    SubElements.ForEach(x => x.CalcAllSizes(DirSizes, Verbose, Level + 1));
                }

                public long CalcSize() => Size > 0 ? Size : SubElements.Where(x => x.Size > 0).Select(x => x.Size).Sum() + SubElements.Where(x => x.Size == 0).Select(x => x.CalcSize()).Sum();
            }
        }
    }
}
