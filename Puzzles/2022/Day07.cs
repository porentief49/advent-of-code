using System;
using System.Net.Sockets;
using System.Text;

namespace Puzzles {
    public partial class Year2022 {
        public class Day07 : DayBase {
            const long _folderSizeLimit = 100000;
            const long _totalFileSystemSize = 70000000;
            const long _updateSize = 30000000;

            protected override string Title { get; } = "Day 7: No Space Left On Device";

            public override void SetupAll() {
                AddInputFile(@"2022\07_Example.txt");
                AddInputFile(@"2022\07_rAiner.txt");
                AddInputFile(@"2022\07_SEGCC.txt");
            }

            public override void Init(string InputFile) => InputData = ReadFile(InputFile, true);

            public override string Solve(bool Part1) {
                FileSystemElement _fileSystem = new("/", true, null, -1);
                FileSystemElement _root = _fileSystem;
                FileSystemElement _curFolder = _root;
                for (int i = 0; i < InputData?.Length; i++) {
                    string[] _split = InputData[i].Split(' ');
                    if (_split[0] != "$") _curFolder?.SubElements.Add(new(_split[1], _split[0] == "dir", _curFolder, _split[0] == "dir" ? -1 : long.Parse(_split[0]))); // listing
                    else if (_split[1] == "cd") _curFolder = _split[2] switch { "/" => _root, ".." => _curFolder.Parent ?? _root, _ => _curFolder.SubElements.First(x => x.Name == _split[2]) }; // command // ls doesn't really have any effect
                }
                List<long> _folderSizes = new();
                _root.CalcFolderSizes();
                _root.CalcFolderSizeList(_folderSizes);
                if (Verbose) _root.List();
                if (Part1) return FormatResult(_folderSizes.Where(x => x <= _folderSizeLimit).Sum(), "total folder sizes");
                return FormatResult(_folderSizes.OrderBy(x => x).First(x => x >= _updateSize - (_totalFileSystemSize - _root.CalcFolderSizes())), "folder size");
            }

            public class FileSystemElement {
                public List<FileSystemElement> SubElements { get; } = new();

                public string Name { get; private set; }
                public bool IsFolder { get; private set; }
                public long Size { get; private set; }
                public FileSystemElement? Parent { get; private set; }

                public FileSystemElement(string Name, bool IsFolder, FileSystemElement? Parent, long Size) {
                    this.Name = Name;
                    this.IsFolder = IsFolder;
                    this.Parent = Parent;
                    this.Size = Size;
                }

                public long CalcFolderSizes() {
                    if (IsFolder && Size == -1) Size = SubElements.Where(x => !x.IsFolder).Select(x => x.Size).Sum() + SubElements.Where(x => x.IsFolder).Select(x => x.CalcFolderSizes()).Sum();
                    return Size;
                }

                public void CalcFolderSizeList(List<long> DirSizes) {
                    if (IsFolder) DirSizes.Add(Size);
                    SubElements.ForEach(x => x.CalcFolderSizeList(DirSizes));
                }

                public void List(int Level = 0) {
                    Console.WriteLine($"{"  ".Repeat(Level)} - " + $"{Name} ({(IsFolder ? "dir" : "file")}, size={Size}){(IsFolder && Size <= _folderSizeLimit ? " -------------> qualifies for Part 1" : string.Empty)}");
                    SubElements.ForEach(x => x.List(Level + 1));
                }
            }
        }
    }
}
