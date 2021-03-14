
using UnityEngine;

public interface IMaceGenerator
{
    IGridMap GenerateMap(Vector2Int size, int seed);
}