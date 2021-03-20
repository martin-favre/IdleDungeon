using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Logging;
using UnityEngine;

public class RecursiveBacktracker : IMazeFactory
{
 
    HashSet<Vector2Int> visitedLocations;

    System.Random random;
    // LilLogger logger = new LilLogger("RecursiveBacktracker");
    public IGridMap GenerateMap(Vector2Int size, int seed)
    {
        random = new System.Random(seed);
        visitedLocations = new HashSet<Vector2Int>();
        IGridMap map = new GridMap(size);
        Vector2Int startingPos = new Vector2Int(0, 0);
        CarvePassage(map, startingPos);

        return map;
    }

    void CarvePassage(IGridMap map, Vector2Int currentPos)
    {
        Directions.Direction[] directions = Directions.GetRandomDirections(random);
        Tile currentTile = map.GetTile(currentPos);

        foreach (var dir in directions)
        {
            Vector2Int newPos = currentPos + Directions.DirToVec(dir);
            if (map.InsideMap(newPos) && !visitedLocations.Contains(newPos))
            {
                Tile newTile = new Tile();
                newTile.SetWall(Directions.GetOpposite(dir), true);
                visitedLocations.Add(newPos);
                currentTile.SetWall(dir, true);
                map.SetTile(newPos, newTile);
                CarvePassage(map, newPos);
            }
        }

    }
}
