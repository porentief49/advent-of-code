using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Puzzles
{
    public partial class Year2021
    {
        public class Day19 : DayBase
        {
            protected override string Title { get; } = "Day 19 - Beacon Scanner";

            public override void Init() => Init(Inputs_2021.Rainer_19);

            public override void Init(string aResource) => Input = Tools.SplitLines(aResource, true);

            private long mMaxManhattanDist = 0;

            public override string SolvePuzzle(bool aPart1)
            {
                if (aPart1)
                { // will calculate part2 as well, cached and then simply returned

                    // parse input
                    var lScanners = new List<cScanner>();
                    foreach (string lLine in Input)
                    {
                        if (lLine.Trim().Length > 0)
                        {
                            if (lLine.Substring(0, 3) == "---") lScanners.Add(new cScanner());
                            else lScanners.Last().AddBeacon(lLine);
                        }
                    }

                    var lKnownBeacons = new List<(int X, int Y, int Z)>();

                    //add beacons of scanner 1 to known beacons list
                    for (int i = 0; i < lScanners[0].BeaconCount; i++) lKnownBeacons.AddIfNew(lScanners[0].Beacon(i, 0));
                    lScanners[0].AlreadyMapped = true;
                    bool lAdded;

                    //now compare the others
                    do
                    {
                        lAdded = false;
                        for (int lToScanner = 0; lToScanner < lScanners.Count; lToScanner++)
                        {
                            if (!lScanners[lToScanner].AlreadyMapped)
                            {
                                bool lFound = false;
                                for (int lRotation = 0; lRotation < 24; lRotation++)
                                {
                                    for (int lThisRefBeacon = 0; lThisRefBeacon < lKnownBeacons.Count; lThisRefBeacon++)
                                    {
                                        for (int lToRefBeacon = 0; lToRefBeacon < lScanners[lToScanner].BeaconCount; lToRefBeacon++)
                                        {
                                            int lDx = lScanners[lToScanner].Beacon(lToRefBeacon, lRotation).X - lKnownBeacons[lThisRefBeacon].X;
                                            int lDy = lScanners[lToScanner].Beacon(lToRefBeacon, lRotation).Y - lKnownBeacons[lThisRefBeacon].Y;
                                            int lDz = lScanners[lToScanner].Beacon(lToRefBeacon, lRotation).Z - lKnownBeacons[lThisRefBeacon].Z;
                                            int lMatchCount = 0;
                                            for (int lThisBeacon = 0; lThisBeacon < lKnownBeacons.Count; lThisBeacon++)
                                            {
                                                for (int lToBeacon = 0; lToBeacon < lScanners[lToScanner].BeaconCount; lToBeacon++)
                                                {
                                                    if (lScanners[lToScanner].Beacon(lToBeacon, lRotation).X - lDx == lKnownBeacons[lThisBeacon].X &&
                                                        lScanners[lToScanner].Beacon(lToBeacon, lRotation).Y - lDy == lKnownBeacons[lThisBeacon].Y &&
                                                        lScanners[lToScanner].Beacon(lToBeacon, lRotation).Z - lDz == lKnownBeacons[lThisBeacon].Z) lMatchCount++;
                                                }
                                            }
                                            if (lMatchCount >= 12)
                                            {
                                                if (Verbose) Console.WriteLine($"\nMatching: {lMatchCount}, ToScanner {lToScanner}, Dx {-lDx}, Dy {-lDy}, Dz {-lDz}, Rotation {lRotation}");
                                                lScanners[lToScanner].Position = (-lDx, -lDy, -lDz, lRotation);
                                                for (int i = 0; i < lScanners[lToScanner].BeaconCount; i++)
                                                {
                                                    (int X, int Y, int Z) lThis = lScanners[lToScanner].Beacon(i, lRotation);
                                                    lKnownBeacons.AddIfNew((lThis.X - lDx, lThis.Y - lDy, lThis.Z - lDz));
                                                }
                                                lScanners[lToScanner].AlreadyMapped = true;
                                                lFound = true;
                                                lAdded = true;
                                                break;
                                            }
                                        }
                                        if (lFound) break;
                                    }
                                    if (lFound) break;
                                }
                            }
                        }
                    } while (lAdded);

                    // calc manhattan distance
                    long lMaxDist = 0;
                    for (int i = 0; i < lScanners.Count; i++)
                    {
                        for (int ii = 0; ii < lScanners.Count; ii++)
                        {
                            long lThisDist = Math.Abs(lScanners[i].Position.X - lScanners[ii].Position.X) + Math.Abs(lScanners[i].Position.Y - lScanners[ii].Position.Y) + Math.Abs(lScanners[i].Position.Z - lScanners[ii].Position.Z);
                            if (lThisDist > lMaxDist)
                            {
                                lMaxDist = lThisDist;
                                if (Verbose) Console.WriteLine($"Distance from {i} --> {ii}: {lThisDist}");
                            }
                        }
                    }
                    mMaxManhattanDist = lMaxDist;
                    return FormatResult(lKnownBeacons.Count, "total beacons");
                    //mMaxManhattanDist = 12317;
                    //return FormatResult(357, "total beacons");
                }
                return FormatResult(mMaxManhattanDist, "max distance");
            }
        }

        public class cScanner
        {
            List<(int X, int Y, int Z)> mBeacons = new List<(int X, int Y, int Z)>();

            public (int X, int Y, int Z, int Rotation) Position = (0, 0, 0, 0);

            public int RelativeTo = -1;

            public bool AlreadyMapped = false;

            public int BeaconCount => mBeacons.Count();

            public static (int X, int Y, int Z) Rotate(int aX, int aY, int aZ, int aOrientation)
            {
                switch (aOrientation)
                {
                    case 0: // X Y Z
                        return (aX, aY, aZ);
                    case 1: // X -Z Y
                        return (aX, -aZ, aY);
                    case 2: // Y -Z -X
                        return (aY, -aZ, -aX);
                    case 3: // Z Y -X
                        return (aZ, aY, -aX);
                    case 4: // -Y Z -X
                        return (-aY, aZ, -aX);
                    case 5: // -Z -Y -X
                        return (-aZ, -aY, -aX);
                    case 6: // -X -Z -Y
                        return (-aX, -aZ, -aY);
                    case 7: // Z -X -Y
                        return (aZ, -aX, -aY);
                    case 8: // X Z -Y
                        return (aX, aZ, -aY);
                    case 9: // -Z X -Y
                        return (-aZ, aX, -aY);
                    case 10: // -Y -Z X
                        return (-aY, -aZ, aX);
                    case 11: // Z -Y X
                        return (aZ, -aY, aX);
                    case 12: // Y Z X
                        return (aY, aZ, aX);
                    case 13: // -Z Y X
                        return (-aZ, aY, aX);
                    case 14: // Z X Y
                        return (aZ, aX, aY);
                    case 15: // -X Z Y
                        return (-aX, aZ, aY);
                    case 16: // -Z -X Y
                        return (-aZ, -aX, aY);
                    case 17: // X -Y -Z
                        return (aX, -aY, -aZ);
                    case 18: // -X -Y Z
                        return (-aX, -aY, aZ);
                    case 19: // Y -X Z
                        return (aY, -aX, aZ);
                    case 20: // -Y X Z
                        return (-aY, aX, aZ);
                    case 21: // Y X -Z
                        return (aY, aX, -aZ);
                    case 22: // -X Y -Z
                        return (-aX, aY, -aZ);
                    case 23: // -Y -X -Z
                        return (-aY, -aX, -aZ);
                    default:
                        return (aX, aY, aZ);
                }
            }

            public (int X, int Y, int Z) Beacon(int aIndex, int aOrientation)
            {
                return Rotate(mBeacons[aIndex].X, mBeacons[aIndex].Y, mBeacons[aIndex].Z, aOrientation);
            }

            public void AddBeacon(string aCoords)
            {
                (int X, int Y, int Z) lCoords;
                string[] lSplit = aCoords.Split(',');
                lCoords.X = int.Parse(lSplit[0]);
                lCoords.Y = int.Parse(lSplit[1]);
                lCoords.Z = int.Parse(lSplit[2]);
                mBeacons.Add(lCoords);
            }
        }
    }
}
