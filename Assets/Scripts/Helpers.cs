using System;
using System.Collections.Generic;
using Logging;
using UnityEngine;

public static class Helpers
{
    public static void DoForAll(Vector2Int size, Action<Vector2Int> action)
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                action(new Vector2Int(x, y));
            }
        }

    }


    // Goes from a Vector2 grid position
    // to a Vector3 real space position
    public static Vector3 ToVec3(Vector2Int vec, float height)
    {
        return new Vector3(vec.x, height, vec.y);
    }

    // Are the grid positions the same?
    public static bool XYSame(Vector3 a, Vector3 b)
    {
        return a.x == b.x && a.z == b.z;
    }

    public static T GetRandom<T>(IList<T> items, IRandomProvider random)
    {
        if (items.Count == 0) throw new Exception("Can't get a random item from an empty list");
        var index = random.RandomInt(0, items.Count);
        return items[index];
    }

    // thanks https://answers.unity.com/questions/288338/how-do-i-compare-quaternions.html
    public static bool Approximately(Quaternion quatA, Quaternion quatB, float acceptableRange)
    {
        return 1 - Mathf.Abs(Quaternion.Dot(quatA, quatB)) < acceptableRange;
    }
}