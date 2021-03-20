
using UnityEngine;

public interface IMazeFactory
{
    IGridMap GenerateMap(Vector2Int size, int seed);
}

// Takes a map and does a second pass on it to add some more stuff
public interface IMazeModifier
{
    bool ImproveMap(IGridMap map, int seed);
}