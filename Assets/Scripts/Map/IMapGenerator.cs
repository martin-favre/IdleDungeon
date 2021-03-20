
using UnityEngine;

public interface IMapFactory
{
    IMap GenerateMap(Vector2Int size, int seed);
}

// Takes a map and does a second pass on it to add some more stuff
public interface IMapModifier
{
    bool ImproveMap(IMap map, int seed);
}