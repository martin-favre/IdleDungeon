using System;
using System.Collections.Generic;
using UnityEngine;

public class GoalAndEndAdder : IMazeImprover
{
    System.Random random;
    public bool ImproveMap(IGridMap map, int seed)
    {
        random = new System.Random(seed);
        // Let's reuse directions, but let them represent quadrants
        // So north-> north-west
        var quadrant = ChooseQuadrant();
        var goaltile = new GoalTile();
        (var success, var location) = PlaceTileInQuadrant(quadrant, map, goaltile);
        if (!success) return success;
        map.Goal = location;
        var startTile = new StartTile();
        (success, location) = PlaceTileInQuadrant(Directions.GetOpposite(quadrant), map, startTile);
        if (success) map.Start = location;
        return success;

    }

    private (bool, Vector2Int) PlaceTileInQuadrant(Directions.Direction quadrant, IGridMap map, Tile tile)
    {

        var topleft = GetTopLeft(quadrant, map);
        var bottomRight = GetBottomRight(quadrant, map);
        var testedLocations = new HashSet<Vector2Int>();
        var untestedLocations = new Queue<Vector2Int>();

        untestedLocations.Enqueue(new Vector2Int(random.Next(topleft.x, bottomRight.x), random.Next(topleft.y, bottomRight.y)));
        while (untestedLocations.Count > 0)
        {
            var currentLocation = untestedLocations.Dequeue();
            testedLocations.Add(currentLocation);
            if (currentLocation.x < topleft.x || currentLocation.y < topleft.y || currentLocation.x > bottomRight.x || currentLocation.y > bottomRight.y) continue;
            if (!map.GetTile(currentLocation).IsClosed())
            {
                map.SetTile(currentLocation, tile);
                return (true, currentLocation);
            }
            else
            {
                foreach (var dir in Directions.directionsVec)
                {
                    var nextLocation = currentLocation + dir;
                    if (!testedLocations.Contains(nextLocation))
                    {
                        untestedLocations.Enqueue(nextLocation);
                    }
                }
            }
        }
        return (false, Vector2Int.zero);
    }

    private Vector2Int GetBottomRight(Directions.Direction quadrant, IGridMap map)
    {
        switch (quadrant)
        {
            case Directions.Direction.North:
                return map.Size / 2;
            case Directions.Direction.South:
                return map.Size;
            case Directions.Direction.West:
                return new Vector2Int(map.Size.x / 2, map.Size.y);
            case Directions.Direction.East:
                return new Vector2Int(map.Size.x, map.Size.y / 2);
            default:
                throw new System.Exception("Unknown Direction");
        }

    }

    private Vector2Int GetTopLeft(Directions.Direction quadrant, IGridMap map)
    {
        switch (quadrant)
        {
            case Directions.Direction.North:
                return Vector2Int.zero;
            case Directions.Direction.South:
                return map.Size / 2;
            case Directions.Direction.West:
                return new Vector2Int(0, map.Size.y / 2);
            case Directions.Direction.East:
                return new Vector2Int(map.Size.x / 2, 0);
            default:
                throw new System.Exception("Unknown Direction");
        }
    }

    private Directions.Direction ChooseQuadrant()
    {
        return Directions.GetRandomDirections(random)[0];
    }
}