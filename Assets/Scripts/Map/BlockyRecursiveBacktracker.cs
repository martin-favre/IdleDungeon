using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Logging;
using UnityEngine;

public class BlockyRecursiveBacktracker : IMazeGenerator
{

    HashSet<Vector2Int> visitedLocations;

    System.Random random;
    // LilLogger logger = new LilLogger("BlockyRecursiveBacktracker");
    public IGridMap GenerateMap(Vector2Int size, int seed)
    {
        random = new System.Random(seed);
        visitedLocations = new HashSet<Vector2Int>();
        IGridMap map = new GridMap(size);
        Vector2Int startingPos = new Vector2Int(1, 1);
        CarvePassage(map, startingPos);

        return map;
    }

    void CarvePassage(IGridMap map, Vector2Int currentPos)
    {
        Directions.Direction[] directions = Directions.GetRandomDirections(random);
        Tile currentTile = map.GetTile(currentPos);

        foreach (var dir in directions)
        {
            Vector2Int newPos = currentPos + 2 * Directions.DirToVec(dir);
            if (InsideMap(newPos, map) && !visitedLocations.Contains(newPos))
            {
                Tile newTile = new Tile();
                newTile.SetAllWalls(true);
                var inbetween = currentPos + Directions.DirToVec(dir);
                var inbetweenTile = new Tile();
                inbetweenTile.SetAllWalls(true);
                visitedLocations.Add(newPos);

                map.SetTile(inbetween, inbetweenTile);
                map.SetTile(newPos, newTile);
                CarvePassage(map, newPos);
            }
        }
    }

    bool InsideMap(Vector2Int pos, IGridMap map)
    {
        if (pos.x == 0 || pos.y == 0 || pos.x == map.Size.x - 1 || pos.y == map.Size.y - 1)
        {
            return false;
        }
        return map.InsideMap(pos);
    }
}
