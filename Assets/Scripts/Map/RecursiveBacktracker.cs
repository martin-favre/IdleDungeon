using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Logging;
using UnityEngine;

public class RecursiveBacktracker : IMapFactory
{
 
    HashSet<Vector2Int> visitedLocations;

    IRandomProvider random;
    // LilLogger logger = new LilLogger("RecursiveBacktracker");
    public IMap GenerateMap(Vector2Int size, IRandomProvider random)
    {
        this.random = random;
        visitedLocations = new HashSet<Vector2Int>();
        IMap map = new GridMap(size);
        Vector2Int startingPos = new Vector2Int(0, 0);
        CarvePassage(map, startingPos);

        return map;
    }

    void CarvePassage(IMap map, Vector2Int currentPos)
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
