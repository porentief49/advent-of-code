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
                //AddInputFile(@"2022\07_Example.txt");
                AddInputFile(@"2022\07_rAiner.txt");

                //1681397 is too high for part 1


                //AddInputFile(@"2022\06_Example3.txt");
                //AddInputFile(@"2022\06_Example4.txt");
                //AddInputFile(@"2022\06_Example5.txt");
                //AddInputFile(@"2022\06_rAiner.txt");
                //AddInputFile(@"2022\06_SEGCC.txt");
            }

            public override void Init(string InputFile) => InputData = ReadFile(InputFile, true);

            private FileElement _fileSystem = new("/", null, 0);
            private FileElement _root;
            private FileElement _curDir;
            private List<long> _dirSizes = new();

            public override string Solve(bool Part1)
            {
                if (Part1)
                {
                    _root = _fileSystem;
                    _curDir = _root;
                    for (int i = 0; i < InputData.Length; i++)
                    {
                        string[] _split = InputData[i].Split(' ');
                        if (_split[0] == "$") // command
                        {
                            switch (_split[1])
                            {
                                case "cd":
                                    switch (_split[2])
                                    {
                                        case "/":
                                            _curDir = _root;
                                            break;
                                        case "..":
                                            _curDir = _curDir.ParentDir;
                                            break;
                                        default:
                                            {
                                                bool lFound = false;
                                                foreach (var _subElement in _curDir.SubElements)
                                                {
                                                    if (_subElement.Name == _split[2])
                                                    {
                                                        _curDir = _subElement;
                                                        lFound = true;
                                                        break;
                                                    }
                                                }
                                                if (!lFound) throw new Exception($"sub dir {_split[2]} not found");
                                            }
                                            break;
                                    }
                                    break;
                                case "ls":
                                    //hmm, nothing really to do here, files&folders should be parsed automatically
                                    break;
                                default:
                                    throw new Exception($"{_split[1]} not implemented!");
                            }
                        }
                        else // listing
                        {
                            if (_split[0] == "dir")
                            {
                                _curDir.SubElements.Add(new(_split[1], _curDir, 0));
                            }
                            else
                            {
                                _curDir.SubElements.Add(new(_split[1], _curDir, long.Parse(_split[0])));
                            }
                        }
                    }
                    _root.GetSizes(_dirSizes);
                }
                if (Part1) return FormatResult(_dirSizes.Where(x => x <= 100000).Sum(), "total folder sizes");
                else
                {
                    const long _totalSize = 70000000;
                    const long _updateSite = 30000000;
                    long _spaceLeft = _totalSize- _root.CalcSize();
                    long _freeUp = _updateSite - _spaceLeft;
                    Console.WriteLine($"current space left: {_spaceLeft}, need to free up {_freeUp}");
                    long _justEnough = _dirSizes.OrderBy(x=>x).First(x=>x>= _freeUp);
                    return FormatResult(_justEnough, "folder size");

                    //int _markerLength = Part1 ? 4 : 14;
                    //for (int i = 0; i < InputData[0].Length - _markerLength; i++) if (InputData[0].Skip(i).Take(_markerLength).Distinct().Count() == _markerLength) return FormatResult(i + _markerLength, "packet start");
                }
            }

            public class FileElement
            {
                public List<FileElement> SubElements = new();
                public string Name;
                public long Size;
                public FileElement ParentDir;

                public FileElement(string Name, FileElement? ParentDir, long Size)
                {
                    this.Name = Name;
                    this.ParentDir = ParentDir;
                    this.Size = Size;
                }

                public void List(int Level = 0)
                {
                    StringBuilder _sb = new("  ".Repeat(Level) + " - " + Name);
                    if (Size == 0)
                    {
                        _sb.Append(" (dir)");
                    }
                    else
                    {
                        _sb.Append($" (file, size={Size})");
                    }
                    Console.WriteLine(_sb.ToString());
                    SubElements.ForEach(x => x.List(Level + 1));
                }

                public void GetSizes(List<long> DirSizes, int Level = 0)
                {
                    StringBuilder _sb = new("  ".Repeat(Level) + " - " + Name);
                    if (Size == 0)
                    {
                        _sb.Append(" (dir)");
                    }
                    else
                    {
                        _sb.Append($" (file, size={Size})");
                    }
                    if (Size == 0)
                    {
                        long _size = CalcSize();
                        DirSizes.Add(_size);
                        _sb.Append($" ----> {_size}");
                        if (_size <= 100000) _sb.Append($" XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
                    }
                    Console.WriteLine(_sb.ToString());
                    SubElements.ForEach(x => x.GetSizes(DirSizes, Level + 1));
                }

                public long CalcSize()
                {
                    if (Size > 0) return Size;
                    return SubElements.Where(x => x.Size > 0).Select(x => x.Size).Sum() + SubElements.Where(x => x.Size == 0).Select(x => x.CalcSize()).Sum();
                }
            }
        }
    }
}
