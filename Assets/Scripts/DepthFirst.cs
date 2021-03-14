
using System;
using System.Collections.Generic;
using Logging;
using UnityEngine;

public class DepthFirst : IPathFinder
{
    HashSet<Vector2Int> testedLocations = new HashSet<Vector2Int>();
    Vector2Int goal;
    IGridMap map;

    LilLogger logger = new LilLogger("DepthFirst");

    public Stack<Vector2Int> FindPath(Vector2Int from, Vector2Int to, IGridMap map)
    {
        goal = to;
        this.map = map;
        if (from == to) new Stack<Vector2Int>();
        var unvisitedPaths = new Stack<Tuple<Vector2Int, List<Vector2Int>>>();
        unvisitedPaths.Push(new Tuple<Vector2Int, List<Vector2Int>>(from, new List<Vector2Int>()));

        while (unvisitedPaths.Count > 0)
        {
            var currentPos = unvisitedPaths.Pop();
            if (currentPos.Item1 == to)
            {
                return new Stack<Vector2Int>(currentPos.Item2.ToArray());
            }
            testedLocations.Add(currentPos.Item1);
            foreach (var dir in Directions.directions)
            {
                // if that wall is open
                if (map.GetTile(currentPos.Item1).GetWall(dir))
                {
                    Vector2Int nextPos = currentPos.Item1 + Directions.DirToVec(dir);
                    if (map.InsideMap(nextPos) && !testedLocations.Contains(nextPos))
                    {
                        var nextPath = new List<Vector2Int>(currentPos.Item2.ToArray());
                        nextPath.Add(nextPos);
                        unvisitedPaths.Push(new Tuple<Vector2Int, List<Vector2Int>>(nextPos, nextPath));
                    }
                }
            }
        }
        logger.Log("Unable to find path, no possible path");
        return new Stack<Vector2Int>();

    }

    private void FindPath(Vector2Int currentPos)
    {
        foreach (var dir in Directions.directions)
        {
            // if that wall is open
            if (map.GetTile(currentPos).GetWall(dir))
            {
                Vector2Int nextPos = currentPos + Directions.DirToVec(dir);
                if (map.InsideMap(nextPos) && !testedLocations.Contains(nextPos))
                {

                }
            }
        }
    }
}