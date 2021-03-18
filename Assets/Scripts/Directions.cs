using System.Linq;
using UnityEngine;

/* Shorthand class to gather all info on Directions */
public static class Directions
{

    public enum Direction
    {
        North = 0,
        South = 1,
        East = 2,
        West = 3,
    }

    public static int GetNumberOfDirections()
    {
        return 4;
    }
    public static readonly Vector2Int[] directionsVec = { Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.left };
    public static readonly Direction[] directions = { Direction.North, Direction.South, Direction.East, Direction.West };
    public static Direction[] GetRandomDirections(System.Random random)
    {
        return directions.OrderBy(x => random.Next()).ToArray();
    }

    public static Vector2Int DirToVec(Direction dir)
    {
        switch (dir)
        {
            case Direction.North:
                return Vector2Int.up;
            case Direction.South:
                return Vector2Int.down;
            case Direction.West:
                return Vector2Int.left;
            case Direction.East:
                return Vector2Int.right;
            default:
                throw new System.Exception("Unknown Direction");
        }

    }

    public static Direction GetOpposite(Direction dir)
    {
        switch (dir)
        {
            case Direction.North:
                return Direction.South;
            case Direction.South:
                return Direction.North;
            case Direction.West:
                return Direction.East;
            case Direction.East:
                return Direction.West;
            default:
                throw new System.Exception("Unknown Direction");
        }
    }

    public static bool IsRightTurn(Direction previous, Direction next) {
        switch (previous)
        {
            case Direction.North:
                return next == Direction.East;
            case Direction.South:
                return next == Direction.West;
            case Direction.West:
                return next == Direction.North;
            case Direction.East:
                return next == Direction.South;
            default:
                throw new System.Exception("Unknown Direction");
        }
    }

        public static bool IsLeftTurn(Direction previous, Direction next) {
        switch (previous)
        {
            case Direction.North:
                return next == Direction.West;
            case Direction.South:
                return next == Direction.East;
            case Direction.West:
                return next == Direction.South;
            case Direction.East:
                return next == Direction.North;
            default:
                throw new System.Exception("Unknown Direction");
        }
    }

}
