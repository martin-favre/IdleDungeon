using System.Collections.Generic;
using UnityEngine;

public class GridMap : IGridMap
{
    private Dictionary<Vector2Int, Tile> tiles;
    private readonly Vector2Int size;

    public GridMap(Vector2Int size)
    {
        this.size = size;
        tiles = new Dictionary<Vector2Int, Tile>(size.x * size.y);
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                tiles[new Vector2Int(x, y)] = new Tile();
            }
        }
    }

    public Vector2Int Size => size;

    public bool InsideMap(Vector2Int position)
    {
        return tiles.ContainsKey(position);
    }

    public Tile GetTile(Vector2Int position)
    {
        if (!InsideMap(position)) throw new System.Exception("Position out of bounds");
        return tiles[position];
    }

    public void SetTile(Vector2Int position, Tile tile)
    {
        if (!InsideMap(position)) throw new System.Exception("Position out of bounds");
        tiles[position] = tile;
    }
}