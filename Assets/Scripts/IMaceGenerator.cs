
using UnityEngine;

public interface IMazeGenerator
{
    IGridMap GenerateMap(Vector2Int size, int seed);
}