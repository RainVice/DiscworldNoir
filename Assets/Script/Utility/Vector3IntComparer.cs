using System.Collections.Generic;
using UnityEngine;

namespace Script.Utility
{
    public class Vector3IntComparer : IComparer<Vector3Int>
    {
        public int Compare(Vector3Int x, Vector3Int y)
        {
            var xComparison = x.x.CompareTo(y.x);
            if (xComparison != 0) return xComparison;
            var yComparison = x.y.CompareTo(y.y);
            if (yComparison != 0) return yComparison;
            return x.z.CompareTo(y.z);
        }
    }
}