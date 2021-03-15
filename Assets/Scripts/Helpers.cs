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
}