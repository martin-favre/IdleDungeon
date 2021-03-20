using UnityEngine;

public interface IGridMap
{
    void SetTile(Vector2Int position, Tile tile);
    Tile GetTile(Vector2Int position);
    Vector2Int Size { get; }

    Vector2Int Goal { get; set;}
    Vector2Int Start { get; set;}

    bool InsideMap(Vector2Int position);

}