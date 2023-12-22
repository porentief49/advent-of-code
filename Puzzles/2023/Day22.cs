using System.Net.Http.Headers;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;

namespace Puzzles {

    public partial class Year2023 {

        public class Day22 : DayBase {

            protected override string Title { get; } = "Day 22: Sand Slabs";

            public override void SetupAll() {
                //AddInputFile(@"2023\22_Example.txt");
                AddInputFile(@"2023\22_rAiner.txt");
            }

            public override void Init(string inputFile) => InputAsLines = ReadLines(inputFile, true);

            public override string Solve() {
                if (Part1) return "";
                var bricks = InputAsLines.Select(i => new Brick(i)).ToList();


                //settle bricks
                if (Verbose || true) Console.WriteLine($"======== SETTLING BRICKS");
                //bool canFall;
                int count = 0;
                bool actuallyFell;
                do {
                    //canFall = false;
                    actuallyFell = false;
                    for (int i = 0; i < bricks.Count; i++) { // we'll let this move down and check it against others
                        //canFall = CanItFall(bricks, i);
                        if (CanItFall(bricks, i)) {
                            //bricks[i] = bricks[i].TestOneStepDown();
                            bricks[i].ZFrom -= 1;
                            bricks[i].ZTo -= 1;
                            count++;
                            if (count % 1000 == 0) PrintStatus(bricks, count);
                            actuallyFell = true;
                            //break;
                        }
                    }
                } while (actuallyFell);

                //PrintStack(bricks);
                if (Part1) {
                    //check if it could be disintegrated
                    if (Verbose || true) Console.WriteLine($"======== CHECK DISINTEGRATION");

                    count = 0;
                    for (int i = 0; i < bricks.Count; i++) {
                        if (Verbose || true) Console.Write($"Trying to disintegrate brick {i} ... ");
                        var bricksMinusOne = bricks.Where((b, idx) => idx != i).ToList();
                        bool goodToDisIntegrate = true; // Assume until learning otherwise
                        int whichBrick = -1;
                        for (int ii = 0; ii < bricksMinusOne.Count; ii++) { // we'll let this move down and check it against others
                            if (CanItFall(bricksMinusOne, ii)) {
                                goodToDisIntegrate = false;
                                whichBrick = ii;
                                break;
                            }
                            //if (canFallX) bricks[i] = bricks[i].TestOneStepDown();
                        }
                        if (goodToDisIntegrate) count++;
                        if (Verbose || true) Console.WriteLine($"{(goodToDisIntegrate ? "YES!" : $"nope - {whichBrick} would fall")}");
                    }
                    return count.ToString();
                }


                //check if it could be disintegrated
                if (Verbose || true) Console.WriteLine($"======== CHECK DISINTEGRATION");

                count = 0;
                for (int i = 0; i < bricks.Count; i++) {
                    if (Verbose || true) Console.Write($"  Trying to disintegrate brick {i} ... ");
                    var bricksMinusOne = bricks.Where((b, idx) => idx != i).Select(b=>b.Copy()).ToList();
                    //bool goodToDisIntegrate = true; // Assume until learning otherwise
                    //int whichBrick = -1;
                    List<bool> fell = bricksMinusOne.Select(b => false).ToList();
                    do {

                        actuallyFell = false;


                        for (int ii = 0; ii < bricksMinusOne.Count; ii++) { // we'll let this move down and check it against others
                            if (CanItFall(bricksMinusOne, ii)) {
                                fell[ii] = true;
                                //goodToDisIntegrate = false;
                                bricksMinusOne[ii].ZFrom -= 1;
                                bricksMinusOne[ii].ZTo -= 1;
                                //whichBrick = ii;
                                //count++;
                                //break;
                                actuallyFell = true;
                                if (Verbose ) Console.WriteLine($"    {ii} would fall");
                            }
                            //if (canFallX) bricks[i] = bricks[i].TestOneStepDown();
                        }
                        //if (goodToDisIntegrate) count++;
                    } while (actuallyFell);
                    int howMany = fell.Count(f => f);
                    count += howMany;
                    if (Verbose || true) Console.WriteLine($"    {howMany} blocks fell");
                }
                return count.ToString();





                bool CanItFall(List<Brick>? brickSubset, int brickIndex) {
                    if (Verbose) Console.WriteLine($"  testing brick {brickIndex} ... ");
                    //canFall = (bricks[brickIndex].ZFrom > 1 && bricks[brickIndex].ZTo > 1);
                    if (brickSubset[brickIndex].ZFrom > 1 && brickSubset[brickIndex].ZTo > 1) {// we're at least 1 above ground
                        if (Verbose) Console.WriteLine($"    is high enough ... ");
                        Brick tryBrick = brickSubset[brickIndex].TestOneStepDown();
                        for (int ii = 0; ii < brickSubset.Count; ii++) {
                            if (brickIndex != ii) {
                                if (tryBrick.Intersects(brickSubset[ii])) {
                                    //canFall = false;
                                    if (Verbose) Console.WriteLine($"      but brick {ii} stopped it");
                                    return false;
                                }
                            }
                        }
                        //if (canFall) {
                        //bricks[brickIndex] = tryBrick;
                        if (Verbose) Console.WriteLine($"      can fall");
                        //Console.WriteLine($"Brick {brickIndex} can fall");
                        return true;
                        //}
                    } else return false;
                }
            }

            private void PrintStack(List<Brick> bricks) {
                int xMin = bricks.Min(b => Math.Min(b.XFrom, b.XTo));
                int xMax = bricks.Max(b => Math.Max(b.XFrom, b.XTo));
                int yMin = bricks.Min(b => Math.Min(b.YFrom, b.YTo));
                int yMax = bricks.Max(b => Math.Max(b.YFrom, b.YTo));
                int zMin = bricks.Min(b => Math.Min(b.ZFrom, b.ZTo));
                int zMax = bricks.Max(b => Math.Max(b.ZFrom, b.ZTo));
                for (int z = zMax; z >= zMin; z--) {
                    Console.WriteLine($"\nLayer{z}");
                    for (int y = yMax; y >= yMin; y--) {
                        for (int x = xMin; x <= xMax; x++) {
                            int which = -1;
                            for (int i = 0; i < bricks.Count; i++) {
                                if (bricks[i].IsInside(x, y, z)) which = i;
                            }
                            Console.Write(which >= 0 ? (char)(which + 65) : '.');
                        }
                        Console.WriteLine();
                    }
                }
            }

            private void PrintStatus(List<Brick> bricks, int index) {
                int zMin = bricks.Min(b => Math.Min(b.ZFrom, b.ZTo));
                int zMax = bricks.Max(b => Math.Max(b.ZFrom, b.ZTo));
                Console.WriteLine($"loop {index}, tower ranging from {zMin} to {zMax}");
            }

            private class Brick {

                public int XFrom;
                public int YFrom;
                public int ZFrom;
                public int XTo;
                public int YTo;
                public int ZTo;
                public List<int> SupportedBy = new();

                public Brick(string definition) {
                    var split = definition.ReplaceAnyChar("~<-", " ").Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    var coord = split[0].Split(',');
                    (XFrom, YFrom, ZFrom) = (int.Parse(coord[0]), int.Parse(coord[1]), int.Parse(coord[2]));
                    coord = split[1].Split(',');
                    (XTo, YTo, ZTo) = (int.Parse(coord[0]), int.Parse(coord[1]), int.Parse(coord[2]));
                }

                public Brick(int xFrom, int xTo, int yFrom, int yTo, int zFrom, int zTo) {
                    XFrom = Math.Min(xFrom, xTo);
                    XTo = Math.Max(xFrom, xTo);
                    YFrom = Math.Min(yFrom, yTo);
                    YTo = Math.Max(yFrom, yTo);
                    ZFrom = Math.Min(zFrom, zTo);
                    ZTo = Math.Max(zFrom, zTo);
                }

                public Brick TestOneStepDown() => new Brick(XFrom, XTo, YFrom, YTo, ZFrom - 1, ZTo - 1);
                public Brick Copy() => new Brick(XFrom, XTo, YFrom, YTo, ZFrom, ZTo);

                public bool Supports(Brick above) {
                    int zTop = Math.Max(ZFrom, ZTo) + 1;
                    for (int x = XFrom; x <= XTo; x++) for (int y = YFrom; y <= YTo; y++) if (above.IsInside(x, y, zTop)) return true;
                    return false;
                }

                public bool Intersects(Brick other) {
                    for (int x = XFrom; x <= XTo; x++) for (int y = YFrom; y <= YTo; y++) for (int z = ZFrom; z <= ZTo; z++) if (other.IsInside(x, y, z)) return true;
                    return false;
                }

                public bool IsInside(int x, int y, int z) {
                    return x >= XFrom && x <= XTo && y >= YFrom && y <= YTo && z >= ZFrom && z <= ZTo;
                }
            }
        }
    }
}
