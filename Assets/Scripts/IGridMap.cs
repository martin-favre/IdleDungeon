using UnityEngine;

public interface IGridMap
{
    void SetTile(Vector2Int position, Tile tile);
    Tile GetTile(Vector2Int position);
    Vector2Int Size { get; }

    bool InsideMap(Vector2Int position);

}