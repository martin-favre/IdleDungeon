using System;
using System.Collections.Generic;
using UnityEngine;

public class GoalAndStartAdder : IMapModifier
{
    System.Random random;
    public bool ImproveMap(IMap map, int seed)
    {
        random = new System.Random(seed);
        // Let's reuse directions, but let them represent quadrants
        // So north-> north-west
        var quadrant = ChooseQuadrant();
        var goaltile = new GoalTile();
        (var success, var location, var openDir) = PlaceTileInQuadrant(quadrant, map, goaltile);
        if (!success) return success;
        goaltile.Direction = openDir;
        map.Goal = location;
        var startTile = new StartTile();
        (success, location, openDir) = PlaceTileInQuadrant(Directions.GetOpposite(quadrant), map, startTile);
        if (success)
        {
            startTile.Direction = openDir;
            map.Start = location;
        }

        return success;

    }

    (bool, Directions.Direction) isValidLocation(IMap map, Vector2Int position)
    {
        if (!map.InsideMap(position)) return (false, Directions.Direction.North);
        // It is a valid location if only one surrounding tile is reachable
        // e.g. x is a valid position here. y is not.
        // ......
        // #x#.y#
        // ######
        int nofFreeTiles = 0;
        var freeDir = Directions.Direction.North;
        foreach (var dir in Directions.directions)
        {
            var newPos = position + Directions.DirToVec(dir);
            if (map.InsideMap(newPos) && !map.GetTile(newPos).IsClosed())
            {
                freeDir = dir;
                nofFreeTiles++;
            }
            if (nofFreeTiles > 1) return (false, freeDir);
        }
        return (nofFreeTiles == 1, freeDir);
    }

    private (bool, Vector2Int, Directions.Direction) PlaceTileInQuadrant(Directions.Direction quadrant, IMap map, Tile tile)
    {

        var testedLocations = new HashSet<Vector2Int>();
        var untestedLocations = new Queue<Vector2Int>();

        untestedLocations.Enqueue(GetStartingPoint(quadrant, map));
        while (untestedLocations.Count > 0)
        {
            var currentLocation = untestedLocations.Dequeue();
            testedLocations.Add(currentLocation);
            if (!map.InsideMap(currentLocation)) continue;
            (var isValid, var freeDir) = isValidLocation(map, currentLocation);
            if (isValid)
            {
                map.SetTile(currentLocation, tile);
                return (true, currentLocation, freeDir);
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
        return (false, Vector2Int.zero, Directions.Direction.North);
    }

    private Vector2Int GetStartingPoint(Directions.Direction quadrant, IMap map)
    {
        switch (quadrant)
        {
            case Directions.Direction.North:
                return Vector2Int.zero;
            case Directions.Direction.South:
                return new Vector2Int(map.Size.x - 1, map.Size.y - 1);
            case Directions.Direction.West:
                return new Vector2Int(0, map.Size.y - 1);
            case Directions.Direction.East:
                return new Vector2Int(map.Size.x - 1, 0);
            default:
                throw new System.Exception("Unknown Direction");
        }

    }

    private Vector2Int GetBottomRight(Directions.Direction quadrant, IMap map)
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

    private Vector2Int GetTopLeft(Directions.Direction quadrant, IMap map)
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