using System;
using UnityEngine;

[Serializable]
public class Tile
{
    [SerializeField] bool[] walls = new bool[4] { false, false, false, false };


    public Tile(Tile defaultTile)
    {
        this.walls = defaultTile.walls.Clone() as bool[];
    }

    public Tile()
    {
    }

    public void SetWall(Directions.Direction direction, bool open)
    {
        walls[(int)direction] = open;
    }

    public bool GetWall(Directions.Direction direction)
    {
        return walls[(int)direction];
    }

}