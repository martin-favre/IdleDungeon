using System;
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
    public static bool XYSame(Vector3 a, Vector3 b) {
        return a.x == b.x && a.z == b.z;
    }
}